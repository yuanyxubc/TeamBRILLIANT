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
    public float snapDistance = 0.5f;

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

    // State
    private CulturalOrb sourceOrb;
    private LineRenderer activeThreadBeam;
    private List<ConnectionThread> connections = new List<ConnectionThread>();
    private bool isPullingThread = false;
    private bool wasSelectPressed = false;

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
            isActive = false;
            return;
        }

        if (!isActive)
        {
            isActive = true;
            Debug.Log("Thread Connection System is now ACTIVE");
        }

        if (rayInteractor == null)
            return;

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
            // Check if raycast hits an orb
            if (rayInteractor != null && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                CulturalOrb orb = hit.collider.GetComponentInParent<CulturalOrb>();
                if (orb != null)
                {
                    StartThreadPull(orb);
                }
            }
        }

        // UPDATE thread beam position while pulling
        if (isPullingThread && activeThreadBeam != null && sourceOrb != null)
        {
            Vector3 controllerPos = rayInteractor.transform.position;
            UpdateThreadBeam(sourceOrb.transform.position, controllerPos);

            // Check for nearby target orb for snapping visual
            CulturalOrb targetOrb = FindNearbyOrb(controllerPos);
            if (targetOrb != null && targetOrb != sourceOrb)
            {
                // Visual snap indicator - change beam color or add glow
                activeThreadBeam.endColor = targetOrb.data.orbColor;
            }
            else
            {
                activeThreadBeam.endColor = sourceOrb.data.orbColor;
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
        // Primary method: Use configured select action
        if (selectAction.action != null)
        {
            return selectAction.action.IsPressed();
        }

        // Fallback 1: Try to read from interactor directly
        if (rayInteractor != null)
        {
            // Check if interactor is actively selecting
            if (rayInteractor.interactablesSelected.Count > 0 || rayInteractor.hasSelection)
            {
                return true;
            }
        }

        // Fallback 2: Use old Input system as last resort (for testing in editor)
        #if UNITY_EDITOR
        if (Input.GetMouseButton(0)) // Left mouse button
        {
            return true;
        }
        #endif

        return false;
    }

    void StartThreadPull(CulturalOrb orb)
    {
        sourceOrb = orb;
        isPullingThread = true;

        Debug.Log($"Started pulling thread from {orb.data.cultureName} orb");

        // Create visual beam (temporary LineRenderer)
        GameObject beamObj = new GameObject("ThreadBeam_Temp");
        activeThreadBeam = beamObj.AddComponent<LineRenderer>();

        // Configure beam
        activeThreadBeam.startWidth = threadWidth;
        activeThreadBeam.endWidth = threadWidth;
        activeThreadBeam.positionCount = 2;
        activeThreadBeam.useWorldSpace = true;

        // Set material and color
        Material beamMaterial = new Material(Shader.Find("Sprites/Default"));
        activeThreadBeam.material = beamMaterial;
        activeThreadBeam.startColor = sourceOrb.data.orbColor;
        activeThreadBeam.endColor = sourceOrb.data.orbColor;

        // Visual quality
        activeThreadBeam.numCapVertices = 5;
        activeThreadBeam.numCornerVertices = 5;
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
        Vector3 controllerPos = rayInteractor.transform.position;
        CulturalOrb targetOrb = FindNearbyOrb(controllerPos);

        // Check if valid connection
        if (targetOrb != null && targetOrb != sourceOrb)
        {
            CreateConnection(sourceOrb, targetOrb);
        }
        else
        {
            Debug.Log("No valid target orb found - connection cancelled");
        }

        // Cleanup temporary beam
        if (activeThreadBeam != null)
        {
            Destroy(activeThreadBeam.gameObject);
        }

        isPullingThread = false;
        sourceOrb = null;
    }

    CulturalOrb FindNearbyOrb(Vector3 position)
    {
        // Find all colliders within snap distance
        Collider[] colliders = Physics.OverlapSphere(position, snapDistance);

        foreach (var col in colliders)
        {
            CulturalOrb orb = col.GetComponentInParent<CulturalOrb>();
            if (orb != null)
            {
                return orb;
            }
        }

        return null;
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
