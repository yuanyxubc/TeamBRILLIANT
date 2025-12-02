using System.Collections;
using UnityEngine;

/// <summary>
/// Main scene controller managing the flow through all 5 narrative scenes
/// </summary>
public class HarmonySceneManager : MonoBehaviour
{
    public static HarmonySceneManager Instance;

    [Header("Scene References")]
    [Tooltip("Assign all 5 cultural orb PREFABS (from Project window)")]
    public CulturalOrb[] culturalOrbPrefabs;

    [Tooltip("Spawn point transforms for each orb (should match orb count)")]
    public Transform[] spawnPoints;

    [Tooltip("Particle systems for sparkles at spawn points")]
    public ParticleSystem[] sparkleParticles;

    // Runtime tracking of instantiated orbs
    private CulturalOrb[] instantiatedOrbs;

    [Header("Narration")]
    [Tooltip("Narration data containing all voice-over clips")]
    public NarrationData narrationData;

    [Header("Scene Timing")]
    [Tooltip("Duration of Scene 1 before auto-transition")]
    public float scene1Duration = 10f;

    [Tooltip("Duration of Scene 2 before auto-transition")]
    public float scene2Duration = 30f;

    [Tooltip("Duration of Scene 4 before auto-transition")]
    public float scene4Duration = 20f;

    [Header("Animation Settings")]
    [Tooltip("Duration for orb spawn animation")]
    public float orbSpawnDuration = 2f;

    [Tooltip("Stagger delay between each orb spawn")]
    public float orbSpawnStagger = 0.5f;

    [Tooltip("Duration for orb ascension in Scene 5")]
    public float orbAscensionDuration = 5f;

    [Tooltip("Height orbs rise to in Scene 5")]
    public float orbAscensionHeight = 10f;

    [Header("Debug")]
    [Tooltip("Enable to see state transition logs")]
    public bool debugMode = true;

    // Current state
    public SceneState CurrentState { get; private set; } = SceneState.CourtyardAwakens;

    // Coroutine tracking
    private Coroutine autoProgressCoroutine;

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
        // Validate setup
        ValidateSetup();

