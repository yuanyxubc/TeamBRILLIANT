# Scene 2.5 Setup Guide: Campus Exploration

This guide walks you through setting up the new Campus Exploration scene in Unity.

---

## ✅ Implementation Status

All code has been implemented! The following components are ready:

### Core Scripts Created:
- ✅ `SceneState.cs` - Modified to include `CampusExploration`
- ✅ `BuildingDestination.cs` - ScriptableObject for building data
- ✅ `OrbBuildingTransition.cs` - Handles orb flight animations
- ✅ `CampusExplorationManager.cs` - Main scene controller
- ✅ `CampusExplorationUI.cs` - UI management
- ✅ `WaypointMarker.cs` - Waypoint display component
- ✅ `HarmonySceneManager.cs` - Modified to integrate Scene 2.5

### Scene Flow:
```
Scene 2 (Voices Rise)
  → [30 seconds auto-progress]
  → Scene 2.5 (Campus Exploration)
  → [Player discovers all orbs]
  → Scene 3 (Connecting Threads)
```

---

## 📋 Unity Setup Steps

### Step 1: Add CampusExplorationManager to Scene

1. Open `CourtyardScene1.unity`
2. Create empty GameObject: `Right-click in Hierarchy → Create Empty`
3. Rename it: `CampusExplorationManager`
4. Add component: `CampusExplorationManager` script
5. Configure in Inspector:
   - **Discovery Radius:** 5 (meters)
   - **Bio Display Duration:** 15 (seconds)
   - **Player Transform:** Will auto-find Main Camera if not set
   - **Debug Mode:** Check this for testing

---

### Step 2: Create Building Marker Transforms

You need to manually place 5 building markers in your scene:

1. Create 5 empty GameObjects in the scene
2. Name them:
   - `Building_Engineering`
   - `Building_Arts`
   - `Building_Library`
   - `Building_Management`
   - `Building_Sciences`
3. Position them where you want orbs to fly to (in front of buildings)
4. These will be assigned to BuildingDestination ScriptableObjects later

**Tip:** Use Top view (Scene View → Y axis) to position them accurately relative to courtyard

---

### Step 3: Create BuildingDestination ScriptableObjects

Create 5 BuildingDestination assets:

1. In Project window: `Right-click → Create → Harmony → Building Destination`
2. Create 5 assets, name them:
   - `EngineeringBuildingDest`
   - `ArtsBuildingDest`
   - `LibraryBuildingDest`
   - `ManagementBuildingDest`
   - `SciencesBuildingDest`

3. For EACH asset, configure in Inspector:

**Example: EngineeringBuildingDest**
```
Building Information:
  - Building Name: "Engineering Building"
  - Building Code: "ENG"
  - Building Marker Name: "Building_Engineering"
    (This MUST match the GameObject name in Hierarchy exactly!)

Orb Assignment:
  - Assigned Orb: [Drag JapaneseOrbData asset]

Faculty Profile:
  - Faculty Photo: [Import 512x512 photo, drag here]
  - Faculty Name: "Dr. [Name]"
  - Faculty Title: "Professor of Electrical Engineering"
  - Faculty Department: "School of Engineering"
  - Faculty Bio: "Dr. [Name] leads research in robotics and has collaborated
                 with institutions in Tokyo. [2-3 sentences]"
  - Cultural Connection: "Spent sabbatical year at University of Tokyo,
                         mentors Japanese international students."

Audio (Optional):
  - Faculty Voice Clip: [Leave empty for now, or record short greeting]
```

**Repeat for all 5 buildings** with different faculty members and cultural connections.

---

### Step 4: Assign BuildingDestinations to Manager

1. Select `CampusExplorationManager` in Hierarchy
2. In Inspector, expand `Building Destinations` array
3. Set Size: `5`
4. Drag your 5 BuildingDestination assets into slots:
   - Element 0: EngineeringBuildingDest
   - Element 1: ArtsBuildingDest
   - Element 2: LibraryBuildingDest
   - Element 3: ManagementBuildingDest
   - Element 4: SciencesBuildingDest

**Important:** Order matters! The first orb spawned in Scene 2 will go to Element 0's building.

---

### Step 5: Create Campus Exploration UI

#### 5a. Create UI Canvas

1. Create Canvas: `Hierarchy → Right-click → UI → Canvas`
2. Rename: `CampusExplorationCanvas`
3. Set Canvas settings:
   - Render Mode: `World Space`
   - Event Camera: [Drag Main Camera]
   - Position: Place in front of player (e.g., X:0, Y:2, Z:5)
   - Scale: 0.01, 0.01, 0.01 (for comfortable VR viewing)

#### 5b. Add CampusExplorationUI Component

1. Select `CampusExplorationCanvas`
2. Add Component: `CampusExplorationUI`

#### 5c. Create Discovery Counter

