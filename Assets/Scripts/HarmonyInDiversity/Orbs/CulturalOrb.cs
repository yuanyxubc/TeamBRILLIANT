using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Cultural Orb - Extends the existing MysticOrb with cultural data and interactions
/// </summary>
[RequireComponent(typeof(MysticOrb))]
[RequireComponent(typeof(Collider))]
public class CulturalOrb : MonoBehaviour
{
    [Header("Cultural Data")]
    public CulturalOrbData data;

    [Header("Interaction Settings")]
    [Tooltip("Proximity radius for glow effect (meters)")]
    public float proximityRadius = 2f;

    [Tooltip("Glow intensity multiplier when player is near")]
    public float proximityGlowMultiplier = 2f;

    [Tooltip("Pulse scale multiplier on activation")]
    public float pulseScaleMultiplier = 0.3f;

    [Tooltip("Duration of pulse animation")]
    public float pulseDuration = 0.5f;

    // Component references
    private MysticOrb mysticOrb;
    private OrbFloating orbFloating;
    private XRGrabInteractable grabInteractable;
    private SphereCollider proximityTrigger;

    // Audio components
    private AudioSource greetingAudioSource;
    private AudioSource humAudioSource;

    // State
    private float baseGlowIntensity = 1f;
    private float currentGlowIntensity = 1f;
    private bool isActivated = false;
    private bool playerInProximity = false;

    // Connection tracking
    public List<CulturalOrb> connectedOrbs = new List<CulturalOrb>();

    void Awake()
    {
        // Get required components
        mysticOrb = GetComponent<MysticOrb>();
        orbFloating = GetComponent<OrbFloating>();

        if (mysticOrb == null)
        {
            Debug.LogError($"CulturalOrb on {gameObject.name} requires MysticOrb component!");
        }
    }

    void Start()
    {
        // Apply cultural color if data exists
        if (data != null && mysticOrb != null)
        {
            mysticOrb.SetOrbColor(data.orbColor);
        }
        else if (data == null)
        {
            Debug.LogWarning($"CulturalOrb on {gameObject.name} has no CulturalOrbData assigned!");
        }

        // Setup interaction
        SetupInteraction();

        // Setup proximity detection
        SetupProximityDetection();

        // Setup audio
        SetupAudio();
    }

    void SetupInteraction()
    {
        // Get or add XRGrabInteractable
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            grabInteractable = gameObject.AddComponent<XRGrabInteractable>();
        }

        // Configure grab interactable
        grabInteractable.movementType = XRBaseInteractable.MovementType.Instantaneous;
        grabInteractable.throwOnDetach = false;

