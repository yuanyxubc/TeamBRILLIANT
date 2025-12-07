# Harmony in Diversity - Project Summary
## VR Cultural Experience on Quest 2

---

## PROJECT OVERVIEW

**Project Name:** Harmony in Diversity

**Platform:** Meta Quest 2 (Android XR)

**Development Environment:** Unity 6 (6000.2.4f1)

**Rendering Pipeline:** Universal Render Pipeline (URP)

**Project Type:** Immersive VR Narrative Experience

**Team:** TeamBRILLIANT

**Duration:** 6 weeks of development

---

## PROJECT CONCEPT

### Vision
An immersive AR/VR narrative experience celebrating cultural diversity through interactive storytelling, where users discover, connect, and celebrate voices from different cultures in a virtual UBCO courtyard environment.

### Core Theme
"Harmony in Diversity" explores how different cultures, like threads of light, can be woven together into a unified tapestry of human connection and understanding.

### Target Audience
- University students
- Cultural awareness programs
- General public interested in diversity and inclusion
- VR experience enthusiasts

---

## TECHNICAL ARCHITECTURE

### Development Stack

**Game Engine:**
- Unity 6000.2.4f1 (Unity 6)

**XR Framework:**
- XR Interaction Toolkit 3.2.1
- XR Hands 1.6.1
- AR Foundation 6.2.0
- Android XR OpenXR 1.0.1

**Graphics:**
- Universal Render Pipeline (URP) 17.2.0
- Custom shader implementation for visual effects
- Real-time lighting and post-processing

**Audio:**
- Unity Audio System with custom Audio Mixer
- Spatial 3D audio for immersive sound
- Dynamic audio layering system

**Input System:**
- Unity's New Input System 1.14.2
- XR controller input via CommonUsages API
- Direct hardware trigger detection for reliability

---

## SYSTEM ARCHITECTURE

### Core Systems

1. **Scene Management System**
   - Centralized scene flow controller (HarmonySceneManager)
   - State-based scene transitions
   - Automatic and manual progression modes
   - 5 distinct narrative scenes

2. **Audio Management System**
   - HarmonyAudioManager singleton
   - Audio Mixer with 5 channels (Ambient, Orbs, Music, Narration, SFX)
   - Spatial 3D audio for cultural orbs
   - Volume control and mixing

3. **Cultural Orb System**
   - 5 cultural orbs representing different cultures
   - Each orb contains:
     - Unique color identity
     - Cultural greeting (audio)
     - Cultural ambient sound (looping)
     - Proximity-based glow effects
     - Touch interaction feedback

4. **Threading Connection System**
   - Physics-based raycast detection
   - Direct XR controller input reading
   - Real-time LineRenderer visualization
   - Connection validation and tracking
   - Dynamic visual feedback

5. **UI Management System**
   - World-space VR-compatible UI
   - Text narration display
   - Connection counter
   - End scene buttons (Restart/Exit)
   - XR UI interaction support

---

## IMPLEMENTED FEATURES

### ✅ Scene 1: The Courtyard Awakens
**Status:** COMPLETE

**Features:**
- Dawn lighting atmosphere with warm directional light
- Particle sparkles at future orb spawn locations
- Gaze detection on sparkles (brightness increase)
- Opening narration (audio + text)
- Automatic transition to Scene 2

**Audio:**
- Ambient environmental sounds
- Opening narration voice-over

**Duration:** 10 seconds (configurable)

---

### ✅ Scene 2: The Voices Rise
**Status:** COMPLETE

**Features:**
- 5 cultural orbs spawn with animation
- Each orb has unique color representing a culture:
  - Japanese (Red)
  - French (Blue)
  - Indian (Green)
  - Mexican (Yellow)
  - Nigerian (Purple)
- Spatial 3D audio from each orb
- Proximity-based glow intensity
- Touch interaction plays cultural greeting
- XR Near-Far Interactor for grabbing
- Scene narration

**Audio:**
- Cultural greeting recordings (per orb)
- Cultural ambient hums (looping per orb)
- Spatial 3D audio with distance-based rolloff
- Scene transition narration

**Duration:** 30 seconds (configurable) + manual exploration time

