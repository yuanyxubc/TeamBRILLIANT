using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Central manager for all audio in Harmony in Diversity
/// Handles spatial audio, music layers, and audio mixing
/// </summary>
public class HarmonyAudioManager : MonoBehaviour
{
    public static HarmonyAudioManager Instance;

    [Header("Audio Mixer")]
    [Tooltip("Reference to the main audio mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Sources")]
    [Tooltip("2D audio source for narration")]
    public AudioSource narrationSource;

    [Tooltip("2D audio source for ambient sounds")]
    public AudioSource ambientSource;

    [Tooltip("2D audio source for music")]
    public AudioSource musicSource;

    [Tooltip("2D audio source for UI/SFX")]
    public AudioSource sfxSource;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float ambientVolume = 0.6f;
    [Range(0f, 1f)] public float orbVolume = 0.8f;
    [Range(0f, 1f)] public float musicVolume = 0.7f;
    [Range(0f, 1f)] public float narrationVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 0.8f;

    [Header("Spatial Audio Settings")]
    [Tooltip("Max distance for orb audio to be heard")]
    public float orbAudioMaxDistance = 20f;

    [Tooltip("Rolloff curve for spatial audio")]
    public AnimationCurve spatialRolloffCurve = AnimationCurve.Linear(0, 1, 1, 0);

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeAudioSources();
        ApplyVolumeSettings();
    }

    void InitializeAudioSources()
    {
        // Create audio sources if they don't exist
        if (narrationSource == null)
        {
            GameObject narrationObj = new GameObject("NarrationSource");
            narrationObj.transform.SetParent(transform);
            narrationSource = narrationObj.AddComponent<AudioSource>();
            narrationSource.spatialBlend = 0f; // 2D
            narrationSource.playOnAwake = false;
        }

        if (ambientSource == null)
        {
            GameObject ambientObj = new GameObject("AmbientSource");
            ambientObj.transform.SetParent(transform);
            ambientSource = ambientObj.AddComponent<AudioSource>();
            ambientSource.spatialBlend = 0f; // 2D
            ambientSource.loop = true;
            ambientSource.playOnAwake = false;
        }

        if (musicSource == null)
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.SetParent(transform);
            musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.spatialBlend = 0f; // 2D
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFXSource");
            sfxObj.transform.SetParent(transform);
            sfxSource = sfxObj.AddComponent<AudioSource>();
            sfxSource.spatialBlend = 0f; // 2D
            sfxSource.playOnAwake = false;
        }

        // Assign to mixer groups if mixer exists
        if (audioMixer != null)
        {
            narrationSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Narration")[0];
            ambientSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Ambient")[0];
            musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
            sfxSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
        }
    }

    void ApplyVolumeSettings()
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
            audioMixer.SetFloat("AmbientVolume", Mathf.Log10(ambientVolume) * 20);
            audioMixer.SetFloat("OrbVolume", Mathf.Log10(orbVolume) * 20);
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
            audioMixer.SetFloat("NarrationVolume", Mathf.Log10(narrationVolume) * 20);
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
        }
    }

    #region Narration

    public void PlayNarration(AudioClip clip, float delay = 0f)
    {
        if (clip != null && narrationSource != null)
        {
            StartCoroutine(PlayNarrationDelayed(clip, delay));
        }
    }

    IEnumerator PlayNarrationDelayed(AudioClip clip, float delay)
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        narrationSource.clip = clip;
        narrationSource.Play();
        Debug.Log($"Playing narration: {clip.name}");
    }

    public void StopNarration()
    {
        if (narrationSource != null)
            narrationSource.Stop();
    }

    #endregion

    #region Ambient & Environment

    public void PlayAmbient(AudioClip clip, float fadeInDuration = 2f)
    {
        if (clip != null && ambientSource != null)
        {
            StartCoroutine(FadeInAmbient(clip, fadeInDuration));
        }
    }

    IEnumerator FadeInAmbient(AudioClip clip, float duration)
    {
        ambientSource.clip = clip;
        ambientSource.volume = 0f;
        ambientSource.Play();

        float elapsed = 0f;
        float targetVolume = ambientVolume;

        while (elapsed < duration)
        {
            ambientSource.volume = Mathf.Lerp(0f, targetVolume, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ambientSource.volume = targetVolume;
        Debug.Log($"Ambient audio playing: {clip.name}");
    }

    public void StopAmbient(float fadeOutDuration = 2f)
    {
        StartCoroutine(FadeOutAmbient(fadeOutDuration));
    }

    IEnumerator FadeOutAmbient(float duration)
    {
        float startVolume = ambientSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            ambientSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ambientSource.Stop();
        ambientSource.volume = startVolume;
    }

    #endregion

    #region SFX

    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, volumeScale);
        }
    }

    #endregion

    #region Music

    public void PlayMusic(AudioClip clip, float fadeInDuration = 3f)
    {
        if (clip != null && musicSource != null)
        {
            StartCoroutine(FadeInMusic(clip, fadeInDuration));
        }
    }

    IEnumerator FadeInMusic(AudioClip clip, float duration)
    {
        musicSource.clip = clip;
        musicSource.volume = 0f;
        musicSource.Play();

        float elapsed = 0f;
        float targetVolume = musicVolume;

        while (elapsed < duration)
        {
            musicSource.volume = Mathf.Lerp(0f, targetVolume, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        musicSource.volume = targetVolume;
        Debug.Log($"Music playing: {clip.name}");
    }

    public void StopMusic(float fadeOutDuration = 3f)
    {
        StartCoroutine(FadeOutMusic(fadeOutDuration));
    }

    IEnumerator FadeOutMusic(float duration)
    {
        float startVolume = musicSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume;
    }

    #endregion

    #region Spatial Audio Helpers

    /// <summary>
    /// Configure an audio source for spatial (3D) playback with proper Quest 2 settings
    /// </summary>
    public void ConfigureSpatialAudio(AudioSource source, float maxDistance = -1)
    {
        if (source == null) return;

        // Use provided max distance or default
        float distance = maxDistance > 0 ? maxDistance : orbAudioMaxDistance;

        // Configure for spatial audio
        source.spatialBlend = 1f; // Fully 3D
        source.rolloffMode = AudioRolloffMode.Custom;
        source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, spatialRolloffCurve);
        source.minDistance = 1f;
        source.maxDistance = distance;
        source.dopplerLevel = 0f; // No doppler for stationary orbs

        // Quest 2 optimization
        source.spread = 0f; // Point source
        source.priority = 128; // Medium priority
    }

    #endregion

    #region Volume Control (for settings menu later)

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        ApplyVolumeSettings();
    }

    public void SetOrbVolume(float volume)
    {
        orbVolume = Mathf.Clamp01(volume);
        ApplyVolumeSettings();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        ApplyVolumeSettings();
    }

    public void SetNarrationVolume(float volume)
    {
        narrationVolume = Mathf.Clamp01(volume);
        ApplyVolumeSettings();
    }

    #endregion
}
