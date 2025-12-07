# Harmony in Diversity - Audio Implementation Guide

## ✅ Stage 1 & 2: COMPLETE!

You now have:
- ✅ HarmonyAudioManager script created
- ✅ CulturalOrbData updated with audio fields
- ✅ CulturalOrb updated to play greetings and hums

---

## 🎵 STAGE 3: Narration Audio System (1-2 hours)

### Option A: Text-to-Speech (Quick & Easy)

**Recommended TTS Services:**
1. **Google Cloud Text-to-Speech** (free tier: 1M chars/month)
   - https://cloud.google.com/text-to-speech
   - High quality, multiple voices
   - Export as MP3/WAV

2. **ElevenLabs** (best quality, limited free)
   - https://elevenlabs.io
   - Ultra-realistic voices
   - 10,000 chars free

3. **TTSMaker** (completely free)
   - https://ttsmaker.com
   - Good quality
   - No registration needed

**Narration Scripts:**

**Scene 1 Opening:**
```
"Every culture is a light, waiting to be seen. In this courtyard, many voices will soon rise."
```

**Scene 2:**
```
"The voices of many cultures rise. Listen... Each orb carries the essence of a people."
```

**Scene 3:**
```
"Now, connect the threads of culture. Draw light from one voice to another. Weave the tapestry of humanity."
```

**Scene 4:**
```
"A tapestry of unity emerges... See how the threads weave together, forming something greater than any single voice."
```

**Scene 5 Closing:**
```
"Together, we weave the colors of humanity into a tapestry of unity. May we carry this harmony with us, always."
```

### Option B: Record Your Own Voice

**Tools:**
- Audacity (free): https://www.audacityteam.org/
- Windows Voice Recorder (built-in)

**Recording Tips:**
1. Quiet environment
2. Speak slowly and clearly
3. Warm, inclusive tone
4. Export as WAV (44.1kHz, 16-bit)

### Implementation:

**Step 3.1: Create Narration Data Structure**

Create: `Assets/Scripts/HarmonyInDiversity/Audio/NarrationData.cs`

```csharp
using UnityEngine;

[CreateAssetMenu(fileName = "NarrationData", menuName = "Harmony/Narration Data")]
public class NarrationData : ScriptableObject
{
    [Header("Scene 1: Courtyard Awakens")]
    public AudioClip scene1Opening;
    public float scene1Delay = 5f;

    [Header("Scene 2: Voices Rise")]
    public AudioClip scene2Opening;
    public float scene2Delay = 3f;

    [Header("Scene 3: Connecting Threads")]
    public AudioClip scene3Opening;
    public float scene3Delay = 3f;

    [Header("Scene 4: Tapestry of Unity")]
    public AudioClip scene4Opening;
    public float scene4Delay = 2f;

    [Header("Scene 5: Reflection")]
    public AudioClip scene5Closing;
    public float scene5Delay = 2f;
}
```

**Step 3.2: Update HarmonySceneManager**

Add narration calls to each scene initialization:

```csharp
[Header("Narration")]
public NarrationData narrationData;

void InitializeScene1()
{
    // ... existing code ...

    // Play narration
    if (narrationData != null && narrationData.scene1Opening != null)
    {
        HarmonyAudioManager.Instance.PlayNarration(narrationData.scene1Opening, narrationData.scene1Delay);
    }
}

// Repeat for InitializeScene2, 3, 4, 5
```

**Step 3.3: Create and Assign Narration Data**

1. In Unity: Right-click Project → Create → Harmony → Narration Data
2. Name it "HarmonyNarration"
3. Drag your narration audio clips into the fields
4. Assign to HarmonySceneManager in inspector

---

## 🎵 STAGE 4: Ambient & Environmental Sounds (1-2 hours)

### Free Audio Resources:

**Best Free Sound Libraries:**
1. **Freesound.org** (CC Licensed)
   - Search: "birds chirping", "wind ambience", "courtyard atmosphere"
   - Register free account to download

2. **OpenGameArt.org**
   - Curated game audio
   - Various CC licenses