        // Initialize Scene 1
        InitializeScene1();
    }

    void ValidateSetup()
    {
        if (culturalOrbPrefabs == null || culturalOrbPrefabs.Length == 0)
        {
            Debug.LogError("HarmonySceneManager: No cultural orb prefabs assigned!");
        }

        if (spawnPoints == null || spawnPoints.Length != culturalOrbPrefabs.Length)
        {
            Debug.LogWarning($"HarmonySceneManager: Spawn points count ({spawnPoints?.Length}) doesn't match orbs count ({culturalOrbPrefabs.Length})");
        }

        if (sparkleParticles == null || sparkleParticles.Length != culturalOrbPrefabs.Length)
        {
            Debug.LogWarning($"HarmonySceneManager: Sparkle particles count ({sparkleParticles?.Length}) doesn't match orbs count ({culturalOrbPrefabs.Length})");
        }

        // Initialize array to track instantiated orbs
        instantiatedOrbs = new CulturalOrb[culturalOrbPrefabs != null ? culturalOrbPrefabs.Length : 0];
    }

    #region Scene 1: The Courtyard Awakens

    void InitializeScene1()
    {
        CurrentState = SceneState.CourtyardAwakens;
        Log("=== SCENE 1: The Courtyard Awakens ===");

        // Activate sparkles at spawn points
        if (sparkleParticles != null)
        {
            foreach (var sparkle in sparkleParticles)
            {
                if (sparkle != null)
                {
                    sparkle.Play();
                }
            }
        }

        // Note: Orbs are not instantiated yet in Scene 1
        // They will be created in Scene 2 via InitializeScene2()

        // Show opening narration (text)
        if (HarmonyUIManager.Instance != null)
        {
            HarmonyUIManager.Instance.ShowNarration("Every culture is a light, waiting to be seen...", 5f);
        }

        // Play narration audio
        if (narrationData != null && narrationData.scene1Opening != null && HarmonyAudioManager.Instance != null)
        {
            HarmonyAudioManager.Instance.PlayNarration(narrationData.scene1Opening, narrationData.scene1Delay);
        }

        // Auto-progress to Scene 2
        autoProgressCoroutine = StartCoroutine(AutoProgressToScene2());
    }

    IEnumerator AutoProgressToScene2()
    {
        yield return new WaitForSeconds(scene1Duration);
        TransitionToState(SceneState.VoicesRise);
    }

    #endregion

    #region Scene 2: The Voices Rise

    void InitializeScene2()
    {
        CurrentState = SceneState.VoicesRise;
        Log("=== SCENE 2: The Voices Rise ===");

        // Re-enable Near-Far Interactors for grabbing orbs
        EnableNearFarInteractors();

        // Stop sparkles
        if (sparkleParticles != null)
        {
            foreach (var sparkle in sparkleParticles)
            {
                if (sparkle != null)
                {
                    sparkle.Stop();
                }
            }
        }

        // Spawn orbs with staggered animation
        if (culturalOrbPrefabs != null && spawnPoints != null)
        {
            for (int i = 0; i < Mathf.Min(culturalOrbPrefabs.Length, spawnPoints.Length); i++)
            {
                if (culturalOrbPrefabs[i] != null && spawnPoints[i] != null)
                {
                    // INSTANTIATE the prefab to create a clone in the scene
                    CulturalOrb orbInstance = Instantiate(culturalOrbPrefabs[i], spawnPoints[i].position, Quaternion.identity);
                    orbInstance.name = $"{culturalOrbPrefabs[i].data.cultureName}Orb"; // Clean name without "(Clone)"
                    orbInstance.gameObject.SetActive(false); // Start hidden, will be activated in animation

                    // Track the instantiated orb
                    instantiatedOrbs[i] = orbInstance;

                    // Animate the spawn
                    StartCoroutine(SpawnOrbAnimation(orbInstance, spawnPoints[i].position, i * orbSpawnStagger));
                }
            }
        }

        // Show narration (text)
        if (HarmonyUIManager.Instance != null)
        {
            HarmonyUIManager.Instance.ShowNarration("The voices of many cultures rise...", 5f);
        }

        // Play narration audio
        if (narrationData != null && narrationData.scene2Opening != null && HarmonyAudioManager.Instance != null)
        {
            HarmonyAudioManager.Instance.PlayNarration(narrationData.scene2Opening, narrationData.scene2Delay);
        }

        // Auto-progress to Scene 3
        autoProgressCoroutine = StartCoroutine(AutoProgressToScene3());
    }

    IEnumerator SpawnOrbAnimation(CulturalOrb orb, Vector3 targetPos, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Activate orb
        orb.gameObject.SetActive(true);

        // Start below target position
        Vector3 startPos = targetPos - Vector3.up * 2f;
        orb.transform.position = startPos;

        // Animate upward
        float elapsed = 0f;
        while (elapsed < orbSpawnDuration)
        {
            float t = elapsed / orbSpawnDuration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            orb.transform.position = Vector3.Lerp(startPos, targetPos, smoothT);

            elapsed += Time.deltaTime;
            yield return null;
        }

        orb.transform.position = targetPos;
        Log($"{orb.data.cultureName} orb spawned");
    }

    IEnumerator AutoProgressToScene3()
    {
        yield return new WaitForSeconds(scene2Duration);
        TransitionToState(SceneState.ConnectingThreads);
    }

    #endregion

    #region Scene 3: Connecting the Threads

    void InitializeScene3()
    {
        CurrentState = SceneState.ConnectingThreads;
        Log("=== SCENE 3: Connecting the Threads ===");

        // Show narration (text)
        if (HarmonyUIManager.Instance != null)
        {
            HarmonyUIManager.Instance.ShowNarration("Connect the threads of culture...", 5f);
            HarmonyUIManager.Instance.ShowConnectionCounter(true);
        }

        // Play narration audio
        if (narrationData != null && narrationData.scene3Opening != null && HarmonyAudioManager.Instance != null)
        {
            HarmonyAudioManager.Instance.PlayNarration(narrationData.scene3Opening, narrationData.scene3Delay);
        }

        // CRITICAL: Disable Near-Far Interactor to prevent it from interfering with threading
        DisableNearFarInteractors();

        // Thread system becomes active (handled by ThreadConnectionSystem.Update)
        // No auto-progression - waits for minimum connections
    }

    void DisableNearFarInteractors()
    {
        // Find all Near-Far Interactors in the scene and disable them
        var nearFarInteractors = FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.Interactors.NearFarInteractor>();
        foreach (var interactor in nearFarInteractors)
        {
            interactor.enabled = false;
            Debug.Log($"Disabled Near-Far Interactor: {interactor.gameObject.name}");
        }
    }

    void EnableNearFarInteractors()
    {
        // Re-enable Near-Far Interactors for other scenes
        var nearFarInteractors = FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.Interactors.NearFarInteractor>(true); // Include inactive
        foreach (var interactor in nearFarInteractors)
        {
            interactor.enabled = true;
            Debug.Log($"Enabled Near-Far Interactor: {interactor.gameObject.name}");
        }
    }

    #endregion

    #region Scene 4: The Tapestry of Unity

    void InitializeScene4()
    {
        CurrentState = SceneState.TapestryOfUnity;
        Log("=== SCENE 4: The Tapestry of Unity ===");

        // Re-enable Near-Far Interactors (no longer need threading)
        EnableNearFarInteractors();

        // Hide connection counter
        if (HarmonyUIManager.Instance != null)
        {
            HarmonyUIManager.Instance.ShowConnectionCounter(false);
            HarmonyUIManager.Instance.ShowNarration("A tapestry of unity emerges...", 5f);
        }

        // Play narration audio
        if (narrationData != null && narrationData.scene4Opening != null && HarmonyAudioManager.Instance != null)
        {
            HarmonyAudioManager.Instance.PlayNarration(narrationData.scene4Opening, narrationData.scene4Delay);
        }

        // Calculate center point for canopy
        Vector3 centerPoint = CalculateCenterPoint();
        centerPoint += Vector3.up * 3f; // Raise above orbs

        // Animate all threads to canopy
        if (ThreadConnectionSystem.Instance != null)
        {
            var threads = ThreadConnectionSystem.Instance.GetAllConnections();
            foreach (var thread in threads)
            {
                if (thread != null)
                {
                    thread.AnimateToCanopy(centerPoint, 3f);
                }
            }
        }

        // Create center orb (optional - visual focal point)
        CreateCenterOrb(centerPoint);

        // Auto-progress to Scene 5
        autoProgressCoroutine = StartCoroutine(AutoProgressToScene5());
    }

    Vector3 CalculateCenterPoint()
    {
        if (instantiatedOrbs == null || instantiatedOrbs.Length == 0)
            return Vector3.zero;

        Vector3 center = Vector3.zero;
        int count = 0;

        foreach (var orb in instantiatedOrbs)
        {
            if (orb != null && orb.gameObject.activeSelf)
            {
                center += orb.transform.position;
                count++;
            }
        }

        return count > 0 ? center / count : Vector3.zero;
    }

    void CreateCenterOrb(Vector3 position)
    {
        // Create a large glowing sphere at the center
        GameObject centerOrb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        centerOrb.name = "CenterOrb";
        centerOrb.transform.position = position;
        centerOrb.transform.localScale = Vector3.one * 0.5f;

        // Make it glow (simple emissive material)
        Renderer renderer = centerOrb.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", Color.white * 2f);
            renderer.material = mat;
        }

        // Gentle pulsing animation
        StartCoroutine(PulseCenterOrb(centerOrb.transform));

        // Destroy after scene ends
        Destroy(centerOrb, scene4Duration + 1f);
    }

    IEnumerator PulseCenterOrb(Transform orb)
    {
        Vector3 baseScale = orb.localScale;
        float time = 0f;

        while (orb != null)
        {
            float scale = 1f + Mathf.Sin(time * 2f) * 0.2f;
            orb.localScale = baseScale * scale;
            time += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator AutoProgressToScene5()
    {
        yield return new WaitForSeconds(scene4Duration);
        TransitionToState(SceneState.Reflection);
    }

    #endregion

    #region Scene 5: Reflection

    void InitializeScene5()
    {
        CurrentState = SceneState.Reflection;
        Log("=== SCENE 5: Reflection Beneath the Light ===");

        // Show closing narration (text)
        if (HarmonyUIManager.Instance != null)
        {
            HarmonyUIManager.Instance.ShowNarration("Together, we weave the colors of humanity...", 7f);
        }

        // Play closing narration audio
        if (narrationData != null && narrationData.scene5Closing != null && HarmonyAudioManager.Instance != null)
        {
            HarmonyAudioManager.Instance.PlayNarration(narrationData.scene5Closing, narrationData.scene5Delay);
        }

        // Animate orbs rising and fading
        if (instantiatedOrbs != null)
        {
            foreach (var orb in instantiatedOrbs)
            {
                if (orb != null && orb.gameObject.activeSelf)
                {
                    StartCoroutine(OrbAscension(orb));
                }
            }
        }

        // Show end UI after delay
        StartCoroutine(ShowEndUI());
    }

    IEnumerator OrbAscension(CulturalOrb orb)
    {
        float elapsed = 0f;
        Vector3 startPos = orb.transform.position;
        Vector3 endPos = startPos + Vector3.up * orbAscensionHeight;

        while (elapsed < orbAscensionDuration)
        {
            float t = elapsed / orbAscensionDuration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            orb.transform.position = Vector3.Lerp(startPos, endPos, smoothT);

            // TODO: Fade material alpha if needed
            // This requires shader support for transparency

            elapsed += Time.deltaTime;
            yield return null;
        }

        orb.gameObject.SetActive(false);
    }

    IEnumerator ShowEndUI()
    {
        yield return new WaitForSeconds(10f);

        if (HarmonyUIManager.Instance != null)
        {
            HarmonyUIManager.Instance.ShowEndScreen();
        }
    }

    #endregion

    #region Public Methods

    public void TransitionToState(SceneState newState)
    {
        // Stop any auto-progress coroutine
        if (autoProgressCoroutine != null)
        {
            StopCoroutine(autoProgressCoroutine);
            autoProgressCoroutine = null;
        }

        // Transition to new state
        switch (newState)
        {
            case SceneState.CourtyardAwakens:
                InitializeScene1();
                break;
            case SceneState.VoicesRise:
                InitializeScene2();
                break;
            case SceneState.ConnectingThreads:
                InitializeScene3();
                break;
            case SceneState.TapestryOfUnity:
                InitializeScene4();
                break;
            case SceneState.Reflection:
                InitializeScene5();
                break;
        }
    }

    public void RestartExperience()
    {
        Log("=== RESTARTING EXPERIENCE ===");

        // Destroy all instantiated orbs
        if (instantiatedOrbs != null)
        {
            foreach (var orb in instantiatedOrbs)
            {
                if (orb != null)
                {
                    Destroy(orb.gameObject);
                }
            }
            // Clear the array
            instantiatedOrbs = new CulturalOrb[culturalOrbPrefabs != null ? culturalOrbPrefabs.Length : 0];
        }

        // Clear all thread connections
        if (ThreadConnectionSystem.Instance != null)
        {
            ThreadConnectionSystem.Instance.ClearAllConnections();
        }

        // Restart from Scene 1
        TransitionToState(SceneState.CourtyardAwakens);
    }

    #endregion

    void Log(string message)
    {
        if (debugMode)
        {
            Debug.Log($"[HarmonySceneManager] {message}");
        }
    }
}
