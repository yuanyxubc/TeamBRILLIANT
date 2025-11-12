using UnityEngine;

/// <summary>
/// Lightning effect for energy orb - Generates electrical arcs on the sphere surface
/// </summary>
public class OrbLightningEffect : MonoBehaviour
{
    [Header("Lightning Settings")]
    [SerializeField] private int maxLightningBolts = 3;
    [SerializeField] private float lightningDuration = 0.2f;
    [SerializeField] private float lightningSpawnInterval = 0.5f;
    
    [Header("Lightning Appearance")]
    [SerializeField] private float lightningRadius = 0.5f;
    [SerializeField] private int lightningSegments = 10;
    [SerializeField] private float lightningDisplacement = 0.1f;
    [SerializeField] private AnimationCurve lightningWidthCurve = AnimationCurve.Linear(0, 1, 1, 0.1f);
    [SerializeField] private float lightningWidth = 0.05f;
    
    [Header("Color")]
    [SerializeField] private Gradient lightningGradient;
    [SerializeField] private Color lightningColor = new Color(0.5f, 0.8f, 1f, 1f);
    
    private float nextLightningTime;
    private System.Collections.Generic.List<LightningBolt> activeLightning = new System.Collections.Generic.List<LightningBolt>();
    
    private class LightningBolt
    {
        public LineRenderer lineRenderer;
        public float destroyTime;
    }
    
    void Start()
    {
        // Set default gradient
        if (lightningGradient == null)
        {
            lightningGradient = new Gradient();
            lightningGradient.SetKeys(
                new GradientColorKey[] { 
                    new GradientColorKey(lightningColor, 0f), 
                    new GradientColorKey(lightningColor, 1f) 
                },
                new GradientAlphaKey[] { 
                    new GradientAlphaKey(1f, 0f), 
                    new GradientAlphaKey(0f, 1f) 
                }
            );
        }
    }
    
    void Update()
    {
        // Spawn new lightning
        if (Time.time >= nextLightningTime && activeLightning.Count < maxLightningBolts)
        {
            SpawnLightning();
            nextLightningTime = Time.time + lightningSpawnInterval;
        }
        
        // Clean up expired lightning
        for (int i = activeLightning.Count - 1; i >= 0; i--)
        {
            if (Time.time >= activeLightning[i].destroyTime)
            {
                if (activeLightning[i].lineRenderer != null)
                {
                    Destroy(activeLightning[i].lineRenderer.gameObject);
                }
                activeLightning.RemoveAt(i);
            }
        }
    }
    
    void SpawnLightning()
    {
        // Create lightning object
        GameObject lightningObj = new GameObject("Lightning");
        lightningObj.transform.parent = transform;
        lightningObj.transform.localPosition = Vector3.zero;
        
        LineRenderer lr = lightningObj.AddComponent<LineRenderer>();
        
        // Configure LineRenderer
        lr.useWorldSpace = false; // Use local space
        lr.positionCount = lightningSegments;
        lr.widthCurve = lightningWidthCurve;
        lr.widthMultiplier = lightningWidth;
        lr.colorGradient = lightningGradient;
        
        // Use URP Unlit shader
        Material lightningMat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        lightningMat.SetColor("_BaseColor", lightningColor);
        lightningMat.SetFloat("_Surface", 1); // Transparent
        lightningMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        lightningMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One); // Additive
        lr.material = lightningMat;
        
        // Generate lightning path
        Vector3 startPoint = Random.onUnitSphere * lightningRadius;
        Vector3 endPoint = Random.onUnitSphere * lightningRadius;
        
        GenerateLightningPath(lr, startPoint, endPoint);
        
        // Add to active list
        LightningBolt bolt = new LightningBolt
        {
            lineRenderer = lr,
            destroyTime = Time.time + lightningDuration
        };
        activeLightning.Add(bolt);
    }
    
    void GenerateLightningPath(LineRenderer lr, Vector3 start, Vector3 end)
    {
        Vector3[] positions = new Vector3[lightningSegments];
        
        for (int i = 0; i < lightningSegments; i++)
        {
            float t = i / (float)(lightningSegments - 1);
            Vector3 position = Vector3.Lerp(start, end, t);
            
            // Add random offset (except start and end points)
            if (i > 0 && i < lightningSegments - 1)
            {
                Vector3 randomOffset = Random.insideUnitSphere * lightningDisplacement;
                position += randomOffset;
            }
            
            // Use local coordinates directly (since LineRenderer is in local space)
            positions[i] = position;
        }
        
        lr.SetPositions(positions);
    }
    
    void OnDestroy()
    {
        // Clean up all lightning
        foreach (var bolt in activeLightning)
        {
            if (bolt.lineRenderer != null)
            {
                Destroy(bolt.lineRenderer.gameObject);
            }
        }
        activeLightning.Clear();
    }
}