3. **BBC Sound Effects** (Free for personal use)
   - https://sound-effects.bbcrewind.co.uk/
   - High quality professional recordings

**Recommended Ambient Sounds:**

| Scene | Sound | Duration | Loop |
|-------|-------|----------|------|
| Scene 1 | Dawn birds chirping | 30s+ | Yes |
| Scene 1 | Gentle wind | 30s+ | Yes |
| Scene 2-3 | Courtyard ambience | 60s+ | Yes |
| Scene 4 | Mystical atmosphere | 60s+ | Yes |
| Scene 5 | Night crickets (optional) | 30s+ | Yes |

### Implementation:

**Step 4.1: Create Ambient Data Structure**

Create: `Assets/Scripts/HarmonyInDiversity/Audio/AmbientAudioData.cs`

```csharp
using UnityEngine;

[CreateAssetMenu(fileName = "AmbientAudio", menuName = "Harmony/Ambient Audio Data")]
public class AmbientAudioData : ScriptableObject
{
    [Header("Scene 1: Dawn")]
    public AudioClip scene1Ambient;

    [Header("Scene 2-3: Day")]
    public AudioClip scene23Ambient;

    [Header("Scene 4-5: Night")]
    public AudioClip scene45Ambient;

    [Header("Transition Settings")]
    public float fadeInDuration = 2f;
    public float fadeOutDuration = 2f;
}
```

**Step 4.2: Update Scene Manager**

Add ambient audio to scene transitions:

```csharp
[Header("Ambient Audio")]
public AmbientAudioData ambientData;

void InitializeScene1()
{
    // ... existing code ...

    // Play ambient
    if (ambientData != null && ambientData.scene1Ambient != null)
    {
        HarmonyAudioManager.Instance.PlayAmbient(ambientData.scene1Ambient, ambientData.fadeInDuration);
    }
}

void InitializeScene2()
{
    // ... existing code ...

    // Transition ambient
    if (ambientData != null && ambientData.scene23Ambient != null)
    {
        HarmonyAudioManager.Instance.StopAmbient(ambientData.fadeOutDuration);
        Invoke("PlayScene23Ambient", ambientData.fadeOutDuration);
    }
}

void PlayScene23Ambient()
{
    if (ambientData != null && ambientData.scene23Ambient != null)
    {
        HarmonyAudioManager.Instance.PlayAmbient(ambientData.scene23Ambient, ambientData.fadeInDuration);
    }
}
```

---

## 🎵 STAGE 5: Thread Connection Sound Effects (1 hour)

### Implementation:

**Step 5.1: Add SFX to ThreadConnectionSystem**

```csharp
[Header("Audio")]
public AudioClip threadPullStartSound;
public AudioClip threadSnapSound;
public AudioClip connectionSuccessSound;

void StartThreadPull(CulturalOrb orb)
{
    // ... existing code ...

    // Play thread pull start sound
    if (threadPullStartSound != null && HarmonyAudioManager.Instance != null)
    {
        HarmonyAudioManager.Instance.PlaySFX(threadPullStartSound, 0.6f);
    }
}

void CreateConnection(CulturalOrb orbA, CulturalOrb orbB)
{
    // ... existing code ...

    // Play connection success sound
    if (connectionSuccessSound != null && HarmonyAudioManager.Instance != null)
    {
        HarmonyAudioManager.Instance.PlaySFX(connectionSuccessSound, 0.8f);
    }
}
```

**Sound Effect Recommendations:**
- Thread pull start: Soft "whoosh" or "shimmer"
- Thread snap: Gentle "click" or "magnetic snap"
- Connection success: Harmonious "chime" or "bell"

**Free SFX Sources:**
- Freesound.org: Search "magic whoosh", "UI positive", "connection"
- SFXR (generate your own): https://sfxr.me/

---

## 🎵 STAGE 6: Dynamic Music System (Advanced, 2-3 hours)

### Concept:
Music layers that build as threads are connected.

**Music Layers:**
1. Base ambient pad (always playing)
2. Layer 1: Subtle strings (after 1st connection)
3. Layer 2: Woodwinds (after 3rd connection)
4. Layer 3: Percussion (after 5th connection)
5. Layer 4: Full melody (after 7th connection)

