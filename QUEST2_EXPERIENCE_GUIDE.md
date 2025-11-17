# QUEST 2 EXPERIENCE GUIDE

Complete walkthrough of what to expect when running "Harmony in Diversity" on Meta Quest 2.

**Total Duration:** 2-5 minutes per playthrough
**Target Device:** Meta Quest 2
**Experience Type:** Solo VR narrative journey

---

## TABLE OF CONTENTS

1. [Launching the App](#launching-the-app)
2. [Scene 1: The Courtyard Awakens](#scene-1-the-courtyard-awakens-0000---0010)
3. [Scene 2: The Voices Rise](#scene-2-the-voices-rise-0010---0040)
4. [Scene 3: Connecting the Threads](#scene-3-connecting-the-threads-0040---variable)
5. [Scene 4: The Tapestry of Unity](#scene-4-the-tapestry-of-unity-after-7-connections)
6. [Scene 5: Reflection Beneath the Light](#scene-5-reflection-beneath-the-light-final)
7. [Controller Reference](#controller-reference)
8. [Testing Checklist](#testing-checklist)
9. [Troubleshooting](#troubleshooting)

---

## LAUNCHING THE APP

### Step 1: Find Your App

1. **Put on Quest 2 headset**
2. Press **Oculus Button** on right controller (the one with Meta/Oculus logo)
3. Navigate to **App Library** (icon at bottom)
4. Click the **filter dropdown** in top right corner
5. Select **"Unknown Sources"** (Unity apps appear here)
6. Scroll to find **"Harmony in Diversity"**

### Step 2: Launch

1. **Click on the app icon** with your controller ray/pointer
2. App will load (may take 5-10 seconds)
3. You'll spawn in the courtyard scene

---

## SCENE 1: The Courtyard Awakens (00:00 - 00:10)

### 🌅 What You'll See

**Environment:**
- UBCO Courtyard at dawn
- Warm orange/golden lighting
- Soft atmospheric fog
- Gentle ambient lighting

**Visual Elements:**
- **5 sparkle particle effects** floating at different positions
- Sparkles are small, twinkling lights
- Positioned in a circle or pattern around you
- Gentle floating/swaying motion

**UI/Narration:**
- Text appears in front of you (world space)
- Message: *"Every culture is a light, waiting to be seen..."*
- Text fades in and out smoothly

### 🎮 What You Can Do

**Movement:**
- **Look around** freely (360 degrees)
- **Walk around** physically (guardian permitting)
- Observe the sparkle locations
- Take in the courtyard environment

**Passive Experience:**
- No interaction required
- Just observe and prepare for what's coming
- Let the narration set the mood

### ⏱️ Duration

**10 seconds** - Automatically transitions to Scene 2

### ✅ Verification Checklist

- [ ] You spawned in the courtyard
- [ ] Controllers are visible and tracking correctly
- [ ] You see 5 sparkle particle effects
- [ ] Narration text is visible and readable
- [ ] Dawn lighting is warm and pleasant
- [ ] No black screens or errors
- [ ] Experience feels smooth (72 FPS)

### 🐛 Potential Issues

**If sparkles are missing:**
- Check console logs - sparkle particles may not be assigned
- Verify ParticleSystems are set to "Play on Awake"

**If text is invisible:**
- Canvas may not have Event Camera assigned
- Check text position is in front of spawn point

---

## SCENE 2: The Voices Rise (00:10 - 00:40)

### 🌟 What You'll See

**Transition:**
- Sparkles fade out/stop emitting
- Brief pause (1-2 seconds)

**Orb Spawn Animation:**
- **5 colorful orbs rise from the ground** at sparkle locations
- Each orb rises with smooth animation (2 seconds)
- Spawns are **staggered** (0.5 seconds apart)
- Order: Japanese (red) → French (blue) → Indian (green) → Mexican (yellow) → Nigerian (purple)

**The 5 Cultural Orbs:**

| Orb | Color | Culture | Location |
|-----|-------|---------|----------|
| 1 | 🔴 Red | Japanese | First spawn point |
| 2 | 🔵 Blue | French | Second spawn point |
| 3 | 🟢 Green | Indian | Third spawn point |
| 4 | 🟡 Yellow | Mexican | Fourth spawn point |
| 5 | 🟣 Purple | Nigerian | Fifth spawn point |

**Orb Behavior:**
- **Floating motion** - gentle up/down, slight sway
- **Rotation** - inner sphere rotates slowly
- **Pulsing glow** - subtle breathing effect
- **Always on** - visible and glowing

**UI/Narration:**
- Text appears: *"The voices of many cultures rise..."*
- Text fades after 5 seconds

### 🎮 What You Can Do

**Proximity Interaction:**
1. **Walk up to any orb** (within 2 meters)
2. Orb **glows brighter** as you approach
3. Glow intensity increases (2x normal)
4. Walk away → glow returns to normal

**Touch Interaction:**
1. **Point controller at orb** (ray/pointer visible)
2. **Press trigger** (index finger trigger on controller)
3. **Text appears** showing:
   - Culture name (large, bold)
   - Greeting in that language (italic)
   - Example: **"Japanese"** / *"Konnichiwa"*
4. Text stays visible for 3 seconds
5. Orb does a **pulse animation** (scale increases briefly)

**Experiment:**
- Try approaching all 5 orbs
- Activate each one to see its greeting
- Notice how colors differ
- Observe the floating patterns

### ⏱️ Duration

**30 seconds** - Automatically transitions to Scene 3

### ✅ Verification Checklist

- [ ] All 5 orbs spawn successfully
- [ ] Each orb has the correct color (red, blue, green, yellow, purple)
- [ ] Orbs float with gentle motion
- [ ] Orbs rotate (inner sphere visible)
- [ ] Proximity glow works (brighten when you approach)
- [ ] Touching orb shows culture info text
- [ ] Text shows culture name + greeting
- [ ] Pulse animation plays on touch
- [ ] All orbs remain visible throughout
- [ ] Transitions smoothly to Scene 3

### 🎓 What Each Orb Shows

Test by touching each orb:

1. **Red Orb:** "Japanese" / "Konnichiwa"
2. **Blue Orb:** "French" / "Bonjour"
3. **Green Orb:** "Indian" / "Namaste"
4. **Yellow Orb:** "Mexican" / "Hola"
5. **Purple Orb:** "Nigerian" / "Sannu"

### 🐛 Potential Issues

**If orbs don't spawn:**
- Check HarmonySceneManager has orb prefabs assigned
- Verify spawn points exist and are positioned correctly
- Check Console for errors

**If proximity glow doesn't work:**
- Verify Main Camera is tagged as "MainCamera"
- Check CulturalOrb script has proximity trigger enabled

**If touch doesn't show text:**
- Check XR Interaction Toolkit is working
- Verify UIManager has culture info text assigned

---

## SCENE 3: Connecting the Threads (00:40 - Variable)

### ✨ The Core Mechanic - Thread Connection

This is the **main interactive scene** where you create connections between cultural orbs.

### 🌟 What You'll See

**UI Changes:**
- Text appears: *"Connect the threads of culture..."*
- **Connection counter** appears at bottom center
- Shows: "Connections: 0 / 7"
- Counter updates as you create connections

**Orbs:**
- Continue floating and glowing
- Ready to be connected
- All 5 visible and accessible

### 🎮 How to Connect Threads - STEP BY STEP

#### Step 1: Target Source Orb
1. **Point right controller** at any orb
2. Aim with the ray/pointer extending from controller
3. Ray should hit the orb (collision detection)

#### Step 2: Start Thread Pull
1. **Press and HOLD right trigger** (index finger trigger)
2. **Light beam appears** from the orb to your controller
3. Beam color matches the source orb's color
4. You're now "pulling" a thread

#### Step 3: Guide the Thread
1. **Keep holding trigger** (don't release yet!)
2. **Move controller** toward another orb
3. The beam follows your controller in real-time
4. Thread has slight arc (Bezier curve)

#### Step 4: Snap to Target
1. **Move controller close** to another orb (within 0.5 meters)
2. Watch for **color change** at beam end
3. End of beam changes to target orb's color (snap indicator)
4. This shows you're in range to connect

#### Step 5: Create Connection
1. **Release trigger** while near target orb
2. **Permanent thread appears** between the two orbs!
3. Thread color is a **blend** of both orb colors
4. **Particle burst** at connection point (visual feedback)
5. **Counter increments**: "Connections: 1 / 7"

#### Step 6: Repeat
- Create **at least 7 connections** total
- Connect different orbs in any pattern you want
- Each connection is permanent (stays visible)

### 🎨 Thread Visual Properties

**Appearance:**
- **Width:** Thin line (0.05 meters)
- **Color:** Blended from both connected orbs
  - Red + Blue = Purple
  - Yellow + Blue = Green
  - Red + Yellow = Orange
- **Effect:** Glowing, emissive
- **Updates:** Follows orbs if they move (stays connected)

**Examples:**
- Japanese (red) + French (blue) = Purple thread
- Indian (green) + Mexican (yellow) = Yellow-green thread
- Any combination creates unique color

### 📏 Connection Rules

**Valid Connections:**
- ✅ Any orb to any other orb
- ✅ Multiple connections per orb (up to 4)
- ✅ Create 7-10 connections total

**Invalid Connections:**
- ❌ Can't connect orb to itself
- ❌ Can't duplicate existing connection
- ❌ Max 4 connections per orb

### 🎯 Minimum Requirement

**You need at least 7 connections** to progress to Scene 4

**Possible connections:**
- With 5 orbs, maximum 10 unique connections possible
- You need 7 out of 10 (70%)
- Create interesting patterns!

### 💡 Creative Connection Patterns

Try these patterns:

**Star Pattern:**
- Connect one center orb to all 4 others (4 connections)
- Then connect 3 more between outer orbs (total 7)

**Circle Pattern:**
- Connect each orb to its neighbors in a circle (5 connections)
- Add 2 cross-connections (total 7)

**Random Pattern:**
- Just connect orbs as you discover them
- Make it organic and natural

### ⏱️ Duration

**User-driven** - No time limit!
- Take your time to explore
- Experiment with patterns
- Average: 1-3 minutes
- Once you reach 7 connections, waits 2 seconds then auto-progresses

### ✅ Verification Checklist

- [ ] Connection counter is visible at bottom
- [ ] Can point at orb and hold trigger
- [ ] Beam extends from orb to controller
- [ ] Beam follows controller movement
- [ ] Beam changes color near target orb (snap)
- [ ] Releasing trigger creates permanent thread
- [ ] Thread stays visible between orbs
- [ ] Thread color is blend of both orb colors
- [ ] Counter increments with each connection
- [ ] Can create multiple connections
- [ ] At 7+ connections, progresses to Scene 4
- [ ] No crashes or freezes

### 🎮 Controller Tips

**For Best Experience:**
- **Smooth movements** - don't jerk the controller
- **Get close** to target orb before releasing (easier snap)
- **Watch the color** - end color change = ready to connect
- **Take your time** - there's no rush
- **Experiment** - try different patterns

**If Thread Won't Connect:**
- Make sure you're **holding trigger** the whole time
- Get **closer** to target orb (within arm's reach)
- Point **directly at** the target orb
- Wait for **color change** indicator

### 🐛 Potential Issues

**Beam doesn't appear when holding trigger:**
- Wait until Scene 3 starts (after 40 seconds from start)
- Check Console for "Thread Connection System is now ACTIVE"
- Verify you're pointing at an orb (not empty space)
- Check ThreadConnectionSystem has Ray Interactor assigned

**Can't create connection:**
- Get closer to target orb (< 0.5 meters)
- Make sure target orb isn't already at max connections (4)
- Verify connection doesn't already exist
- Try a different orb pair

**Counter doesn't update:**
- Check HarmonyUIManager has connection counter text assigned
- Look at Console for connection logs

**Scene doesn't progress after 7 connections:**
- Check Console for transition message
- Verify HarmonySceneManager is present in scene
- Wait a few more seconds (2 second delay is intentional)

---

## SCENE 4: The Tapestry of Unity (After 7 Connections)

### 🌌 The Transformation

This is the **climactic visual scene** where your connections form a collective tapestry.

### 🌟 What You'll See

**Transition Begins:**
- Connection counter disappears
- Text appears: *"A tapestry of unity emerges..."*
- Music would swell here (if audio was enabled)

**Thread Animation (Duration: 3 seconds):**
1. **All threads detach** from orbs
2. **Threads float upward** smoothly
3. They rise about **5 meters** into the air
4. Threads **spread out** to form overhead pattern
5. Animation uses smooth easing (not linear)

**Canopy Formation:**
- Threads form **geometric pattern** overhead
- All threads converge toward a **center point**
- Pattern is semi-random but aesthetically pleasing
- Colors blend where threads overlap
- Creates a "cathedral of light" effect

**Center Orb:**
- **Large glowing sphere** appears at center point
- Size: ~0.5 meters diameter
- Color: White/light (blended from all cultures)
- **Pulsing animation** - gentle breathing effect
- Emissive glow (HDR)

**Overall Effect:**
- Look up to see the full canopy
- Colorful threads overhead like stained glass
- Center focal point drawing everything together
- Sense of completion and unity

### 🎮 What You Can Do

**Exploration:**
- **Look up** at the canopy (tilt head back)
- **Walk around** underneath
- **Observe** the thread patterns
- **Enjoy** the visual spectacle

**Passive Experience:**
- No interaction required
- Take in the beauty
- Appreciate the collective creation
- Reflect on the connections you made

### ⏱️ Duration

**20 seconds** - Automatically transitions to Scene 5

### ✅ Verification Checklist

- [ ] Counter disappeared
- [ ] Narration text appeared
- [ ] Threads animated upward smoothly
- [ ] Threads formed canopy overhead
- [ ] Center orb appeared
- [ ] Center orb pulses gently
- [ ] All thread colors still visible
- [ ] No threads disappeared
- [ ] Visual effect is impressive
- [ ] No performance drops
- [ ] Transitions smoothly to Scene 5

### 🎨 Visual Details

**Canopy Characteristics:**
- **Height:** ~5 meters above ground
- **Spread:** Threads radiate from center
- **Density:** All your connections visible
- **Movement:** Slight gentle sway (if implemented)
- **Colors:** Rainbow of blended colors
- **Lighting:** Threads emit light (illuminates area)

**Center Orb Details:**
- **Size:** 3-5x larger than cultural orbs
- **Pulse Rate:** ~0.5 Hz (twice per second)
- **Pulse Scale:** 20% size variation
- **Glow:** Bright emission (visible even in light)

### 💡 Artistic Intent

This scene represents:
- **Unity from diversity** - many threads, one tapestry
- **Collective creation** - you built this together (conceptually)
- **Beauty in connection** - individual cultures remain distinct but harmonious
- **Transcendence** - threads rise from ground to sky

### 🐛 Potential Issues

**Threads disappear instead of forming canopy:**
- Check ConnectionThread.AnimateToCanopy() is being called
- Verify animation duration is set (should be 3 seconds)
- Look for errors in Console

**Center orb doesn't appear:**
- Check CreateCenterOrb() function in HarmonySceneManager
- Verify object is being instantiated

**Performance drops in this scene:**
- Many LineRenderers may strain GPU
- Check Quality Settings if FPS drops below 72
- Consider reducing thread count if needed

---

## SCENE 5: Reflection Beneath the Light (Final)

### 🌠 The Closing

This is the **conclusion** of the narrative journey.

### 🌟 What You'll See

**Narration:**
- Text appears: *"Together, we weave the colors of humanity... May we carry this harmony with us."*
- Longer text, stays visible for ~7 seconds
- Reflective message to take with you

**Orb Ascension:**
- **All 5 cultural orbs** begin rising
- Smooth upward movement (5 second duration)
- Rise to **10 meters** height
- **Fade out** as they ascend (transparency increases)
- Gradual, graceful exit
- One by one they disappear into the sky

**Canopy Effect:**
- Threads remain visible initially
- May fade along with orbs
- Eventually scene returns to base lighting

**End Screen (After 10 seconds):**
- UI panel fades in
- Semi-transparent dark background
- **Title:** "Harmony in Diversity"
- **Restart Button:** Click to replay
- **Exit Button:** Click to quit (if enabled)

### 🎮 What You Can Do

**During Ascension (First 5 seconds):**
- **Watch the orbs rise** - passive observation
- **Look around** as they ascend
- **Reflect** on the experience

**End Screen Interaction:**
- **Point controller** at buttons
- **Click Restart** → Returns to Scene 1 (complete reset)
- **Click Exit** → Returns to Quest home menu
- Take your time deciding

### ⏱️ Duration

- **Narration:** 7 seconds
- **Ascension:** 5 seconds
- **Fade/Transition:** 3 seconds
- **End Screen:** Indefinite (waits for user)

**Total:** ~15 seconds + end screen

### ✅ Verification Checklist

- [ ] Closing narration appeared and was readable
- [ ] All 5 orbs began rising
- [ ] Orbs moved smoothly upward
- [ ] Orbs faded out as they rose
- [ ] Orbs eventually disappeared
- [ ] End screen appeared after ~10 seconds
- [ ] Title is visible: "Harmony in Diversity"
- [ ] Restart button is present and clickable
- [ ] Exit button is present (if included)
- [ ] Clicking Restart resets to Scene 1
- [ ] All orbs respawn on restart
- [ ] Connection counter resets to 0
- [ ] Experience can be replayed

### 🔄 Restart Behavior

When you click **Restart**:

**What Resets:**
- ✅ All orbs respawn at original positions
- ✅ All threads are deleted
- ✅ Connection counter returns to 0 / 7
- ✅ Scene state returns to Scene 1
- ✅ Sparkles reappear
- ✅ Narration plays from beginning

**What Persists:**
- ✅ Your position in the space
- ✅ Controller state
- ✅ Settings/configuration

You get a **fresh experience** just like the first time!

### 💡 Reflective Intent

This scene represents:
- **Return to source** - orbs ascend to where they came from
- **Continuation** - the experience lives on in your memory
- **Choice** - you decide whether to revisit or move on
- **Invitation** - to contribute your own voice (in full version with AR)

### 🐛 Potential Issues

**Orbs don't rise:**
- Check OrbAscension coroutine in HarmonySceneManager
- Verify animation duration is set
- Look for errors in Console

**End screen doesn't appear:**
- Check HarmonyUIManager.ShowEndScreen() is being called
- Verify EndScreenPanel is assigned in Inspector
- Check panel is active in hierarchy when it should appear

**Restart button doesn't work:**
- Check button OnClick listener is connected
- Verify HarmonySceneManager.RestartExperience() exists
- Check for errors when clicking

**Buttons can't be clicked:**
- Verify Canvas has Event Camera assigned
- Check XR Interaction with UI is configured
- Ensure buttons have correct UI components

---

## CONTROLLER REFERENCE

### Right Controller (Primary)

**Buttons Used:**
- **Trigger (Index Finger):**
  - Scene 2: Touch orbs to see greetings
  - Scene 3: Hold to pull thread, release to connect
  - Scene 5: Click UI buttons
- **Oculus/Menu Button:**
  - Pause experience
  - Open system menu
  - Exit app

**Visual:**
- Should have ray/pointer extending from it
- Ray used for pointing and selection

### Left Controller

**Buttons Used:**
- Not used in MVP
- Just tracked for presence/position

**Visual:**
- Should be visible and tracking
- No ray/pointer needed

### Both Controllers

**Tracking:**
- Should be visible at all times
- Should match your physical hand positions
- Should respond to button presses immediately

**Haptics:**
- Vibration feedback when touching orbs
- Vibration on successful connection

---

## TESTING CHECKLIST

### Pre-Experience Check

- [ ] Quest 2 is fully charged
- [ ] Guardian boundary is set up
- [ ] Adequate play space (2m x 2m minimum)
- [ ] Controllers have fresh batteries
- [ ] App launches without errors

### Scene 1 Verification

- [ ] Spawn position is correct
- [ ] Courtyard environment visible
- [ ] 5 sparkles visible
- [ ] Narration text appears
- [ ] Dawn lighting looks good
- [ ] Controllers visible
- [ ] Can look around 360°
- [ ] No black screens
- [ ] Transitions after 10 seconds

### Scene 2 Verification

- [ ] Sparkles stop
- [ ] 5 orbs spawn (red, blue, green, yellow, purple)
- [ ] Orbs rise with animation
- [ ] Narration appears
- [ ] Orbs float gently
- [ ] Proximity glow works
- [ ] Can touch orbs
- [ ] Culture info appears (name + greeting)
- [ ] All 5 orbs show correct info
- [ ] Transitions after 30 seconds

### Scene 3 Verification

- [ ] Narration appears
- [ ] Connection counter appears (0 / 7)
- [ ] Can point at orb
- [ ] Holding trigger creates beam
- [ ] Beam follows controller
- [ ] Beam color matches source orb
- [ ] Snap indicator works (color change)
- [ ] Releasing creates thread
- [ ] Thread stays visible
- [ ] Thread color is blended
- [ ] Counter increments
- [ ] Can create 7+ connections
- [ ] Transitions at 7 connections

### Scene 4 Verification

- [ ] Counter disappears
- [ ] Narration appears
- [ ] Threads animate upward
- [ ] Canopy forms overhead
- [ ] Center orb appears
- [ ] Center orb pulses
- [ ] Visual effect is impressive
- [ ] All threads visible in canopy
- [ ] Transitions after 20 seconds

### Scene 5 Verification

- [ ] Narration appears
- [ ] Orbs begin rising
- [ ] Orbs fade out
- [ ] End screen appears
- [ ] Title visible
- [ ] Restart button works
- [ ] Exit button works (if present)
- [ ] Restart resets experience completely

### Performance Check

- [ ] Maintains 72 FPS throughout
- [ ] No stuttering or lag
- [ ] No dropped frames
- [ ] Smooth animations
- [ ] Responsive controls
- [ ] Quick button response
- [ ] No overheating
- [ ] Comfortable to play

### Comfort Check

- [ ] No VR sickness
- [ ] No eye strain
- [ ] Comfortable viewing distances
- [ ] Text is readable
- [ ] UI is accessible
- [ ] No need to turn head uncomfortably
- [ ] Can complete without breaks

---

## TROUBLESHOOTING

### Black Screen Issues

**Symptom:** See nothing but black when app launches

**Solutions:**
1. Take off headset, press Oculus button
2. Force quit app: Menu > Quit
3. Re-launch app
4. If persists: Restart Quest 2
5. If still black: Check Unity Console logs for errors
6. Verify XR Origin/Rig exists in scene
7. Check Main Camera is active and positioned correctly

### Performance Issues

**Symptom:** Laggy, stuttering, low framerate

**Solutions:**
1. **Shader compilation lag (first run):**
   - Wait 30-60 seconds
   - Restart app - should be smoother
2. **Persistent lag:**
   - Check Quest 2 isn't overheating
   - Close other running apps
   - Restart Quest 2
3. **In Unity:**
   - Reduce Quality Settings
   - Lower shadow quality
   - Reduce particle counts
   - Enable Multi-view rendering

### Thread Pulling Not Working

**Symptom:** Can't pull thread beam from orbs

**Solutions:**
1. **Wait for Scene 3:**
   - Not available until 40 seconds in
   - Watch for narration: "Connect the threads..."
2. **Check you're pointing at orb:**
   - Ray must hit orb collision
   - Get closer if needed
3. **Check trigger button:**
   - Hold trigger fully pressed
   - Don't release until ready to connect
4. **Console check:**
   - Look for "Thread Connection System is now ACTIVE"
   - If not there, check ThreadConnectionSystem setup

### Connection Not Creating

**Symptom:** Release trigger but no thread appears

**Solutions:**
1. **Get closer to target orb:**
   - Must be within 0.5 meters
   - Watch for color change at beam end
2. **Check not already connected:**
   - Can't duplicate existing connection
   - Try different orb pair
3. **Check orb connection limit:**
   - Max 4 connections per orb
   - Try orb with fewer connections
4. **Console messages:**
   - Look for "connection already exists"
   - Look for "maximum connections" message

### UI Not Visible

**Symptom:** Can't see narration text or buttons

**Solutions:**
1. **Look around:**
   - UI might be behind you
   - Turn 360° to locate
2. **Check Canvas setup:**
   - Verify Event Camera assigned
   - Check Canvas Render Mode is World Space
3. **Position issue:**
   - Canvas might be too far/close
   - Check position in Unity Editor

### Orbs Missing

**Symptom:** Some or all orbs don't appear in Scene 2

**Solutions:**
1. **Wait for spawn animation:**
   - Orbs spawn with stagger
   - Wait 10-15 seconds after Scene 2 starts
2. **Look around:**
   - Orbs spawn in circle around you
   - Turn to see all 5
3. **Check setup:**
   - Verify all orb prefabs assigned in HarmonySceneManager
   - Check spawn points exist
   - Console should show "orb spawned" messages

### App Crashes

**Symptom:** App quits unexpectedly

**Solutions:**
1. **Check Unity Console:**
   - Look for errors before building
   - Fix all compilation errors
2. **Use Development Build:**
   - Enable in Build Settings
   - Provides more error info
3. **Check device logs:**
   - Connect Quest 2 to PC
   - Run: `adb logcat`
   - Look for crash messages
4. **Common causes:**
   - Missing scene references
   - Null reference exceptions
   - Out of memory

### Controllers Not Visible

**Symptom:** Can't see controller models

**Solutions:**
1. **Check controller battery:**
   - Low battery can cause tracking issues
   - Replace batteries
2. **Re-pair controllers:**
   - Quest Settings > Devices > Controllers
   - Pair again
3. **XR Rig check:**
   - Verify controllers are children of XR Origin
   - Check controller GameObjects are active

### Audio Not Playing

**Symptom:** No sound (narration, effects)

**Note:** MVP version has **NO AUDIO** - this is intentional!
- Only visual experience in MVP
- Audio will be added in post-MVP iterations
- If you added audio yourself, check audio sources and mixer

---

## EXPERIENCE FLOW DIAGRAM

```
Launch App
    ↓
SCENE 1 (10s auto)
    ↓ [Sparkles → observe]
    ↓
SCENE 2 (30s auto)
    ↓ [Orbs spawn → proximity → touch]
    ↓
SCENE 3 (user-driven)
    ↓ [Pull threads → connect orbs]
    ↓ [Need 7+ connections]
    ↓
SCENE 4 (20s auto)
    ↓ [Threads form canopy → observe]
    ↓
SCENE 5 (10s + UI)
    ↓ [Orbs ascend → end screen]
    ↓
    ├─→ [Restart] → SCENE 1
    └─→ [Exit] → Quest Home
```

---

## TIMING REFERENCE

| Scene | Duration | Type | Key Moment |
|-------|----------|------|------------|
| 1 | 10 seconds | Auto | Sparkles |
| 2 | 30 seconds | Auto | Orbs spawn + interact |
| 3 | 1-3 minutes | User | Thread connections |
| 4 | 20 seconds | Auto | Canopy forms |
| 5 | ~15 seconds | Auto | Orbs rise |
| End | Indefinite | User | Restart/Exit |

**Total:** ~2-5 minutes per playthrough

---

## SUCCESS CRITERIA

The experience is successful if:

1. ✅ **All 5 scenes play through** without errors or crashes
2. ✅ **Thread connection works** and feels intuitive
3. ✅ **Visual progression** is clear and meaningful
4. ✅ **Performance is smooth** (72 FPS consistent)
5. ✅ **Controls are responsive** with no lag
6. ✅ **Comfortable to experience** - no VR sickness
7. ✅ **Restart works** - can replay immediately
8. ✅ **Visually impressive** - especially Scene 4 canopy

---

## NOTES FOR TESTING

### First Playthrough

- **Don't rush** - take time to explore each scene
- **Note any issues** - write down bugs or problems
- **Check all interactions** - try touching all orbs
- **Experiment with connections** - try different patterns
- **Observe details** - colors, animations, transitions

### Second Playthrough

- **Try different connection patterns** - make different tapestry
- **Test restart function** - verify it works correctly
- **Push boundaries** - try to break it (edge cases)
- **Check consistency** - does it behave the same?

### Things to Document

- Any visual glitches
- Performance drops (note when/where)
- Interaction issues (what didn't work)
- Timing problems (too fast/slow)
- Ideas for improvement
- What felt good/bad

---

## EXPECTED EXPERIENCE QUALITY

### Visual Quality

- **Framerate:** Locked 72 FPS (no drops)
- **Resolution:** Sharp and clear
- **Colors:** Vibrant and distinct
- **Lighting:** Appropriate for each scene
- **Effects:** Smooth particle systems and animations

### Interaction Quality

- **Responsiveness:** < 50ms input lag
- **Precision:** Accurate ray pointing
- **Feedback:** Clear visual/haptic response
- **Intuitiveness:** No tutorial needed (self-explanatory)

### Comfort Quality

- **No motion sickness** - static experience
- **Readable text** - appropriate size and distance
- **Comfortable viewing** - no neck strain
- **Appropriate pacing** - not too fast/slow

---

## DEMO TIPS

If showing to others:

1. **Explain the concept first** - cultural diversity celebration
2. **Show Scene 3 mechanic** - demonstrate thread pulling
3. **Let them explore** - don't rush them
4. **Point out details** - colors, patterns, canopy
5. **Ask for feedback** - what did they feel?

---

**Enjoy the experience! May the threads of culture weave a beautiful tapestry.** 🌟🎨🌈
