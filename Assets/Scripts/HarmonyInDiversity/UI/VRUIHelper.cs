using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.UI;

/// <summary>
/// Helper script to ensure VR UI interaction works correctly
/// Attach to any Canvas to make it VR-compatible
/// </summary>
[RequireComponent(typeof(Canvas))]
public class VRUIHelper : MonoBehaviour
{
    [Header("Auto-Setup")]
    [Tooltip("Automatically configure canvas for VR on Start")]
    public bool autoSetup = true;

    [Header("Canvas Settings")]
    [Tooltip("Distance from player (in meters)")]
    public float canvasDistance = 3f;

    [Tooltip("Canvas scale (0.001-0.01 works well)")]
    public float canvasScale = 0.003f;

    [Header("Camera Reference")]
    [Tooltip("Leave empty to auto-find Main Camera")]
    public Camera vrCamera;

    void Start()
    {
        if (autoSetup)
        {
            SetupVRCanvas();
            EnsureEventSystem();
        }
    }

    void SetupVRCanvas()
    {
        Canvas canvas = GetComponent<Canvas>();

        // Set to World Space
        if (canvas.renderMode != RenderMode.WorldSpace)
        {
            canvas.renderMode = RenderMode.WorldSpace;
            Debug.Log("VRUIHelper: Set Canvas to World Space");
        }

        // Find camera if not assigned
        if (vrCamera == null)
        {
            vrCamera = Camera.main;
            if (vrCamera == null)
            {
                vrCamera = FindObjectOfType<Camera>();
            }
        }

        // Set event camera
        if (canvas.worldCamera == null && vrCamera != null)
        {
            canvas.worldCamera = vrCamera;
            Debug.Log($"VRUIHelper: Set event camera to {vrCamera.name}");
        }

        // Position canvas in front of camera
        if (vrCamera != null)
        {
            transform.position = vrCamera.transform.position + vrCamera.transform.forward * canvasDistance;
            transform.rotation = Quaternion.LookRotation(transform.position - vrCamera.transform.position);
            transform.localScale = Vector3.one * canvasScale;
            Debug.Log($"VRUIHelper: Positioned canvas {canvasDistance}m in front of camera");
        }

        // Ensure proper raycaster
        var graphicRaycaster = GetComponent<GraphicRaycaster>();
        if (graphicRaycaster == null)
        {
            gameObject.AddComponent<GraphicRaycaster>();
            Debug.Log("VRUIHelper: Added GraphicRaycaster");
        }

        // Check for Tracked Device Graphic Raycaster (optional but better for VR)
        var trackedRaycaster = GetComponent<TrackedDeviceGraphicRaycaster>();
        if (trackedRaycaster == null)
        {
            // Try to add it, but don't fail if XR Interaction Toolkit isn't fully set up
            Debug.Log("VRUIHelper: Using standard GraphicRaycaster (TrackedDevice not found)");
        }
    }

    void EnsureEventSystem()
    {
        // Check if EventSystem exists
        EventSystem eventSystem = FindObjectOfType<EventSystem>();

        if (eventSystem == null)
        {
            // Create EventSystem
            GameObject eventSystemObj = new GameObject("EventSystem");
            eventSystem = eventSystemObj.AddComponent<EventSystem>();
            Debug.Log("VRUIHelper: Created EventSystem");
        }

        // Check for XR UI Input Module
        var xrInputModule = eventSystem.GetComponent<XRUIInputModule>();
        if (xrInputModule == null)
        {
            // Try to add it
            try
            {
                eventSystem.gameObject.AddComponent<XRUIInputModule>();
                Debug.Log("VRUIHelper: Added XR UI Input Module");

                // Disable old input module if present
                var standaloneInput = eventSystem.GetComponent<StandaloneInputModule>();
                if (standaloneInput != null)
                {
                    standaloneInput.enabled = false;
                    Debug.Log("VRUIHelper: Disabled Standalone Input Module");
                }
            }
            catch
            {
                Debug.LogWarning("VRUIHelper: Could not add XR UI Input Module - make sure XR Interaction Toolkit is properly installed");
            }
        }
    }

    [ContextMenu("Setup VR Canvas Now")]
    public void SetupNow()
    {
        SetupVRCanvas();
        EnsureEventSystem();
    }

    void OnDrawGizmos()
    {
        // Draw canvas outline in Scene view
        if (GetComponent<Canvas>().renderMode == RenderMode.WorldSpace)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(rectTransform.rect.width, rectTransform.rect.height, 0));
        }
    }
}