**Cultures Represented:**
1. Japanese - "Konnichiwa" + traditional Japanese music
2. French - "Bonjour" + French cultural sounds
3. Indian - "Namaste" + Indian traditional music
4. Mexican - "Hola" + Mexican cultural sounds
5. Nigerian - "Sannu" + Nigerian traditional music

---

### ✅ Scene 3: Connecting the Threads
**Status:** COMPLETE

**Features:**
- Thread connection mechanic (core interaction)
- Point controller at orb → Hold trigger → Pull thread
- Thread beam visualization with LineRenderer
- Physics-based raycast detection (not XR Interaction Toolkit dependent)
- Direct XR controller trigger detection via CommonUsages API
- Snap detection when near target orb (visual + audio feedback)
- Color-blended threads between orbs
- Connection counter UI
- Minimum 7 connections required for progression
- Near-Far Interactor disabled to prevent interference

**Audio:**
- Thread pull start sound (whoosh/shimmer)
- Thread snap sound (when near target orb)
- Connection success sound (chime/bell)
- Scene narration

**Technical Innovations:**
- Switched from XR Interaction Toolkit raycasts to Physics.Raycast for reliability
- Direct hardware input reading bypasses Input Action system issues
- Dynamic shader selection with multiple fallbacks
- Smart snap detection prevents audio spam

**Duration:** Manual (waits for connection threshold)

---

### ✅ Scene 4: The Tapestry of Unity
**Status:** COMPLETE

**Features:**
- Thread canopy formation (threads weave upward)
- All threads animate to overhead position
- Center convergence point
- Particle effects (optional)
- Scene narration
- Automatic transition to Scene 5

**Audio:**
- Scene narration
- Ambient soundscape

**Duration:** 20 seconds (configurable)

---

### ✅ Scene 5: Reflection
**Status:** COMPLETE

**Features:**
- Orbs ascend and fade animation
- Closing narration (audio + text)
- End scene UI with buttons:
  - Restart button (restarts experience)
  - Exit button (quits application)
- VR-compatible button interaction
- Near-Far Interactor re-enabled for UI

**Audio:**
- Closing narration voice-over

**Duration:** Manual (user-controlled via buttons)

---

## AUDIO SYSTEM

### Architecture

**Audio Mixer Channels:**
1. Master (controls all)
2. Ambient (environmental sounds)
3. Orbs (greetings + hums)
4. Music (background music - optional)
5. Narration (voice-over)
6. SFX (sound effects)

### Spatial Audio Configuration
- 3D Audio Sources on each orb
- Custom rolloff curve (logarithmic)
- Max distance: 20 meters
- Min distance: 1 meter
- Doppler level: 0 (orbs are stationary)

### Audio Assets
- **Cultural Greetings:** 5 clips (one per culture)
- **Cultural Hums:** 5 looping clips
- **Narration:** 5 clips (one per scene)
- **Thread SFX:** 3 clips (pull, snap, success)
- **Ambient:** 2-3 clips (dawn, day atmosphere)

### Audio Specifications
- Format: WAV/OGG Vorbis
- Sample Rate: 44.1kHz
- Bit Depth: 16-bit
- Compression: Vorbis (70-80% quality)
- Spatial Blend: 1.0 (fully 3D for orbs)

---

## VISUAL EFFECTS

### Orb Visual System
- Base orb model (MysticOrb prefab)
- Dynamic color assignment per culture
- Pulsing glow effect on interaction
- Proximity-based brightness
- Particle effects (optional)

### Thread Visualization
- LineRenderer with dynamic color blending
- Width: 0.05 meters
- Shader: Unlit/Color (with fallbacks)
- Real-time position tracking
- Smooth bezier curve (configurable)

### Lighting
- Scene 1: Dawn (warm orange directional light)
- Scene 2-3: Day (neutral lighting)
- Scene 4-5: Dusk/Night (cool blue ambient)
- Post-processing: Bloom for glow effects

---

## KEY TECHNICAL CHALLENGES & SOLUTIONS

### Challenge 1: Thread Connection Not Working in VR
**Problem:** Threading system worked in Unity Editor but not on Quest 2. No threads appeared, no controller response.

