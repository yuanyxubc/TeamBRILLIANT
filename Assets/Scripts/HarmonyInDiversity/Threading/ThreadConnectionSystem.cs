using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the thread connection mechanic - the core interaction of Scene 3
/// </summary>
public class ThreadConnectionSystem : MonoBehaviour
{
    public static ThreadConnectionSystem Instance;

    [Header("Thread Settings")]
    [Tooltip("Width of the thread line")]
    public float threadWidth = 0.05f;

    [Tooltip("Distance for auto-snapping to target orb")]
    public float snapDistance = 1.5f; // Increased from 0.5f for easier connections

    [Tooltip("Maximum connections allowed per orb")]
    public int maxConnectionsPerOrb = 4;

    [Tooltip("Minimum connections needed to progress to Scene 4")]
    public int minimumConnectionsForCompletion = 7;

    [Header("Controller References")]
    [Tooltip("Assign the XRRayInteractor from the right controller")]
    public XRRayInteractor rayInteractor;

    [Header("Input (Auto-configured)")]
    [Tooltip("Reference to the select action - will be auto-configured from controller")]
    public InputActionProperty selectAction;

    [Header("Particle Effects (Optional)")]
    [Tooltip("Particle effect to spawn on successful connection")]
    public GameObject connectionBurstPrefab;

    [Header("Audio (Optional)")]
    [Tooltip("Sound when starting to pull a thread from an orb")]
    public AudioClip threadPullStartSound;

    [Tooltip("Sound when thread snaps to target orb")]
    public AudioClip threadSnapSound;

    [Tooltip("Sound when connection is successfully created")]
    public AudioClip connectionSuccessSound;

    [Tooltip("Volume for thread SFX (0-1)")]
    [Range(0f, 1f)]
    public float sfxVolume = 0.8f;

    // State
    private CulturalOrb sourceOrb;
    private LineRenderer activeThreadBeam;
    private List<ConnectionThread> connections = new List<ConnectionThread>();
    private bool isPullingThread = false;
    private bool wasSelectPressed = false;
    private CulturalOrb lastSnapTarget = null; // Track last orb we snapped to

