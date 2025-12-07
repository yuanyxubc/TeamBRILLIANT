# Harmony in Diversity - Narration Scripts & Setup Guide

## ✅ Code Implementation: COMPLETE!

You now have:
- ✅ NarrationData.cs script created
- ✅ HarmonySceneManager updated for all 5 scenes
- ✅ Automatic narration playback with delay

---

## 🎙️ NARRATION SCRIPTS

### Scene 1: The Courtyard Awakens (Opening)
**Context:** Dawn. Player enters empty courtyard with sparkles
**Tone:** Gentle, inviting, mysterious

```
Every culture is a light, waiting to be seen. In this courtyard, many voices will soon rise.
```

**Duration:** ~8 seconds
**Delay:** 5 seconds after scene starts

---

### Scene 2: The Voices Rise (Opening)
**Context:** Orbs spawn and float. Cultural hums begin
**Tone:** Wonder, appreciation

```
The voices of many cultures rise. Listen... Each orb carries the essence of a people.
```

**Duration:** ~7 seconds
**Delay:** 3 seconds after orbs spawn

---

### Scene 3: Connecting the Threads (Opening)
**Context:** Threading mechanic activated
**Tone:** Instructive but poetic, encouraging

```
Now, connect the threads of culture. Draw light from one voice to another. Weave the tapestry of humanity.
```

**Duration:** ~10 seconds
**Delay:** 3 seconds after scene starts

---

### Scene 4: The Tapestry of Unity (Opening)
**Context:** Threads weave into canopy overhead
**Tone:** Awe, celebration

```
A tapestry of unity emerges... See how the threads weave together, forming something greater than any single voice.
```

**Duration:** ~10 seconds
**Delay:** 2 seconds after canopy forms

---

### Scene 5: Reflection (Closing)
**Context:** Orbs rise and fade. Experience concludes
**Tone:** Reflective, hopeful, inspiring

```
Together, we weave the colors of humanity into a tapestry of unity. May we carry this harmony with us, always.
```

**Duration:** ~12 seconds
**Delay:** 2 seconds after scene starts

---

## 🎵 HOW TO GENERATE NARRATION AUDIO

### Option 1: TTSMaker (Free, No Login) ⭐ RECOMMENDED

**Website:** https://ttsmaker.com/

**Steps:**
1. Go to TTSMaker.com
2. Select **Language: English**
3. Select **Voice:**
   - "Jenny" (US Female - warm, clear)
   - or "Guy" (US Male - calm, deep)
4. Paste narration text
5. **Speed:** 0.9x (slightly slower for clarity)
6. Click **"Convert to Speech"**
7. Click **"Download"** (MP3 format)
8. Repeat for all 5 narration scripts

**Voice Settings:**
- **Volume:** 100%
- **Speed:** 0.9x (slower is better for VR)
- **Pitch:** 0 (normal)

---

### Option 2: ElevenLabs (Best Quality, Limited Free) ⭐⭐⭐

**Website:** https://elevenlabs.io/

**Steps:**
1. Create free account (10,000 chars/month free)
2. Go to Speech Synthesis
3. Select Voice:
   - "Rachel" (Natural, warm)
   - "Josh" (Deep, authoritative)
   - "Bella" (Calm, gentle)
4. Paste text
5. Click "Generate"
6. Download as MP3
7. Repeat for all scripts

**Settings:**
- **Stability:** 75%
- **Clarity:** 85%
- **Style Exaggeration:** 25%

**Note:** ElevenLabs has the most realistic voices but limited free quota.

---

### Option 3: Google Cloud Text-to-Speech (Free Tier)

**Website:** https://cloud.google.com/text-to-speech

**Steps:**
1. Create Google Cloud account (free tier: 1M chars/month)
2. Enable Text-to-Speech API
3. Use the demo on the website OR:
4. Use the API with Python/curl

**Recommended Voice:**
- `en-US-Neural2-F` (Female, natural)
- `en-US-Neural2-J` (Male, natural)

**Settings:**
- Speaking Rate: 0.9
- Pitch: 0
- Volume: 0