**Root Causes:**
1. XRGrabInteractable disabled in Scene 3 made orbs invisible to XR raycasts
2. XR Interaction Toolkit raycast only works with enabled interactables
3. URP shader not included in Quest 2 build (shader null error)
4. Near-Far Interactor intercepting trigger input

**Solutions:**
1. Kept XRGrabInteractable enabled but prevented grab behavior via event handling
2. Switched to Physics.Raycast instead of XR Interaction Toolkit raycasts
3. Implemented shader fallback system (Unlit/Color → Sprites/Default → Hidden/Internal-Colored)
4. Disabled Near-Far Interactor during Scene 3, re-enabled in other scenes
5. Added direct XR controller input detection via UnityEngine.XR.InputDevices API

**Result:** Threading system now works perfectly in VR with reliable trigger detection and visible thread beams.

---

### Challenge 2: UI Buttons Not Clickable in VR
**Problem:** Restart/Exit buttons worked with mouse in Editor but not with VR controller trigger on Quest 2.

**Root Cause:**
Near-Far Interactor disabled in Scene 3 was never re-enabled in Scene 5.

**Solution:**
Added EnableNearFarInteractors() call to Scene 5 initialization.

**Result:** Controller ray pointer visible, buttons fully functional in VR.

---

### Challenge 3: Audio Not Playing in VR
**Problem:** Cultural orb audio and narration not playing on Quest 2 device.

**Root Cause:**
Audio system not properly initialized, mixer groups not assigned, spatial audio not configured.

**Solution:**
1. Created HarmonyAudioManager singleton
2. Implemented audio mixer with proper channel routing
3. Configured spatial audio settings for Quest 2
4. Updated CulturalOrbData with audio fields
5. Added audio playback to CulturalOrb and scene transitions

**Result:** Full spatial audio system with greetings, hums, narration, and SFX.

---

## PERFORMANCE OPTIMIZATION

### Target Performance
- **Frame Rate:** 72 FPS (Quest 2 minimum)
- **Draw Calls:** < 150 per frame
- **Triangles:** < 100k visible
- **Audio Sources:** < 20 simultaneous

### Optimization Techniques
1. **Shader Optimization:**
   - Simple unlit shaders for threads and UI
   - Minimal shader complexity for Quest 2

2. **Audio Optimization:**
   - Vorbis compression (70-80% quality)
   - Streaming for long clips
   - Compressed in memory for short clips
   - Spatial audio with custom rolloff

3. **Rendering:**
   - URP with optimized settings
   - Occlusion culling
   - LOD system (future enhancement)
   - Particle system pooling (future enhancement)

4. **Memory Management:**
   - Lazy instantiation of orbs (created in Scene 2, not Scene 1)
   - Audio clip compression
   - Texture atlasing (future)

---

## CODE ARCHITECTURE

### Key Scripts

**Core Systems:**
- `HarmonySceneManager.cs` - Scene flow and state management
- `HarmonyAudioManager.cs` - Audio system controller
- `HarmonyUIManager.cs` - UI display and interaction

**Cultural Orb System:**
- `CulturalOrbData.cs` - ScriptableObject for orb data
- `CulturalOrb.cs` - Orb behavior and interaction
- `MysticOrb.cs` - Base orb visual effects
- `OrbFloating.cs` - Floating animation

**Threading System:**
- `ThreadConnectionSystem.cs` - Main threading controller
- `ConnectionThread.cs` - Individual thread representation

**UI System:**
- `VRUIHelper.cs` - VR UI configuration helper

**Data:**
- `SceneState.cs` - Enum for scene states
- `NarrationData.cs` - ScriptableObject for narration clips

### Design Patterns Used
1. **Singleton Pattern:** HarmonySceneManager, HarmonyAudioManager, ThreadConnectionSystem
2. **ScriptableObject Pattern:** CulturalOrbData, NarrationData
3. **Component-Based Architecture:** CulturalOrb with modular components
4. **State Machine:** Scene progression system
5. **Observer Pattern:** Event-based UI updates

---

## DEVELOPMENT TIMELINE

