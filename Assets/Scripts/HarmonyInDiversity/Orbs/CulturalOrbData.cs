using UnityEngine;

/// <summary>
/// ScriptableObject that stores data for a cultural orb
/// Create instances via Assets > Create > Harmony > Cultural Orb Data
/// </summary>
[CreateAssetMenu(fileName = "CulturalOrbData", menuName = "Harmony/Cultural Orb Data", order = 1)]
public class CulturalOrbData : ScriptableObject
{
    [Header("Culture Information")]
    [Tooltip("Name of the culture (e.g., Japanese, French)")]
    public string cultureName = "Unknown Culture";

    [Tooltip("Traditional greeting in this culture")]
    public string greetingText = "Hello";

    [Header("Visual Properties")]
    [Tooltip("Primary color for this orb")]
    public Color orbColor = Color.white;

    [Tooltip("Optional: Reference to the orb prefab variant")]
    public GameObject orbPrefab;
}
