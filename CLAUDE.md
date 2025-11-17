# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

TeamBRILLIANT is a Unity VR project built for Android XR devices. The project features a UBCO courtyard environment with interactive elements including food ordering/spawning systems and mystical energy orbs with visual effects.

**Unity Version:** 6000.2.4f1 (Unity 6)

**Target Platform:** Android XR (OpenXR)

## Key Unity Packages

- **XR Foundation:**
  - `com.unity.xr.androidxr-openxr` (1.0.1) - Android XR backend
  - `com.unity.xr.openxr` (1.15.1) - OpenXR runtime
  - `com.unity.xr.interaction.toolkit` (3.2.1) - Core XR interactions
  - `com.unity.xr.hands` (1.6.1) - Hand tracking support
  - `com.unity.xr.arfoundation` (6.2.0) - AR Foundation

- **Rendering:**
  - `com.unity.render-pipelines.universal` (17.2.0) - Universal Render Pipeline (URP)

- **Other:**
  - `com.unity.inputsystem` (1.14.2) - New Input System
  - `com.unity.cinemachine` (2.10.4) - Camera management

## Project Structure

### Core Custom Scripts (`Assets/Scripts/`)

**Mystic Orb System:**
- `MysticOrb.cs` - Main orb controller managing rotation, pulsing effects, glow intensity, and particle systems
- `OrbFloating.cs` - Handles vertical floating and horizontal swaying motion with random timer initialization
- `OrbEnergyFlow.cs` - Energy flow effects between orbs
- `OrbLightningEffect.cs` - Lightning visual effects
- `OrbShaderHelper.cs` - Shader property management
- `Editor/MysticOrbCreator.cs` - Editor tool for creating orb prefabs

**UBCO Courtyard System (`Assets/UBCO Courtyard Assets/Script/`):**
- `FoodMenuController.cs` - Main UI controller for food ordering system
  - Manages dropdown menu (Burger, Pizza, Cola)
  - Controls scale slider (0.5x - 3.0x range)
  - Handles "boom" toggle for showing/hiding all spawned food
  - Spawns food prefabs with XR grab interactions
  - Particle effects for boom activation/deactivation
- `FoodGrabbable.cs` - Makes food objects grabbable in VR using XR Interaction Toolkit
  - Requires Rigidbody, Collider, XRGrabInteractable components
  - Supports haptic feedback on grab/release
  - Configurable throw mechanics
- `TriggerMenu.cs` - Menu trigger system
- `Editor/FoodPrefabSetup.cs` - Editor tool for setting up food prefabs

### Unity XR Samples

The project includes extensive samples from Unity XR packages:
- `Assets/Samples/XR Interaction Toolkit/3.2.1/` - Hand interaction demos and starter assets
- `Assets/Samples/XR Hands/1.6.1/` - Hand visualization samples
- `Assets/VRTemplateAssets/` - VR template assets including UI, controllers, tutorials

### Scenes

- `Assets/Scenes/CourtyardScene1.unity` - UBCO courtyard environment (primary scene)
- `Assets/Scenes/SampleScene/SampleScene.unity` - Sample/test scene

## Architecture Notes

### XR Interaction System

The project uses Unity's XR Interaction Toolkit for all VR interactions:
- All grabbable objects require `XRGrabInteractable` component
- `FoodGrabbable.cs` is the standard pattern for making objects interactive
- Haptic feedback is integrated through XR controller interactions
- Movement type is set to `Instantaneous` for smooth grabbing

### Food Spawning System

The food menu system follows this architecture:
1. `FoodMenuController` manages UI and spawning logic
2. Food prefabs are instantiated at a spawn point with progressive offset
3. Each spawned food receives `FoodGrabbable` component automatically if missing
4. All spawned objects are tracked in a list for bulk operations (scale, show/hide)
5. Scale changes apply retroactively to all existing spawned items

### Mystic Orb System

Energy orbs use a multi-component architecture:
- `MysticOrb` - Core controller (rotation, pulsing, glow)
- `OrbFloating` - Physics-independent motion (decoupled from main controller)
- Additional effect scripts for specialized visuals
- Shader-based effects controlled via material properties (`_EmissionColor`)

## Development Commands

### Opening the Project

Open this project in Unity Hub using Unity 6000.2.4f1. The project will auto-generate `.csproj` files when opened.

### Building for Android XR

1. File > Build Settings
2. Select Android platform
3. Ensure XR settings are configured (Project Settings > XR Plug-in Management)
4. Build

Note: This is an Android XR project specifically targeting devices like Meta Quest with OpenXR.

### Working with Git LFS

PNG files are tracked with Git LFS:
```bash
# Ensure LFS is installed
git lfs install

# Pull LFS files
git lfs pull
```

## Important Conventions

### Component Requirements

When creating grabbable objects:
- Always use `[RequireComponent]` attributes for dependencies
- Auto-add missing components in `Awake()` or setup methods
- Log warnings when default components are added

### Event Subscription

Follow the pattern in `FoodGrabbable.cs` and `FoodMenuController.cs`:
- Subscribe to events in `Start()`
- Unsubscribe in `OnDestroy()` to prevent memory leaks
- Use Unity Events (e.g., `selectEntered.AddListener()`)

### Material and Shader Properties

When modifying shader properties:
- Use serialized property names (e.g., `"_EmissionColor"`)
- Store base colors/values for dynamic modifications
- Use HDR color values for emission (`color * intensity`)

### XR Interaction Toolkit Usage

- Import from `UnityEngine.XR.Interaction.Toolkit`
- Use full namespace for interactables: `UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable`
- Use full namespace for interactors: `UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor`
- Event args: `SelectEnterEventArgs`, `SelectExitEventArgs`

### Script Organization

- Custom gameplay scripts: `Assets/Scripts/`
- Editor scripts: `Assets/Scripts/Editor/`
- Asset-specific scripts: Within asset folder (e.g., `Assets/UBCO Courtyard Assets/Script/`)
- Sample scripts: Remain in `Assets/Samples/` (do not modify)

---

## PROJECT IMPLEMENTATION PLAN: "Harmony in Diversity"

### Project Concept

**Harmony in Diversity** is an integrated AR/VR narrative experience celebrating cultural diversity through two complementary phases:

1. **Contribute Phase (AR - Daytime):** Mobile AR app where users contribute cultural voices (greetings, songs) in the physical UBCO courtyard, appearing as "seeds of light"
2. **Connect Phase (VR - Nighttime):** VR experience where users interact with community-contributed content transformed into glowing orbs, weaving them into a collective tapestry

**Five VR Scenes:**
1. The Courtyard Awakens (Introduction)
2. The Voices Rise (Discovery)
3. Connecting the Threads (Active Connection)
4. The Tapestry of Unity (Celebration)
5. Reflection Beneath the Light (Closure)

### Leveraging Existing Systems

**✓ Already Implemented:**
- UBCO Courtyard environment (CourtyardScene1.unity)
- MysticOrb system (perfect foundation for cultural orbs)
- XR Interaction Toolkit integration
- URP rendering pipeline
- AR Foundation package (v6.2.0)

**New Development Required:**
- AR mobile contribution app
- VR scene flow management (5 scenes)
- Cultural orb spawning and interaction systems
- Thread connection mechanics
- Dynamic audio system
- Data pipeline for AR→VR content

---

## PHASE 1: Core Systems & Data Architecture

### 1.1 Cultural Contribution Data System

**Purpose:** Define data structure for cultural contributions from AR to VR

**Data Structure (`CulturalContribution.cs`):**
```csharp
public class CulturalContribution
{
    public string contributionId;           // Unique identifier
    public string cultureName;              // e.g., "Japanese", "French", "Ugandan"
    public string greeting;                 // Text of greeting
    public AudioClip recordedAudio;         // Recorded greeting audio
    public Vector3 worldPosition;           // Position in courtyard
    public Color orbColor;                  // Unique color for this culture
    public AudioClip culturalHum;           // Background hum/instrument sound
    public DateTime timestamp;              // When contributed
    public string contributorName;          // Optional anonymous/named
}
```

**Implementation Files:**
- `Assets/Scripts/Data/CulturalContribution.cs` - Data class
- `Assets/Scripts/Data/CulturalDatabase.cs` - ScriptableObject or JSON-based database
- `Assets/Scripts/Data/ContributionLoader.cs` - Loads contributions for VR scenes