1. Under Canvas, create: `UI → Text - TextMeshPro`
2. Rename: `DiscoveryCounter`
3. Configure:
   - Text: "Buildings Discovered: 0 / 5"
   - Font Size: 24
   - Alignment: Center
   - Position: Top of canvas

#### 5d. Create Faculty Profile Panel

1. Under Canvas, create: `UI → Panel`
2. Rename: `FacultyProfilePanel`
3. Add children:
   - `UI → Image` → Name: `FacultyPhoto` (512x512)
   - `UI → Text - TextMeshPro` → Name: `FacultyNameText`
   - `UI → Text - TextMeshPro` → Name: `FacultyTitleText`
   - `UI → Text - TextMeshPro` → Name: `FacultyBioText` (text area)
   - `UI → Text - TextMeshPro` → Name: `BuildingNameText`

Layout example:
```
[FacultyPhoto - top center, 200x200]
[FacultyNameText - below photo, bold, size 32]
[FacultyTitleText - below name, italic, size 20]
[BuildingNameText - below title, size 18]
[FacultyBioText - bottom, text area, size 16]
```

#### 5e. Create Completion Message Panel

1. Under Canvas, create: `UI → Panel`
2. Rename: `CompletionMessagePanel`
3. Add child:
   - `UI → Text - TextMeshPro` → Name: `CompletionMessageText`
   - Text: "All cultures discovered across UBCO campus!\n\nReturning to courtyard..."
   - Font Size: 28
   - Alignment: Center

#### 5f. Create Waypoint Container

1. Under Canvas, create: `Empty GameObject`
2. Rename: `WaypointContainer`
3. This will hold instantiated waypoint markers

---

### Step 6: Create Waypoint Marker Prefab

1. Create Canvas: `Hierarchy → Right-click → UI → Canvas`
2. Rename: `WaypointMarker`
3. Set Canvas to: `World Space`
4. Scale: 0.005, 0.005, 0.005 (small, visible from distance)
5. Add Panel to Canvas
6. Add children to Panel:
   - `UI → Image` → Name: `MarkerIcon` (circle shape, will show orb color)
   - `UI → Text - TextMeshPro` → Name: `DistanceText`

Layout:
```
[MarkerIcon - top, 64x64 circle]
[DistanceText - below, shows "Building Name\n45m"]
```

7. Add Component to Canvas: `WaypointMarker` script
8. Assign references in Inspector:
   - Marker Icon: [Drag MarkerIcon Image]
   - Distance Text: [Drag DistanceText]
   - Update Interval: 0.2
   - Height Above Building: 5

9. Drag Canvas to Project window to create prefab: `WaypointMarkerPrefab`
10. Delete from Hierarchy (it will be instantiated at runtime)

---

### Step 7: Connect UI to Manager

1. Select `CampusExplorationCanvas`
2. In `CampusExplorationUI` component, assign all references:
   - **Waypoint Container:** [Drag WaypointContainer]
   - **Faculty Profile Panel:** [Drag FacultyProfilePanel]
   - **Completion Message Panel:** [Drag CompletionMessagePanel]
   - **Faculty Photo Image:** [Drag FacultyPhoto Image]
   - **Faculty Name Text:** [Drag FacultyNameText]
   - **Faculty Title Text:** [Drag FacultyTitleText]
   - **Faculty Bio Text:** [Drag FacultyBioText]
   - **Building Name Text:** [Drag BuildingNameText]
   - **Discovery Counter Text:** [Drag DiscoveryCounter]
   - **Completion Message Text:** [Drag CompletionMessageText]
   - **Waypoint Marker Prefab:** [Drag WaypointMarkerPrefab from Project]

3. Select `CampusExplorationManager` in Hierarchy
4. Assign reference:
   - **Exploration UI:** [Drag CampusExplorationCanvas]

---

### Step 8: Audio Setup (Optional)

If you want narration and sound effects:

1. Create/import audio clips:
   - Scene entry narration (8-10s): "Each culture finds a home across UBCO's academic buildings..."
   - Scene exit narration (6-8s): "Now that you've explored UBCO's diversity, let's connect these cultures..."
   - Discovery chime (1-2s): Uplifting sound
   - All discovered sound (3-4s): Triumphant completion sound

2. Select `CampusExplorationManager`
3. Assign audio clips:
   - **Scene Entry Narration:** [Drag audio clip]
   - **Scene Exit Narration:** [Drag audio clip]
   - **Discovery Chime:** [Drag audio clip]
   - **All Discovered Sound:** [Drag audio clip]

---

### Step 9: Initial State Setup

By default, `FacultyProfilePanel` and `CompletionMessagePanel` should be INACTIVE:

1. Select `FacultyProfilePanel` in Hierarchy
2. Uncheck checkbox at top of Inspector (deactivate)
3. Select `CompletionMessagePanel`
4. Uncheck checkbox (deactivate)

