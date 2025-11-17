# HARMONY IN DIVERSITY - COMPLETE UNITY SETUP GUIDE

This guide provides step-by-step instructions to set up the simplified MVP in Unity.

**Time Required:** 1-2 hours
**Difficulty:** Intermediate
**Unity Version:** 6000.2.4f1

---

## TABLE OF CONTENTS

1. [Prerequisites Check](#1-prerequisites-check)
2. [Create Cultural Orb Data Assets](#2-create-cultural-orb-data-assets)
3. [Set Up Orb Prefab Variants](#3-set-up-orb-prefab-variants)
4. [Create Spawn Points](#4-create-spawn-points)
5. [Create Sparkle Particle Systems](#5-create-sparkle-particle-systems)
6. [Set Up World Space UI Canvas](#6-set-up-world-space-ui-canvas)
7. [Create Manager GameObjects](#7-create-manager-gameobjects)
8. [Configure XR Interaction](#8-configure-xr-interaction)
9. [Final Scene Assembly](#9-final-scene-assembly)
10. [Testing & Validation](#10-testing--validation)
11. [Troubleshooting](#11-troubleshooting)

---

## 1. PREREQUISITES CHECK

### Verify Packages

1. Open **Window > Package Manager**
2. Ensure these packages are installed:
   - ✅ XR Interaction Toolkit (3.2.1+)
   - ✅ Universal Render Pipeline (17.2.0+)
   - ✅ TextMeshPro (auto-installed with Unity 6)
   - ✅ XR Plugin Management (4.5.1+)
   - ✅ OpenXR (1.15.1+)

### Verify Scripts Compiled

1. Check **Console** window for any errors
2. All 7 scripts should compile without errors:
   - `CulturalOrbData.cs`
   - `CulturalOrb.cs`
   - `ConnectionThread.cs`
   - `ThreadConnectionSystem.cs`
   - `SceneState.cs`
   - `HarmonySceneManager.cs`
   - `HarmonyUIManager.cs`

If you see errors, fix them before proceeding.

---

## 2. CREATE CULTURAL ORB DATA ASSETS

### Step 2.1: Create ScriptableObject Assets

1. In **Project** window, navigate to `Assets/`
2. Create folder: `Assets/Data/CulturalOrbs/`
3. Right-click in `CulturalOrbs` folder
4. Select **Create > Harmony > Cultural Orb Data**
5. Name it: `JapaneseOrbData`
6. Repeat 4 more times for:
   - `FrenchOrbData`
   - `IndianOrbData`
   - `MexicanOrbData`
   - `NigerianOrbData`

You should now have 5 ScriptableObject assets.

### Step 2.2: Configure Each Cultural Orb Data

**JapaneseOrbData:**
- Culture Name: `Japanese`
- Greeting Text: `Konnichiwa`
- Orb Color: Red `(R:255, G:0, B:0)` or Hex `#FF0000`
- Orb Prefab: *(leave empty for now)*

**FrenchOrbData:**
- Culture Name: `French`
- Greeting Text: `Bonjour`
- Orb Color: Blue `(R:0, G:0, B:255)` or Hex `#0000FF`
- Orb Prefab: *(leave empty for now)*

**IndianOrbData:**
- Culture Name: `Indian`
- Greeting Text: `Namaste`
- Orb Color: Green `(R:0, G:255, B:0)` or Hex `#00FF00`
- Orb Prefab: *(leave empty for now)*

**MexicanOrbData:**
- Culture Name: `Mexican`
- Greeting Text: `Hola`
- Orb Color: Yellow `(R:255, G:255, B:0)` or Hex `#FFFF00`
- Orb Prefab: *(leave empty for now)*

**NigerianOrbData:**
- Culture Name: `Nigerian`
- Greeting Text: `Sannu`
- Orb Color: Purple `(R:128, G:0, B:128)` or Hex `#800080`
- Orb Prefab: *(leave empty for now)*

---

## 3. SET UP ORB PREFAB VARIANTS

### Step 3.1: Create Prefab Variants

1. In **Project** window, locate `Assets/MysticOrb.prefab`
2. Create folder: `Assets/Prefabs/CulturalOrbs/`
3. **Drag** `MysticOrb.prefab` into `CulturalOrbs` folder (creates a copy)
4. Rename to `JapaneseOrb`
5. Repeat 4 more times for:
   - `FrenchOrb`
   - `IndianOrb`
   - `MexicanOrb`
   - `NigerianOrb`

### Step 3.2: Configure Each Prefab

For **EACH of the 5 orb prefabs**, do the following:

1. **Double-click** the prefab to enter Prefab Edit Mode
2. **Add CulturalOrb component:**
   - Select root GameObject in Hierarchy
   - Click **Add Component**
   - Search for `CulturalOrb`
   - Click to add it

3. **Assign CulturalOrbData:**
   - In **CulturalOrb** component Inspector
   - Find the **Data** field
   - Drag the corresponding ScriptableObject:
     - `JapaneseOrb` → `JapaneseOrbData`
     - `FrenchOrb` → `FrenchOrbData`
     - etc.

4. **Verify Components Present:**
   - Should have: `MysticOrb`, `OrbFloating`, `CulturalOrb`
   - `CulturalOrb` will auto-add `XRGrabInteractable` and `SphereCollider` at runtime

5. **Adjust Material Color (Optional):**
   - Select child object with Renderer
   - In Material, set Emission Color to match culture color
   - This gives a base color even before runtime

6. **Click** the back arrow (←) or **File > Save** to exit Prefab Mode

Repeat for all 5 orb prefabs.

---

## 4. CREATE SPAWN POINTS

Spawn points determine where orbs appear in the courtyard.

### Step 4.1: Create Spawn Point Parent

1. In **Hierarchy**, right-click in empty space
2. Select **Create Empty**
3. Name it: `OrbSpawnPoints`
4. Set Position: `(0, 0, 0)` (or center of courtyard)

### Step 4.2: Create 5 Child Spawn Points

1. Right-click `OrbSpawnPoints`
2. Select **Create Empty**
3. Name it: `SpawnPoint_01_Japanese`
4. Repeat 4 more times:
   - `SpawnPoint_02_French`
   - `SpawnPoint_03_Indian`
   - `SpawnPoint_04_Mexican`
   - `SpawnPoint_05_Nigerian`

### Step 4.3: Position Spawn Points

Arrange the 5 spawn points in a circle or meaningful pattern:

**Example Circle Pattern (radius = 5 meters):**

- `SpawnPoint_01_Japanese`: Position `(5, 1.5, 0)`
- `SpawnPoint_02_French`: Position `(1.5, 1.5, 4.75)`
- `SpawnPoint_03_Indian`: Position `(-4, 1.5, 3)`
- `SpawnPoint_04_Mexican`: Position `(-4, 1.5, -3)`
- `SpawnPoint_05_Nigerian`: Position `(1.5, 1.5, -4.75)`

**Tips:**
- Y position should be at a comfortable viewing height (1-2 meters)
- Ensure they're inside the courtyard environment
- Space them so player can walk between them
- Use Scene view to visually position them

---

## 5. CREATE SPARKLE PARTICLE SYSTEMS

Each spawn point needs a sparkle particle effect for Scene 1.

### Step 5.1: Create Particle System Prefab

1. In **Hierarchy**, right-click
2. Select **Effects > Particle System**
3. Name it: `SparkleParticle`

### Step 5.2: Configure Particle System

Select `SparkleParticle` and configure in Inspector:

**Main Module:**
- Duration: `5.00`
- Looping: ✅ **Checked**
- Start Lifetime: `2`
- Start Speed: `0.5`
- Start Size: `0.1`
- Start Color: White or light yellow
- Play On Awake: ✅ **Checked**

**Emission Module:**
- Rate over Time: `20`

**Shape Module:**
- Shape: `Sphere`
- Radius: `0.3`

**Renderer Module:**
- Render Mode: `Billboard`
- Material: `Default-Particle` (Unity built-in)

### Step 5.3: Create 5 Instances

1. **Duplicate** `SparkleParticle` 4 times (Ctrl+D)
2. You should have 5 particle systems

### Step 5.4: Parent to Spawn Points

1. **Drag** first particle system onto `SpawnPoint_01_Japanese`
2. Rename to `Sparkle_Japanese`
3. Set **local** position to `(0, 0, 0)` (centered on spawn point)
4. Repeat for all 5:
   - `Sparkle_Japanese` → child of `SpawnPoint_01_Japanese`
   - `Sparkle_French` → child of `SpawnPoint_02_French`
   - `Sparkle_Indian` → child of `SpawnPoint_03_Indian`
   - `Sparkle_Mexican` → child of `SpawnPoint_04_Mexican`
   - `Sparkle_Nigerian` → child of `SpawnPoint_05_Nigerian`

All sparkles should now be positioned at spawn points.

---

## 6. SET UP WORLD SPACE UI CANVAS

The UI displays narration, culture info, and end screen.

### Step 6.1: Create Canvas

1. In **Hierarchy**, right-click
2. Select **UI > Canvas**
3. Name it: `HarmonyUI_Canvas`

4. In **Canvas** component:
   - Render Mode: `World Space`
   - Event Camera: *(assign Main Camera / XR Camera later)*

5. Set **RectTransform**:
   - Position: `(0, 2.5, 5)` (in front of player spawn)
   - Rotation: `(0, 0, 0)`
   - Scale: `(0.01, 0.01, 0.01)`
   - Width: `1000`
   - Height: `600`

### Step 6.2: Create Narration Text

1. Right-click `HarmonyUI_Canvas`
2. Select **UI > Text - TextMeshPro**
   - *If prompted to import TMP Essentials, click Import*
3. Name it: `NarrationText`

4. Configure **TextMeshProUGUI** component:
   - Text: `Narration appears here`
   - Font Size: `48`
   - Alignment: Center (both horizontal and vertical)
   - Color: White
   - Auto Size: *(optional)* Enable with min 24, max 72

5. Set **RectTransform**:
   - Anchor Preset: **Top Center** (Alt+click for position too)
   - Pos Y: `-100`
   - Width: `900`
   - Height: `150`

### Step 6.3: Create Culture Info Text

1. Duplicate `NarrationText` (Ctrl+D)
2. Rename to: `CultureInfoText`

3. Set **RectTransform**:
   - Anchor Preset: **Center**
   - Pos X: `0`, Pos Y: `0`
   - Width: `600`
   - Height: `200`

4. Configure **TextMeshProUGUI**:
   - Font Size: `36`
   - Alignment: Center

### Step 6.4: Create Connection Counter Text

1. Duplicate `CultureInfoText`
2. Rename to: `ConnectionCounterText`

3. Set **RectTransform**:
   - Anchor Preset: **Bottom Center**
   - Pos Y: `50`
   - Width: `400`
   - Height: `80`

4. Configure **TextMeshProUGUI**:
   - Text: `Connections: 0 / 7`
   - Font Size: `32`

### Step 6.5: Create End Screen Panel

1. Right-click `HarmonyUI_Canvas`
2. Select **UI > Panel**
3. Name it: `EndScreenPanel`

4. Set **RectTransform**:
   - Anchor Preset: **Stretch** (full canvas)
   - Left/Right/Top/Bottom: all `0`

5. Configure **Image** component:
   - Color: Semi-transparent black `(R:0, G:0, B:0, A:200)`

6. **Add Title Text:**
   - Right-click `EndScreenPanel` > **UI > Text - TextMeshPro**
   - Name: `EndTitle`
   - Text: `"Harmony in Diversity"`
   - Font Size: `64`
   - Alignment: Center
   - RectTransform:
     - Anchor: Top Center
     - Pos Y: `-100`
     - Width: `800`, Height: `100`

7. **Add Restart Button:**
   - Right-click `EndScreenPanel` > **UI > Button - TextMeshPro**
   - Name: `RestartButton`
   - RectTransform:
     - Anchor: Center
     - Pos Y: `-50`
     - Width: `300`, Height: `80`
   - Child Text:
     - Set text to: `"Restart"`
     - Font Size: `36`

8. **Add Exit Button (Optional):**
   - Duplicate `RestartButton`
   - Name: `ExitButton`
   - Pos Y: `-150`
   - Child Text: `"Exit"`

---

## 7. CREATE MANAGER GAMEOBJECTS

The manager scripts control the experience flow.

### Step 7.1: Create HarmonySceneManager

1. In **Hierarchy**, right-click
2. Select **Create Empty**
3. Name it: `HarmonySceneManager`

4. Click **Add Component**
5. Search for: `HarmonySceneManager`
6. Add it

### Step 7.2: Assign References to HarmonySceneManager

In the **HarmonySceneManager** component Inspector:

**Cultural Orbs (Size: 5):**
- Expand the array
- Drag each orb **prefab** from Project window:
  - Element 0: `JapaneseOrb`
  - Element 1: `FrenchOrb`
  - Element 2: `IndianOrb`
  - Element 3: `MexicanOrb`
  - Element 4: `NigerianOrb`

**Spawn Points (Size: 5):**
- Drag each spawn point **from Hierarchy**:
  - Element 0: `SpawnPoint_01_Japanese`
  - Element 1: `SpawnPoint_02_French`
  - Element 2: `SpawnPoint_03_Indian`
  - Element 3: `SpawnPoint_04_Mexican`
  - Element 4: `SpawnPoint_05_Nigerian`

**Sparkle Particles (Size: 5):**
- Drag each sparkle particle **from Hierarchy**:
  - Element 0: `Sparkle_Japanese` (child of SpawnPoint_01)
  - Element 1: `Sparkle_French`
  - Element 2: `Sparkle_Indian`
  - Element 3: `Sparkle_Mexican`
  - Element 4: `Sparkle_Nigerian`

**Scene Timing:**
- Scene 1 Duration: `10` (seconds)
- Scene 2 Duration: `30` (seconds)
- Scene 4 Duration: `20` (seconds)

**Animation Settings:**
- Orb Spawn Duration: `2`
- Orb Spawn Stagger: `0.5`
- Orb Ascension Duration: `5`
- Orb Ascension Height: `10`

**Debug:**
- Debug Mode: ✅ **Checked** (for development)

### Step 7.3: Create ThreadConnectionSystem

1. In **Hierarchy**, right-click
2. Select **Create Empty**
3. Name it: `ThreadConnectionSystem`

4. Click **Add Component**
5. Add: `ThreadConnectionSystem`

### Step 7.4: Assign References to ThreadConnectionSystem

**Thread Settings:**
- Thread Width: `0.05`
- Snap Distance: `0.5`
- Max Connections Per Orb: `4`
- Minimum Connections For Completion: `7`

**Controller References:**
- Ray Interactor: *(assign in Step 8)*

**Particle Effects:**
- Connection Burst Prefab: *(leave empty for now - optional)*

### Step 7.5: Create HarmonyUIManager

1. Select the `HarmonyUI_Canvas` GameObject
2. Click **Add Component**
3. Add: `HarmonyUIManager`

### Step 7.6: Assign References to HarmonyUIManager

In the **HarmonyUIManager** component:

**UI Canvas:**
- Main Canvas: Drag `HarmonyUI_Canvas` (itself)

**Text Elements:**
- Narration Text: Drag `NarrationText` from Hierarchy
- Culture Info Text: Drag `CultureInfoText`
- Connection Counter Text: Drag `ConnectionCounterText`

**End Screen:**
- End Screen Panel: Drag `EndScreenPanel`
- Restart Button: Drag `RestartButton`
- Exit Button: Drag `ExitButton` (if created)

---

## 8. CONFIGURE XR INTERACTION

### Step 8.1: Locate XR Rig

Your scene should already have an XR Rig from the VR template. Look for:
- `XR Origin` or `XR Rig` in Hierarchy
- Should have child objects:
  - `Camera Offset` or `Main Camera`
  - `LeftHand Controller` or `Left Controller`
  - `RightHand Controller` or `Right Controller`

### Step 8.2: Tag Main Camera

1. Find the Main Camera (usually inside XR Origin)
2. In Inspector, set **Tag** to: `MainCamera`
   - This is needed for proximity detection

### Step 8.3: Assign Event Camera to Canvas

1. Select `HarmonyUI_Canvas`
2. In **Canvas** component
3. **Event Camera** field: Drag the Main Camera from Hierarchy

### Step 8.4: Find XRRayInteractor

1. Expand `RightHand Controller` in Hierarchy
2. Look for a GameObject with **XRRayInteractor** component
   - Might be named: `Ray Interactor`, `Direct Interactor`, or similar
   - If it doesn't exist, create one:
     - Right-click `RightHand Controller` > Create Empty
     - Name: `RayInteractor`
     - Add Component: `XRRayInteractor`

### Step 8.5: Assign Ray Interactor to ThreadConnectionSystem

1. Select `ThreadConnectionSystem` GameObject
2. In **ThreadConnectionSystem** component
3. **Ray Interactor** field: Drag the `XRRayInteractor` GameObject from Hierarchy

### Step 8.6: Configure Input Actions (CRITICAL)

The thread pulling mechanic uses the controller trigger. Ensure it's configured:

1. In **Project** window, find your **XRI Input Actions** asset
   - Usually at: `Assets/XRI/Settings/` or `Assets/Samples/.../XRI Default Input Actions`
   - If you can't find it, use **Window > Asset Management > XR > Input Actions**

2. **Double-click** to open Input Actions editor

3. Find the **XRI RightHand Interaction** action map

4. Verify **Select** action exists:
   - Path should include: `<XRController>{RightHand}/triggerPressed`
   - If missing, add a binding:
     - Click `+` next to Select action
     - Add Binding
     - Path: `<XRController>{RightHand}/triggerPressed`

5. **Save** the Input Actions asset

6. Back in scene, select `RightHand Controller`
7. Find **ActionBasedController** component
8. Ensure **Select Action** references the correct action

---

## 9. FINAL SCENE ASSEMBLY

### Step 9.1: Verify Hierarchy Structure

Your Hierarchy should look like this:

```
Scene: HarmonyInDiversity
├── Directional Light
├── UBCO Courtyard (environment)
├── XR Origin (or XR Rig)
│   ├── Main Camera (Tag: MainCamera)
│   ├── LeftHand Controller
│   └── RightHand Controller
│       └── RayInteractor (XRRayInteractor component)
├── OrbSpawnPoints
│   ├── SpawnPoint_01_Japanese
│   │   └── Sparkle_Japanese (ParticleSystem)
│   ├── SpawnPoint_02_French
│   │   └── Sparkle_French
│   ├── SpawnPoint_03_Indian
│   │   └── Sparkle_Indian
│   ├── SpawnPoint_04_Mexican
│   │   └── Sparkle_Mexican
│   └── SpawnPoint_05_Nigerian
│       └── Sparkle_Nigerian
├── HarmonySceneManager
├── ThreadConnectionSystem
└── HarmonyUI_Canvas (HarmonyUIManager component)
    ├── NarrationText
    ├── CultureInfoText
    ├── ConnectionCounterText
    └── EndScreenPanel
        ├── EndTitle
        ├── RestartButton
        └── ExitButton
```

### Step 9.2: Player Spawn Position

1. Select `XR Origin` (or XR Rig)
2. Set Position:
   - X: `0`
   - Y: `0` (or ground level)
   - Z: `-3` (a few meters back from spawn points)
3. Set Rotation: `(0, 0, 0)` (facing forward)

This ensures player starts in good position to see sparkles.

### Step 9.3: Lighting Setup (Scene 1 - Dawn)

1. Select `Directional Light`
2. Configure for dawn lighting:
   - Color: Warm orange `#FFA550`
   - Intensity: `0.7`
   - Rotation: `(50, -30, 0)` (low angle like sunrise)

3. **Environment Settings:**
   - Window > Rendering > Lighting
   - Environment tab:
     - Skybox: Use default or find a dawn/sunrise skybox
     - Environment Lighting > Source: Skybox
     - Ambient Intensity: `1.0`

### Step 9.4: Post-Processing (Optional but Recommended)

1. Create **Global Volume:**
   - Hierarchy > Right-click > **Volume > Global Volume**
   - Name: `PostProcessing`

2. Add **Volume** component (auto-added)
   - Profile: Create new profile
   - Click **New** next to Profile

3. **Add Override > Bloom:**
   - Intensity: `0.3` (enable checkbox)
   - Threshold: `0.9`
   - This makes orbs glow nicely

4. **Add Override > Tonemapping:**
   - Mode: ACES

---

## 10. TESTING & VALIDATION

### Step 10.1: Console Check

1. Open **Console** window (Ctrl+Shift+C)
2. Click **Play** (▶)
3. Watch for these logs:
   ```
   [HarmonySceneManager] === SCENE 1: The Courtyard Awakens ===
   [HarmonySceneManager] Japanese orb spawned
   [HarmonySceneManager] === SCENE 2: The Voices Rise ===
   Thread Connection System is now ACTIVE
   [HarmonySceneManager] === SCENE 3: Connecting the Threads ===
   ```

4. **No errors** should appear

### Step 10.2: Scene 1 Validation (First 10 seconds)

✅ **Expected Behavior:**
- 5 sparkle particles visible at spawn points
- Narration text appears: "Every culture is a light, waiting to be seen..."
- After 10 seconds, auto-transitions to Scene 2

### Step 10.3: Scene 2 Validation (10-40 seconds)

✅ **Expected Behavior:**
- Sparkles stop
- 5 orbs rise from spawn points (staggered 0.5s apart)
- Each orb has its color (red, blue, green, yellow, purple)
- Narration: "The voices of many cultures rise..."
- Orbs float gently
- Walk close to an orb → it glows brighter
- Point at orb and press trigger → shows culture name + greeting
- After 30 seconds, auto-transitions to Scene 3

### Step 10.4: Scene 3 Validation (Thread Connection)

✅ **Expected Behavior:**
- Narration: "Connect the threads of culture..."
- Connection counter appears: "Connections: 0 / 7"
- **Point at orb + hold trigger** → beam extends from orb to controller
- **Release trigger near another orb** → permanent thread appears
- Thread color blends both orb colors
- Counter updates: "Connections: 1 / 7"
- At 7 connections → auto-transitions to Scene 4

**Common Issues:**
- ❌ Beam doesn't appear: Check Ray Interactor is assigned
- ❌ Can't connect: Ensure orbs have colliders
- ❌ Trigger doesn't work: Check Input Actions configuration

### Step 10.5: Scene 4 Validation

✅ **Expected Behavior:**
- Threads animate upward to form canopy
- Center orb appears and pulses
- Narration: "A tapestry of unity emerges..."
- After 20 seconds → Scene 5

### Step 10.6: Scene 5 Validation

✅ **Expected Behavior:**
- Narration: "Together, we weave the colors of humanity..."
- Orbs rise slowly and fade
- After 10 seconds, end screen appears
- Click "Restart" → returns to Scene 1

---

## 11. TROUBLESHOOTING

### Issue: Scripts don't compile

**Symptoms:** Red errors in Console

**Solutions:**
1. Check you're using Unity 6000.2.4f1
2. Verify all packages installed (XR Interaction Toolkit, URP, TMP)
3. Check namespace imports at top of scripts
4. Delete `Library` folder and reopen project (forces recompile)

---

### Issue: Orbs don't spawn

**Symptoms:** No orbs appear in Scene 2

**Solutions:**
1. Check Console for errors
2. Verify `HarmonySceneManager` has all 5 orb prefabs assigned
3. Verify spawn points are assigned and positioned
4. Check that orb prefabs have `CulturalOrbData` assigned
5. Ensure orbs are not already in scene (should only be prefabs)

---

### Issue: Sparkles don't appear

**Symptoms:** Scene 1 has no visual effects

**Solutions:**
1. Check particle systems are children of spawn points
2. Verify "Play On Awake" is checked on ParticleSystem
3. Check particle material is assigned
4. Verify spawn points are in front of camera

---

### Issue: Thread beam doesn't appear when trigger pressed

**Symptoms:** Holding trigger does nothing

**Solutions:**
1. **Check Ray Interactor assignment:**
   - Select `ThreadConnectionSystem`
   - Verify `Ray Interactor` field is not empty
   - Should reference the XRRayInteractor from Right Controller

2. **Check Input Actions:**
   - Open XRI Input Actions asset
   - Verify Select action has trigger binding
   - Ensure ActionBasedController references correct actions

3. **Check Scene State:**
   - Thread system only active in Scene 3
   - Wait for Scene 3 or manually transition (add debug buttons)

4. **Check orb colliders:**
   - Select orb prefab
   - Should have collider (added by CulturalOrb script)
   - Ensure collision detection is enabled

---

### Issue: Connections don't persist

**Symptoms:** Thread appears then disappears

**Solutions:**
1. Check Console for "connection already exists" message
2. Verify `ConnectionThread` script is creating LineRenderer
3. Check that threads are being added to connections list
4. Verify orbs have valid CulturalOrbData

---

### Issue: UI text not visible

**Symptoms:** Narration or info doesn't show

**Solutions:**
1. Check Canvas is World Space mode
2. Verify Event Camera is assigned (Main Camera)
3. Check text color is not same as background
4. Verify `HarmonyUIManager` has all text fields assigned
5. Check text is not behind other UI elements (Z order)

---

### Issue: Scene doesn't auto-progress

**Symptoms:** Stuck in Scene 1 or 2

**Solutions:**
1. Check Console for coroutine errors
2. Verify timing values are > 0
3. Check that `HarmonySceneManager` Start() is being called
4. Add debug log in TransitionToState() to verify it's called

---

### Issue: VR controller not working

**Symptoms:** Can't interact with anything

**Solutions:**
1. Verify XR Plugin Management is configured:
   - Edit > Project Settings > XR Plug-in Management
   - Enable OpenXR for your platform
2. Check ActionBasedController has all actions assigned
3. Verify Input Action asset is referenced in XR Interaction Manager
4. Test in Unity Editor with XR Device Simulator:
   - Window > XR > XR Device Simulator

---

### Issue: Performance is poor / low FPS

**Symptoms:** Experience is laggy or stuttering

**Solutions:**
1. Reduce particle count in sparkle systems
2. Lower post-processing quality (Bloom intensity)
3. Bake lighting for courtyard environment
4. Reduce orb material complexity
5. Check profiler (Window > Analysis > Profiler)
6. Build to device and test (Editor is slower)

---

### Issue: Colors don't match expected

**Symptoms:** Orbs are wrong color

**Solutions:**
1. Verify CulturalOrbData has correct colors set
2. Check that MysticOrb.SetOrbColor() is being called
3. Verify orb material supports emission color
4. Check lighting isn't washing out colors (reduce intensity)
5. Ensure URP is rendering correctly

---

## TESTING CHECKLIST

Before considering setup complete, verify:

- [ ] All scripts compile without errors
- [ ] 5 CulturalOrbData assets created and configured
- [ ] 5 orb prefabs with CulturalOrb component
- [ ] 5 spawn points positioned in scene
- [ ] 5 sparkle particle systems created
- [ ] World space UI canvas with all text elements
- [ ] HarmonySceneManager with all references assigned
- [ ] ThreadConnectionSystem with Ray Interactor assigned
- [ ] HarmonyUIManager with all UI references assigned
- [ ] Main Camera tagged as "MainCamera"
- [ ] XR Rig positioned correctly
- [ ] Input Actions configured for trigger
- [ ] Scene 1 plays automatically on start
- [ ] Sparkles visible and playing
- [ ] Orbs spawn in Scene 2
- [ ] Orbs glow when approached
- [ ] Thread can be pulled from orb
- [ ] Thread creates connection
- [ ] Connection counter updates
- [ ] Scene 4 canopy forms
- [ ] Scene 5 orbs rise
- [ ] End screen appears
- [ ] Restart button works

---

## NEXT STEPS AFTER SETUP

Once everything is working:

1. **Adjust Timing:**
   - Modify scene durations in `HarmonySceneManager`
   - Test pacing feels right

2. **Polish Visuals:**
   - Fine-tune particle effects
   - Adjust orb materials
   - Improve lighting

3. **Add More Orbs:**
   - Scale to 10-20 orbs
   - Create more CulturalOrbData assets
   - Add spawn points

4. **Build to Device:**
   - File > Build Settings
   - Select Android
   - Build and Run on Quest

5. **Add Audio (Post-MVP):**
   - Record greetings
   - Add cultural hums
   - Implement audio system

---

## SUPPORT & RESOURCES

If you encounter issues not covered here:

1. Check Unity Console for specific error messages
2. Review script comments for usage notes
3. Verify package versions match requirements
4. Test on different hardware if available
5. Refer to CLAUDE.md for architecture details

**Common Unity/XR Resources:**
- XR Interaction Toolkit Documentation
- Unity Forum - VR section
- OpenXR Plugin documentation

---

**Setup guide complete! You're ready to test the Harmony in Diversity MVP.**