**Key Features:**
- Support for ScriptableObject-based storage (offline/demo mode)
- JSON serialization for future backend integration
- Default sample contributions for testing without AR app
- Culture-specific color palette system

### 1.2 Scene Management System

**Purpose:** Manage flow between 5 VR scenes with transitions and state persistence

**Scene Flow Manager (`SceneFlowManager.cs`):**
- Singleton pattern to persist across scene loads
- Track current scene index (0-4)
- Handle scene transitions with fade effects
- Persist player progress (which orbs touched, connections made)
- Trigger narrations at appropriate times

**Scene States:**
```csharp
public enum HarmonyScene
{
    CourtyardAwakens,      // Scene 1
    VoicesRise,            // Scene 2
    ConnectingThreads,     // Scene 3
    TapestryOfUnity,       // Scene 4
    Reflection             // Scene 5
}
```

**Implementation Files:**
- `Assets/Scripts/SceneManagement/SceneFlowManager.cs` - Main scene controller
- `Assets/Scripts/SceneManagement/SceneTransition.cs` - Fade/transition effects
- `Assets/Scripts/SceneManagement/SceneState.cs` - Per-scene state data

**Key Features:**
- Automatic scene progression triggers
- Manual override for testing individual scenes
- Save/load scene state
- Transition timing configuration

### 1.3 Narration System

**Purpose:** Play narration audio at specific scene moments

**Narration Manager (`NarrationManager.cs`):**
- Queue-based narration system
- Trigger narrations by scene events or time
- Subtitle support (TextMeshPro UI)
- Volume ducking for background audio

**Narration Data:**
- Scene 1 Opening: "Every culture is a light, waiting to be seen..."
- Scene 5 Closing: "Together, we weave the colors of humanity... May we carry this harmony with us."

**Implementation Files:**
- `Assets/Scripts/Audio/NarrationManager.cs`
- `Assets/Scripts/Audio/NarrationClip.cs` - ScriptableObject for narration data
- `Assets/Scripts/UI/SubtitleDisplay.cs` - UI for subtitles

### 1.4 Audio Management System

**Purpose:** Handle spatial audio, dynamic music, and audio mixing

**Audio Manager Components:**
1. **Spatial Audio:** 3D audio sources attached to orbs
2. **Dynamic Music System:** Adaptive music that evolves with connections
3. **Audio Mixer:** Separate channels for ambience, orbs, music, narration

**Implementation Files:**
- `Assets/Scripts/Audio/DynamicMusicController.cs` - Layered music system
- `Assets/Scripts/Audio/SpatialAudioSource.cs` - Wrapper for orb audio
- `Assets/Audio/Mixers/MainAudioMixer.mixer` - Unity Audio Mixer

**Dynamic Music Approach:**
- Start with minimal ambient layer
- Add instrumental layers as orbs connect (additive mixing)
- Each connection adds harmonic layer
- Scene 4 plays full orchestration

---

## PHASE 2: Cultural Orb System (Extending MysticOrb)

### 2.1 Cultural Orb Component

**Purpose:** Extend existing MysticOrb system for cultural content

**Cultural Orb (`CulturalOrb.cs`):**
- Inherits/extends MysticOrb functionality
- Stores reference to CulturalContribution data
- Unique color based on culture
- Plays cultural hum continuously
- Plays greeting audio on interaction
- Manages connection state

**Component Hierarchy:**
```
CulturalOrb (MonoBehaviour)
├─ MysticOrb (rotation, pulsing, glow)
├─ OrbFloating (floating motion)
├─ OrbInteractable (XR interaction)
├─ AudioSource (cultural hum - spatial)
└─ AudioSource (greeting playback)
```

**Implementation Files:**
- `Assets/Scripts/CulturalOrbs/CulturalOrb.cs` - Main orb controller
- `Assets/Scripts/CulturalOrbs/OrbInteractable.cs` - XR interaction handling
- `Assets/Scripts/CulturalOrbs/OrbConnectionPoint.cs` - Connection socket

**Key Features:**
- Proximity detection (glow intensifies when player approaches)
- Gaze detection (brighten on look)
- Touch/gaze interaction triggers greeting
- Visual feedback for connection state
- Support for multiple connection points per orb

### 2.2 Orb Spawner System

**Purpose:** Spawn cultural orbs from sparkle locations

**Orb Spawner (`OrbSpawner.cs`):**
- Reads CulturalContribution data
- Spawns sparkle particles at contribution positions (Scene 1)
- Animates orb spawn from sparkles (Scene 2)
- Applies culture-specific colors and audio

**Spawn Sequence:**
1. Scene 1: Spawn sparkle particles at positions
2. Gaze brightening effect on sparkles
3. Scene 2 transition: Sparkles rise and transform into orbs
4. Orbs float to final positions

**Implementation Files:**
- `Assets/Scripts/CulturalOrbs/OrbSpawner.cs`
- `Assets/Scripts/CulturalOrbs/SparkleParticle.cs` - Sparkle effect
- `Assets/Scripts/CulturalOrbs/OrbSpawnAnimation.cs` - Spawn transition

---

## PHASE 3: Scene-Specific Implementation

### 3.1 Scene 1: The Courtyard Awakens

**Environment Setup:**
- **Lighting:** Dawn atmosphere with golden directional light
- **Skybox:** Gradient sky (orange/pink horizon to blue)
- **Post-Processing:** Bloom, color grading for warm tones
- **Fog:** URP volumetric fog for mist effect
- **Time:** Simulated dawn (5:30 AM lighting)

**Audio:**
- Ambient birds chirping (looping)
- Distant footsteps (occasional)
- Gentle wind rustling
- Opening narration trigger (5 seconds after scene start)

**Interactions:**
- Player spawns at courtyard entrance
- Sparkle particles at each contribution location
- Gaze detection: sparkles brighten when looked at (raycast from camera)
- No movement restrictions

**Implementation Files:**
- `Assets/Scripts/Scenes/Scene1_CourtyardAwakens.cs` - Scene controller
- `Assets/Scripts/Effects/GazeBrightening.cs` - Gaze detection for sparkles

**Scene-Specific Components:**
- Directional Light with animated intensity (dawn simulation)
- Particle systems for sparkles (one per contribution)
- Audio sources for ambient sounds
- Narration trigger (TimedEvent component)

### 3.2 Scene 2: The Voices Rise

**Visual Changes:**
- Orbs spawn and rise from sparkle locations
- Each orb glows with unique cultural color
- Light threads extend from orb to ground

**Audio:**
- Each orb emits cultural hum (drum, flute, chant, etc.)
- Spatial audio with 3D rolloff
- Greeting plays on interaction

**Interactions:**
- **Proximity:** Approach orb → glow intensifies, hum gets louder
- **Gaze:** Look at orb → subtle pulse effect
- **Touch/Select:** Grab or point at orb → play recorded greeting
- **Visual Feedback:** Light thread pulses when greeting plays

**Implementation Files:**
- `Assets/Scripts/Scenes/Scene2_VoicesRise.cs` - Scene controller
- `Assets/Scripts/CulturalOrbs/ProximityDetector.cs` - Player distance tracking
- `Assets/Scripts/CulturalOrbs/GreetingPlayer.cs` - Audio playback on interaction
- `Assets/Scripts/Effects/LightThread.cs` - Thread from orb to ground (LineRenderer)

**Orb Interaction System:**
- Use XR Interaction Toolkit hover/select events
- Hand tracking: pinch gesture to activate
- Controller: trigger press to activate
- Gaze + hand raise for hands-free mode

### 3.3 Scene 3: Connecting the Threads

**Core Mechanic: Thread Connection**

**Connection System (`ThreadConnectionSystem.cs`):**
1. Player points controller at source orb
2. Hold trigger to "pull" thread of light from orb
3. Thread follows controller position (bezier curve)
4. Point at target orb and release to connect
5. Connection creates visual and audio feedback

**Visual Feedback:**
- Thread beam from source to controller (LineRenderer with shader)
- Snapping indicator when near valid target
- Connection creates particle burst
- Connected orbs pulse in sync
- Thread color blends both orb colors

**Audio Feedback:**
- Thread pulling: gentle hum
- Connection success: harmonic chord
- Background music gains new layer with each connection
- Both cultural hums blend into harmony