### Week 1-2: Foundation & Setup
- Unity project setup
- XR Interaction Toolkit integration
- Scene 1 & 2 basic implementation
- Cultural orb spawning system

### Week 3-4: Core Threading Mechanic
- Thread connection system design
- Initial implementation (Editor-only)
- VR testing and debugging
- Physics.Raycast solution implementation

### Week 5: Audio System
- Audio manager implementation
- Cultural audio integration
- Narration system
- Thread SFX
- Spatial audio configuration

### Week 6: Polish & Bug Fixes
- VR UI interaction fixes
- Shader compatibility fixes
- Performance optimization
- Scene transitions polish
- Final testing and deployment

---

## PROJECT STATISTICS

### Codebase
- **Total Scripts:** ~20 C# scripts
- **Lines of Code:** ~3,000+ lines
- **ScriptableObjects:** 7+ data assets
- **Scenes:** 1 main scene with 5 states

### Assets
- **3D Models:** UBCO Courtyard + 5 orb variants
- **Audio Clips:** 15-20 clips (greetings, hums, narration, SFX)
- **Materials:** Custom orb materials with unique colors
- **Prefabs:** Cultural orb prefabs, particle effects

### Unity Packages
- XR Interaction Toolkit (3.2.1)
- XR Hands (1.6.1)
- AR Foundation (6.2.0)
- Android XR OpenXR (1.0.1)
- URP (17.2.0)
- New Input System (1.14.2)
- Cinemachine (2.10.4)

---

## CULTURAL REPRESENTATION

### Represented Cultures
1. **Japanese**
   - Color: Red
   - Greeting: "Konnichiwa"
   - Cultural sound: Traditional Japanese instruments

2. **French**
   - Color: Blue
   - Greeting: "Bonjour"
   - Cultural sound: French cultural music

3. **Indian**
   - Color: Green
   - Greeting: "Namaste"
   - Cultural sound: Indian traditional instruments

4. **Mexican**
   - Color: Yellow
   - Greeting: "Hola"
   - Cultural sound: Mexican cultural music

5. **Nigerian**
   - Color: Purple
   - Greeting: "Sannu" (Hausa)
   - Cultural sound: Nigerian traditional music

### Cultural Sensitivity
- Authentic representations sought through research
- Community involvement planned for future iterations
- Respectful portrayal of cultural elements
- Educational purpose: promoting cultural awareness and unity

---

## TESTING & VALIDATION

### Testing Environments
1. **Unity Editor (Play Mode)**
   - Rapid iteration and debugging
   - Mouse/keyboard controls for testing
   - Visual debugging tools

2. **Quest 2 Device**
   - Real-world VR testing
   - Performance validation
   - User experience testing
   - Input system validation

### Testing Methodology
- Iterative development with frequent VR testing
- ADB logcat for real-time debugging on device
- Performance profiling with Unity Profiler
- User experience testing (informal)

### Known Issues (Resolved)
- ✅ Threading not working in VR (FIXED)
- ✅ Shader not found error (FIXED)
- ✅ UI buttons not clickable (FIXED)
- ✅ Audio not playing (FIXED)
- ✅ Trigger input detection (FIXED)

---

## USER EXPERIENCE FLOW

### Complete Experience Duration
**Total Time:** 5-10 minutes

**Scene Breakdown:**
1. Scene 1: 10 seconds (automatic)
2. Scene 2: 30 seconds + exploration time
3. Scene 3: 2-5 minutes (manual threading)
4. Scene 4: 20 seconds (automatic)
5. Scene 5: Variable (user-controlled)

### Interaction Paradigm
- **Passive Observation:** Scene 1, 4, 5
- **Active Exploration:** Scene 2
- **Creative Interaction:** Scene 3 (core mechanic)

### Narrative Arc
1. **Introduction:** Mysterious courtyard, sparkles of light
2. **Discovery:** Orbs appear, each carrying a cultural voice
3. **Interaction:** Player connects cultures through light threads
4. **Transformation:** Threads weave into unified tapestry
5. **Reflection:** Experience concludes with hopeful message

---

## ACCESSIBILITY FEATURES