        // Subscribe to events
        grabInteractable.selectEntered.AddListener(OnActivated);
        grabInteractable.hoverEntered.AddListener(OnHoverEnter);
        grabInteractable.hoverExited.AddListener(OnHoverExit);
    }

    void Update()
    {
        // NOTE: We keep XRGrabInteractable enabled during Scene 3 for raycast detection
        // Actual grab behavior is prevented in OnActivated() method
    }

    void SetupProximityDetection()
    {
        // Add proximity trigger collider
        proximityTrigger = gameObject.AddComponent<SphereCollider>();
        proximityTrigger.isTrigger = true;
        proximityTrigger.radius = proximityRadius;

        // Note: Tag is not needed - proximity detection uses trigger colliders
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if player camera entered proximity
        if (other.CompareTag("MainCamera") || other.name.Contains("Camera"))
        {
            playerInProximity = true;
            SetGlowIntensity(proximityGlowMultiplier);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if player camera exited proximity
        if (other.CompareTag("MainCamera") || other.name.Contains("Camera"))
        {
            playerInProximity = false;
            SetGlowIntensity(baseGlowIntensity);
        }
    }

    void OnHoverEnter(HoverEnterEventArgs args)
    {
        // Additional visual feedback on hover
        if (!playerInProximity)
        {
            SetGlowIntensity(proximityGlowMultiplier * 0.75f);
        }
    }

    void OnHoverExit(HoverExitEventArgs args)
    {
        // Return to normal if not in proximity
        if (!playerInProximity)
        {
            SetGlowIntensity(baseGlowIntensity);
        }
    }

    void OnActivated(SelectEnterEventArgs args)
    {
        // During Scene 3 (thread connection), prevent actual grabbing but keep interactable enabled for raycasts
        if (HarmonySceneManager.Instance != null &&
            HarmonySceneManager.Instance.CurrentState == SceneState.ConnectingThreads)
        {
            // Cancel the grab interaction
            if (grabInteractable != null && grabInteractable.isSelected)
            {
                grabInteractable.interactionManager.CancelInteractableSelection((IXRSelectInteractable)grabInteractable);
            }
            return; // Don't process activation during Scene 3
        }

        if (!isActivated)
        {
            isActivated = true;
            TriggerPulseEffect();

            // Show culture info in UI
            if (HarmonyUIManager.Instance != null && data != null)
            {
                HarmonyUIManager.Instance.ShowCultureInfo(data.cultureName, data.greetingText);
            }

            // Play greeting audio
            PlayGreeting();

            // Haptic feedback
            SendHapticFeedback(args.interactorObject, 0.5f, 0.2f);
        }
    }

    public void TriggerPulseEffect()
    {
        StartCoroutine(PulseAnimation());
    }

    IEnumerator PulseAnimation()
    {
        float elapsed = 0f;
        Vector3 originalScale = transform.localScale;

        while (elapsed < pulseDuration)
        {
            float t = elapsed / pulseDuration;
            float scale = 1f + Mathf.Sin(t * Mathf.PI) * pulseScaleMultiplier;
            transform.localScale = originalScale * scale;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
    }

    void SetGlowIntensity(float intensity)
    {
        currentGlowIntensity = intensity;

        if (mysticOrb != null && data != null)
        {
            mysticOrb.SetOrbColor(data.orbColor * intensity);
        }
    }

    void SendHapticFeedback(IXRInteractor interactor, float amplitude, float duration)
    {
        if (interactor is XRBaseInputInteractor controllerInteractor)
        {
            if (controllerInteractor.xrController != null)
            {
                controllerInteractor.xrController.SendHapticImpulse(amplitude, duration);
            }
        }
    }

    public void AddConnection(CulturalOrb otherOrb)
    {
        if (!connectedOrbs.Contains(otherOrb))
        {
            connectedOrbs.Add(otherOrb);
        }
    }

    public bool IsConnectedTo(CulturalOrb otherOrb)
    {
        return connectedOrbs.Contains(otherOrb);
    }

    #region Audio Setup and Control

    void SetupAudio()
    {
        if (data == null) return;

        // Create greeting audio source (one-shot, 3D spatial)
        GameObject greetingObj = new GameObject("GreetingAudio");
        greetingObj.transform.SetParent(transform);
        greetingObj.transform.localPosition = Vector3.zero;
        greetingAudioSource = greetingObj.AddComponent<AudioSource>();
        greetingAudioSource.playOnAwake = false;
        greetingAudioSource.loop = false;
        greetingAudioSource.clip = data.greetingAudio;
        greetingAudioSource.volume = data.greetingVolume;

        // Configure spatial audio for greeting
        if (HarmonyAudioManager.Instance != null)
        {
            HarmonyAudioManager.Instance.ConfigureSpatialAudio(greetingAudioSource);
        }

        // Assign to Orb mixer group
        if (HarmonyAudioManager.Instance != null && HarmonyAudioManager.Instance.audioMixer != null)
        {
            var orbGroup = HarmonyAudioManager.Instance.audioMixer.FindMatchingGroups("Orbs");
            if (orbGroup.Length > 0)
            {
                greetingAudioSource.outputAudioMixerGroup = orbGroup[0];
            }
        }

        // Create cultural hum audio source (looping, 3D spatial)
        GameObject humObj = new GameObject("CulturalHumAudio");
        humObj.transform.SetParent(transform);
        humObj.transform.localPosition = Vector3.zero;
        humAudioSource = humObj.AddComponent<AudioSource>();
        humAudioSource.playOnAwake = false;
        humAudioSource.loop = true;
        humAudioSource.clip = data.culturalHum;
        humAudioSource.volume = data.humVolume;

        // Configure spatial audio for hum
        if (HarmonyAudioManager.Instance != null)
        {
            HarmonyAudioManager.Instance.ConfigureSpatialAudio(humAudioSource);
        }

        // Assign to Orb mixer group
        if (HarmonyAudioManager.Instance != null && HarmonyAudioManager.Instance.audioMixer != null)
        {
            var orbGroup = HarmonyAudioManager.Instance.audioMixer.FindMatchingGroups("Orbs");
            if (orbGroup.Length > 0)
            {
                humAudioSource.outputAudioMixerGroup = orbGroup[0];
            }
        }

        // Start playing cultural hum in Scene 2 (after orb spawns)
        if (HarmonySceneManager.Instance != null &&
            HarmonySceneManager.Instance.CurrentState == SceneState.VoicesRise)
        {
            StartCulturalHum();
        }
    }

    public void StartCulturalHum()
    {
        if (humAudioSource != null && data != null && data.culturalHum != null)
        {
            humAudioSource.Play();
            Debug.Log($"{data.cultureName} orb: Cultural hum started");
        }
    }

    public void StopCulturalHum(float fadeOutDuration = 1f)
    {
        if (humAudioSource != null && humAudioSource.isPlaying)
        {
            StartCoroutine(FadeOutHum(fadeOutDuration));
        }
    }

    IEnumerator FadeOutHum(float duration)
    {
        float startVolume = humAudioSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            humAudioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        humAudioSource.Stop();
        humAudioSource.volume = startVolume;
    }

    public void PlayGreeting()
    {
        if (greetingAudioSource != null && data != null && data.greetingAudio != null)
        {
            greetingAudioSource.Play();
            Debug.Log($"{data.cultureName} orb: Playing greeting");
        }
    }

    #endregion

    void OnDestroy()
    {
        // Clean up event listeners
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnActivated);
            grabInteractable.hoverEntered.RemoveListener(OnHoverEnter);
            grabInteractable.hoverExited.RemoveListener(OnHoverExit);
        }
    }
}
