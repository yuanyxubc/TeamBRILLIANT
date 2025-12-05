using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main controller for Scene 2.5 - Campus Exploration
/// Manages orb transitions, player discovery, and scene flow
/// </summary>
public class CampusExplorationManager : MonoBehaviour
{
    public static CampusExplorationManager Instance;

    [Header("Configuration")]
    [Tooltip("All building destinations in the scene (assign in inspector)")]
    public BuildingDestination[] buildingDestinations;

    [Tooltip("Distance at which player discovers an orb (meters)")]
    public float discoveryRadius = 5f;

    [Tooltip("Time to wait after faculty bio displayed before orb returns")]
    public float bioDisplayDuration = 15f;

    [Header("References")]
    [Tooltip("Player transform (usually XR Origin or Main Camera)")]
    public Transform playerTransform;

    [Tooltip("Reference to UI controller")]
    public CampusExplorationUI explorationUI;

    [Header("Audio")]
    [Tooltip("Narration played at scene start")]
    public AudioClip sceneEntryNarration;

    [Tooltip("Narration played when all orbs discovered")]
    public AudioClip sceneExitNarration;

    [Tooltip("Sound played when orb is discovered")]
    public AudioClip discoveryChime;

    [Tooltip("Sound played when all orbs found")]
    public AudioClip allDiscoveredSound;

    [Header("Debug")]
    public bool debugMode = true;

    // Runtime state
    private Dictionary<CulturalOrb, OrbBuildingTransition> orbTransitions;
    private Dictionary<CulturalOrb, BuildingDestination> orbBuildingMap;
    private HashSet<CulturalOrb> discoveredOrbs;
    private HashSet<CulturalOrb> returnedOrbs;
    private bool isSceneActive = false;
    private bool allOrbsDiscovered = false;

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