### Free Music Resources:

1. **Incompetech** (Kevin MacLeod) - Free with attribution
   - https://incompetech.com/
   - Search: "ambient", "world", "peaceful"

2. **Purple Planet** - Free for non-commercial
   - https://www.purple-planet.com/
   - Royalty-free

3. **Bensound** - Free with attribution
   - https://www.bensound.com/

### Implementation:

**Step 6.1: Create Dynamic Music Controller**

Create: `Assets/Scripts/HarmonyInDiversity/Audio/DynamicMusicController.cs`

```csharp
using System.Collections;
using UnityEngine;

public class DynamicMusicController : MonoBehaviour
{
    public static DynamicMusicController Instance;

    [Header("Music Layers")]
    public AudioClip baseLayer;
    public AudioClip layer1; // Strings
    public AudioClip layer2; // Woodwinds
    public AudioClip layer3; // Percussion
    public AudioClip layer4; // Full melody

    [Header("Activation Thresholds")]
    public int layer1Threshold = 1; // connections
    public int layer2Threshold = 3;
    public int layer3Threshold = 5;
    public int layer4Threshold = 7;

    [Header("Settings")]
    public float layerFadeInDuration = 3f;

    private AudioSource[] audioSources;
    private int currentConnections = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetupAudioSources();
    }

    void SetupAudioSources()
    {
        audioSources = new AudioSource[5];

        // Create audio sources for each layer
        AudioClip[] clips = { baseLayer, layer1, layer2, layer3, layer4 };
        string[] names = { "BaseLayer", "Layer1_Strings", "Layer2_Woodwinds", "Layer3_Percussion", "Layer4_Melody" };

        for (int i = 0; i < 5; i++)
        {
            GameObject layerObj = new GameObject(names[i]);
            layerObj.transform.SetParent(transform);
            audioSources[i] = layerObj.AddComponent<AudioSource>();
            audioSources[i].clip = clips[i];
            audioSources[i].loop = true;
            audioSources[i].playOnAwake = false;
            audioSources[i].volume = 0f;
            audioSources[i].spatialBlend = 0f; // 2D

            // Assign to music mixer group
            if (HarmonyAudioManager.Instance != null && HarmonyAudioManager.Instance.audioMixer != null)
            {
                var musicGroup = HarmonyAudioManager.Instance.audioMixer.FindMatchingGroups("Music");
                if (musicGroup.Length > 0)
                {
                    audioSources[i].outputAudioMixerGroup = musicGroup[0];
                }
            }
        }
    }

    public void StartMusic()
    {
        // Play all layers but only base is audible
        foreach (var source in audioSources)
        {
            if (source != null && source.clip != null)
            {
                source.Play();
            }
        }

        // Fade in base layer
        StartCoroutine(FadeInLayer(0, layerFadeInDuration));
    }

    public void OnConnectionMade(int connectionCount)
    {
        currentConnections = connectionCount;

        // Activate layers based on connection count
        if (connectionCount >= layer1Threshold && audioSources[1].volume == 0)
        {
            StartCoroutine(FadeInLayer(1, layerFadeInDuration));
        }
        if (connectionCount >= layer2Threshold && audioSources[2].volume == 0)
        {
            StartCoroutine(FadeInLayer(2, layerFadeInDuration));
        }
        if (connectionCount >= layer3Threshold && audioSources[3].volume == 0)
        {
            StartCoroutine(FadeInLayer(3, layerFadeInDuration));
        }
        if (connectionCount >= layer4Threshold && audioSources[4].volume == 0)
        {
            StartCoroutine(FadeInLayer(4, layerFadeInDuration));
        }
    }

    IEnumerator FadeInLayer(int layerIndex, float duration)
    {
        if (layerIndex < 0 || layerIndex >= audioSources.Length) yield break;

        AudioSource source = audioSources[layerIndex];
        float elapsed = 0f;
        float targetVolume = 1f;

        while (elapsed < duration)
        {
            source.volume = Mathf.Lerp(0f, targetVolume, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        source.volume = targetVolume;
        Debug.Log($"Music layer {layerIndex} activated");
    }

    public void StopMusic(float fadeOutDuration = 3f)
    {
        StartCoroutine(FadeOutAllLayers(fadeOutDuration));
    }

    IEnumerator FadeOutAllLayers(float duration)
    {
        float elapsed = 0f;
        float[] startVolumes = new float[audioSources.Length];

        for (int i = 0; i < audioSources.Length; i++)
        {
            startVolumes[i] = audioSources[i].volume;
        }

        while (elapsed < duration)
        {
            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i].volume = Mathf.Lerp(startVolumes[i], 0f, elapsed / duration);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        foreach (var source in audioSources)
        {
            source.Stop();
            source.volume = 0f;
        }
    }
}
```

