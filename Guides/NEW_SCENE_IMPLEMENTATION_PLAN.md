# Implementation Plan: New Scene 2.5 - "Campus Exploration"
## Injecting Building Exploration Between Scene 2 and Scene 3

---

## 📋 TABLE OF CONTENTS
1. [Overview & Scene Flow](#overview--scene-flow)
2. [Technical Architecture](#technical-architecture)
3. [Building & Faculty Mapping](#building--faculty-mapping)
4. [Component Design](#component-design)
5. [UI System Design](#ui-system-design)
6. [Audio Design](#audio-design)
7. [Movement & Navigation](#movement--navigation)
8. [State Management](#state-management)
9. [Visual Effects](#visual-effects)
10. [Implementation Roadmap](#implementation-roadmap)
11. [Testing Strategy](#testing-strategy)
12. [Asset Requirements](#asset-requirements)

---

## 1. OVERVIEW & SCENE FLOW

### Original Flow
```
Scene 1: Courtyard Awakens
    ↓
Scene 2: Voices Rise (orbs spawn, interactions)
    ↓
Scene 3: Connecting Threads (threading mechanic)
    ↓
Scene 4: Tapestry of Unity
    ↓
Scene 5: Reflection
```

### New Flow
```
Scene 1: Courtyard Awakens
    ↓
Scene 2: Voices Rise (orbs spawn, interactions)
    ↓
[NEW] Scene 2.5: Campus Exploration (orbs go to buildings)
    ↓
Scene 3: Connecting Threads (threading mechanic)
    ↓
Scene 4: Tapestry of Unity
    ↓
Scene 5: Reflection
```

### Scene 2.5 Detailed Flow

```
1. TRANSITION IN (from Scene 2)
   - Narration: "Each culture finds a home in UBCO's academic buildings..."
   - All 5 orbs simultaneously start moving to their assigned buildings
   - Orbs leave light trails as they move
   - Camera briefly follows orbs to show destinations

2. EXPLORATION PHASE
   - Player is free to move around campus (locomotion enabled)
   - UI waypoint markers show orb locations on buildings
   - Player walks/teleports to each building
   - When player within proximity (3-5 meters):
     → Orb pulses brightly
     → Faculty/staff profile appears (photo + bio)
     → Orb plays cultural greeting audio
     → After viewing delay (10-15 seconds), orb flies back to courtyard

3. TRACKING & COMPLETION
   - UI shows: "Discovered: X / 5 buildings"
   - Each discovered orb returns to original Scene 2 position
   - When all 5 orbs discovered and returned:
     → Congratulatory message
     → Automatic transition to Scene 3

4. TRANSITION OUT (to Scene 3)
   - All orbs back in courtyard formation
   - Narration: "Now that you've explored UBCO's diversity across campus,
                 it's time to weave these connections together..."
   - Seamless transition to original Scene 3
```

---

## 2. TECHNICAL ARCHITECTURE

### 2.1 New SceneState Enum

**File:** `Assets/Scripts/HarmonyInDiversity/Core/SceneState.cs`

```csharp
public enum SceneState
{
    CourtyardAwakens,       // Scene 1
    VoicesRise,             // Scene 2
    CampusExploration,      // NEW - Scene 2.5
    ConnectingThreads,      // Scene 3 (formerly)
    TapestryOfUnity,        // Scene 4
    Reflection              // Scene 5
}
```

**Impact:** This is the only change to existing enum. All existing scenes shift down conceptually but code references remain the same.

---

### 2.2 New Core Components

#### Component 1: `BuildingDestination.cs`
**Purpose:** Defines a UBCO building with position, orb assignment, and faculty data

```csharp
[CreateAssetMenu(fileName = "BuildingDestination", menuName = "Harmony/Building Destination")]
public class BuildingDestination : ScriptableObject
{
    [Header("Building Information")]
    public string buildingName;              // e.g., "Arts & Sciences Building"
    public string buildingCode;              // e.g., "ASC"
    public Vector3 buildingPosition;         // World position in scene
    public Transform buildingTransform;      // Reference to building GameObject

    [Header("Orb Assignment")]
    public CulturalOrbData assignedOrb;     // Which orb goes here

    [Header("Faculty Profile")]
    public Sprite facultyPhoto;              // Photo of professor/staff
    public string facultyName;               // e.g., "Dr. Jane Smith"
    public string facultyTitle;              // e.g., "Professor of Engineering"
    public string facultyDepartment;         // e.g., "School of Engineering"
    [TextArea(3, 6)]
    public string facultyBio;                // Short bio (2-3 sentences)
    public string culturalConnection;        // How they connect to this culture

    [Header("Audio")]
    public AudioClip facultyVoiceClip;       // Optional: faculty greeting
    public AudioClip ambientBuildingSound;   // Optional: building-specific ambient sound
}
```

**Why:** Encapsulates all building-related data in one reusable ScriptableObject

---

#### Component 2: `OrbBuildingTransition.cs`
**Purpose:** Handles orb movement to/from buildings

```csharp
public class OrbBuildingTransition : MonoBehaviour
{
    [Header("Transition Settings")]
    public float transitionDuration = 5f;     // Time to reach building
    public AnimationCurve transitionCurve;    // Easing curve for movement
    public float orbitHeight = 2f;            // Height orb orbits at building
    public float orbitSpeed = 0.5f;           // Rotation speed while orbiting

    [Header("Trail Effect")]
    public TrailRenderer trailRenderer;       // Light trail during flight
    public ParticleSystem flightParticles;    // Particles during flight

    private CulturalOrb orb;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isAtBuilding = false;
    private bool isReturning = false;

    public void Initialize(CulturalOrb culturalOrb, Vector3 startPos, Vector3 targetPos)
    {
        orb = culturalOrb;
        originalPosition = startPos;
        targetPosition = targetPos;
    }

    public void TransitionToBuilding()
    {
        StartCoroutine(MoveToBuilding());
    }

    public void TransitionToCourtyard()
    {
        isReturning = true;
        StartCoroutine(ReturnToCourtyard());
    }

    private IEnumerator MoveToBuilding()
    {
        EnableTrailEffect(true);

        float elapsed = 0f;
        Vector3 startPos = orb.transform.position;
        Vector3 endPos = targetPosition + Vector3.up * orbitHeight;

        while (elapsed < transitionDuration)
        {
            float t = elapsed / transitionDuration;
            float curveT = transitionCurve.Evaluate(t);

            // Arc trajectory (higher in middle)
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, curveT);
            float arcHeight = Mathf.Sin(t * Mathf.PI) * 5f; // 5m arc
            currentPos.y += arcHeight;

            orb.transform.position = currentPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        orb.transform.position = endPos;
        isAtBuilding = true;
        EnableTrailEffect(false);

        // Start orbiting
        StartCoroutine(OrbitBuilding());
    }

    private IEnumerator OrbitBuilding()
    {
        Vector3 orbitCenter = targetPosition + Vector3.up * orbitHeight;
        float angle = 0f;

        while (isAtBuilding && !isReturning)
        {
            angle += orbitSpeed * Time.deltaTime;

            // Circular orbit
            Vector3 offset = new Vector3(
                Mathf.Cos(angle) * 1.5f,
                0,
                Mathf.Sin(angle) * 1.5f
            );

            orb.transform.position = orbitCenter + offset;

            yield return null;
        }
    }

    private IEnumerator ReturnToCourtyard()
    {
        isAtBuilding = false;
        EnableTrailEffect(true);

        float elapsed = 0f;
        Vector3 startPos = orb.transform.position;

        while (elapsed < transitionDuration)
        {
            float t = elapsed / transitionDuration;
            float curveT = transitionCurve.Evaluate(t);

            Vector3 currentPos = Vector3.Lerp(startPos, originalPosition, curveT);
            float arcHeight = Mathf.Sin(t * Mathf.PI) * 3f;
            currentPos.y += arcHeight;

            orb.transform.position = currentPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        orb.transform.position = originalPosition;
        EnableTrailEffect(false);
        isReturning = false;

        // Notify manager that return is complete
        CampusExplorationManager.Instance.NotifyOrbReturned(orb);
    }

    private void EnableTrailEffect(bool enabled)
    {
        if (trailRenderer != null)
            trailRenderer.enabled = enabled;

        if (flightParticles != null)
        {
            if (enabled)
                flightParticles.Play();
            else
                flightParticles.Stop();
        }
    }
}
```

---

#### Component 3: `CampusExplorationManager.cs`
**Purpose:** Main controller for Scene 2.5

```csharp
public class CampusExplorationManager : MonoBehaviour
{
    public static CampusExplorationManager Instance;

    [Header("Configuration")]
    [Tooltip("All building destinations in scene")]
    public BuildingDestination[] buildingDestinations;

    [Tooltip("Proximity distance to trigger discovery (meters)")]
    public float discoveryRadius = 5f;

    [Tooltip("Time to wait after faculty bio displayed before orb returns")]
    public float bioDisplayDuration = 15f;

    [Header("References")]
    public Transform playerTransform;        // XR Origin / Main Camera
    public CampusExplorationUI explorationUI;

    [Header("Audio")]
    public AudioClip sceneEntryNarration;
    public AudioClip sceneExitNarration;
    public AudioClip discoveryChime;         // Plays when orb discovered
    public AudioClip allDiscoveredSound;     // Plays when all orbs found

    // Runtime state
    private Dictionary<CulturalOrb, OrbBuildingTransition> orbTransitions;
    private Dictionary<CulturalOrb, BuildingDestination> orbBuildingMap;
    private HashSet<CulturalOrb> discoveredOrbs;
    private HashSet<CulturalOrb> returnedOrbs;
    private bool isSceneActive = false;
    private bool allOrbsDiscovered = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        orbTransitions = new Dictionary<CulturalOrb, OrbBuildingTransition>();
        orbBuildingMap = new Dictionary<CulturalOrb, BuildingDestination>();
        discoveredOrbs = new HashSet<CulturalOrb>();
        returnedOrbs = new HashSet<CulturalOrb>();
    }

    public void InitializeScene()
    {
        isSceneActive = true;

        // Play entry narration
        if (HarmonyAudioManager.Instance != null && sceneEntryNarration != null)
        {
            HarmonyAudioManager.Instance.PlayNarration(sceneEntryNarration);
        }

        // Get instantiated orbs from HarmonySceneManager
        CulturalOrb[] orbs = FindObjectsOfType<CulturalOrb>();

        // Map orbs to buildings and initiate transitions
        for (int i = 0; i < Mathf.Min(orbs.Length, buildingDestinations.Length); i++)
        {
            CulturalOrb orb = orbs[i];
            BuildingDestination building = buildingDestinations[i];

            // Store mapping
            orbBuildingMap[orb] = building;

            // Create transition component
            OrbBuildingTransition transition = orb.gameObject.AddComponent<OrbBuildingTransition>();
            transition.Initialize(orb, orb.transform.position, building.buildingPosition);
            orbTransitions[orb] = transition;

            // Start transition to building
            transition.TransitionToBuilding();
        }

        // Update UI
        explorationUI.Initialize(buildingDestinations.Length);
        explorationUI.ShowWaypoints(orbBuildingMap);

        Debug.Log($"Campus Exploration Scene initialized with {orbs.Length} orbs");
    }

    void Update()
    {
        if (!isSceneActive || allOrbsDiscovered)
            return;

        // Check proximity to each undiscovered orb
        foreach (var kvp in orbBuildingMap)
        {
            CulturalOrb orb = kvp.Key;
            BuildingDestination building = kvp.Value;

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

        Debug.Log($"Discovered {building.buildingName} with {building.facultyName}");

        // Visual feedback
        orb.TriggerPulseEffect();

        // Audio feedback
        if (HarmonyAudioManager.Instance != null)
        {
            HarmonyAudioManager.Instance.PlaySFX(discoveryChime, 0.8f);

            // Play cultural greeting
            if (orb.data != null && orb.data.greetingAudio != null)
            {
                orb.PlayGreeting();
            }

            // Play faculty voice clip if available
            if (building.facultyVoiceClip != null)
            {
                HarmonyAudioManager.Instance.PlaySFX(building.facultyVoiceClip, 1f);
            }
        }

        // Show faculty profile UI
        explorationUI.ShowFacultyProfile(building);

        // Update counter
        explorationUI.UpdateDiscoveryCounter(discoveredOrbs.Count, buildingDestinations.Length);

        // Start return timer
        StartCoroutine(ReturnOrbAfterDelay(orb, building));
    }

    private IEnumerator ReturnOrbAfterDelay(CulturalOrb orb, BuildingDestination building)
    {
        yield return new WaitForSeconds(bioDisplayDuration);

        // Hide faculty profile
        explorationUI.HideFacultyProfile();

        // Trigger orb return
        if (orbTransitions.ContainsKey(orb))
        {
            orbTransitions[orb].TransitionToCourtyard();
        }
    }

    public void NotifyOrbReturned(CulturalOrb orb)
    {
        returnedOrbs.Add(orb);

        Debug.Log($"Orb returned. {returnedOrbs.Count}/{discoveredOrbs.Count} back in courtyard");

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

        Debug.Log("All orbs discovered and returned - completing scene");

        // Play completion sound
        if (HarmonyAudioManager.Instance != null && allDiscoveredSound != null)
        {
            HarmonyAudioManager.Instance.PlaySFX(allDiscoveredSound, 1f);
        }

        // Show completion message
        explorationUI.ShowCompletionMessage();

        // Play exit narration
        if (HarmonyAudioManager.Instance != null && sceneExitNarration != null)
        {
            HarmonyAudioManager.Instance.PlayNarration(sceneExitNarration);
        }

        // Hide waypoints
        explorationUI.HideWaypoints();

        // Transition to Scene 3 after delay
        StartCoroutine(TransitionToScene3());
    }

    private IEnumerator TransitionToScene3()
    {
        yield return new WaitForSeconds(3f);

        // Clean up transition components
        foreach (var transition in orbTransitions.Values)
        {
            if (transition != null)
                Destroy(transition);
        }
        orbTransitions.Clear();

        // Transition to original Scene 3
        if (HarmonySceneManager.Instance != null)
        {
            HarmonySceneManager.Instance.TransitionToState(SceneState.ConnectingThreads);
        }
    }

    public void CleanupScene()
    {
        isSceneActive = false;
        discoveredOrbs.Clear();
        returnedOrbs.Clear();
        orbTransitions.Clear();
        orbBuildingMap.Clear();
    }
}
```

---

## 3. BUILDING & FACULTY MAPPING

### 3.1 Suggested UBCO Building → Culture → Faculty Mapping

| Orb Culture | Building | Faculty/Staff Profile | Connection Rationale |
|-------------|----------|----------------------|---------------------|
| **Japanese (Red)** | Engineering Building | Dr. [Name], Professor of Electrical Engineering | Research in Japanese robotics collaboration; spent sabbatical in Tokyo |
| **Indian (Green)** | Arts & Sciences Building | Dr. [Name], Professor of Computer Science | From Mumbai; leads AI research lab; mentors Indian students |
| **French (Blue)** | Library / International Building | [Name], International Student Advisor | Fluent in French; supports Francophone students; from Quebec |
| **Mexican (Yellow)** | Management Building | Dr. [Name], Professor of Economics | Research on Latin American markets; teaches Spanish business practices |
| **Nigerian (Purple)** | Sciences Building | Dr. [Name], Professor of Biology | From Lagos; focuses on African biodiversity; active in Nigerian student community |

**Note:** Replace with actual UBCO faculty members. Requires:
- Faculty consent and photo permissions
- Verification of cultural connections
- Ethics approval for using names/images

---

### 3.2 Building Position Setup in Unity

**Approach 1: Manual Transform Placement**
```
1. Place empty GameObjects in scene named "Building_Engineering", "Building_Arts", etc.
2. Position them at actual building locations relative to courtyard
3. Assign transforms to BuildingDestination ScriptableObjects
```

**Approach 2: GPS-Based (if courtyard is geo-accurate)**
```
- Use real UBCO building GPS coordinates
- Convert to Unity world space relative to courtyard origin
- Programmatically place orbs at calculated positions
```

**Recommended:** Manual placement for accuracy and art direction

---

## 4. COMPONENT DESIGN

### 4.1 File Structure

```
Assets/Scripts/HarmonyInDiversity/
├── Core/
│   ├── HarmonySceneManager.cs (MODIFIED)
│   └── SceneState.cs (MODIFIED)
├── CampusExploration/ (NEW FOLDER)
│   ├── CampusExplorationManager.cs (NEW)
│   ├── OrbBuildingTransition.cs (NEW)
│   ├── BuildingDestination.cs (NEW)
│   ├── CampusExplorationUI.cs (NEW)
│   ├── WaypointMarker.cs (NEW)
│   └── FacultyProfilePanel.cs (NEW)
```

---

### 4.2 Modified: `HarmonySceneManager.cs`

**Changes Required:**

```csharp
// Add new scene initialization method
#region Scene 2.5: Campus Exploration

void InitializeScene2_5()
{
    CurrentState = SceneState.CampusExploration;
    Log("=== SCENE 2.5: Campus Exploration ===");

    // Initialize campus exploration manager
    if (CampusExplorationManager.Instance != null)
    {
        CampusExplorationManager.Instance.InitializeScene();
    }
    else
    {
        Debug.LogError("CampusExplorationManager not found in scene!");
    }

    // No auto-progression - waits for all orbs to be discovered and returned
}

#endregion

// Modify Scene 2 to transition to Scene 2.5 instead of Scene 3
IEnumerator AutoProgressToScene3()
{
    yield return new WaitForSeconds(scene2Duration);

    // CHANGED: Now goes to CampusExploration instead of ConnectingThreads
    TransitionToState(SceneState.CampusExploration);
}

// Update TransitionToState switch statement
public void TransitionToState(SceneState newState)
{
    // ... existing code ...

    switch (newState)
    {
        case SceneState.CourtyardAwakens:
            InitializeScene1();
            break;
        case SceneState.VoicesRise:
            InitializeScene2();
            break;
        case SceneState.CampusExploration:  // NEW
            InitializeScene2_5();
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
```

---

## 5. UI SYSTEM DESIGN

### 5.1 Component: `CampusExplorationUI.cs`

```csharp
public class CampusExplorationUI : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject waypointContainer;        // Parent for waypoint markers
    public GameObject facultyProfilePanel;      // Faculty bio panel
    public TextMeshProUGUI discoveryCounterText;
    public GameObject completionMessagePanel;

    [Header("Faculty Profile Elements")]
    public Image facultyPhotoImage;
    public TextMeshProUGUI facultyNameText;
    public TextMeshProUGUI facultyTitleText;
    public TextMeshProUGUI facultyBioText;
    public TextMeshProUGUI buildingNameText;

    [Header("Waypoint Prefab")]
    public GameObject waypointMarkerPrefab;

    private Dictionary<CulturalOrb, WaypointMarker> activeWaypoints;

    void Awake()
    {
        activeWaypoints = new Dictionary<CulturalOrb, WaypointMarker>();

        // Initialize UI state
        if (facultyProfilePanel != null)
            facultyProfilePanel.SetActive(false);

        if (completionMessagePanel != null)
            completionMessagePanel.SetActive(false);
    }

    public void Initialize(int totalBuildings)
    {
        UpdateDiscoveryCounter(0, totalBuildings);
    }

    public void ShowWaypoints(Dictionary<CulturalOrb, BuildingDestination> orbBuildingMap)
    {
        foreach (var kvp in orbBuildingMap)
        {
            CulturalOrb orb = kvp.Key;
            BuildingDestination building = kvp.Value;

            // Create waypoint marker
            GameObject markerObj = Instantiate(waypointMarkerPrefab, waypointContainer.transform);
            WaypointMarker marker = markerObj.GetComponent<WaypointMarker>();

            if (marker != null)
            {
                marker.Initialize(orb, building);
                activeWaypoints[orb] = marker;
            }
        }
    }

    public void HideWaypoints()
    {
        foreach (var marker in activeWaypoints.Values)
        {
            if (marker != null)
                marker.Hide();
        }
        activeWaypoints.Clear();
    }

    public void ShowFacultyProfile(BuildingDestination building)
    {
        if (facultyProfilePanel == null)
            return;

        // Populate faculty info
        if (facultyPhotoImage != null)
            facultyPhotoImage.sprite = building.facultyPhoto;

        if (facultyNameText != null)
            facultyNameText.text = building.facultyName;

        if (facultyTitleText != null)
            facultyTitleText.text = building.facultyTitle;

        if (facultyBioText != null)
            facultyBioText.text = building.facultyBio;

        if (buildingNameText != null)
            buildingNameText.text = building.buildingName;

        // Show panel with fade-in
        facultyProfilePanel.SetActive(true);
        StartCoroutine(FadeInPanel(facultyProfilePanel));
    }

    public void HideFacultyProfile()
    {
        if (facultyProfilePanel != null)
        {
            StartCoroutine(FadeOutPanel(facultyProfilePanel));
        }
    }

    public void UpdateDiscoveryCounter(int discovered, int total)
    {
        if (discoveryCounterText != null)
        {
            discoveryCounterText.text = $"Buildings Discovered: {discovered} / {total}";
        }
    }

    public void ShowCompletionMessage()
    {
        if (completionMessagePanel != null)
        {
            completionMessagePanel.SetActive(true);
            TextMeshProUGUI messageText = completionMessagePanel.GetComponentInChildren<TextMeshProUGUI>();
            if (messageText != null)
            {
                messageText.text = "All cultures discovered across UBCO campus!\n\nReturning to courtyard...";
            }
        }
    }

    private IEnumerator FadeInPanel(GameObject panel)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = panel.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOutPanel(GameObject panel)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            yield break;

        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        panel.SetActive(false);
    }
}
```

---

### 5.2 Component: `WaypointMarker.cs`

```csharp
public class WaypointMarker : MonoBehaviour
{
    [Header("References")]
    public Image markerIcon;                // Icon showing orb color
    public TextMeshProUGUI distanceText;    // Shows distance to building
    public Transform targetTransform;       // Building location

    [Header("Settings")]
    public float updateInterval = 0.1f;     // Update distance every 0.1s

    private CulturalOrb associatedOrb;
    private BuildingDestination building;
    private Transform playerTransform;
    private bool isActive = true;

    public void Initialize(CulturalOrb orb, BuildingDestination buildingDest)
    {
        associatedOrb = orb;
        building = buildingDest;
        targetTransform = building.buildingTransform;

        // Set marker color to orb color
        if (markerIcon != null && orb.data != null)
        {
            markerIcon.color = orb.data.orbColor;
        }

        // Get player reference
        playerTransform = Camera.main.transform;

        // Start updating distance
        InvokeRepeating(nameof(UpdateDistance), 0f, updateInterval);
    }

    void Update()
    {
        if (!isActive || targetTransform == null || playerTransform == null)
            return;

        // Always face player
        transform.LookAt(playerTransform);
        transform.Rotate(0, 180, 0); // Flip to face player

        // Position in 3D space above building
        transform.position = targetTransform.position + Vector3.up * 5f;
    }

    private void UpdateDistance()
    {
        if (!isActive || targetTransform == null || playerTransform == null)
            return;

        float distance = Vector3.Distance(playerTransform.position, targetTransform.position);

        if (distanceText != null)
        {
            distanceText.text = $"{building.buildingName}\n{distance:F0}m";
        }
    }

    public void Hide()
    {
        isActive = false;
        CancelInvoke(nameof(UpdateDistance));
        gameObject.SetActive(false);
    }
}
```

---

## 6. AUDIO DESIGN

### 6.1 New Audio Clips Needed

| Audio Type | Description | Duration | Use Case |
|------------|-------------|----------|----------|
| **Scene Entry Narration** | "Each culture finds a home across UBCO's academic buildings. Follow the orbs and discover the faculty who shape our diverse community..." | 8-10s | Plays at start of Scene 2.5 |
| **Scene Exit Narration** | "Now that you've explored UBCO's diversity across campus, it's time to weave these connections together..." | 6-8s | Plays when all orbs returned |
| **Discovery Chime** | Gentle, uplifting chime sound | 1-2s | Plays when player discovers orb at building |
| **All Discovered Sound** | Triumphant, completion sound | 3-4s | Plays when all orbs discovered |
| **Orb Flight Whoosh** | Swooshing wind sound with cultural undertones | 3-5s (loop) | Plays during orb transitions |
| **Faculty Voice Clips** (optional) | Each faculty member: "Hello, I'm Dr. X. Welcome to [Building]..." | 5-10s each | Plays alongside faculty profile |

### 6.2 Audio Implementation

**In `CampusExplorationManager.cs`:**
```csharp
[Header("Audio")]
public AudioClip sceneEntryNarration;
public AudioClip sceneExitNarration;
public AudioClip discoveryChime;
public AudioClip allDiscoveredSound;
public AudioClip orbFlightWhoosh;        // NEW
```

**In `OrbBuildingTransition.cs`:**
```csharp
private AudioSource flightAudioSource;

private void EnableTrailEffect(bool enabled)
{
    // ... existing code ...

    // Flight sound effect
    if (flightAudioSource != null)
    {
        if (enabled)
            flightAudioSource.Play();
        else
            flightAudioSource.Stop();
    }
}
```

### 6.3 Spatial Audio Considerations

- **Faculty voice clips:** Play as 2D (non-spatial) since they're narration
- **Discovery chime:** Play as 3D spatial audio from orb position
- **Orb flight whoosh:** Play as 3D spatial audio attached to moving orb
- **Narration:** Play as 2D through HarmonyAudioManager

---

## 7. MOVEMENT & NAVIGATION

### 7.1 Player Locomotion Options

**Option 1: Continuous Movement (Recommended)**
- Enable continuous locomotion on XR Origin
- Player uses thumbstick to walk around campus
- More immersive but may cause VR sickness for some

**Implementation:**
```csharp
// Enable locomotion provider during Scene 2.5
void EnableLocomotion()
{
    var locomotionSystem = FindObjectOfType<LocomotionSystem>();
    if (locomotionSystem != null)
    {
        var continuousMove = locomotionSystem.GetComponent<ContinuousMoveProviderBase>();
        if (continuousMove != null)
            continuousMove.enabled = true;
    }
}

void DisableLocomotion()
{
    var locomotionSystem = FindObjectOfType<LocomotionSystem>();
    if (locomotionSystem != null)
    {
        var continuousMove = locomotionSystem.GetComponent<ContinuousMoveProviderBase>();
        if (continuousMove != null)
            continuousMove.enabled = false;
    }
}
```

**Option 2: Teleportation**
- Enable teleportation ray interactor
- Player points and teleports to building locations
- More comfortable for VR, less immersive

**Option 3: Hybrid (Best)**
- Allow both continuous and teleportation
- Player chooses comfort level
- Teleport areas at each building for quick travel

---

### 7.2 Waypoint Navigation System

**Visual Waypoint Markers:**
- Floating 3D icons above each building
- Color-coded to match orb color
- Show building name and distance
- Pulsing animation to draw attention

**Mini-Map (Optional):**
- 2D overhead map in corner of view
- Shows player position (blue dot)
- Shows orb positions (colored dots)
- Shows discovered buildings (grayed out)

---

## 8. STATE MANAGEMENT

### 8.1 Scene State Tracking

**Data to Track:**
```csharp
public class CampusExplorationState
{
    public int totalBuildings;
    public int discoveredCount;
    public int returnedCount;
    public List<string> discoveredBuildingNames;
    public float sceneStartTime;
    public float sceneCompletionTime;

    public bool IsComplete => returnedCount == totalBuildings;
}
```

**Persistence (Optional):**
- Save state to PlayerPrefs
- Allow resume if player quits mid-scene
- Track which buildings visited for analytics

---

### 8.2 Orb State Transitions

```
State 1: AT_COURTYARD (Scene 2)
    ↓ (Scene 2.5 starts)
State 2: TRANSITIONING_TO_BUILDING
    ↓ (Arrives at building)
State 3: ORBITING_BUILDING
    ↓ (Player approaches)
State 4: DISCOVERED (showing faculty profile)
    ↓ (Timer expires)
State 5: TRANSITIONING_TO_COURTYARD
    ↓ (Arrives back)
State 6: RETURNED_TO_COURTYARD
```

**Implementation:**
```csharp
public enum OrbExplorationState
{
    AtCourtyard,
    TransitioningToBuilding,
    OrbitingBuilding,
    Discovered,
    TransitioningToCourtyard,
    ReturnedToCourtyard
}

// Track state per orb
private Dictionary<CulturalOrb, OrbExplorationState> orbStates;
```

---

## 9. VISUAL EFFECTS

### 9.1 Orb Flight Trail

**TrailRenderer Settings:**
```csharp
TrailRenderer trail = orb.gameObject.AddComponent<TrailRenderer>();
trail.time = 2f;                          // 2 second trail
trail.startWidth = 0.3f;
trail.endWidth = 0.05f;
trail.material = glowMaterial;            // Emissive material
trail.startColor = orb.data.orbColor;
trail.endColor = new Color(orb.data.orbColor.r, orb.data.orbColor.g, orb.data.orbColor.b, 0f);
trail.enabled = false;                    // Only enable during flight
```

### 9.2 Discovery Visual Effects

**Particle Burst:**
```csharp
// When player discovers orb
ParticleSystem discoveryBurst = Instantiate(discoveryBurstPrefab, orb.transform.position, Quaternion.identity);
var main = discoveryBurst.main;
main.startColor = orb.data.orbColor;
discoveryBurst.Play();
Destroy(discoveryBurst.gameObject, 3f);
```

**Orb Pulse:**
```csharp
// Scale pulse animation (already exists in CulturalOrb)
orb.TriggerPulseEffect();
```

**Light Beam to Sky:**
```csharp
// Vertical light beam from orb when discovered
GameObject beam = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
beam.transform.position = orb.transform.position + Vector3.up * 10f;
beam.transform.localScale = new Vector3(0.5f, 20f, 0.5f);
// Apply emissive material with orb color
// Animate fade out over 3 seconds
```

### 9.3 Camera Guidance (Optional)

**Initial Orb Departure:**
```csharp
// When orbs first leave courtyard, briefly follow them with camera
IEnumerator CameraFollowOrbs()
{
    // Store original camera position
    Vector3 originalPos = Camera.main.transform.position;
    Quaternion originalRot = Camera.main.transform.rotation;

    // Smoothly look at orbs flying away
    float elapsed = 0f;
    float duration = 3f;

    while (elapsed < duration)
    {
        // Look towards average orb position
        Vector3 avgOrbPos = GetAverageOrbPosition();
        Camera.main.transform.LookAt(avgOrbPos);

        elapsed += Time.deltaTime;
        yield return null;
    }

    // Return camera control to player
    Camera.main.transform.position = originalPos;
    Camera.main.transform.rotation = originalRot;
}
```

---

## 10. IMPLEMENTATION ROADMAP

### Phase 1: Core Architecture (Days 1-2)

**Tasks:**
- [ ] Create `SceneState.CampusExploration` enum entry
- [ ] Create new folder: `Assets/Scripts/HarmonyInDiversity/CampusExploration/`
- [ ] Implement `BuildingDestination.cs` ScriptableObject
- [ ] Implement `OrbBuildingTransition.cs` component
- [ ] Implement `CampusExplorationManager.cs` core logic
- [ ] Modify `HarmonySceneManager.cs` to add Scene 2.5 initialization

**Testing:**
- Verify Scene 2 → 2.5 transition works
- Verify orbs can transition to test positions
- Verify Scene 2.5 → 3 transition works

**Deliverable:** Basic scene flow with orb transitions

---

### Phase 2: UI System (Days 3-4)

**Tasks:**
- [ ] Design faculty profile panel UI in Unity Canvas
- [ ] Implement `CampusExplorationUI.cs`
- [ ] Implement `WaypointMarker.cs`
- [ ] Create waypoint marker prefab
- [ ] Implement discovery counter UI
- [ ] Implement completion message UI

**Testing:**
- Verify faculty profile displays correctly
- Verify waypoints point to correct buildings
- Verify distance calculations are accurate
- Verify UI fades in/out smoothly

**Deliverable:** Complete UI system for exploration

---

### Phase 3: Building Setup & Faculty Data (Days 5-6)

**Tasks:**
- [ ] Research UBCO buildings and faculty for each culture
- [ ] Obtain faculty consent and photos (legal/ethics)
- [ ] Place building marker transforms in Unity scene
- [ ] Create 5 `BuildingDestination` ScriptableObjects
- [ ] Populate each with faculty data, photos, bios
- [ ] Assign orbs to buildings

**Testing:**
- Verify building positions are accurate relative to courtyard
- Verify each orb goes to correct building
- Verify faculty profiles display correct info

**Deliverable:** 5 complete building destinations with faculty profiles

---

### Phase 4: Audio Integration (Day 7)

**Tasks:**
- [ ] Write narration scripts (entry, exit)
- [ ] Record or source narration audio
- [ ] Record or source sound effects (chime, completion, whoosh)
- [ ] (Optional) Record faculty voice clips
- [ ] Integrate all audio into `CampusExplorationManager`
- [ ] Configure spatial audio for flight sounds

**Testing:**
- Verify narration plays at correct times
- Verify discovery sound plays on orb discovery
- Verify flight sounds follow orbs
- Verify audio doesn't overlap incorrectly

**Deliverable:** Complete audio experience

---

### Phase 5: Visual Effects & Polish (Days 8-9)

**Tasks:**
- [ ] Add TrailRenderer to orbs during flight
- [ ] Create discovery particle burst effect
- [ ] Add light beam effect on discovery (optional)
- [ ] Implement camera follow for orb departure (optional)
- [ ] Polish orb orbit animation
- [ ] Add glowing effects to discovered buildings

**Testing:**
- Verify trail appears during flight only
- Verify particles match orb colors
- Verify effects don't cause performance issues

**Deliverable:** Polished visual experience

---

### Phase 6: Locomotion & Navigation (Day 10)

**Tasks:**
- [ ] Enable continuous movement provider for Scene 2.5
- [ ] (Optional) Enable teleportation system
- [ ] Configure movement speed and comfort settings
- [ ] Add teleport areas at buildings (if using teleport)
- [ ] Test navigation on Quest 2/3 hardware

**Testing:**
- Verify player can reach all buildings
- Verify no collision issues
- Verify comfort (no VR sickness)
- Test movement speed is appropriate

**Deliverable:** Functional campus navigation

---

### Phase 7: Integration Testing (Days 11-12)

**Tasks:**
- [ ] Test complete flow: Scene 1 → 2 → 2.5 → 3 → 4 → 5
- [ ] Test discovering orbs in different orders
- [ ] Test edge cases (walking away during bio display, etc.)
- [ ] Performance testing on Quest 2/3
- [ ] Bug fixing
- [ ] Polish transitions

**Testing Scenarios:**
1. Discover all orbs in order (Japanese → French → Indian → Mexican → Nigerian)
2. Discover orbs in reverse order
3. Discover 2 orbs, walk away, return
4. Wait for all orbs to return before discovering some
5. Sprint through scene as fast as possible

**Deliverable:** Stable, polished Scene 2.5

---

### Phase 8: Documentation & Handoff (Day 13)

**Tasks:**
- [ ] Document Scene 2.5 architecture
- [ ] Create Unity scene setup guide
- [ ] Document how to add/modify buildings and faculty
- [ ] Create troubleshooting guide
- [ ] Record demo video

**Deliverable:** Complete documentation

---

## 11. TESTING STRATEGY

### 11.1 Unit Tests

**Test 1: Orb Transition**
```
Given: Orb at courtyard position
When: InitializeScene2_5() is called
Then: Orb transitions to assigned building within 5 seconds
```

**Test 2: Discovery Trigger**
```
Given: Player more than 5m from orb
When: Player moves within 3m of orb
Then: Faculty profile displays within 0.5 seconds
```

**Test 3: Return Trigger**
```
Given: Faculty profile displayed for 15 seconds
When: Bio timer expires
Then: Orb begins return transition within 1 second
```

**Test 4: Scene Completion**
```
Given: 4/5 orbs returned to courtyard
When: 5th orb returns to courtyard
Then: Completion message appears and Scene 3 starts within 3 seconds
```

---

### 11.2 Integration Tests

**Test 5: Full Scene Flow**
```
1. Start at Scene 2 (orbs in courtyard)
2. Wait for Scene 2 timer (30s)
3. Verify transition to Scene 2.5
4. Verify all orbs fly to buildings
5. Visit each building and discover orb
6. Wait for all orbs to return
7. Verify transition to Scene 3
8. Verify Scene 3 threading works normally
```

**Test 6: Skip/Fast-Forward**
```
- Rapidly discover all orbs within 30 seconds
- Verify no race conditions
- Verify all orbs return properly
- Verify Scene 3 starts correctly
```

---

### 11.3 Performance Tests

**Metrics to Monitor:**
- FPS during orb flight (target: 72 FPS on Quest 2)
- FPS with all waypoints visible (target: 72 FPS)
- Memory usage increase in Scene 2.5 (target: <50MB)
- Draw calls (target: <200)
- Loading time for faculty photos (target: <1s)

---

### 11.4 User Experience Tests

**Questions for Testers:**
1. Did you understand you needed to find the orbs at buildings?
2. Were the waypoint markers helpful?
3. Could you read the faculty bios comfortably in VR?
4. Was the movement speed comfortable (no VR sickness)?
5. Did the scene feel too long or too short?
6. Did you feel connected to UBCO campus after this scene?

---

## 12. ASSET REQUIREMENTS

### 12.1 3D Assets

| Asset | Description | Source |
|-------|-------------|--------|
| **Building Markers** | 5 empty GameObjects with transforms | Create in Unity |
| **Building Exteriors** (optional) | Low-poly 3D models of UBCO buildings | Model or source from asset store |
| **Waypoint Marker Prefab** | UI element with icon + text | Create in Unity Canvas |
| **Trail Material** | Emissive material for orb trails | Create in Unity |

---

### 12.2 2D Assets

| Asset | Description | Specifications | Source |
|-------|-------------|----------------|--------|
| **Faculty Photos** | Headshots of 5 faculty/staff | 512x512 px, PNG, transparent background optional | Photography or provided by faculty |
| **Building Icons** (optional) | Icons representing each building | 256x256 px, PNG, transparent background | Design or icon library |
| **Discovery Icon** | Icon for discovery achievement | 128x128 px, PNG | Design |
| **Waypoint Icon** | Marker shown above buildings | 128x128 px, PNG | Design |

---

### 12.3 Audio Assets

| Asset | File Format | Duration | Source |
|-------|-------------|----------|--------|
| **Scene Entry Narration** | WAV or OGG, 44.1kHz, 16-bit | 8-10s | Voice actor or text-to-speech |
| **Scene Exit Narration** | WAV or OGG, 44.1kHz, 16-bit | 6-8s | Voice actor or text-to-speech |
| **Discovery Chime** | WAV or OGG, 44.1kHz, 16-bit | 1-2s | Sound library or design |
| **All Discovered Sound** | WAV or OGG, 44.1kHz, 16-bit | 3-4s | Sound library or design |
| **Orb Flight Whoosh** | WAV or OGG, 44.1kHz, 16-bit | 3-5s loop | Sound library or design |
| **Faculty Voice Clips** (optional) | WAV or OGG, 44.1kHz, 16-bit | 5-10s each | Record from faculty |

---

### 12.4 Data Assets (ScriptableObjects)

| Asset | Quantity | Data Required |
|-------|----------|---------------|
| **BuildingDestination** | 5 | Building name, position, faculty data, audio clips |
| **NarrationData** | 1 (modify existing) | Add Scene 2.5 narration clips |

---

## 13. ADDITIONAL FEATURES & ENHANCEMENTS

### 13.1 Optional: Mini-Map

**Implementation:**
```csharp
public class MiniMapController : MonoBehaviour
{
    public RenderTexture miniMapTexture;
    public Camera miniMapCamera;          // Top-down orthographic camera
    public RawImage miniMapDisplay;       // UI element

    public Transform playerMarker;        // Player position indicator
    public GameObject orbMarkerPrefab;    // Orb position indicators

    void Update()
    {
        // Update player marker position
        UpdatePlayerMarker();

        // Update orb markers
        UpdateOrbMarkers();
    }
}
```

**Pros:**
- Helps players navigate large campus
- Shows spatial relationship between buildings
- Reduces confusion

**Cons:**
- Additional performance cost
- More UI clutter
- Requires top-down camera setup

**Recommendation:** Add if user testing shows navigation difficulties

---

### 13.2 Optional: Building Information Plaques

**Concept:** When player looks at building, show info plaque

**Implementation:**
```csharp
public class BuildingInfoPlaque : MonoBehaviour
{
    public BuildingDestination buildingData;
    public GameObject plaquePrefab;

    void Update()
    {
        // Raycast from player camera
        if (Physics.Raycast(Camera.main.transform.position,
                           Camera.main.transform.forward,
                           out RaycastHit hit, 50f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                // Player looking at building
                ShowPlaque();
            }
            else
            {
                HidePlaque();
            }
        }
    }
}
```

**Plaque Content:**
- Building name
- Year built
- Departments housed
- "An orb awaits you here..."

---

### 13.3 Optional: Achievement System

**Achievements:**
- "Explorer" - Discover all buildings
- "Speed Runner" - Complete scene in under 5 minutes
- "Thorough Reader" - Read all faculty bios completely
- "First Contact" - Discover your first building

**Implementation:**
```csharp
public class AchievementManager : MonoBehaviour
{
    public void UnlockAchievement(string achievementId)
    {
        // Display achievement toast UI
        // Save to PlayerPrefs
        // (Future) Sync to online leaderboard
    }
}
```

---

### 13.4 Optional: Photo Mode

**Feature:** Allow players to take screenshots at each building

**Implementation:**
```csharp
public class PhotoMode : MonoBehaviour
{
    public void CapturePhoto()
    {
        string filename = $"UBCO_Harmony_{DateTime.Now:yyyyMMdd_HHmmss}.png";
        ScreenCapture.CaptureScreenshot(filename);

        // Show "Photo saved!" message
        Debug.Log($"Photo saved: {filename}");
    }
}
```

**UI:** Add "Take Photo" button when near building

---

## 14. KNOWN CHALLENGES & SOLUTIONS

### Challenge 1: VR Sickness During Movement

**Problem:** Continuous locomotion can cause VR sickness

**Solutions:**
- Reduce movement speed (2-3 m/s max)
- Add vignette effect during movement
- Offer teleportation alternative
- Add snap turning instead of smooth turning
- Test extensively on real hardware

---

### Challenge 2: Campus Scale

**Problem:** Real UBCO campus may be too large for comfortable VR traversal

**Solutions:**
- **Option A:** Scale down campus (e.g., 50% size) while maintaining layout
- **Option B:** Use "fast travel" between buildings (teleport with loading screen)
- **Option C:** Artistic compression (buildings closer than reality)
- **Recommended:** Option A - Scale to 60-70% of real size

**Implementation:**
```csharp
// Scale all building positions
foreach (var building in buildingDestinations)
{
    building.buildingPosition = courtyardCenter + (building.buildingPosition - courtyardCenter) * 0.6f;
}
```

---

### Challenge 3: Faculty Consent & Privacy

**Problem:** Using real faculty names/photos requires consent

**Solutions:**
1. **Legal Route:**
   - Obtain written consent from each faculty member
   - Sign photo release forms
   - Get ethics approval from UBCO REB

2. **Alternative Route:**
   - Use fictional faculty members
   - Use stylized avatars instead of photos
   - Use "representative" descriptions without names

3. **Hybrid Route:**
   - Use titles/roles without names: "Engineering Professor, specializing in robotics"
   - Use silhouettes or illustrated portraits
   - Focus on cultural connection rather than individual identity

**Recommended:** Start with Alternative Route for MVP, pursue Legal Route if experience gets official UBCO backing

---

### Challenge 4: Performance with Multiple UI Elements

**Problem:** 5 waypoints + faculty profile + discovery counter = many UI elements

**Solutions:**
- Use object pooling for waypoint markers
- Disable distant waypoints (>50m away)
- Use LOD system for UI (simpler at distance)
- Optimize UI sprites (atlasing, compression)
- Limit to 3 visible waypoints at a time (nearest)

---

### Challenge 5: Orb Occlusion

**Problem:** Buildings may block view of orbs

**Solutions:**
- Orbs orbit above buildings (5-10m high)
- Add X-ray shader for occluded orbs (outline visible through walls)
- Waypoint markers always visible regardless of occlusion
- Add minimap showing orb positions

---

## 15. SUCCESS METRICS

### Quantitative Metrics

- **Completion Rate:** % of players who discover all 5 orbs
- **Average Completion Time:** Target 5-10 minutes
- **Discovery Order:** Which buildings visited first (analytics)
- **Movement Distance:** Total distance player traveled
- **Frame Rate:** Maintain 72 FPS on Quest 2

### Qualitative Metrics

- **User Feedback:** "Did you feel more connected to UBCO?"
- **Faculty Recognition:** "Could you name any faculty members you met?"
- **Spatial Memory:** "Can you point to where the Engineering building is?"
- **Engagement:** "Was this scene enjoyable?"

---

## 16. FUTURE EXPANSION IDEAS

### 16.1 Dynamic Building Selection

**Concept:** Orbs go to different buildings each playthrough

**Implementation:**
```csharp
// Shuffle building assignments
List<BuildingDestination> shuffledBuildings = new List<BuildingDestination>(buildingDestinations);
shuffledBuildings.Shuffle();

for (int i = 0; i < orbs.Length; i++)
{
    AssignOrbToBuilding(orbs[i], shuffledBuildings[i]);
}
```

**Pros:** Increases replayability
**Cons:** May not match cultural logic (Japanese orb at random building)

---

### 16.2 Time-of-Day Variant

**Concept:** Buildings look different based on time (day/night/sunset)

**Implementation:**
- Check system time
- Load appropriate lighting preset
- Morning: Warm light, few students
- Afternoon: Bright, busy
- Evening: Golden hour, ambient lights on

---

### 16.3 Seasonal Events

**Concept:** Special faculty/events during cultural holidays

**Example:**
- During Diwali week: Indian orb shows professor with Diwali celebration photo
- During Cherry Blossom season: Japanese orb shows special sakura event

---

## 17. FINAL CHECKLIST

### Pre-Implementation
- [ ] Get stakeholder approval for Scene 2.5 concept
- [ ] Research and select 5 UBCO buildings
- [ ] Identify faculty members for each culture (or plan fictional profiles)
- [ ] Obtain necessary consents and ethics approval
- [ ] Prepare building photographs or 3D assets

### Implementation
- [ ] Modify `SceneState.cs` enum
- [ ] Create all new components (8 scripts)
- [ ] Modify `HarmonySceneManager.cs`
- [ ] Set up building markers in Unity scene
- [ ] Create 5 `BuildingDestination` ScriptableObjects
- [ ] Design and implement UI (faculty profile, waypoints, counter)
- [ ] Record/source all audio assets
- [ ] Implement visual effects (trails, particles)
- [ ] Configure locomotion system
- [ ] Test on Quest 2/3 hardware

### Testing
- [ ] Unit test each component
- [ ] Integration test full scene flow
- [ ] Performance test (FPS, memory)
- [ ] User experience test with 5+ people
- [ ] Bug fixes and polish

### Documentation
- [ ] Update project README
- [ ] Document Scene 2.5 architecture
- [ ] Create guide for adding new buildings
- [ ] Record demo video

---

## 18. ESTIMATED TIMELINE

| Phase | Duration | Tasks |
|-------|----------|-------|
| **Phase 1:** Core Architecture | 2 days | Scene state, managers, transitions |
| **Phase 2:** UI System | 2 days | Faculty profile, waypoints, counters |
| **Phase 3:** Building Setup | 2 days | Research, consents, ScriptableObjects |
| **Phase 4:** Audio Integration | 1 day | Record, integrate audio |
| **Phase 5:** Visual Effects | 2 days | Trails, particles, polish |
| **Phase 6:** Locomotion | 1 day | Movement system |
| **Phase 7:** Integration Testing | 2 days | Testing and bug fixes |
| **Phase 8:** Documentation | 1 day | Docs and demo video |
| **Buffer Time** | 2 days | Unexpected issues |

**Total: 15 days (3 weeks)**

---

## 19. CONCLUSION

This implementation plan provides a comprehensive roadmap for injecting Scene 2.5 "Campus Exploration" between the existing Scenes 2 and 3. The new scene:

✅ **Grounds the experience firmly in UBCO campus** - Players physically explore campus buildings
✅ **Highlights UBCO faculty/staff** - Celebrates real people who make UBCO diverse
✅ **Maintains existing scenes** - No modifications to current Scene 2 or 3
✅ **Adds gameplay variety** - Movement and exploration break up orb interactions
✅ **Scalable** - Can easily add more buildings or modify faculty profiles
✅ **Technically feasible** - Uses existing Unity/XR Toolkit capabilities

The key to success is focusing on **authentic UBCO connections** - real buildings, real (or realistic) faculty stories, and genuine cultural representation. This transforms "Harmony in Diversity" from a generic cultural experience into something that could only exist at UBCO.

---

## 20. NEXT STEPS

1. **Review this plan** with your team and stakeholders
2. **Choose faculty members** (or decide on fictional approach)
3. **Obtain consents** and ethics approval if using real people
4. **Begin Phase 1 implementation** - Core architecture (2 days)
5. **Iterate based on testing** - Especially test locomotion comfort

**Ready to begin implementation?** Start with `SceneState.cs` modification and `CampusExplorationManager.cs` skeleton, then build from there.

Good luck! This is going to make your project uniquely UBCO. 🎓✨