---

### Option 4: Record Your Own Voice

**Tools:**
- **Audacity** (Free): https://www.audacityteam.org/
- **Windows Voice Recorder** (Built-in)
- **QuickTime** (Mac)

**Recording Tips:**
1. **Environment:**
   - Quiet room
   - Close windows
   - Turn off fans/AC
   - Use pillow fort for sound dampening

2. **Technique:**
   - Speak slowly and clearly
   - Use warm, inviting tone
   - Take deep breath before each line
   - Record multiple takes

3. **Equipment:**
   - Any mic is fine (even phone headset)
   - 6-12 inches from mic
   - Speak at consistent volume

4. **Post-Processing in Audacity:**
   - Effect → Noise Reduction (remove background hiss)
   - Effect → Normalize (consistent volume)
   - Effect → Compressor (smooth dynamics)
   - Export as WAV (44.1kHz, 16-bit)

---

## 🎚️ AUDIO SPECIFICATIONS

### Format:
- **File Type:** WAV or MP3
- **Sample Rate:** 44.1kHz
- **Bit Depth:** 16-bit
- **Channels:** Mono (recommended for narration)

### Duration Per Scene:
- Scene 1: 6-10 seconds
- Scene 2: 6-8 seconds
- Scene 3: 9-12 seconds
- Scene 4: 9-12 seconds
- Scene 5: 10-15 seconds

**Total:** ~45-55 seconds of narration audio

---

## 📥 IMPORTING TO UNITY

### Step 1: Convert to Proper Format (if needed)

If your TTS gave you MP3, you can use it directly OR convert to WAV:

**Online Converter:**
- https://online-audio-converter.com/
- Upload MP3
- Convert to WAV, 44.1kHz, Mono
- Download

### Step 2: Import to Unity

1. Create folder: `Assets/Audio/Narration/`
2. Drag all 5 audio files into Unity
3. Name them clearly:
   - `Scene1_CourtyardAwakens.wav`
   - `Scene2_VoicesRise.wav`
   - `Scene3_ConnectingThreads.wav`
   - `Scene4_TapestryUnity.wav`
   - `Scene5_Reflection.wav`

### Step 3: Configure Audio Import Settings

Select each audio file in Unity:
1. **Load Type:** Streaming (for longer clips > 5sec)
2. **Preload Audio Data:** True
3. **Compression Format:** Vorbis
4. **Quality:** 70-80%
5. **Sample Rate Setting:** Preserve Sample Rate
6. Click **Apply**

---

## 🎮 UNITY SETUP

### Step 1: Create NarrationData Asset

1. In Project window, right-click
2. **Create → Harmony → Narration Data**
3. Name it: `HarmonyNarration`

### Step 2: Assign Audio Clips

1. Select `HarmonyNarration` asset
2. In Inspector, assign your narration clips:

| Field | Assign |
|-------|--------|
| **Scene 1 Opening** | Scene1_CourtyardAwakens |
| **Scene 1 Delay** | 5 (seconds) |
| **Scene 2 Opening** | Scene2_VoicesRise |
| **Scene 2 Delay** | 3 |
| **Scene 3 Opening** | Scene3_ConnectingThreads |
| **Scene 3 Delay** | 3 |
| **Scene 4 Opening** | Scene4_TapestryUnity |
| **Scene 4 Delay** | 2 |
| **Scene 5 Closing** | Scene5_Reflection |
| **Scene 5 Delay** | 2 |

### Step 3: Assign to HarmonySceneManager

1. In Hierarchy, select `HarmonySceneManager`
2. In Inspector, find **"Narration"** section
3. Drag `HarmonyNarration` asset into the **Narration Data** field
4. Done!

---

## 🧪 TESTING

### In Unity Editor (Play Mode):
1. Press Play
2. Wait 5 seconds → Should hear Scene 1 narration
3. Scene 2 → Should hear Scene 2 narration after orbs spawn
4. Scene 3 → Should hear Scene 3 narration
5. Complete 7 connections → Scene 4 narration
6. Wait for Scene 5 → Should hear closing narration