They will be shown automatically during gameplay.

---

## 🎮 Testing the Scene

### Quick Test Procedure:

1. **Play the scene** in Unity
2. Wait for Scene 2 to complete (~30 seconds)
3. Orbs should fly to buildings automatically
4. **Move player camera** close to an orb (within 5 meters)
5. Faculty profile should appear
6. After 15 seconds, orb returns to courtyard
7. Repeat for all 5 orbs
8. When all returned, Scene 3 should start

### Debug Console Output:

You should see logs like:
```
[CampusExploration] === Initializing Campus Exploration Scene ===
[CampusExploration] Found 5 cultural orbs in scene
[CampusExploration] Mapped Japanese orb to Engineering Building
[HarmonySceneManager] === SCENE 2.5: Campus Exploration ===
Japanese orb arrived at building
[CampusExploration] Discovered Engineering Building - Dr. [Name]
Japanese orb returned to courtyard
[CampusExploration] All orbs discovered and returned - completing scene
[CampusExploration] Transitioning to Scene 3 (Connecting Threads)
[HarmonySceneManager] === SCENE 3: Connecting the Threads ===
```

---

## 🐛 Troubleshooting

### Problem: Orbs don't fly to buildings

**Solution:**
- Check that BuildingDestination assets have `Building Transform` assigned
- Verify building marker GameObjects are in scene
- Check Console for errors

### Problem: Faculty profile doesn't appear

**Solution:**
- Verify `FacultyProfilePanel` is assigned to `CampusExplorationUI`
- Check that `discoveryRadius` is large enough (try 10 meters for testing)
- Ensure UI Canvas is set to World Space with correct Event Camera

### Problem: Waypoints don't appear

**Solution:**
- Check that `WaypointMarkerPrefab` is assigned
- Verify `WaypointContainer` exists and is assigned
- Check if waypoints are spawning behind camera (they should be above buildings)

### Problem: Scene doesn't transition to Scene 3

**Solution:**
- Check that all orbs return to courtyard
- Look for Console errors
- Verify `HarmonySceneManager` exists in scene

### Problem: "CampusExplorationManager not found" error

**Solution:**
- Make sure you added `CampusExplorationManager` GameObject to scene
- Verify the script component is attached
- Check that it's active in Hierarchy

---

## 📦 Required Assets Summary

### Must Create:
1. **5 Building Marker GameObjects** (in scene)
2. **5 BuildingDestination ScriptableObjects** (in Project)
3. **CampusExplorationCanvas** with UI components
4. **WaypointMarkerPrefab** (in Project)
5. **CampusExplorationManager GameObject** (in scene)

### Optional Assets:
1. Faculty photos (5 images, 512x512 PNG)
2. Audio clips (4 clips: entry, exit, discovery, completion)
3. Faculty voice clips (5 clips, one per building)

---

## 🎨 Faculty Profile Suggestions

If you don't have real UBCO faculty yet, use placeholder data:

**Engineering Building:**
```
Name: Dr. Kenji Tanaka
Title: Professor of Robotics
Department: School of Engineering
Bio: Dr. Tanaka leads research in human-robot interaction and has
     published over 50 papers on autonomous systems.
Cultural Connection: From Tokyo, collaborates with Japanese universities
                     on robotics research and mentors international students.
```

**Arts Building:**
```
Name: Dr. Priya Sharma
Title: Professor of Computer Science
Department: Irving K. Barber Faculty of Arts and Social Sciences
Bio: Dr. Sharma specializes in artificial intelligence and machine learning,
     with focus on ethical AI development.
Cultural Connection: Originally from Mumbai, leads initiatives to increase
                     diversity in tech fields and supports Indian student community.
```

Repeat this pattern for remaining buildings.

---

## ✨ Enhancement Ideas (After Basic Setup)

Once the basic scene works, consider adding:

1. **Trail Effects:** Add TrailRenderer to orbs during flight
2. **Particle Bursts:** Play particles when player discovers orb
3. **Minimap:** Small overhead map showing building locations
4. **Teleportation:** Quick travel between buildings
5. **Achievement System:** Track which buildings discovered first
6. **Photo Mode:** Let players take screenshots at buildings

---

## 📞 Need Help?

If you encounter issues:

1. Check Unity Console for error messages
2. Verify all references are assigned in Inspector
3. Test in Editor first before building to device
4. Use Debug Mode on CampusExplorationManager for verbose logging

---

## 🎉 You're Done!

Once everything is set up:

1. Test the complete flow: Scene 1 → 2 → 2.5 → 3 → 4 → 5
2. Adjust timing values if needed (discovery radius, bio duration)
3. Replace placeholder faculty with real UBCO faculty (with permission)
4. Build and test on Quest 2/3

**Congratulations!** Scene 2.5 is now integrated and your experience is uniquely grounded in UBCO campus! 🎓✨