### Current Implementation
- Text narration (visual backup for audio)
- Adjustable audio volumes per channel
- Multiple interaction modes (controller + hand tracking support)
- Clear visual feedback for all interactions
- Generous snap distance for threading (1.5m)

### Future Enhancements
- Subtitle system for all narration
- Colorblind-friendly palette options
- Seated/standing mode options
- Difficulty settings (connection requirements)

---

## TECHNICAL SPECIFICATIONS

### Hardware Requirements
**Minimum:**
- Meta Quest 2
- 6GB RAM
- 500MB storage

**Recommended:**
- Meta Quest 3
- Good lighting for hand tracking (optional)

### Build Settings
- **Platform:** Android
- **Graphics API:** OpenGL ES 3.2 / Vulkan
- **Target SDK:** Android 10+
- **Texture Compression:** ASTC
- **Stereo Rendering:** Multi-pass

### Performance Metrics (Actual)
- **Frame Rate:** 72 FPS (stable on Quest 2)
- **Memory Usage:** ~400MB
- **Build Size:** ~150-200MB
- **Load Time:** < 5 seconds

---

## FUTURE ENHANCEMENTS

### Planned Features (Not Yet Implemented)

1. **Floor Mosaic (Scene 4)**
   - Procedurally generated mosaic from thread connections
   - Step detection with audio swell per culture
   - Visual glow effect on active cells

2. **Dynamic Music System**
   - Layered music that builds with connections
   - Adaptive soundtrack
   - Harmonic blend of cultural sounds

3. **AR Contribution App**
   - Mobile AR app for contributing cultural voices
   - Geolocation-based courtyard detection
   - User-generated cultural content
   - Real-time sync with VR experience

4. **Additional Features:**
   - Multiplayer support (multiple users in same VR space)
   - More cultures (scale to 10-20 orbs)
   - Tutorial scene for first-time users
   - Achievement system
   - Photo mode for sharing experiences

5. **Advanced Visual Effects:**
   - Custom shaders for orbs and threads
   - More complex particle systems
   - Dynamic lighting system
   - Post-processing effects

---

## LESSONS LEARNED

### Technical Insights

1. **XR Development Complexity:**
   - Editor behavior differs significantly from device behavior
   - Always test on actual hardware early and often
   - VR-specific debugging requires ADB logcat

2. **Input System Challenges:**
   - XR Interaction Toolkit has limitations
   - Direct hardware input reading (CommonUsages) more reliable
   - Multiple fallback systems increase robustness

3. **Shader Compatibility:**
   - URP shaders not always included in builds
   - Implement shader fallback systems
   - Test with simple built-in shaders first

4. **Audio in VR:**
   - Spatial audio requires careful configuration
   - Audio mixing crucial for immersion
   - Test audio balance in actual headset (sounds different than speakers)

5. **UI in VR:**
   - World space canvas essential
   - XR UI Input Module required
   - Near-Far vs Ray Interactor conflicts need management

### Development Best Practices

1. **Iterative Testing:**
   - Build to device frequently
   - Don't trust Editor Play mode for VR
   - Use ADB logcat for debugging

2. **Modular Architecture:**
   - Singleton managers for global systems
   - ScriptableObjects for data
   - Component-based design for flexibility

3. **State Management:**
   - Centralized scene management
   - Clear state transitions
   - Enable/disable systems per scene state

4. **Debugging Strategies:**
   - Comprehensive Debug.Log statements
   - Visual debugging in VR (gizmos, rays)
   - Performance profiling on device

---

## PROJECT IMPACT & SIGNIFICANCE

### Educational Value
- Promotes cultural awareness and understanding
- Interactive learning about diversity
- Experiential education through VR
- Encourages empathy and connection

### Technical Innovation
- Novel threading mechanic for VR
- Robust input detection solution
- Spatial audio implementation
- Cultural representation in VR

### Research Contribution
- Demonstrates VR for social good
- Cultural heritage preservation
- Human-computer interaction research
- Immersive storytelling techniques

---

## CONCLUSION

"Harmony in Diversity" successfully demonstrates how VR technology can create meaningful experiences that celebrate cultural diversity. Through innovative threading mechanics, spatial audio, and immersive storytelling, the project creates a unique platform for cultural connection and understanding.