    // UI reference
    private bool isActive = false;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Validate ray interactor
        if (rayInteractor == null)
        {
            Debug.LogWarning("ThreadConnectionSystem: No XRRayInteractor assigned! Please assign it in the inspector.");
        }
        else
        {
            // Try to auto-configure the select action from the controller
            ConfigureSelectAction();
        }
    }

    void ConfigureSelectAction()
    {
        // Try to get the controller interactor
        if (rayInteractor != null && rayInteractor.transform.parent != null)
        {
            var controllerInteractor = rayInteractor.transform.parent.GetComponentInChildren<UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor>();
            if (controllerInteractor != null)
            {
                // Get the select action reference from the controller
                var selectActionRef = controllerInteractor.GetType().GetProperty("selectAction");
                if (selectActionRef != null)
                {
                    selectAction = (InputActionProperty)selectActionRef.GetValue(controllerInteractor);
                    Debug.Log("ThreadConnectionSystem: Select action auto-configured from controller");
                }
            }
        }

        // If auto-config failed, we'll fall back to manual trigger detection
        if (selectAction.action == null)
        {
            Debug.LogWarning("ThreadConnectionSystem: Could not auto-configure select action. Will use fallback input detection.");
        }
    }

    void Update()
    {
        // Only active during Scene 3
        if (HarmonySceneManager.Instance == null ||
            HarmonySceneManager.Instance.CurrentState != SceneState.ConnectingThreads)
        {
            if (isActive)
            {
                isActive = false;
                Debug.Log("Thread Connection System is now INACTIVE");
            }
            return;
        }

        if (!isActive)
        {
            isActive = true;
            Debug.Log("=== Thread Connection System is now ACTIVE ===");
            Debug.Log("In Unity Editor: Click and hold left mouse on orbs to create threads");
            Debug.Log("On Quest 2: Point and hold trigger on orbs to create threads");
        }

        if (rayInteractor == null)
        {
            Debug.LogError("ThreadConnectionSystem: Ray Interactor is null!");
            return;
        }

        HandleThreadPulling();
    }

    void HandleThreadPulling()
    {
        // Check if trigger is pressed using the configured input action
        bool triggerPressed = GetSelectPressed();

        // Detect button press (rising edge)
        bool selectJustPressed = triggerPressed && !wasSelectPressed;
        bool selectJustReleased = !triggerPressed && wasSelectPressed;
        wasSelectPressed = triggerPressed;

        // START pulling thread (on button press)
        if (selectJustPressed && !isPullingThread)
        {
            Debug.Log("Trigger pressed - attempting to start thread pull");

            CulturalOrb hitOrb = null;

            #if UNITY_EDITOR
            // In Editor: Use mouse raycast for testing
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit mouseHit, 100f))
                {
                    Debug.Log($"Mouse Raycast hit: {mouseHit.collider.gameObject.name}");
                    hitOrb = mouseHit.collider.GetComponentInParent<CulturalOrb>();
                }
                else
                {
                    Debug.LogWarning("Mouse raycast didn't hit anything");
                }
            }
            #else
            // In VR Build: Use Physics.Raycast from controller position/direction
            if (rayInteractor != null)
            {
                Ray controllerRay = new Ray(rayInteractor.transform.position, rayInteractor.transform.forward);
                if (Physics.Raycast(controllerRay, out RaycastHit vrHit, 100f))
                {
                    Debug.Log($"VR Physics Raycast hit: {vrHit.collider.gameObject.name} at distance {vrHit.distance:F2}m");
                    hitOrb = vrHit.collider.GetComponentInParent<CulturalOrb>();

                    if (hitOrb != null)
                    {
                        Debug.Log($"Found CulturalOrb: {hitOrb.data.cultureName}");
                    }
                }
                else
                {
                    Debug.LogWarning($"VR raycast from {rayInteractor.transform.position} forward {rayInteractor.transform.forward} didn't hit anything");
                }
            }
            #endif

            if (hitOrb != null)
            {
                StartThreadPull(hitOrb);
            }
            else if (hitOrb == null)
            {
                Debug.LogWarning("No orb found at raycast target");
            }
        }

        // UPDATE thread beam position while pulling
        if (isPullingThread && activeThreadBeam != null && sourceOrb != null)
        {
            Vector3 controllerPos = rayInteractor.transform.position;
            Vector3 beamEndPoint = controllerPos;

            #if UNITY_EDITOR
            // In editor, use mouse raycast to get world position
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit mouseHit, 100f))
                {
                    beamEndPoint = mouseHit.point;
                }
                else
                {
                    // If no hit, project mouse position forward from camera
                    beamEndPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5f));
                }
            }
            #else
            // In VR, project beam forward from controller
            if (rayInteractor != null)
            {
                Ray controllerRay = new Ray(rayInteractor.transform.position, rayInteractor.transform.forward);
                if (Physics.Raycast(controllerRay, out RaycastHit vrHit, 100f))
                {
                    beamEndPoint = vrHit.point;
                }
                else
                {
                    // If no hit, project forward 5 meters
                    beamEndPoint = rayInteractor.transform.position + rayInteractor.transform.forward * 5f;
                }
            }
            #endif

            UpdateThreadBeam(sourceOrb.transform.position, beamEndPoint);

            // Check for nearby target orb for snapping visual
            CulturalOrb targetOrb = FindNearbyOrb(beamEndPoint);
            if (targetOrb != null && targetOrb != sourceOrb)
            {
                // Visual snap indicator - change beam color or add glow
                activeThreadBeam.endColor = targetOrb.data.orbColor;

                // Play snap sound when entering snap range (only once per target)
                if (lastSnapTarget != targetOrb && threadSnapSound != null && HarmonyAudioManager.Instance != null)
                {
                    HarmonyAudioManager.Instance.PlaySFX(threadSnapSound, sfxVolume * 0.6f); // Slightly quieter
                    lastSnapTarget = targetOrb;
                }
            }
            else
            {
                activeThreadBeam.endColor = sourceOrb.data.orbColor;
                lastSnapTarget = null; // Reset when not snapping to anything
            }
        }

        // COMPLETE pulling (on button release)
        if (selectJustReleased && isPullingThread)
        {
            CompleteThreadPull();
        }
    }

    bool GetSelectPressed()
    {
        #if UNITY_EDITOR
        // In Editor: Prioritize mouse input for testing
        if (Input.GetMouseButton(0)) // Left mouse button
        {
            return true;
        }
        #endif

        // VR Method 1: Use configured select action (if available)
        if (selectAction.action != null && selectAction.action.IsPressed())
        {
            Debug.Log("Trigger detected via Select Action");
            return true;
        }

        // VR Method 2: Direct XR Input - Read trigger from right controller
        UnityEngine.XR.InputDevice rightController = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand);
        if (rightController.isValid)
        {
            float triggerValue;
            if (rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out triggerValue))
            {
                if (triggerValue > 0.5f) // Trigger pressed beyond threshold
                {
                    Debug.Log($"Trigger detected via XR Input: {triggerValue:F2}");
                    return true;
                }
            }

            // Fallback: Check grip button
            bool gripPressed;
            if (rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out gripPressed))
            {
                if (gripPressed)
                {
                    Debug.Log("Grip button detected via XR Input");
                    return true;
                }
            }
        }

        // VR Method 3: Try to read from interactor directly (unlikely to work for our case)
        if (rayInteractor != null)
        {
            // Check if interactor is actively selecting
            if (rayInteractor.interactablesSelected.Count > 0 || rayInteractor.hasSelection)
            {
                Debug.Log("Trigger detected via Ray Interactor selection");
                return true;
            }
        }

        return false;
    }

    void StartThreadPull(CulturalOrb orb)
    {
        sourceOrb = orb;
        isPullingThread = true;

        Debug.Log($"✓ Started pulling thread from {orb.data.cultureName} orb");

        // Play thread pull start sound
        if (threadPullStartSound != null && HarmonyAudioManager.Instance != null)
        {
            HarmonyAudioManager.Instance.PlaySFX(threadPullStartSound, sfxVolume);
        }

        // Create visual beam (temporary LineRenderer)
        GameObject beamObj = new GameObject("ThreadBeam_Temp");
        activeThreadBeam = beamObj.AddComponent<LineRenderer>();

        // Configure beam
        activeThreadBeam.startWidth = threadWidth;
        activeThreadBeam.endWidth = threadWidth;
        activeThreadBeam.positionCount = 2;
        activeThreadBeam.useWorldSpace = true;

        // Set material and color - Use built-in shader that works on all platforms
        Shader lineShader = Shader.Find("Unlit/Color");
        if (lineShader == null)
        {
            // Fallback: Use Sprites/Default if Unlit/Color not found
            lineShader = Shader.Find("Sprites/Default");
            Debug.LogWarning("Unlit/Color shader not found, using Sprites/Default as fallback");
        }
        if (lineShader == null)
        {
            // Final fallback: Use Hidden/Internal-Colored
            lineShader = Shader.Find("Hidden/Internal-Colored");
            Debug.LogWarning("Sprites/Default shader not found, using Hidden/Internal-Colored as fallback");
        }

        if (lineShader != null)
        {
            Material beamMaterial = new Material(lineShader);
            beamMaterial.color = sourceOrb.data.orbColor;
            activeThreadBeam.material = beamMaterial;
            Debug.Log($"Thread beam material created with shader: {lineShader.name}");
        }
        else
        {
            Debug.LogError("No suitable shader found for thread beam! Using default LineRenderer material.");
        }

        activeThreadBeam.startColor = sourceOrb.data.orbColor;
        activeThreadBeam.endColor = sourceOrb.data.orbColor;

        // Visual quality
        activeThreadBeam.numCapVertices = 5;
        activeThreadBeam.numCornerVertices = 5;

        Debug.Log($"Thread beam created with color {sourceOrb.data.orbColor}");
    }

    void UpdateThreadBeam(Vector3 start, Vector3 end)
    {
        if (activeThreadBeam != null)
        {
            activeThreadBeam.SetPosition(0, start);
            activeThreadBeam.SetPosition(1, end);
        }
    }

    void CompleteThreadPull()
    {
        Vector3 releasePoint = rayInteractor.transform.position;

        #if UNITY_EDITOR
        // In editor, use mouse position
        if (Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit mouseHit, 100f))
            {
                releasePoint = mouseHit.point;
            }
        }
        #else
        // In VR, use controller raycast point
        if (rayInteractor != null)
        {
            Ray controllerRay = new Ray(rayInteractor.transform.position, rayInteractor.transform.forward);
            if (Physics.Raycast(controllerRay, out RaycastHit vrHit, 100f))
            {
                releasePoint = vrHit.point;
            }
            else
            {
                // If no hit, use point 5 meters forward
                releasePoint = rayInteractor.transform.position + rayInteractor.transform.forward * 5f;
            }
        }
        #endif

        CulturalOrb targetOrb = FindNearbyOrb(releasePoint);

        // Check if valid connection
        if (targetOrb != null && targetOrb != sourceOrb)
        {
            CreateConnection(sourceOrb, targetOrb);
        }
        else
        {
            if (targetOrb == null)
            {
                Debug.Log($"No orb within {snapDistance}m of release point - connection cancelled");
            }
            else if (targetOrb == sourceOrb)
            {
                Debug.Log("Cannot connect orb to itself - connection cancelled");
            }
        }

        // Cleanup temporary beam
        if (activeThreadBeam != null)
        {
            Destroy(activeThreadBeam.gameObject);
        }

        isPullingThread = false;
        sourceOrb = null;
        lastSnapTarget = null; // Reset snap target
    }

    CulturalOrb FindNearbyOrb(Vector3 position)
    {
        // Find all colliders within snap distance
        Collider[] colliders = Physics.OverlapSphere(position, snapDistance);

        CulturalOrb closestOrb = null;
        float closestDistance = float.MaxValue;

        foreach (var col in colliders)
        {
            CulturalOrb orb = col.GetComponentInParent<CulturalOrb>();
            if (orb != null)
            {
                float distance = Vector3.Distance(position, orb.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestOrb = orb;
                }
            }
        }

        if (closestOrb != null)
        {
            Debug.Log($"Found nearby orb: {closestOrb.data.cultureName} at distance {closestDistance:F2}m");
        }

        return closestOrb;
    }

    void CreateConnection(CulturalOrb orbA, CulturalOrb orbB)
    {
        // Validation checks
        if (ConnectionExists(orbA, orbB))
        {
            Debug.Log($"Connection already exists between {orbA.data.cultureName} and {orbB.data.cultureName}");
            return;
        }

        if (orbA.connectedOrbs.Count >= maxConnectionsPerOrb)
        {
            Debug.Log($"{orbA.data.cultureName} orb has reached maximum connections");
            return;
        }

        if (orbB.connectedOrbs.Count >= maxConnectionsPerOrb)
        {
            Debug.Log($"{orbB.data.cultureName} orb has reached maximum connections");
            return;
        }

        // Create connection thread GameObject
        GameObject threadObj = new GameObject($"Thread_{orbA.data.cultureName}_{orbB.data.cultureName}");
        ConnectionThread thread = threadObj.AddComponent<ConnectionThread>();
        thread.Initialize(orbA, orbB, threadWidth);

        connections.Add(thread);

        // Update orbs
        orbA.AddConnection(orbB);
        orbB.AddConnection(orbA);

        Debug.Log($"✓ Created connection between {orbA.data.cultureName} and {orbB.data.cultureName} ({connections.Count} total)");

        // Play connection success sound
        if (connectionSuccessSound != null && HarmonyAudioManager.Instance != null)
        {
            HarmonyAudioManager.Instance.PlaySFX(connectionSuccessSound, sfxVolume);
        }

        // Visual feedback - particle burst
        CreateConnectionBurst(orbA.transform.position, orbB.transform.position);

        // Update UI
        if (HarmonyUIManager.Instance != null)
        {
            HarmonyUIManager.Instance.UpdateConnectionCount(connections.Count);
        }

        // Check for scene completion
        if (connections.Count >= minimumConnectionsForCompletion)
        {
            Debug.Log($"Minimum connections reached ({connections.Count}/{minimumConnectionsForCompletion}) - Transitioning to Scene 4");

            // Small delay before transition
            Invoke(nameof(TransitionToScene4), 2f);
        }
    }

    void TransitionToScene4()
    {
        if (HarmonySceneManager.Instance != null)
        {
            HarmonySceneManager.Instance.TransitionToState(SceneState.TapestryOfUnity);
        }
    }

    bool ConnectionExists(CulturalOrb orbA, CulturalOrb orbB)
    {
        return connections.Exists(c =>
            (c.orbA == orbA && c.orbB == orbB) ||
            (c.orbA == orbB && c.orbB == orbA));
    }

    void CreateConnectionBurst(Vector3 posA, Vector3 posB)
    {
        Vector3 midpoint = (posA + posB) / 2f;

        if (connectionBurstPrefab != null)
        {
            GameObject burst = Instantiate(connectionBurstPrefab, midpoint, Quaternion.identity);
            Destroy(burst, 3f);
        }
    }

    public List<ConnectionThread> GetAllConnections()
    {
        return connections;
    }

    public int GetConnectionCount()
    {
        return connections.Count;
    }

    public void ClearAllConnections()
    {
        foreach (var thread in connections)
        {
            if (thread != null)
            {
                Destroy(thread.gameObject);
            }
        }
        connections.Clear();
    }
}