**Step 6.2: Hook Up to ThreadConnectionSystem**

In `ThreadConnectionSystem.CreateConnection()`:

```csharp
// Notify dynamic music system
if (DynamicMusicController.Instance != null)
{
    DynamicMusicController.Instance.OnConnectionMade(connections.Count);
}
```

**Step 6.3: Start Music in Scene 3**

In `HarmonySceneManager.InitializeScene3()`:

```csharp
// Start dynamic music
if (DynamicMusicController.Instance != null)
{
    DynamicMusicController.Instance.StartMusic();
}
```

---

## 🎵 STAGE 7: Where to Find Audio Assets

### Cultural Greeting Recordings:

**Option 1: Record with Community Members**
- Contact UBCO cultural student organizations
- Record native speakers
- 5-10 seconds per greeting
- Get permission/release forms

**Option 2: Use Free TTS**
- Google Translate TTS (quick but robotic)
- TTSMaker with different language voices
- ElevenLabs (best quality)

**Option 3: Free Audio Libraries**
- Forvo.com (pronunciation database)
- Commons.wikimedia.org (public domain audio)

### Cultural Hum/Music:

**Free Cultural Music Resources:**
1. **Free Music Archive** - https://freemusicarchive.org/
   - Search: "world music", "traditional", "cultural"
   - Filter by CC licenses

2. **Musopen** - https://musopen.org/
   - Classical music, public domain
   - World music collection

3. **Internet Archive Audio** - https://archive.org/details/audio
   - Vast collection of cultural recordings
   - Public domain

**Recommended Search Terms per Culture:**
- Japanese: "koto", "shakuhachi", "taiko"
- French: "accordion", "classical french"
- Indian: "sitar", "tabla", "raga"
- Mexican: "mariachi", "guitar", "flute"
- Nigerian: "djembe", "talking drum", "afrobeat"

**Audio Specifications:**
- Format: WAV or OGG
- Sample Rate: 44.1kHz
- Bit Depth: 16-bit
- Length: 10-30 seconds (looping)

---

## 🎵 STAGE 8: Unity Setup Checklist

### Setup Steps in Unity:

**1. Create Audio Mixer:**
   - Project → Right-click → Create → Audio → Audio Mixer
   - Name: `HarmonyAudioMixer`
   - Create groups: Master, Ambient, Orbs, Music, Narration, SFX
   - Expose volume parameters

**2. Create HarmonyAudioManager GameObject:**
   - Hierarchy → Create Empty → Name: "HarmonyAudioManager"
   - Add `HarmonyAudioManager` script
   - Assign `HarmonyAudioMixer` to the script
   - The script will auto-create audio sources

**3. Create NarrationData Asset:**
   - Project → Right-click → Create → Harmony → Narration Data
   - Name: "HarmonyNarration"
   - Assign narration clips

**4. Create AmbientAudioData Asset:**
   - Project → Right-click → Create → Harmony → Ambient Audio Data
   - Name: "HarmonyAmbient"
   - Assign ambient clips

**5. Update CulturalOrbData Assets:**
   - Select each cultural orb data asset
   - Assign greeting audio clip
   - Assign cultural hum clip
   - Adjust volume levels (0.7-1.0)

**6. Create DynamicMusicController GameObject (optional):**
   - Hierarchy → Create Empty → Name: "DynamicMusicController"
   - Add `DynamicMusicController` script
   - Assign music layer clips