The project overcame significant technical challenges in VR development, particularly around input detection, audio systems, and cross-platform compatibility. The final implementation runs smoothly on Quest 2 hardware, providing users with an engaging 5-10 minute experience that leaves a lasting impression about the beauty of cultural diversity and human connection.

Key achievements include:
- Fully functional VR threading mechanic
- Comprehensive spatial audio system
- Five distinct narrative scenes
- Cultural representation of 5 world cultures
- Robust input detection and interaction systems
- Professional narration and sound design
- Polished user experience from start to finish

The project serves as a foundation for future enhancements, including AR integration, multiplayer support, and expanded cultural representation. It demonstrates the potential of VR as a medium for promoting diversity, inclusion, and human connection.

---

## ACKNOWLEDGMENTS

**Development Tools:**
- Unity Technologies (Unity 6)
- Meta (Quest 2 platform)
- XR Interaction Toolkit team

**Audio Resources:**
- TTSMaker (narration generation)
- Freesound.org (sound effects)
- Free Music Archive (cultural music)

**Technical Support:**
- Unity documentation
- XR Interaction Toolkit documentation
- Claude Code (development assistance)

**Cultural Inspiration:**
- UBCO community
- Global cultural heritage

---

## CONTACT & LINKS

**Project:** Harmony in Diversity
**Team:** TeamBRILLIANT
**Platform:** Meta Quest 2
**Engine:** Unity 6

**Repository:** [Include Git repository link if applicable]
**Documentation:** Available in project CLAUDE.md
**Demo Video:** [Include if available]

---

**END OF SUMMARY**

---

## APPENDIX: TECHNICAL DETAILS FOR DEVELOPERS

### Key File Structure
```
Assets/
├── Scenes/
│   └── CourtyardScene1.unity (main scene)
├── Scripts/
│   └── HarmonyInDiversity/
│       ├── Core/
│       │   ├── HarmonySceneManager.cs
│       │   └── SceneState.cs
│       ├── Audio/
│       │   ├── HarmonyAudioManager.cs
│       │   └── NarrationData.cs
│       ├── Orbs/
│       │   ├── CulturalOrb.cs
│       │   ├── CulturalOrbData.cs
│       │   └── MysticOrb.cs
│       ├── Threading/
│       │   ├── ThreadConnectionSystem.cs
│       │   └── ConnectionThread.cs
│       └── UI/
│           ├── HarmonyUIManager.cs
│           └── VRUIHelper.cs
├── Prefabs/
│   └── CulturalOrbs/ (5 orb prefabs)
├── Audio/
│   ├── Narration/ (5 narration clips)
│   ├── Greetings/ (5 greeting clips)
│   ├── Hums/ (5 cultural hum clips)
│   └── SFX/ (thread connection sounds)
└── Materials/
    └── OrbMaterials/ (5 colored materials)
```

### Critical Code Snippets

**Direct XR Input Detection:**
```csharp
UnityEngine.XR.InputDevice rightController =
    UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand);
if (rightController.isValid)
{
    float triggerValue;
    if (rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out triggerValue))
    {
        if (triggerValue > 0.5f) { /* Trigger pressed */ }
    }
}
```

**Physics Raycast for VR:**
```csharp
Ray controllerRay = new Ray(rayInteractor.transform.position, rayInteractor.transform.forward);
if (Physics.Raycast(controllerRay, out RaycastHit hit, 100f))
{
    CulturalOrb orb = hit.collider.GetComponentInParent<CulturalOrb>();
    // Process orb interaction
}
```

**Shader Fallback System:**
```csharp
Shader lineShader = Shader.Find("Unlit/Color");
if (lineShader == null)
    lineShader = Shader.Find("Sprites/Default");
if (lineShader == null)
    lineShader = Shader.Find("Hidden/Internal-Colored");
```

### Build Configuration
- **Unity Version:** 6000.2.4f1
- **Build Target:** Android (ARM64)
- **API Compatibility:** .NET Standard 2.1
- **Scripting Backend:** IL2CPP
- **Target SDK:** Android 10.0 (API 29)
- **Minimum SDK:** Android 7.0 (API 24)