**Connection Rules:**
- Any orb can connect to any other orb
- Multiple connections per orb allowed
- Minimum connections needed: N-1 (where N = number of orbs)
- No self-connections

**Implementation Files:**
- `Assets/Scripts/Threading/ThreadConnectionSystem.cs` - Main connection manager
- `Assets/Scripts/Threading/ConnectionThread.cs` - Individual thread visual/data
- `Assets/Scripts/Threading/ThreadBeam.cs` - Controller-to-orb beam effect
- `Assets/Scripts/Threading/ConnectionValidator.cs` - Validate connection attempts
- `Assets/Scripts/Effects/ConnectionBurst.cs` - Particle burst on connect

**Dynamic Music System:**
- Start with base ambient track
- Each connection adds instrumental layer (stored in AudioMixer)
- Smooth crossfade between layers
- Track connection count to determine music intensity

### 3.4 Scene 4: The Tapestry of Unity

**Trigger Condition:** All required connections made (threshold: 80% of possible connections)

**Visual Transformation:**
1. All threads weave upward to form canopy
2. Threads create geometric patterns overhead
3. Orbs descend and merge into floor mosaic
4. Mosaic sections colored by cultural contributions

**Floor Mosaic System:**
- Procedurally generated mosaic using Voronoi diagram
- Each cell corresponds to a cultural contribution
- Cell color = orb color with transparency
- Emission map for glow effect

**Interactions:**
- **Walk on Mosaic:** Stepping on cell swells that culture's harmony
- **Center Orb:** Large central orb formed from all contributions
- **Center Touch:** Triggers final radiant bloom (particle explosion + light flash)

**Audio:**
- All cultural sounds synchronized into harmonic soundscape
- Stepping on cell: volume boost for that culture (spatial audio)
- Center orb touch: crescendo of all sounds

**Implementation Files:**
- `Assets/Scripts/Scenes/Scene4_TapestryUnity.cs` - Scene controller
- `Assets/Scripts/Tapestry/CanopyGenerator.cs` - Overhead thread weaving
- `Assets/Scripts/Tapestry/FloorMosaicGenerator.cs` - Procedural mosaic
- `Assets/Scripts/Tapestry/MosaicCell.cs` - Individual cell component
- `Assets/Scripts/Tapestry/StepDetector.cs` - Detect player stepping on cells
- `Assets/Scripts/Tapestry/CenterOrbBloom.cs` - Final bloom effect

**Canopy Generation:**
- Use existing threads from Scene 3
- Animate upward movement (Lerp over time)
- Create additional connecting threads for density
- Shader effect for glowing threads

**Step Detection System:**
- Raycast downward from player position
- Detect which mosaic cell is beneath player
- Trigger audio swell for that cell's culture
- Visual pulse effect on active cell

### 3.5 Scene 5: Reflection Beneath the Light

**Visual Sequence:**
1. Canopy softly pulsates (sine wave on emission)
2. Closing narration plays
3. Orbs separate from mosaic and rise into sky
4. Final pulse of light as orbs fade
5. Return to gentle ambient lighting

**UI System:**
- Fade in UI panel after narration completes
- Two buttons: "Replay Experience" | "Exit"
- Optional: "Contribute Your Voice" callout with AR app info
- Use Unity's new UI Toolkit or TextMeshPro

**Interactions:**
- Player observes (minimal interaction)
- Can look around freely
- UI interaction: gaze + select or controller point + trigger

**Implementation Files:**
- `Assets/Scripts/Scenes/Scene5_Reflection.cs` - Scene controller
- `Assets/Scripts/UI/EndSceneUI.cs` - Menu UI controller
- `Assets/Scripts/Effects/OrbAscension.cs` - Orbs rising animation
- `Assets/Scripts/Effects/FinalPulse.cs` - Concluding light effect

**Orb Ascension Animation:**
- Disable physics
- Animate position upward (AnimationCurve for easing)
- Fade alpha over time (material property)
- Destroy after animation completes

---

## PHASE 4: AR Contribution App (Mobile)

### 4.1 AR App Architecture

**Platform:** Android (AR Foundation + ARCore)

**Core Features:**
1. AR camera view of courtyard
2. Geolocation detection (verify user in UBCO courtyard)
3. Cultural contribution form
4. Audio recording
5. Seed of light visualization in AR
6. Data submission to backend

**App Flow:**
1. Launch app → Request camera + location permissions
2. Detect courtyard location via GPS
3. Show AR view with UI overlay
4. User selects culture or enters custom
5. Record greeting (5-10 seconds max)
6. Place "seed of light" at current position in AR
7. Submit contribution

**Implementation Files (Separate Unity Project or Scene):**
- `Assets/Scripts/AR/ARContributionManager.cs` - Main AR app controller
- `Assets/Scripts/AR/LocationValidator.cs` - GPS verification
- `Assets/Scripts/AR/AudioRecorder.cs` - Microphone recording
- `Assets/Scripts/AR/ARSeedVisualizer.cs` - Seed of light AR effect
- `Assets/Scripts/AR/ContributionSubmitter.cs` - Backend communication

### 4.2 Geolocation System

**UBCO Courtyard Bounds:**
- Define geofence around courtyard coordinates
- Validate user is within ~50m radius
- Option to override for testing

**Location Tracking:**
- Use Unity's `Input.location` service
- Convert GPS to Unity world position (relative mapping)
- Store world position with contribution

### 4.3 Audio Recording

**Recording System:**
- Use Unity Microphone class
- Max duration: 10 seconds
- Format: WAV or compressed format
- Visual feedback: waveform display during recording

**Audio Processing:**
- Normalize audio levels
- Trim silence from start/end
- Compress for storage/transmission

### 4.4 Seed of Light AR Visual

**AR Visualization:**
- Spawn particle effect at user's feet in AR
- Gentle pulsing light effect
- Color preview of future orb
- Persist for 5 seconds then fade

**Implementation:**
- Use AR Foundation's AR Raycast
- Detect ground plane
- Instantiate particle prefab at hit point

### 4.5 Data Backend (Optional for MVP)

**Option 1: Local Storage (Demo Mode)**
- Save contributions to JSON file
- Manually transfer to VR project
- Load in VR as ScriptableObjects

**Option 2: Firebase Backend**
- Firebase Realtime Database for contribution storage
- Firebase Storage for audio files
- REST API for VR app to fetch contributions

**Option 3: Custom Server**
- Node.js/Python backend
- PostgreSQL/MongoDB database
- File storage for audio
- REST API endpoints

**Recommended for MVP:** Option 1 (Local Storage) for initial development

---

## PHASE 5: Visual Effects & Polish

### 5.1 Particle Systems

**Required Particle Effects:**
1. **Sparkles (Scene 1):** Gentle floating particles at contribution points
2. **Orb Spawn (Scene 2):** Burst of light as orb emerges
3. **Connection Burst (Scene 3):** Explosion when threads connect
4. **Thread Glow (Scene 3):** Particles along thread paths
5. **Canopy Shimmer (Scene 4):** Glitter effect on overhead threads
6. **Mosaic Glow (Scene 4):** Pulsing light from mosaic cells
7. **Center Bloom (Scene 4):** Radiant explosion from center orb
8. **Orb Ascension Trail (Scene 5):** Trail particles as orbs rise

**Implementation:**
- Use Unity's Particle System
- URP-compatible particle shaders
- GPU particles for performance
- Pooling system for frequent effects

**Files:**
- `Assets/Prefabs/Particles/` - Prefab for each effect
- `Assets/Scripts/Effects/ParticlePooler.cs` - Object pooling

### 5.2 Shader Effects

**Custom Shaders Needed:**
1. **Orb Shader:** Pulsing emission, color blending, fresnel glow
2. **Thread Shader:** Flowing light along line, color gradient
3. **Mosaic Shader:** Emission map, step-activated glow
4. **Canopy Shader:** Shimmering threads with transparency

**Implementation:**
- Use Shader Graph for URP compatibility
- HDR color support for bloom
- Animated properties (time-based scrolling)

**Files:**
- `Assets/Shaders/OrbGlow.shadergraph`
- `Assets/Shaders/ThreadFlow.shadergraph`
- `Assets/Shaders/MosaicEmission.shadergraph`
- `Assets/Shaders/CanopyShimmer.shadergraph`

