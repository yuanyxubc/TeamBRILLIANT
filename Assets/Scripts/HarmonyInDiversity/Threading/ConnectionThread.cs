using System.Collections;
using UnityEngine;

/// <summary>
/// Represents a permanent connection between two cultural orbs
/// </summary>
public class ConnectionThread : MonoBehaviour
{
    [Header("Connected Orbs")]
    public CulturalOrb orbA;
    public CulturalOrb orbB;

    [Header("Visual Settings")]
    public float threadWidth = 0.05f;
    public int lineSegments = 10;

    private LineRenderer lineRenderer;
    private Material threadMaterial;
    private Color blendedColor;

    public void Initialize(CulturalOrb a, CulturalOrb b, float width)
    {
        orbA = a;
        orbB = b;
        threadWidth = width;

        SetupLineRenderer();
        UpdateThreadColor();
    }

    void SetupLineRenderer()
    {
        // Add LineRenderer if not present
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // Configure LineRenderer
        lineRenderer.startWidth = threadWidth;
        lineRenderer.endWidth = threadWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;

        // Create material (using Sprites/Default shader which works with URP)
        threadMaterial = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material = threadMaterial;

        // Optional: Better visual quality
        lineRenderer.numCapVertices = 5;
        lineRenderer.numCornerVertices = 5;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;
    }

    void UpdateThreadColor()
    {
        if (orbA != null && orbA.data != null && orbB != null && orbB.data != null)
        {
            // Blend the two orb colors
            blendedColor = Color.Lerp(orbA.data.orbColor, orbB.data.orbColor, 0.5f);

            // Apply to LineRenderer
            lineRenderer.startColor = blendedColor;
            lineRenderer.endColor = blendedColor;

            // Apply to material
            threadMaterial.color = blendedColor;
        }
    }

    void Update()
    {
        // Keep thread connected between orbs
        if (orbA != null && orbB != null)
        {
            lineRenderer.SetPosition(0, orbA.transform.position);
            lineRenderer.SetPosition(1, orbB.transform.position);
        }
    }

    /// <summary>
    /// Animate thread to canopy formation in Scene 4
    /// </summary>
    public void AnimateToCanopy(Vector3 centerPoint, float duration)
    {
        StartCoroutine(MoveToCanopy(centerPoint, duration));
    }

    IEnumerator MoveToCanopy(Vector3 target, float duration)
    {
        if (orbA == null || orbB == null)
            yield break;

        Vector3 startA = orbA.transform.position;
        Vector3 startB = orbB.transform.position;

        // Create target positions above center point with some randomness
        Vector3 targetA = target + Vector3.up * 5f + Random.insideUnitSphere * 2f;
        Vector3 targetB = target + Vector3.up * 5f + Random.insideUnitSphere * 2f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // Use smooth curve for animation
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            // Update line positions
            lineRenderer.SetPosition(0, Vector3.Lerp(startA, targetA, smoothT));
            lineRenderer.SetPosition(1, Vector3.Lerp(startB, targetB, smoothT));

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set final positions
        lineRenderer.SetPosition(0, targetA);
        lineRenderer.SetPosition(1, targetB);
    }

    public Color GetBlendedColor()
    {
        return blendedColor;
    }
}
