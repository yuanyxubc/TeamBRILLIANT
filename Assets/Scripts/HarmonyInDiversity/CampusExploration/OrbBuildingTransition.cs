using System.Collections;
using UnityEngine;

/// <summary>
/// Handles orb movement to and from buildings during Campus Exploration scene
/// </summary>
public class OrbBuildingTransition : MonoBehaviour
{
    [Header("Transition Settings")]
    [Tooltip("Time it takes to fly to/from building")]
    public float transitionDuration = 5f;

    [Tooltip("Height above building where orb orbits")]
    public float orbitHeight = 3f;

    [Tooltip("Radius of orbit circle")]
    public float orbitRadius = 1.5f;

    [Tooltip("Speed of orbit rotation")]
    public float orbitSpeed = 0.5f;

    [Tooltip("Animation curve for movement (ease in/out)")]
    public AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    // References
    private CulturalOrb orb;
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    // State
    private bool isAtBuilding = false;
    private bool isReturning = false;
    private Coroutine orbitCoroutine;

    /// <summary>
    /// Initialize the transition component
    /// </summary>
    public void Initialize(CulturalOrb culturalOrb, Vector3 startPos, Vector3 targetPos)
    {
        orb = culturalOrb;
        originalPosition = startPos;
        targetPosition = targetPos;
    }

    /// <summary>
    /// Start transition to building
    /// </summary>
    public void TransitionToBuilding()
    {
        StartCoroutine(MoveToBuilding());
    }

    /// <summary>
    /// Start transition back to courtyard
    /// </summary>
    public void TransitionToCourtyard()
    {
        isReturning = true;

        // Stop orbiting
        if (orbitCoroutine != null)
        {
            StopCoroutine(orbitCoroutine);
            orbitCoroutine = null;
        }

        StartCoroutine(ReturnToCourtyard());
    }

    private IEnumerator MoveToBuilding()
    {
        if (orb == null)
        {
            Debug.LogError("OrbBuildingTransition: Orb reference is null!");
            yield break;
        }

        Vector3 startPos = orb.transform.position;
        Vector3 endPos = targetPosition + Vector3.up * orbitHeight;

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            float t = elapsed / transitionDuration;
            float curveT = transitionCurve.Evaluate(t);

            // Calculate position with arc
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, curveT);

            // Add arc height (sine wave for nice curve)
            float arcHeight = Mathf.Sin(t * Mathf.PI) * 5f;
            currentPos.y += arcHeight;

            orb.transform.position = currentPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set final position
        orb.transform.position = endPos;
        isAtBuilding = true;

        // Start orbiting
        orbitCoroutine = StartCoroutine(OrbitBuilding());

        Debug.Log($"{orb.data.cultureName} orb arrived at building");
    }

    private IEnumerator OrbitBuilding()
    {
        Vector3 orbitCenter = targetPosition + Vector3.up * orbitHeight;
        float angle = Random.Range(0f, Mathf.PI * 2f); // Random start angle

        while (isAtBuilding && !isReturning)
        {
            angle += orbitSpeed * Time.deltaTime;

            // Calculate orbit position
            Vector3 offset = new Vector3(
                Mathf.Cos(angle) * orbitRadius,
                0,
                Mathf.Sin(angle) * orbitRadius
            );

            orb.transform.position = orbitCenter + offset;

            yield return null;
        }
    }

    private IEnumerator ReturnToCourtyard()
    {
        isAtBuilding = false;

        Vector3 startPos = orb.transform.position;

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            float t = elapsed / transitionDuration;
            float curveT = transitionCurve.Evaluate(t);

            // Calculate position with arc
            Vector3 currentPos = Vector3.Lerp(startPos, originalPosition, curveT);

            // Add arc height
            float arcHeight = Mathf.Sin(t * Mathf.PI) * 3f;
            currentPos.y += arcHeight;

            orb.transform.position = currentPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set final position
        orb.transform.position = originalPosition;
        isReturning = false;

        Debug.Log($"{orb.data.cultureName} orb returned to courtyard");

        // Notify manager
        if (CampusExplorationManager.Instance != null)
        {
            CampusExplorationManager.Instance.NotifyOrbReturned(orb);
        }
    }

    private void OnDestroy()
    {
        // Clean up coroutines
        if (orbitCoroutine != null)
        {
            StopCoroutine(orbitCoroutine);
        }
    }
}