### 5.3 Lighting & Post-Processing

**Per-Scene Lighting:**

**Scene 1 (Dawn):**
- Directional Light: Warm orange (#FFA500), intensity 0.7
- Ambient: Gradient sky
- Shadows: Soft shadows, low resolution
- Post-Processing: Bloom (medium), Vignette (subtle)

**Scene 2-3 (Day to Twilight):**
- Gradually reduce directional light intensity
- Increase ambient contribution
- Cool down color temperature
- Maintain bloom for orb glow

**Scene 4-5 (Night):**
- Directional Light: Cool blue (#87CEEB), low intensity (moonlight)
- Ambient: Dark gradient
- Increase bloom intensity for canopy/mosaic
- Add subtle fog for atmosphere

**Implementation:**
- Use URP Volume Profiles per scene
- Timeline for gradual lighting transitions
- Baked lightmaps for static geometry

**Files:**
- `Assets/Settings/VolumeProfiles/Scene1_Dawn.asset`
- `Assets/Settings/VolumeProfiles/Scene4_Night.asset`

### 5.4 UI/UX Polish

**Worldspace UI:**
- All UI in world space (no screen-space overlays)
- Use TextMeshPro for text rendering
- Canvas scaled for comfortable VR viewing
- Smooth fade-in/out transitions

**Interaction Feedback:**
- Haptic feedback on all interactions
- Audio confirmation sounds
- Visual highlighting on hover
- Smooth color transitions

**Tutorial/Onboarding:**
- Optional Scene 0: Brief tutorial
- Show hand/controller visualization
- Demonstrate thread pulling mechanic
- Skip option for experienced users

---

## PHASE 6: Implementation Order & Dependencies

### Stage 1: Foundation (Week 1)
1. Set up data structures (CulturalContribution, etc.)
2. Create sample contribution data (5-10 cultures)
3. Implement SceneFlowManager
4. Set up scene structure (duplicate CourtyardScene1.unity 5 times)

### Stage 2: Core Orb System (Week 1-2)
1. Extend MysticOrb → CulturalOrb
2. Implement OrbSpawner with sample data
3. Add proximity/gaze detection
4. Integrate greeting audio playback
5. Test in Scene 2 environment

### Stage 3: Scene 1 & 2 (Week 2)
1. Configure Scene 1 lighting (dawn)
2. Create sparkle particles
3. Implement gaze brightening
4. Add narration system
5. Implement Scene 1→2 transition (sparkles to orbs)
6. Polish Scene 2 orb interactions

### Stage 4: Thread Connection System (Week 3)
1. Implement ThreadConnectionSystem
2. Create thread beam visuals
3. Add connection validation
4. Implement particle burst feedback
5. Test connection mechanics thoroughly
6. Integrate dynamic music system

### Stage 5: Scene 4 Implementation (Week 3-4)
1. Implement canopy generation
2. Create floor mosaic generator
3. Add step detection system
4. Implement center orb bloom effect
5. Synchronize audio system
6. Polish visual transitions

### Stage 6: Scene 5 & Polish (Week 4)
1. Implement orb ascension animation
2. Create end scene UI
3. Add closing narration
4. Polish all transitions
5. Optimize performance
6. Test complete flow

### Stage 7: AR App (Week 5) - Optional
1. Set up AR Foundation project
2. Implement geolocation
3. Create contribution UI
4. Add audio recording
5. Implement seed visualization
6. Test AR → VR data pipeline

### Stage 8: Testing & Refinement (Week 5-6)
1. User testing on target hardware
2. Performance optimization
3. Audio mixing and balancing
4. Bug fixes
5. Final polish pass

---

## PHASE 7: Technical Specifications

### Performance Targets

**Target Hardware:** Meta Quest 2/3, Pico 4

**Performance Goals:**
- 72 FPS minimum (Quest 2)
- 90 FPS target (Quest 3)
- < 150 draw calls per frame
- < 100k triangles visible
- < 50 dynamic lights

**Optimization Strategies:**
- LOD for orb models
- Particle system pooling
- Occlusion culling for courtyard
- Baked lighting where possible
- Texture atlasing
- Mesh batching for threads

### Audio Specifications

**Audio Files:**
- Format: WAV (uncompressed) or OGG Vorbis (compressed)
- Sample Rate: 44.1 kHz
- Bit Depth: 16-bit
- Cultural hums: Looping, 5-10 seconds
- Greetings: One-shot, 2-5 seconds
- Narration: Mono, clear voice
- Music: Stereo, layered stems

**Spatial Audio:**
- Audio Source components on orbs
- 3D spatial blend
- Custom rolloff curve (log)
- Max distance: 20 meters
- Doppler level: 0 (no doppler)

### Input Mapping

**XR Controllers:**
- Grip: Select/activate orb
- Trigger: Pull/release thread
- Thumbstick: Movement (if needed)
- Menu Button: Pause/settings

**Hand Tracking:**
- Pinch: Select/activate orb
- Grab Gesture: Pull thread
- Point Gesture: Aim thread

**Gaze + Gesture:**
- Gaze at orb + hand raise: Activate
- Gaze at target + release hand: Connect thread

### Data Persistence

**Save Data:**
- Player progress through scenes
- Connections made
- Orbs interacted with
- Preferences (subtitles on/off, audio levels)

**Save Location:**
- Android: Application.persistentDataPath
- Format: JSON

**Files:**
- `Assets/Scripts/Data/SaveSystem.cs`
- `Assets/Scripts/Data/PlayerProgress.cs`

---

## PHASE 8: Asset Requirements

### 3D Models Needed

**From Existing Assets:**
- ✓ UBCO Courtyard environment
- ✓ Orb base model (MysticOrb prefab)

**New Models Required:**
- Ground mosaic tiles (or procedural)
- Center orb (larger, more detailed)
- Optional: Cultural symbols for orb surfaces

### Audio Assets Needed

**Recordings:**
1. **Greetings (5-10 cultures minimum):**
   - Japanese: "Konnichiwa"
   - French: "Bonjour"
   - Luganda: "Oli Otya"
   - Spanish: "Hola"
   - Mandarin: "Nǐ hǎo"
   - Hindi: "Namaste"
   - Arabic: "Marhaba"
   - German: "Guten Tag"
   - Swahili: "Jambo"
   - Italian: "Ciao"

2. **Cultural Hums/Instruments:**
   - Drum loops (African djembe)
   - Flute melodies (Bamboo flute)
   - String instruments (Sitar, Koto)
   - Vocal chants
   - Each 5-10 seconds, looping

3. **Narration:**
   - Opening: "Every culture is a light, waiting to be seen..."
   - Closing: "Together, we weave the colors of humanity... May we carry this harmony with us."
   - Professional voice actor (warm, inclusive tone)

4. **Ambient Audio:**
   - Birds chirping (dawn)
   - Gentle wind
   - Distant footsteps
   - Night crickets

5. **Music:**
   - Layered adaptive music system
   - Base ambient layer
   - 5-8 additive instrumental layers
   - Full orchestration for Scene 4
   - Format: Separate stems

6. **UI/Feedback Sounds:**
   - Thread pull start
   - Connection success
   - Orb activation
   - Button clicks

### Textures & Materials

**Orb Materials:**
- Base color textures (procedural OK)
- Emission maps
- HDR emission values
- Transparency for threads

**Environment:**
- ✓ Existing courtyard textures
- Mosaic floor texture (if not procedural)
- Skybox textures (dawn, day, night)

### Particle Textures

- Sparkle sprite
- Soft particle glow
- Burst star shapes
- Thread glow gradient

---

## PHASE 9: Testing Strategy

### Unit Testing

**Test Components:**
- CulturalContribution data loading
- Thread connection validation
- Orb spawning logic
- Audio playback triggers
- Scene state persistence

**Testing Framework:**
- Unity Test Framework
- Play Mode tests for interaction
- Edit Mode tests for data structures

### Integration Testing

**Test Scenarios:**
1. Complete flow Scene 1→5
2. Thread connection with various orb counts (3, 5, 10, 20)
3. Audio synchronization
4. Scene transitions
5. Save/load progress

### Performance Testing

**Metrics to Monitor:**
- FPS on target hardware
- Draw calls
- Memory usage
- Audio latency
- Load times

**Tools:**
- Unity Profiler
- RenderDoc (for GPU analysis)
- Quest Performance Overlay

### User Testing

**Test Questions:**
1. Is the thread connection mechanic intuitive?
2. Is the narrative clear without tutorial?
3. Are the cultural representations respectful?
4. Is the pacing appropriate?
5. Are there any comfort issues (VR sickness)?

**Test Group:** 5-10 users from diverse backgrounds

---

## PHASE 10: Cultural Sensitivity & Inclusivity

### Design Principles

1. **Authentic Representation:**
   - Use actual recordings from community members
   - Research accurate cultural symbols/colors
   - Avoid stereotypes or appropriation

2. **Inclusive Color Palette:**
   - Ensure colors are culturally appropriate
   - Consider color blindness (accessibility)
   - Avoid colors with negative cultural connotations

3. **Audio Authenticity:**
   - Native speakers for greetings
   - Traditional instruments where possible
   - Respect sacred/ceremonial sounds (avoid if inappropriate for use)

4. **Community Involvement:**
   - Involve UBCO cultural groups in development
   - Get feedback from represented communities
   - Credit contributors

5. **Accessibility:**
   - Subtitle support for all narration
   - Visual cues for audio feedback
   - Adjustable interaction modes (hand tracking, controller, gaze)
   - Comfort mode for VR (reduced motion)

### Cultural Research

**Resources:**
- UBCO International Student Services
- Cultural student organizations
- Academic advisors on intercultural communication
- Community partners

**Documentation:**
- Document source and permission for each cultural element
- Maintain attribution list
- Cultural sensitivity review checklist

---

## PHASE 11: Future Enhancements (Post-MVP)

### AR App Advanced Features
- Social sharing of contributions
- View other contributions in AR
- Contribution heatmap
- Photo mode with AR effects
- Multi-language UI

### VR Experience Additions
- Multiplayer mode (multiple users in same VR space)
- Contribution gallery (browse all contributions)
- Custom orb creation mode
- Seasonal events (holiday-themed)
- Analytics dashboard (most-connected orbs, etc.)

### Technical Improvements
- Cloud backend for real-time sync
- AI-generated cultural hums (with permission)
- Dynamic orb mesh generation based on culture
- Advanced procedural mosaic patterns
- Real-time translation of greetings

### Educational Extensions
- Cultural fact cards per orb
- Interactive timeline of cultural exchange
- Quiz mode about different cultures
- Integration with UBCO courses
- Export experience video for sharing

---

## Implementation Checklist Summary

### Must-Have for MVP
- [ ] Cultural contribution data system (5-10 sample cultures)
- [ ] Scene flow management (5 scenes)
- [ ] Cultural orb spawning and basic interactions
- [ ] Thread connection mechanic (Scene 3)
- [ ] Basic audio system (greetings + ambient)
- [ ] Scene 1, 2, 3 fully functional
- [ ] Simplified Scene 4 (without floor mosaic)
- [ ] Scene 5 with basic UI
- [ ] Narration system

### Should-Have for Polish
- [ ] Floor mosaic generation (Scene 4)
- [ ] Step detection and harmony swell
- [ ] Dynamic music system
- [ ] All particle effects
- [ ] Custom shaders for orbs/threads
- [ ] Complete lighting setup
- [ ] Haptic feedback
- [ ] Subtitle system

### Nice-to-Have for Full Release
- [ ] AR contribution app
- [ ] Backend data pipeline
- [ ] Tutorial scene
- [ ] Save/load system
- [ ] Performance optimizations
- [ ] User testing iteration
- [ ] Accessibility features
- [ ] Multiple language support

---

## Key Development Principles

1. **Iterative Development:** Build core loop first (Scene 2-3), then expand
2. **Modular Design:** Each system should work independently
3. **Test Early:** Test thread mechanics ASAP (most complex interaction)
4. **Performance Focus:** Target hardware has limitations, optimize continuously
5. **Cultural Respect:** When in doubt about cultural elements, consult community
6. **Player Comfort:** VR comfort is paramount (smooth movement, stable framerate)
7. **Audio Priority:** Audio is central to narrative, invest time in quality
8. **Scalable Data:** Design for 5-10 orbs initially, but support 20+ for full deployment

---

## Next Steps to Begin Implementation

1. **Review and validate this plan** with team/stakeholders
2. **Set up project branches** in Git (feature branches for each system)
3. **Create sample cultural contribution data** (JSON + audio files)
4. **Prototype thread connection mechanic** in isolated test scene
5. **Record placeholder narration** (team member can read script)
6. **Gather sample cultural audio** (Creative Commons or record with permission)
7. **Begin Stage 1 implementation** (Foundation - data structures)

This plan provides a comprehensive roadmap from current state to full "Harmony in Diversity" experience. Adjust timelines and priorities based on team size, skills, and project deadline.

---
---

# SIMPLIFIED MVP IMPLEMENTATION PLAN

## MVP Scope & Assumptions

### Core Simplifications

1. **Visual-Only Experience:** No audio implementation (greetings, hums, music, narration)
2. **Fixed Orb Count:** Exactly 5 cultural orbs (easily scalable to more)
3. **Existing Assets Only:** Use current MysticOrb prefabs and UBCO Courtyard environment
4. **Single Scene Architecture:** All 5 narrative phases in one Unity scene with state-based transitions
5. **Simplified Data:** Hardcoded cultural data (no JSON/database needed for MVP)
6. **Controller-Only Input:** VR controllers only (no hand tracking for MVP)
7. **Basic VFX:** Unity's built-in particle systems (no custom shaders initially)

### What Makes This MVP Stand Out

Despite simplifications, the experience will showcase:
- **Innovative Thread Connection Mechanic:** Unique VR interaction pulling light between orbs
- **Progressive Visual Transformation:** Scene evolves from sparse sparkles to complex light tapestry
- **Emotional Narrative Arc:** Visual storytelling through lighting, color, and motion
- **Cultural Representation:** 5 distinct cultures with unique colors and visual symbolism
- **Satisfying Interactions:** Tactile feedback through visual effects and controller haptics

---

## MVP Feature Set

### Included Features

**Scene 1: The Courtyard Awakens**
- Dawn lighting (directional light with warm tones)
- 5 sparkle particle systems at orb spawn points
- Gaze-based brightening effect on sparkles
- Text-based narration UI (simple TextMeshPro panel)

**Scene 2: The Voices Rise**
- 5 cultural orbs spawn from sparkles with animation
- Each orb has unique color (red, blue, green, yellow, purple)
- Proximity glow effect (intensifies when player approaches)
- Touch interaction (controller raycast or grab to activate)
- Visual pulse effect on activation
- Simple text UI shows culture name on touch

**Scene 3: Connecting the Threads**
- Thread connection mechanic (pull thread from orb to orb)
- LineRenderer-based thread visualization
- Particle burst on successful connection
- Thread color blends source and target orb colors
- Connection counter UI
- Progress indicator (X/10 connections made)

**Scene 4: The Tapestry of Unity**
- Trigger when minimum connections reached (7 of 10 possible)
- Threads animate upward to form overhead canopy
- Simple geometric pattern (no complex mosaic)
- Center point where all threads converge
- Pulsing light effect at center
- Touch center to trigger bloom particle explosion

**Scene 5: Reflection**
- Orbs slowly rise and fade (simple animation)
- Final text narration UI
- Restart button returns to Scene 1 state

### Excluded Features (Future Iterations)

- ❌ Audio (all types)
- ❌ AR contribution app
- ❌ Floor mosaic with step detection
- ❌ Custom shaders
- ❌ Hand tracking
- ❌ Save/load system
- ❌ Dynamic lighting transitions
- ❌ Advanced particle effects
- ❌ Backend/database

---

## Implementation Roadmap (3 Weeks)

### Week 1: Foundation & Core Systems

**Day 1-2: Project Setup**
- [ ] Duplicate CourtyardScene1.unity → HarmonyInDiversity.unity
- [ ] Create folder structure:
  ```
  Assets/Scripts/HarmonyInDiversity/
  ├── Core/
  ├── Orbs/
  ├── Threading/
  ├── Effects/
  └── UI/
  ```
- [ ] Set up 5 spawn points in courtyard (Transform GameObjects)
- [ ] Define 5 cultural colors in code

**Day 3-4: Cultural Orb System**
- [ ] Create `CulturalOrbData.cs` ScriptableObject
- [ ] Create 5 CulturalOrbData assets (Japanese-Red, French-Blue, Indian-Green, Mexican-Yellow, Nigerian-Purple)
- [ ] Create `CulturalOrb.cs` component extending MysticOrb
- [ ] Implement proximity detection system
- [ ] Add controller interaction (XR Interaction Toolkit)
- [ ] Create visual pulse effect on activation

**Day 5-7: Scene State Manager**
- [ ] Create `HarmonySceneManager.cs` singleton
- [ ] Implement scene state enum (Awakens, VoicesRise, ConnectingThreads, Tapestry, Reflection)
- [ ] Add state transition methods
- [ ] Create simple fade transition effect
- [ ] Implement UI system for narration text
- [ ] Test state transitions manually

### Week 2: Thread Connection & Visual Effects

**Day 8-9: Thread Connection Mechanic (Critical Path)**
- [ ] Create `ThreadConnectionSystem.cs`
- [ ] Implement thread pulling:
  - Hold trigger on orb → start thread pull
  - LineRenderer follows controller position
  - Release trigger near another orb → create connection
- [ ] Create `ConnectionThread.cs` for persistent connections
- [ ] Implement connection validation (no duplicates, no self-connections)
- [ ] Add snapping visual indicator when near valid target

**Day 10-11: Thread Visuals**
- [ ] Set up LineRenderer with gradient (source color → target color)
- [ ] Add glow effect using LineRenderer width curve
- [ ] Implement Bezier curve for thread shape (slight arc)
- [ ] Create connection particle burst prefab
- [ ] Add thread pulse animation on creation
- [ ] Implement thread management (store in list, track count)

**Day 12-14: Scene Transitions**
- [ ] Implement Scene 1 (sparkles only, orbs inactive)
- [ ] Implement Scene 1→2 transition (sparkles → orbs rise)
- [ ] Implement Scene 2 active state (orbs interactable)
- [ ] Implement Scene 2→3 transition (enable thread system)
- [ ] Test full flow up to Scene 3

### Week 3: Tapestry Scene & Polish

**Day 15-16: Scene 4 Implementation**
- [ ] Detect connection threshold (7+ connections)
- [ ] Implement thread canopy animation:
  - Animate all thread endpoints upward
  - Create converging pattern to center point
  - Add central glow orb
- [ ] Create bloom particle effect for center activation
- [ ] Test Scene 3→4 transition

**Day 17-18: Scene 5 & UI**
- [ ] Implement orb ascension animation (move up, fade out)
- [ ] Create end-scene UI panel (world space canvas)
- [ ] Add restart button functionality
- [ ] Polish all UI text (narration for each scene)
- [ ] Test complete scene flow 1→5

**Day 19-21: Polish & Testing**
- [ ] Add haptic feedback for all interactions
- [ ] Improve lighting (post-processing bloom for orbs)
- [ ] Optimize particle systems
- [ ] Performance testing on Quest 2/3
- [ ] Bug fixes
- [ ] Final playtest and adjustments

---

## Detailed Implementation Specifications

### 1. Cultural Orb System

**File: `Assets/Scripts/HarmonyInDiversity/Orbs/CulturalOrbData.cs`**
```csharp
[CreateAssetMenu(fileName = "CulturalOrb", menuName = "Harmony/Cultural Orb Data")]
public class CulturalOrbData : ScriptableObject
{
    public string cultureName;      // e.g., "Japanese"
    public Color orbColor;          // Primary color
    public string greetingText;     // e.g., "Konnichiwa" (displayed in UI)
    public GameObject orbPrefab;    // Reference to MysticOrb variant
}
```

**5 Cultural Orbs (Hardcoded):**
1. **Japanese** - Red (#FF0000) - "Konnichiwa"
2. **French** - Blue (#0000FF) - "Bonjour"
3. **Indian** - Green (#00FF00) - "Namaste"
4. **Mexican** - Yellow (#FFFF00) - "Hola"
5. **Nigerian** - Purple (#800080) - "Sannu"

**File: `Assets/Scripts/HarmonyInDiversity/Orbs/CulturalOrb.cs`**
```csharp
public class CulturalOrb : MonoBehaviour
{
    public CulturalOrbData data;
    private MysticOrb mysticOrb;
    private OrbFloating orbFloating;
    private float glowIntensity = 1f;
    private bool isActivated = false;

    // State
    public List<CulturalOrb> connectedOrbs = new List<CulturalOrb>();

    // Components
    private XRGrabInteractable grabInteractable;
    private SphereCollider proximityTrigger;

    void Start()
    {
        mysticOrb = GetComponent<MysticOrb>();
        orbFloating = GetComponent<OrbFloating>();

        // Apply cultural color
        mysticOrb.SetOrbColor(data.orbColor);

        // Setup interaction
        SetupInteraction();

        // Setup proximity detection
        SetupProximityDetection();
    }

    void SetupInteraction()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
            grabInteractable = gameObject.AddComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnActivated);
    }

    void SetupProximityDetection()
    {
        proximityTrigger = gameObject.AddComponent<SphereCollider>();
        proximityTrigger.isTrigger = true;
        proximityTrigger.radius = 2f; // 2 meter proximity
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MainCamera"))
        {
            // Increase glow when player approaches
            SetGlowIntensity(2f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MainCamera"))
        {
            SetGlowIntensity(1f);
        }
    }

    void OnActivated(SelectEnterEventArgs args)
    {
        if (!isActivated)
        {
            isActivated = true;
            TriggerPulseEffect();
            HarmonyUIManager.Instance.ShowCultureInfo(data.cultureName, data.greetingText);

            // Haptic feedback
            if (args.interactorObject is XRBaseControllerInteractor controller)
            {
                controller.SendHapticImpulse(0.5f, 0.2f);
            }
        }
    }

    void TriggerPulseEffect()
    {
        // Simple scale pulse animation
        StartCoroutine(PulseAnimation());
    }

    IEnumerator PulseAnimation()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 originalScale = transform.localScale;

        while (elapsed < duration)
        {
            float scale = 1f + Mathf.Sin(elapsed / duration * Mathf.PI) * 0.3f;
            transform.localScale = originalScale * scale;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
    }

    void SetGlowIntensity(float intensity)
    {
        glowIntensity = intensity;
        mysticOrb.SetOrbColor(data.orbColor * intensity);
    }

    public void AddConnection(CulturalOrb otherOrb)
    {
        if (!connectedOrbs.Contains(otherOrb))
        {
            connectedOrbs.Add(otherOrb);
        }
    }
}
```

### 2. Thread Connection System

**File: `Assets/Scripts/HarmonyInDiversity/Threading/ThreadConnectionSystem.cs`**
```csharp
public class ThreadConnectionSystem : MonoBehaviour
{
    public static ThreadConnectionSystem Instance;

    [Header("Thread Settings")]
    public GameObject threadPrefab;
    public float threadWidth = 0.05f;
    public float snapDistance = 0.5f;
    public int maxConnectionsPerOrb = 4;

    [Header("References")]
    public XRRayInteractor rayInteractor;

    private CulturalOrb sourceOrb;
    private LineRenderer activeThreadBeam;
    private List<ConnectionThread> connections = new List<ConnectionThread>();
    private bool isPullingThread = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        if (HarmonySceneManager.Instance.CurrentState != SceneState.ConnectingThreads)
            return;

        HandleThreadPulling();
    }

    void HandleThreadPulling()
    {
        // Trigger pressed
        if (rayInteractor.selectAction.action.IsPressed() && !isPullingThread)
        {
            // Check if raycasting an orb
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                CulturalOrb orb = hit.collider.GetComponent<CulturalOrb>();
                if (orb != null)
                {
                    StartThreadPull(orb);
                }
            }
        }

        // Update thread beam position
        if (isPullingThread && activeThreadBeam != null)
        {
            Vector3 controllerPos = rayInteractor.transform.position;
            UpdateThreadBeam(sourceOrb.transform.position, controllerPos);

            // Check for snap to target
            CulturalOrb targetOrb = FindNearbyOrb(controllerPos);
            if (targetOrb != null && targetOrb != sourceOrb)
            {
                // Visual snap indicator
                // TODO: Add snap visual
            }
        }

        // Trigger released
        if (!rayInteractor.selectAction.action.IsPressed() && isPullingThread)
        {
            CompleteThreadPull();
        }
    }

    void StartThreadPull(CulturalOrb orb)
    {
        sourceOrb = orb;
        isPullingThread = true;

        // Create visual beam
        GameObject beamObj = new GameObject("ThreadBeam");
        activeThreadBeam = beamObj.AddComponent<LineRenderer>();
        activeThreadBeam.startWidth = threadWidth;
        activeThreadBeam.endWidth = threadWidth;
        activeThreadBeam.material = new Material(Shader.Find("Sprites/Default"));
        activeThreadBeam.startColor = sourceOrb.data.orbColor;
        activeThreadBeam.endColor = sourceOrb.data.orbColor;
        activeThreadBeam.positionCount = 2;
    }

    void UpdateThreadBeam(Vector3 start, Vector3 end)
    {
        activeThreadBeam.SetPosition(0, start);
        activeThreadBeam.SetPosition(1, end);
    }

    void CompleteThreadPull()
    {
        Vector3 controllerPos = rayInteractor.transform.position;
        CulturalOrb targetOrb = FindNearbyOrb(controllerPos);

        if (targetOrb != null && targetOrb != sourceOrb)
        {
            // Valid connection
            CreateConnection(sourceOrb, targetOrb);
        }

        // Cleanup
        if (activeThreadBeam != null)
            Destroy(activeThreadBeam.gameObject);

        isPullingThread = false;
        sourceOrb = null;
    }

    CulturalOrb FindNearbyOrb(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, snapDistance);
        foreach (var col in colliders)
        {
            CulturalOrb orb = col.GetComponent<CulturalOrb>();
            if (orb != null)
                return orb;
        }
        return null;
    }

    void CreateConnection(CulturalOrb orbA, CulturalOrb orbB)
    {
        // Check if connection already exists
        if (ConnectionExists(orbA, orbB))
            return;

        // Check max connections
        if (orbA.connectedOrbs.Count >= maxConnectionsPerOrb ||
            orbB.connectedOrbs.Count >= maxConnectionsPerOrb)
            return;

        // Create connection thread
        GameObject threadObj = Instantiate(threadPrefab);
        ConnectionThread thread = threadObj.AddComponent<ConnectionThread>();
        thread.Initialize(orbA, orbB, threadWidth);

        connections.Add(thread);

        // Update orbs
        orbA.AddConnection(orbB);
        orbB.AddConnection(orbA);

        // Visual feedback
        CreateConnectionBurst(orbA.transform.position, orbB.transform.position);

        // Haptic feedback
        // TODO: Trigger haptic on controller

        // Check for scene completion
        if (connections.Count >= 7) // Minimum threshold
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
        // TODO: Instantiate particle burst prefab
    }

    public List<ConnectionThread> GetAllConnections()
    {
        return connections;
    }
}
```

**File: `Assets/Scripts/HarmonyInDiversity/Threading/ConnectionThread.cs`**
```csharp
public class ConnectionThread : MonoBehaviour
{
    public CulturalOrb orbA;
    public CulturalOrb orbB;
    private LineRenderer lineRenderer;

    public void Initialize(CulturalOrb a, CulturalOrb b, float width)
    {
        orbA = a;
        orbB = b;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.positionCount = 2;

        // Blend colors
        Color blendedColor = Color.Lerp(a.data.orbColor, b.data.orbColor, 0.5f);
        lineRenderer.startColor = blendedColor;
        lineRenderer.endColor = blendedColor;

        // Material
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material.color = blendedColor;
    }

    void Update()
    {
        if (orbA != null && orbB != null)
        {
            lineRenderer.SetPosition(0, orbA.transform.position);
            lineRenderer.SetPosition(1, orbB.transform.position);
        }
    }

    public void AnimateToCanopy(Vector3 centerPoint, float duration)
    {
        StartCoroutine(MoveToCanopy(centerPoint, duration));
    }

    IEnumerator MoveToCanopy(Vector3 target, float duration)
    {
        Vector3 startA = orbA.transform.position;
        Vector3 startB = orbB.transform.position;
        Vector3 targetA = target + Vector3.up * 5f + Random.insideUnitSphere * 2f;
        Vector3 targetB = target + Vector3.up * 5f + Random.insideUnitSphere * 2f;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            lineRenderer.SetPosition(0, Vector3.Lerp(startA, targetA, t));
            lineRenderer.SetPosition(1, Vector3.Lerp(startB, targetB, t));
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
```

### 3. Scene State Management

**File: `Assets/Scripts/HarmonyInDiversity/Core/HarmonySceneManager.cs`**
```csharp
public enum SceneState
{
    CourtyardAwakens,
    VoicesRise,
    ConnectingThreads,
    TapestryOfUnity,
    Reflection
}

public class HarmonySceneManager : MonoBehaviour
{
    public static HarmonySceneManager Instance;

    [Header("Scene References")]
    public CulturalOrb[] culturalOrbs;
    public Transform[] spawnPoints;
    public ParticleSystem[] sparkleParticles;

    [Header("State")]
    public SceneState CurrentState = SceneState.CourtyardAwakens;

    [Header("Timing")]
    public float scene1Duration = 10f;
    public float scene2Duration = 30f;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        InitializeScene1();
        StartCoroutine(AutoProgressScene1());
    }

    void InitializeScene1()
    {
        // Activate sparkles
        foreach (var sparkle in sparkleParticles)
        {
            sparkle.Play();
        }

        // Hide orbs
        foreach (var orb in culturalOrbs)
        {
            orb.gameObject.SetActive(false);
        }

        // Show narration
        HarmonyUIManager.Instance.ShowNarration("Every culture is a light, waiting to be seen...", 5f);
    }

    IEnumerator AutoProgressScene1()
    {
        yield return new WaitForSeconds(scene1Duration);
        TransitionToState(SceneState.VoicesRise);
    }

    public void TransitionToState(SceneState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
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

    void InitializeScene2()
    {
        // Stop sparkles
        foreach (var sparkle in sparkleParticles)
        {
            sparkle.Stop();
        }

        // Spawn orbs with animation
        for (int i = 0; i < culturalOrbs.Length; i++)
        {
            StartCoroutine(SpawnOrbAnimation(culturalOrbs[i], spawnPoints[i].position, i * 0.5f));
        }

        HarmonyUIManager.Instance.ShowNarration("The voices of many cultures rise...", 5f);

        // Auto-transition after duration
        StartCoroutine(AutoProgressScene2());
    }

    IEnumerator SpawnOrbAnimation(CulturalOrb orb, Vector3 targetPos, float delay)
    {
        yield return new WaitForSeconds(delay);

        orb.gameObject.SetActive(true);
        orb.transform.position = targetPos - Vector3.up * 2f;

        // Animate upward
        float duration = 2f;
        float elapsed = 0f;
        Vector3 startPos = orb.transform.position;

        while (elapsed < duration)
        {
            orb.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        orb.transform.position = targetPos;
    }

    IEnumerator AutoProgressScene2()
    {
        yield return new WaitForSeconds(scene2Duration);
        TransitionToState(SceneState.ConnectingThreads);
    }

    void InitializeScene3()
    {
        HarmonyUIManager.Instance.ShowNarration("Connect the threads of culture...", 5f);
        // Thread system is now active (handled in ThreadConnectionSystem.Update)
    }

    void InitializeScene4()
    {
        HarmonyUIManager.Instance.ShowNarration("A tapestry of unity emerges...", 5f);

        // Animate threads to canopy
        var threads = ThreadConnectionSystem.Instance.GetAllConnections();
        Vector3 centerPoint = Vector3.zero;
        foreach (var orb in culturalOrbs)
        {
            centerPoint += orb.transform.position;
        }
        centerPoint /= culturalOrbs.Length;
        centerPoint += Vector3.up * 3f;

        foreach (var thread in threads)
        {
            thread.AnimateToCanopy(centerPoint, 3f);
        }

        // Create center orb
        CreateCenterOrb(centerPoint);

        // Auto-transition after 20 seconds
        StartCoroutine(AutoProgressScene4());
    }

    void CreateCenterOrb(Vector3 position)
    {
        // TODO: Instantiate large central orb with bloom effect
    }

    IEnumerator AutoProgressScene4()
    {
        yield return new WaitForSeconds(20f);
        TransitionToState(SceneState.Reflection);
    }

    void InitializeScene5()
    {
        HarmonyUIManager.Instance.ShowNarration("Together, we weave the colors of humanity...", 7f);

        // Animate orbs rising and fading
        foreach (var orb in culturalOrbs)
        {
            StartCoroutine(OrbAscension(orb));
        }

        // Show end UI after 10 seconds
        StartCoroutine(ShowEndUI());
    }

    IEnumerator OrbAscension(CulturalOrb orb)
    {
        float duration = 5f;
        float elapsed = 0f;
        Vector3 startPos = orb.transform.position;
        Vector3 endPos = startPos + Vector3.up * 10f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            orb.transform.position = Vector3.Lerp(startPos, endPos, t);

            // Fade out (assuming material has transparency)
            // TODO: Fade material alpha

            elapsed += Time.deltaTime;
            yield return null;
        }

        orb.gameObject.SetActive(false);
    }

    IEnumerator ShowEndUI()
    {
        yield return new WaitForSeconds(10f);
        HarmonyUIManager.Instance.ShowEndScreen();
    }

    public void RestartExperience()
    {
        // Reset everything
        foreach (var orb in culturalOrbs)
        {
            orb.gameObject.SetActive(false);
            orb.connectedOrbs.Clear();
        }

        var threads = ThreadConnectionSystem.Instance.GetAllConnections();
        foreach (var thread in threads)
        {
            Destroy(thread.gameObject);
        }
        threads.Clear();

        CurrentState = SceneState.CourtyardAwakens;
        InitializeScene1();
        StartCoroutine(AutoProgressScene1());
    }
}
```

### 4. UI System

**File: `Assets/Scripts/HarmonyInDiversity/UI/HarmonyUIManager.cs`**
```csharp
public class HarmonyUIManager : MonoBehaviour
{
    public static HarmonyUIManager Instance;

    [Header("UI Panels")]
    public Canvas mainCanvas;
    public TextMeshProUGUI narrationText;
    public TextMeshProUGUI cultureInfoText;
    public GameObject endScreenPanel;
    public Button restartButton;

    void Awake()
    {
        if (Instance == null) Instance = this;

        // Setup
        narrationText.gameObject.SetActive(false);
        cultureInfoText.gameObject.SetActive(false);
        endScreenPanel.SetActive(false);

        restartButton.onClick.AddListener(OnRestartClicked);
    }

    public void ShowNarration(string text, float duration)
    {
        StartCoroutine(ShowTextTemporary(narrationText, text, duration));
    }

    public void ShowCultureInfo(string cultureName, string greeting)
    {
        StartCoroutine(ShowTextTemporary(cultureInfoText, $"{cultureName}\n{greeting}", 3f));
    }

    IEnumerator ShowTextTemporary(TextMeshProUGUI textComponent, string text, float duration)
    {
        textComponent.text = text;
        textComponent.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        textComponent.gameObject.SetActive(false);
    }

    public void ShowEndScreen()
    {
        endScreenPanel.SetActive(true);
    }

    void OnRestartClicked()
    {
        endScreenPanel.SetActive(false);
        HarmonySceneManager.Instance.RestartExperience();
    }
}
```

---

## Asset Checklist

### Required Assets (All Existing)

**3D Models:**
- [x] MysticOrb.prefab (use for all 5 cultural orbs)
- [x] UBCO Courtyard environment
- [x] XR Rig (player)

**Materials:**
- [ ] 5 material variants of MysticOrb material (one per culture color)
- [ ] Thread material (simple emissive)

**Particle Systems:**
- [ ] Sparkle particle (modify existing or create simple one)
- [ ] Connection burst particle (simple radial burst)

**UI:**
- [ ] World space canvas with TextMeshPro
- [ ] Simple button prefab

### Setup Tasks

1. **Create 5 Orb Variants:**
   - Duplicate MysticOrb.prefab 5 times
   - Name: JapaneseOrb, FrenchOrb, IndianOrb, MexicanOrb, NigerianOrb
   - Adjust material emission color for each

2. **Scene Setup:**
   - Place 5 spawn point transforms in courtyard
   - Position them in a circle or meaningful pattern
   - Add sparkle particle systems at each spawn point

3. **XR Interaction Setup:**
   - Ensure XR Rig has XRRayInteractor on controllers
   - Configure input actions for trigger press

---

## Testing Checklist

### Functionality Tests

**Scene 1:**
- [ ] Sparkles appear at all 5 spawn points
- [ ] Narration text displays correctly
- [ ] Auto-transitions to Scene 2 after 10 seconds

**Scene 2:**
- [ ] All 5 orbs spawn with animation
- [ ] Each orb has correct color
- [ ] Proximity glow works when approaching orbs
- [ ] Touching orb shows culture info text
- [ ] Auto-transitions to Scene 3 after 30 seconds

**Scene 3:**
- [ ] Can select orb with controller raycast
- [ ] Thread beam appears and follows controller
- [ ] Thread snaps to nearby orb
- [ ] Connection creates permanent thread between orbs
- [ ] Connection count increases
- [ ] Transitions to Scene 4 at 7 connections

**Scene 4:**
- [ ] Threads animate upward to form canopy
- [ ] Center point is visible
- [ ] Auto-transitions to Scene 5 after 20 seconds

**Scene 5:**
- [ ] Orbs rise and fade
- [ ] End screen UI appears
- [ ] Restart button resets experience

### Performance Tests

- [ ] Maintains 72 FPS on Quest 2
- [ ] No stuttering during thread creation
- [ ] Smooth orb animations
- [ ] Particle systems don't cause lag

### VR Comfort Tests

- [ ] No excessive motion blur
- [ ] UI is readable in VR
- [ ] Interactions feel responsive
- [ ] Haptic feedback is not overwhelming

---

## Development Tips

### Quick Testing
- Add keyboard shortcuts to jump between scenes (for Unity editor testing)
- Create a debug menu to manually trigger state transitions
- Log connection count to console

### Common Pitfalls
- Ensure XR Interaction Toolkit is properly configured
- Check that orb colliders don't interfere with thread connection
- Use world space canvas for VR UI (not screen space)
- Test on device early and often (performance differs from editor)

### Optimization
- Use object pooling for particle effects
- Disable orb physics when not needed
- Reduce LineRenderer resolution for threads (5-10 points max)
- Bake lighting for courtyard environment

---

## MVP Deliverables

1. **Working VR Experience:**
   - 5-scene narrative flow
   - Thread connection mechanic functional
   - Visual progression from sparse to rich

2. **Demo Video:**
   - Record 3-minute playthrough
   - Show all 5 scenes
   - Highlight thread connection interaction

3. **Documentation:**
   - This implementation plan
   - Setup instructions for testing
   - Known limitations

---

## Timeline Summary

- **Week 1:** Foundation, orb system, scene manager
- **Week 2:** Thread connection (critical), visual effects
- **Week 3:** Tapestry scene, polish, testing

**Total Development Time:** 3 weeks (60-80 hours)

**Recommended Team Size:** 2-3 developers

---

## Post-MVP Expansion Path

Once MVP is complete, prioritize these additions:

1. **Audio Layer** (Week 4):
   - Add cultural greetings (recorded audio)
   - Ambient soundscape
   - Simple background music

2. **Advanced Visuals** (Week 5):
   - Custom shaders for orbs and threads
   - More complex particle effects
   - Dynamic lighting transitions

3. **Floor Mosaic** (Week 6):
   - Implement Voronoi mosaic generation
   - Add step detection
   - Culture harmony swell effect

4. **Scalability** (Week 7):
   - Support 10-20 orbs
   - Dynamic spawn positioning
   - JSON-based cultural data

5. **AR App** (Weeks 8-10):
   - Build contribution app
   - Implement data pipeline
   - Test integration

---

## Success Criteria for MVP

The MVP is successful if:

1. ✅ All 5 scenes flow without errors
2. ✅ Thread connection mechanic is intuitive and satisfying
3. ✅ Visual narrative is emotionally engaging
4. ✅ Experience runs smoothly on Quest 2 (72 FPS)
5. ✅ Can be completed in 5-10 minutes
6. ✅ Demonstrates unique value of VR for cultural storytelling

---

**This simplified plan maintains the core vision of "Harmony in Diversity" while providing a clear, achievable path to a working MVP in 3 weeks.**
