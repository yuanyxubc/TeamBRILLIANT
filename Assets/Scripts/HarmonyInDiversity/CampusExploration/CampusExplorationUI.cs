using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI controller for Campus Exploration scene
/// Manages faculty profiles, waypoints, and discovery counter
/// </summary>
public class CampusExplorationUI : MonoBehaviour
{
    [Header("UI Panels")]
    [Tooltip("Container for waypoint markers")]
    public GameObject waypointContainer;

    [Tooltip("Faculty profile display panel")]
    public GameObject facultyProfilePanel;

    [Tooltip("Completion message panel")]
    public GameObject completionMessagePanel;

    [Header("Faculty Profile Elements")]
    [Tooltip("Image showing faculty photo")]
    public Image facultyPhotoImage;

    [Tooltip("Faculty name text")]
    public TextMeshProUGUI facultyNameText;

    [Tooltip("Faculty title text")]
    public TextMeshProUGUI facultyTitleText;

    [Tooltip("Faculty bio text")]
    public TextMeshProUGUI facultyBioText;

    [Tooltip("Building name text")]
    public TextMeshProUGUI buildingNameText;

    [Header("Discovery Counter")]
    [Tooltip("Text showing buildings discovered")]
    public TextMeshProUGUI discoveryCounterText;

    [Header("Completion Message")]
    [Tooltip("Text for completion message")]
    public TextMeshProUGUI completionMessageText;

    [Header("Waypoint System")]
    [Tooltip("Prefab for waypoint markers (will be instantiated)")]
    public GameObject waypointMarkerPrefab;

    // Runtime tracking
    private Dictionary<CulturalOrb, WaypointMarker> activeWaypoints;

    void Awake()
    {
        activeWaypoints = new Dictionary<CulturalOrb, WaypointMarker>();

        // Initialize UI state - hide everything
        if (facultyProfilePanel != null)
            facultyProfilePanel.SetActive(false);

        if (completionMessagePanel != null)
            completionMessagePanel.SetActive(false);
    }

    /// <summary>
    /// Initialize UI for scene start
    /// </summary>
    public void Initialize(int totalBuildings)
    {
        UpdateDiscoveryCounter(0, totalBuildings);
    }

    /// <summary>
    /// Create waypoint markers for all orb-building pairs
    /// </summary>
    public void ShowWaypoints(Dictionary<CulturalOrb, BuildingDestination> orbBuildingMap)
    {
        if (waypointMarkerPrefab == null)
        {
            Debug.LogWarning("CampusExplorationUI: No waypoint marker prefab assigned!");
            return;
        }

        if (waypointContainer == null)
        {
            Debug.LogWarning("CampusExplorationUI: No waypoint container assigned!");
            return;
        }

        foreach (var kvp in orbBuildingMap)
        {
            CulturalOrb orb = kvp.Key;
            BuildingDestination building = kvp.Value;

            // Create waypoint marker
            GameObject markerObj = Instantiate(waypointMarkerPrefab, waypointContainer.transform);
            WaypointMarker marker = markerObj.GetComponent<WaypointMarker>();

            if (marker != null)
            {
                marker.Initialize(orb, building);
                activeWaypoints[orb] = marker;
            }
            else
            {
                Debug.LogWarning($"Waypoint prefab missing WaypointMarker component!");
            }
        }

        Debug.Log($"Created {activeWaypoints.Count} waypoint markers");
    }

    /// <summary>
    /// Hide all waypoint markers
    /// </summary>
    public void HideWaypoints()
    {
        foreach (var marker in activeWaypoints.Values)
        {
            if (marker != null)
            {
                marker.Hide();
            }
        }
        activeWaypoints.Clear();
    }

    /// <summary>
    /// Display faculty profile panel with building information
    /// </summary>
    public void ShowFacultyProfile(BuildingDestination building)
    {
        if (facultyProfilePanel == null)
        {
            Debug.LogWarning("CampusExplorationUI: Faculty profile panel not assigned!");
            return;
        }

        // Populate faculty information
        if (facultyPhotoImage != null && building.facultyPhoto != null)
        {
            facultyPhotoImage.sprite = building.facultyPhoto;
            facultyPhotoImage.gameObject.SetActive(true);
        }
        else if (facultyPhotoImage != null)
        {
            facultyPhotoImage.gameObject.SetActive(false);
        }

        if (facultyNameText != null)
            facultyNameText.text = building.facultyName;

        if (facultyTitleText != null)
            facultyTitleText.text = building.facultyTitle;

        if (facultyBioText != null)
        {
            string bioText = building.facultyBio;
            if (!string.IsNullOrEmpty(building.culturalConnection))
            {
                bioText += "\n\n<i>" + building.culturalConnection + "</i>";
            }
            facultyBioText.text = bioText;
        }

        if (buildingNameText != null)
            buildingNameText.text = building.buildingName;

        // Show panel with fade
        facultyProfilePanel.SetActive(true);
        StartCoroutine(FadeInPanel(facultyProfilePanel));

        Debug.Log($"Showing faculty profile: {building.facultyName}");
    }

    /// <summary>
    /// Hide faculty profile panel
    /// </summary>
    public void HideFacultyProfile()
    {
        if (facultyProfilePanel != null && facultyProfilePanel.activeSelf)
        {
            StartCoroutine(FadeOutPanel(facultyProfilePanel));
        }
    }

    /// <summary>
    /// Update discovery counter UI
    /// </summary>
    public void UpdateDiscoveryCounter(int discovered, int total)
    {
        if (discoveryCounterText != null)
        {
            discoveryCounterText.text = $"Buildings Discovered: {discovered} / {total}";

            // Change color when complete
            if (discovered >= total)
            {
                discoveryCounterText.color = Color.green;
            }
        }
    }

    /// <summary>
    /// Show completion message
    /// </summary>
    public void ShowCompletionMessage()
    {
        if (completionMessagePanel != null)
        {
            // Set message text
            if (completionMessageText != null)
            {
                completionMessageText.text = "All cultures discovered across UBCO campus!\n\nReturning to courtyard...";
            }

            completionMessagePanel.SetActive(true);
            StartCoroutine(FadeInPanel(completionMessagePanel));

            Debug.Log("Showing completion message");
        }
    }

    /// <summary>
    /// Hide all UI elements when scene completes
    /// </summary>
    public void HideAllUI()
    {
        // Hide faculty profile panel
        if (facultyProfilePanel != null && facultyProfilePanel.activeSelf)
        {
            facultyProfilePanel.SetActive(false);
        }

        // Hide discovery counter
        if (discoveryCounterText != null && discoveryCounterText.gameObject.activeSelf)
        {
            discoveryCounterText.gameObject.SetActive(false);
        }

        // Hide completion message panel
        if (completionMessagePanel != null && completionMessagePanel.activeSelf)
        {
            completionMessagePanel.SetActive(false);
        }

        // Hide waypoints (should already be hidden, but just in case)
        HideWaypoints();

        Debug.Log("All Campus Exploration UI hidden");
    }

    private IEnumerator FadeInPanel(GameObject panel)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panel.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOutPanel(GameObject panel)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            panel.SetActive(false);
            yield break;
        }

        float elapsed = 0f;
        float duration = 0.5f;
        float startAlpha = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        panel.SetActive(false);
    }
}
