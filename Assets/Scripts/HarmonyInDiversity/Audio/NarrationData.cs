using UnityEngine;

/// <summary>
/// ScriptableObject that stores all narration audio clips for Harmony in Diversity
/// Create via: Assets > Create > Harmony > Narration Data
/// </summary>
[CreateAssetMenu(fileName = "HarmonyNarration", menuName = "Harmony/Narration Data", order = 3)]
public class NarrationData : ScriptableObject
{
    [Header("Scene 1: The Courtyard Awakens")]
    [Tooltip("Opening narration for Scene 1 (dawn atmosphere)")]
    public AudioClip scene1Opening;

    [Tooltip("Delay before playing Scene 1 narration (seconds)")]
    public float scene1Delay = 5f;

    [Header("Scene 2: The Voices Rise")]
    [Tooltip("Opening narration for Scene 2 (orbs appear)")]
    public AudioClip scene2Opening;

    [Tooltip("Delay before playing Scene 2 narration (seconds)")]
    public float scene2Delay = 3f;

    [Header("Scene 3: Connecting the Threads")]
    [Tooltip("Opening narration for Scene 3 (threading mechanic)")]
    public AudioClip scene3Opening;

    [Tooltip("Delay before playing Scene 3 narration (seconds)")]
    public float scene3Delay = 3f;

    [Header("Scene 4: The Tapestry of Unity")]
    [Tooltip("Opening narration for Scene 4 (canopy formation)")]
    public AudioClip scene4Opening;

    [Tooltip("Delay before playing Scene 4 narration (seconds)")]
    public float scene4Delay = 2f;

    [Header("Scene 5: Reflection")]
    [Tooltip("Closing narration for Scene 5 (finale)")]
    public AudioClip scene5Closing;

    [Tooltip("Delay before playing Scene 5 narration (seconds)")]
    public float scene5Delay = 2f;

    [Header("Optional: Additional Narration")]
    [Tooltip("Optional mid-scene narration or guidance")]
    public AudioClip scene3Guidance; // Optional: "Try connecting different cultures..."
}