        // Initialize collections
        orbTransitions = new Dictionary<CulturalOrb, OrbBuildingTransition>();
        orbBuildingMap = new Dictionary<CulturalOrb, BuildingDestination>();
        discoveredOrbs = new HashSet<CulturalOrb>();
        returnedOrbs = new HashSet<CulturalOrb>();
    }

    void Start()
    {
        // Auto-find player if not assigned
        if (playerTransform == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                playerTransform = mainCam.transform;
                Log("Auto-assigned player transform to Main Camera");
            }
            else
            {
                Debug.LogError("CampusExplorationManager: No player transform assigned and Main Camera not found!");
            }
        }

        // Validate UI reference
        if (explorationUI == null)
        {
            explorationUI = FindObjectOfType<CampusExplorationUI>();
            if (explorationUI != null)
            {
                Log("Auto-found CampusExplorationUI component");
            }
        }
    }

    /// <summary>
    /// Initialize the Campus Exploration scene
    /// Called by HarmonySceneManager when transitioning to Scene 2.5
    /// </summary>
    public void InitializeScene()
    {
        isSceneActive = true;
        allOrbsDiscovered = false;

        Log("=== Initializing Campus Exploration Scene ===");

        // Validate building destinations
        if (buildingDestinations == null || buildingDestinations.Length == 0)
        {
            Debug.LogError("CampusExplorationManager: No building destinations assigned!");
            return;
        }

        // Play entry narration
        if (HarmonyAudioManager.Instance != null && sceneEntryNarration != null)
        {
            HarmonyAudioManager.Instance.PlayNarration(sceneEntryNarration, 0f);
        }

        // Find all instantiated cultural orbs in the scene
        CulturalOrb[] orbs = FindObjectsOfType<CulturalOrb>();

        if (orbs.Length == 0)
        {
            Debug.LogError("CampusExplorationManager: No cultural orbs found in scene!");
            return;
        }

        Log($"Found {orbs.Length} cultural orbs in scene");

        // Map orbs to buildings and create transitions
        int mappingCount = Mathf.Min(orbs.Length, buildingDestinations.Length);

        for (int i = 0; i < mappingCount; i++)
        {
            CulturalOrb orb = orbs[i];
            BuildingDestination building = buildingDestinations[i];

            // Find building transform by name in scene
            GameObject buildingObj = GameObject.Find(building.buildingMarkerName);
            if (buildingObj == null)
            {
                Debug.LogError($"Building marker '{building.buildingMarkerName}' not found in scene! Please create a GameObject with this exact name.");
                continue;
            }

            // Store the found transform
            building.buildingTransform = buildingObj.transform;
            Log($"Found building marker: {building.buildingMarkerName} at position {building.buildingTransform.position}");

            // Store mapping
            orbBuildingMap[orb] = building;

            // Create and attach transition component
            OrbBuildingTransition transition = orb.gameObject.AddComponent<OrbBuildingTransition>();
            transition.Initialize(orb, orb.transform.position, building.buildingTransform.position);
            orbTransitions[orb] = transition;

            // Start transition to building
            transition.TransitionToBuilding();

            Log($"Mapped {orb.data.cultureName} orb to {building.buildingName}");
        }

        // Initialize UI
        if (explorationUI != null)
        {
            explorationUI.Initialize(mappingCount);
            explorationUI.ShowWaypoints(orbBuildingMap);
        }
        else
        {
            Debug.LogWarning("CampusExplorationManager: No UI controller found!");
        }

        Log($"Campus Exploration initialized with {mappingCount} orb-building pairs");
    }

    void Update()
    {
        if (!isSceneActive || allOrbsDiscovered)
            return;

        if (playerTransform == null)
            return;

        // Check proximity to each undiscovered orb
        foreach (var kvp in orbBuildingMap)
        {
            CulturalOrb orb = kvp.Key;
            BuildingDestination building = kvp.Value;

            // Skip if already discovered
            if (discoveredOrbs.Contains(orb))
                continue;

            // Check distance to player
            float distance = Vector3.Distance(playerTransform.position, orb.transform.position);

            if (distance <= discoveryRadius)
            {
                DiscoverOrb(orb, building);
            }
        }
    }

    private void DiscoverOrb(CulturalOrb orb, BuildingDestination building)
    {
        discoveredOrbs.Add(orb);

        Log($"Discovered {building.buildingName} - {building.facultyName}");

        // Visual feedback - pulse effect
        orb.TriggerPulseEffect();

        // Audio feedback
        if (HarmonyAudioManager.Instance != null)
        {
            // Discovery chime
            if (discoveryChime != null)
            {
                HarmonyAudioManager.Instance.PlaySFX(discoveryChime, 0.8f);
            }

            // Cultural greeting
            if (orb.data != null && orb.data.greetingAudio != null)
            {
                orb.PlayGreeting();
            }

            // Faculty voice (if available)
            if (building.facultyVoiceClip != null)
            {
                HarmonyAudioManager.Instance.PlaySFX(building.facultyVoiceClip, 1f);
            }
        }

        // Show faculty profile UI
        if (explorationUI != null)
        {
            explorationUI.ShowFacultyProfile(building);
            explorationUI.UpdateDiscoveryCounter(discoveredOrbs.Count, buildingDestinations.Length);
        }

        // Start return timer
        StartCoroutine(ReturnOrbAfterDelay(orb, building));
    }

    private IEnumerator ReturnOrbAfterDelay(CulturalOrb orb, BuildingDestination building)
    {
        yield return new WaitForSeconds(bioDisplayDuration);

        // Hide faculty profile
        if (explorationUI != null)
        {
            explorationUI.HideFacultyProfile();
        }

        // Trigger orb return
        if (orbTransitions.ContainsKey(orb))
        {
            orbTransitions[orb].TransitionToCourtyard();
        }
    }

    /// <summary>
    /// Called by OrbBuildingTransition when an orb completes its return
    /// </summary>
    public void NotifyOrbReturned(CulturalOrb orb)
    {
        returnedOrbs.Add(orb);

        Log($"Orb returned: {returnedOrbs.Count}/{discoveredOrbs.Count}");

        // Check if all orbs discovered and returned
        if (returnedOrbs.Count == buildingDestinations.Length)
        {
            CompleteScene();
        }
    }

    private void CompleteScene()
    {
        allOrbsDiscovered = true;
        isSceneActive = false;

        Log("All orbs discovered and returned - completing scene");

        // Play completion sound
        if (HarmonyAudioManager.Instance != null && allDiscoveredSound != null)
        {
            HarmonyAudioManager.Instance.PlaySFX(allDiscoveredSound, 1f);
        }

        // Show completion message
        if (explorationUI != null)
        {
            explorationUI.ShowCompletionMessage();
            explorationUI.HideWaypoints();
        }

        // Play exit narration
        if (HarmonyAudioManager.Instance != null && sceneExitNarration != null)
        {
            HarmonyAudioManager.Instance.PlayNarration(sceneExitNarration, 1f);
        }

        // Transition to Scene 3 after delay
        StartCoroutine(TransitionToScene3());
    }

    private IEnumerator TransitionToScene3()
    {
        // Wait for narration and celebration
        yield return new WaitForSeconds(5f);

        // Hide all UI elements before transitioning
        if (explorationUI != null)
        {
            explorationUI.HideAllUI();
        }

        // Clean up transition components
        foreach (var transition in orbTransitions.Values)
        {
            if (transition != null)
            {
                Destroy(transition);
            }
        }
        orbTransitions.Clear();

        Log("Transitioning to Scene 3 (Connecting Threads)");

        // Transition to original Scene 3
        if (HarmonySceneManager.Instance != null)
        {
            HarmonySceneManager.Instance.TransitionToState(SceneState.ConnectingThreads);
        }
    }

    /// <summary>
    /// Clean up scene state (called when restarting experience)
    /// </summary>
    public void CleanupScene()
    {
        isSceneActive = false;
        allOrbsDiscovered = false;
        discoveredOrbs.Clear();
        returnedOrbs.Clear();
        orbTransitions.Clear();
        orbBuildingMap.Clear();

        Log("Campus Exploration scene cleaned up");
    }

    private void Log(string message)
    {
        if (debugMode)
        {
            Debug.Log($"[CampusExploration] {message}");
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
