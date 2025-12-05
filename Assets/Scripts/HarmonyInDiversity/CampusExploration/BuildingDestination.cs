using UnityEngine;

/// <summary>
/// ScriptableObject that stores data for a UBCO building destination
/// Create instances via Assets > Create > Harmony > Building Destination
/// </summary>
[CreateAssetMenu(fileName = "BuildingDestination", menuName = "Harmony/Building Destination", order = 2)]
public class BuildingDestination : ScriptableObject
{
    [Header("Building Information")]
    [Tooltip("Name of the building (e.g., 'Engineering Building')")]
    public string buildingName = "Unknown Building";

    [Tooltip("Building code (e.g., 'ENG')")]
    public string buildingCode = "XXX";

    [Tooltip("Name of the GameObject in scene (e.g., 'Building_Engineering')")]
    public string buildingMarkerName = "Building_Engineering";

    [Header("Runtime Reference (Do Not Edit)")]
    [Tooltip("Transform found at runtime - do not assign manually")]
    [HideInInspector]
    public Transform buildingTransform;

    [Header("Orb Assignment")]
    [Tooltip("Which cultural orb should fly to this building")]
    public CulturalOrbData assignedOrb;

    [Header("Faculty Profile")]
    [Tooltip("Photo of the professor/staff member (512x512 recommended)")]
    public Sprite facultyPhoto;

    [Tooltip("Faculty member's name (e.g., 'Dr. Jane Smith')")]
    public string facultyName = "Faculty Name";

    [Tooltip("Faculty member's title (e.g., 'Professor of Engineering')")]
    public string facultyTitle = "Title";

    [Tooltip("Faculty member's department")]
    public string facultyDepartment = "Department";

    [Tooltip("Short biography (2-3 sentences)")]
    [TextArea(3, 6)]
    public string facultyBio = "Biography goes here...";

    [Tooltip("How this faculty member connects to the culture")]
    [TextArea(2, 4)]
    public string culturalConnection = "Cultural connection...";

    [Header("Audio (Optional)")]
    [Tooltip("Optional: Faculty greeting audio clip")]
    public AudioClip facultyVoiceClip;
}
