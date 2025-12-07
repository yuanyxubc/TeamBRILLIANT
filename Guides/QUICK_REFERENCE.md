# HARMONY IN DIVERSITY - QUICK REFERENCE

## File Locations

### Scripts
```
Assets/Scripts/HarmonyInDiversity/
├── Core/
│   ├── SceneState.cs
│   └── HarmonySceneManager.cs
├── Orbs/
│   ├── CulturalOrbData.cs
│   └── CulturalOrb.cs
├── Threading/
│   ├── ConnectionThread.cs
│   └── ThreadConnectionSystem.cs
└── UI/
    └── HarmonyUIManager.cs
```

### Data Assets (Create these)
```
Assets/Data/CulturalOrbs/
├── JapaneseOrbData.asset
├── FrenchOrbData.asset
├── IndianOrbData.asset
├── MexicanOrbData.asset
└── NigerianOrbData.asset
```

### Prefabs (Create these)
```
Assets/Prefabs/CulturalOrbs/
├── JapaneseOrb.prefab
├── FrenchOrb.prefab
├── IndianOrb.prefab
├── MexicanOrb.prefab
└── NigerianOrb.prefab
```

---

## Scene Hierarchy Quick Setup

```
HarmonyInDiversity.unity
├── XR Origin
│   ├── Main Camera (Tag: MainCamera) ← IMPORTANT
│   └── RightHand Controller
│       └── RayInteractor ← Assign to ThreadConnectionSystem
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
├── HarmonySceneManager ← Assign: orbs, spawn points, sparkles
├── ThreadConnectionSystem ← Assign: Ray Interactor
└── HarmonyUI_Canvas (+ HarmonyUIManager component)
    ├── NarrationText
    ├── CultureInfoText
    ├── ConnectionCounterText
    └── EndScreenPanel
        ├── RestartButton
        └── ExitButton
```

---

## Critical Assignments Checklist

### HarmonySceneManager Component
- [ ] Cultural Orbs [5]: All 5 orb **prefabs** from Project
- [ ] Spawn Points [5]: All 5 spawn point **GameObjects** from Hierarchy
- [ ] Sparkle Particles [5]: All 5 sparkle **ParticleSystems** from Hierarchy

### ThreadConnectionSystem Component
- [ ] Ray Interactor: XRRayInteractor from RightHand Controller

### HarmonyUIManager Component
- [ ] Main Canvas: HarmonyUI_Canvas
- [ ] Narration Text: NarrationText
- [ ] Culture Info Text: CultureInfoText
- [ ] Connection Counter Text: ConnectionCounterText
- [ ] End Screen Panel: EndScreenPanel
- [ ] Restart Button: RestartButton
- [ ] Exit Button: ExitButton (optional)

### Canvas Component
- [ ] Render Mode: World Space
- [ ] Event Camera: Main Camera

### Main Camera
- [ ] Tag: "MainCamera"

---

## Scene Flow

```
Scene 1: Courtyard Awakens (10s auto)
    ↓ Sparkles appear
    ↓ Narration shown

Scene 2: Voices Rise (30s auto)
    ↓ Orbs spawn with animation
    ↓ Proximity glow active
    ↓ Touch shows culture info

Scene 3: Connecting Threads (user-driven)
    ↓ Pull threads between orbs
    ↓ Connection counter shown
    ↓ Minimum 7 connections needed

Scene 4: Tapestry of Unity (20s auto)
    ↓ Threads form canopy
    ↓ Center orb pulses

Scene 5: Reflection (10s → end screen)
    ↓ Orbs rise and fade
    ↓ End screen with restart
```

---

## Common Console Messages

### Good Messages (Expected)
```
[HarmonySceneManager] === SCENE 1: The Courtyard Awakens ===
[HarmonySceneManager] Japanese orb spawned
[HarmonySceneManager] === SCENE 2: The Voices Rise ===
Thread Connection System is now ACTIVE
[HarmonySceneManager] === SCENE 3: Connecting the Threads ===
Started pulling thread from Japanese orb
✓ Created connection between Japanese and French (1 total)
Minimum connections reached (7/7) - Transitioning to Scene 4
```

### Warning Messages (Check but may be OK)
```
No XRRayInteractor assigned! → Assign in inspector
Spawn points count doesn't match → Verify counts
```

### Error Messages (Must Fix)
```
NullReferenceException → Missing assignment
MissingComponentException → Component not on GameObject
```

---

## Testing Controls

### Unity Editor (with XR Device Simulator)
- **W/A/S/D**: Move camera
- **Right Mouse + Drag**: Rotate camera
- **Ctrl + Right Mouse**: Simulate controller
- **Grip**: Right Ctrl
- **Trigger**: Right Mouse Button

### On VR Device
- **Right Trigger**: Hold to pull thread, release to connect
- **Move physically**: Walk around orbs
- **Point at UI**: Use ray to interact with buttons

---

## Performance Targets

- **FPS**: 72+ (Quest 2), 90+ (Quest 3)
- **Draw Calls**: < 150
- **Orb Count**: 5 (MVP), scalable to 20+
- **Thread Count**: Max 10 (5 orbs × 2 connections each)

---

## Cultural Orb Data Reference

| Culture | Color | Hex | Greeting |
|---------|-------|-----|----------|
| Japanese | Red | #FF0000 | Konnichiwa |
| French | Blue | #0000FF | Bonjour |
| Indian | Green | #00FF00 | Namaste |
| Mexican | Yellow | #FFFF00 | Hola |
| Nigerian | Purple | #800080 | Sannu |

---

## Quick Fixes

### Can't pull thread
1. Check ThreadConnectionSystem has Ray Interactor assigned
2. Verify you're in Scene 3 (wait 40 seconds after start)
3. Check Input Actions configured

### Orbs don't spawn
1. Check HarmonySceneManager has orb prefabs assigned
2. Verify spawn points exist and are assigned
3. Check Console for errors

### UI not visible
1. Verify Canvas Render Mode is World Space
2. Check Event Camera is assigned
3. Verify position is in front of player

### No sparkles
1. Check ParticleSystems are assigned to HarmonySceneManager
2. Verify Play On Awake is enabled
3. Check particle material exists

---

## Debug Shortcuts (Add if needed)

Add these to a debug script for faster testing:

```csharp
// Skip to Scene 3
if (Input.GetKeyDown(KeyCode.Alpha3))
    HarmonySceneManager.Instance.TransitionToState(SceneState.ConnectingThreads);

// Skip to Scene 4
if (Input.GetKeyDown(KeyCode.Alpha4))
    HarmonySceneManager.Instance.TransitionToState(SceneState.TapestryOfUnity);

// Restart
if (Input.GetKeyDown(KeyCode.R))
    HarmonySceneManager.Instance.RestartExperience();
```

---

## Build Settings

### Android (Quest)
- **Platform**: Android
- **Texture Compression**: ASTC
- **Minimum API Level**: Android 10.0 (API 29)
- **Scripting Backend**: IL2CPP
- **Target Architectures**: ARM64

### XR Settings
- **XR Plug-in Management**: OpenXR
- **OpenXR Feature Groups**: Meta Quest Support

---

This quick reference covers the most common setup steps and troubleshooting. For detailed instructions, see **UNITY_SETUP_GUIDE.md**.