### On Quest 2:
1. Build and deploy
2. Experience should have voice-over throughout
3. Narration plays automatically at scene transitions

### Verify:
- ✅ Narration volume is balanced with orb audio
- ✅ Narration is clear and understandable
- ✅ Delays feel natural (not too fast/slow)
- ✅ No audio clipping or distortion

---

## 🎚️ VOLUME ADJUSTMENT

If narration is too loud/quiet:

**Option 1: In Audio Mixer**
1. Open `HarmonyAudioMixer`
2. Find **Narration** group
3. Adjust **Volume** slider

**Option 2: In HarmonyAudioManager**
1. Select HarmonyAudioManager in Hierarchy
2. Adjust **Narration Volume** slider (0-1)

**Recommended Balance:**
- Narration: 1.0 (loudest)
- Orb greetings: 0.8
- Orb hums: 0.6-0.7
- Ambient: 0.5-0.6
- SFX: 0.8

---

## 🎭 VOICE DIRECTION NOTES

### Scene 1: "Every culture is a light..."
- **Mood:** Mysterious, inviting
- **Pace:** Slow, deliberate
- **Emphasis:** "light", "waiting to be seen", "rise"

### Scene 2: "The voices of many cultures rise..."
- **Mood:** Wonder, appreciation
- **Pace:** Medium, flowing
- **Emphasis:** "Listen", "essence"

### Scene 3: "Connect the threads..."
- **Mood:** Encouraging, guiding
- **Pace:** Clear, instructional
- **Emphasis:** "Connect", "threads", "weave"

### Scene 4: "A tapestry of unity emerges..."
- **Mood:** Awe, celebration
- **Pace:** Slightly slower, reverent
- **Emphasis:** "tapestry", "weave together", "greater"

### Scene 5: "Together, we weave the colors..."
- **Mood:** Hopeful, reflective, inspiring
- **Pace:** Slow, heartfelt
- **Emphasis:** "Together", "harmony", "always"
- **Note:** This is the emotional climax - let it breathe

---

## 📊 CHECKLIST

Setup:
- [ ] NarrationData.cs created
- [ ] HarmonySceneManager updated (done automatically)
- [ ] Generate/record 5 narration audio clips
- [ ] Import audio to Unity
- [ ] Create HarmonyNarration asset
- [ ] Assign audio clips to asset
- [ ] Assign asset to HarmonySceneManager
- [ ] Test in Play mode
- [ ] Build and test on Quest 2

---

## 🚀 QUICK START (30 minutes)

1. **Go to TTSMaker.com** (no login needed)
2. **Paste Scene 1 text**, select voice "Jenny", speed 0.9x
3. **Download MP3**
4. **Repeat for all 5 scenes** (takes 10 minutes)
5. **Create folder** `Assets/Audio/Narration/` in Unity
6. **Drag all MP3s** into Unity
7. **Right-click** in Project → Create → Harmony → Narration Data
8. **Assign clips** to the narration data asset
9. **Assign asset** to HarmonySceneManager
10. **Press Play** and enjoy narration!

---

## 💡 PRO TIPS

1. **Test different voices:** Try 2-3 different TTS voices and pick your favorite
2. **Adjust speed:** 0.85x-0.95x works best for VR (people read text slower in VR)
3. **Add pauses:** Use commas and periods to create natural pauses
4. **Record backups:** Keep your raw TTS files in case you need to adjust
5. **Voice consistency:** Use the same voice for all 5 scenes
6. **Test in VR early:** Narration sounds different in headset vs speakers

---

## 🎯 WHAT YOU'LL HAVE

After completing this:
- ✅ Professional voice-over narration
- ✅ Guides players through the experience
- ✅ Automatic playback at scene transitions
- ✅ Synced with visual events
- ✅ Balanced with other audio

**Total added value: HUGE! Narration makes it feel like a complete, polished experience.**

---

**Ready to generate your narration audio?** Start with TTSMaker.com - it's free, fast, and sounds great! 🎙️✨