**7. Update HarmonySceneManager:**
   - Select HarmonySceneManager GameObject
   - Assign NarrationData reference
   - Assign AmbientAudioData reference

**8. Test Each Scene:**
   - Scene 1: Dawn ambience + opening narration
   - Scene 2: Orb hums + greetings on touch
   - Scene 3: Thread SFX + dynamic music
   - Scene 4: Full music + ambient
   - Scene 5: Closing narration

---

## 🎵 Testing & Optimization

### Quest 2 Audio Optimization:

**1. Audio Compression:**
   - In Unity, select audio clips
   - Inspector → Force to Mono (for ambient/music)
   - Keep Stereo for narration
   - Compression: Vorbis (for Quest 2)
   - Quality: 70-80% (balance size/quality)

**2. Spatial Audio Settings:**
   - Min Distance: 1m
   - Max Distance: 20m
   - Rolloff: Custom curve (logarithmic)
   - Doppler Level: 0 (for stationary orbs)

**3. Memory Management:**
   - Load Type: Streaming (for long ambient tracks)
   - Load Type: Compressed in Memory (for short SFX)
   - Preload Audio Data: True (for critical sounds)

**4. Performance Tips:**
   - Limit to 20 simultaneous audio sources max
   - Use object pooling for SFX
   - Reduce sample rate for hums (22kHz acceptable)

### Testing Checklist:

- [ ] All orbs play greetings on touch
- [ ] Cultural hums start in Scene 2
- [ ] Narration plays at scene starts
- [ ] Ambient audio fades smoothly
- [ ] Thread connection SFX trigger correctly
- [ ] Dynamic music layers activate on connections
- [ ] No audio clipping or distortion
- [ ] Spatial audio works (walk around orbs)
- [ ] Audio mixer volumes balanced
- [ ] No lag when audio starts

---

## 🎵 Quick Start Summary

**Minimum Viable Audio (1-2 hours):**
1. Create HarmonyAudioManager → set up in scene
2. Record/download 5 cultural greetings
3. Download 5 cultural hum loops
4. Assign to CulturalOrbData assets
5. Test in VR

**Full Audio Experience (4-6 hours):**
1. Complete minimum viable audio
2. Add narration (TTS or recorded)
3. Add ambient sounds per scene
4. Add thread connection SFX
5. (Optional) Implement dynamic music system
6. Test and balance all audio levels

---

## 🎵 Example Audio Asset List

Here's what you need to collect:

### Critical (Must Have):
- [ ] 5x Cultural greeting recordings (2-5 sec each)
- [ ] 5x Cultural hum loops (10-30 sec each)
- [ ] 5x Narration clips (scene openings/closings)
- [ ] 2x Ambient loops (dawn, day)

### Nice to Have:
- [ ] Thread connection SFX (3 clips)
- [ ] UI button click sounds
- [ ] Orb activation chime
- [ ] Dynamic music layers (5 clips)

### Total Audio Files: 15-30 clips
### Total Storage: 20-50 MB (compressed)

---

## 🎵 Free Audio Creation Tools

**Audio Editing:**
- **Audacity** (Windows/Mac/Linux) - Free
- **Ocenaudio** (Windows/Mac) - Free, easier than Audacity

**Audio Conversion:**
- **FFmpeg** (command line) - Free
- **Online Audio Converter** - https://online-audio-converter.com/

**Loop Creation:**
- **LoopAuditioneer** - Makes seamless loops
- Audacity (Generate → Crossfade Loop)

**TTS Options:**
- **TTSMaker** - https://ttsmaker.com/ (Free, no login)
- **Google Cloud TTS** - High quality, free tier
- **Narakeet** - https://www.narakeet.com/ (Free tier available)

---

## Next Steps:

1. Create HarmonyAudioManager in your scene
2. Download/record 5 cultural greetings
3. Find 5 cultural music loops
4. Assign to CulturalOrbData assets
5. Build and test on Quest 2
6. Iterate based on experience

**Need help with any specific stage? Let me know!**
