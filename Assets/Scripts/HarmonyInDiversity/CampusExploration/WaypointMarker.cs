using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Waypoint marker that floats above buildings and shows distance to player
/// </summary>
public class WaypointMarker : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("Icon showing orb color")]
    public Image markerIcon;

    [Tooltip("Text showing building name and distance")]
    public TextMeshProUGUI distanceText;

    [Header("Settings")]
    [Tooltip("How often to update distance (seconds)")]
    public float updateInterval = 0.2f;

    [Tooltip("Height above building to position marker")]
    public float heightAboveBuilding = 5f;

    // References
    private CulturalOrb associatedOrb;
    private BuildingDestination building;
    private Transform playerTransform;
    private bool isActive = true;

    /// <summary>
    /// Initialize the waypoint marker
    /// </summary>
    public void Initialize(CulturalOrb orb, BuildingDestination buildingDest)
    {
        associatedOrb = orb;
        building = buildingDest;

        // Get player reference
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            playerTransform = mainCam.transform;
        }

        // Set marker color to match orb color
        if (markerIcon != null && orb.data != null)
        {
            markerIcon.color = orb.data.orbColor;
        }

        // Start updating distance
        InvokeRepeating(nameof(UpdateDistance), 0f, updateInterval);
    }

    void Update()
    {
        if (!isActive || building == null || building.buildingTransform == null)
            return;

        if (playerTransform == null)
        {
            // Try to find player again
            Camera mainCam = Camera.main;
            if (mainCam != null)
                playerTransform = mainCam.transform;

            return;
        }

        // Position above building
        transform.position = building.buildingTransform.position + Vector3.up * heightAboveBuilding;

        // Always face player
        transform.LookAt(playerTransform);
        transform.Rotate(0, 180, 0); // Flip to face player correctly
    }

    private void UpdateDistance()
    {
        if (!isActive || building == null || building.buildingTransform == null || playerTransform == null)
            return;

        // Calculate distance
        float distance = Vector3.Distance(playerTransform.position, building.buildingTransform.position);

        // Update text
        if (distanceText != null)
        {
            distanceText.text = $"{building.buildingName}\n{distance:F0}m";
        }
    }

    /// <summary>
    /// Hide and deactivate this waypoint marker
    /// </summary>
    public void Hide()
    {
        isActive = false;
        CancelInvoke(nameof(UpdateDistance));
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(UpdateDistance));
    }
}
