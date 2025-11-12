using UnityEngine;

/// <summary>
/// Energy Flow Effect for Energy Orb - Controls UV scrolling and rotation effects
/// </summary>
public class OrbEnergyFlow : MonoBehaviour
{
    [Header("Material Settings")]
    [SerializeField] private Material energyMaterial;
    
    [Header("UV Scrolling")]
    [SerializeField] private Vector2 scrollSpeed = new Vector2(0.1f, 0.2f);
    [SerializeField] private string mainTextureProperty = "_MainTex";
    
    [Header("Rotation Effect")]
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private Vector3 rotationAxis = new Vector3(0, 1, 0.3f);
    
    [Header("Emission Settings")]
    [SerializeField] private bool enableEmission = true;
    [SerializeField] private Color emissionColor = new Color(1f, 0.84f, 0f, 1f); // Golden yellow
    [SerializeField] private float emissionIntensity = 20f; // HDR intensity for Bloom effect (recommended 20-50)
    [SerializeField] private string emissionColorProperty = "_EmissionColor";
    [SerializeField] private bool useAdditiveBlending = true; // Use additive blending for stronger glow effect
    [SerializeField] private bool forceOpaqueForEmission = false; // Force opaque mode for stronger emission (experimental)

    private Vector2 currentOffset;
    private Renderer objectRenderer;
    private Material instanceMaterial;
    
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        
        if (objectRenderer != null)
        {
            // Create material instance to avoid affecting other objects
            instanceMaterial = objectRenderer.material;
        }
        else if (energyMaterial != null)
        {
            instanceMaterial = energyMaterial;
        }
        
        // Setup emission
        if (instanceMaterial != null && enableEmission)
        {
            SetupEmission();
        }
    }
    
    void SetupEmission()
    {
        if (instanceMaterial == null) 
        {
            Debug.LogWarning("OrbEnergyFlow: instanceMaterial is null, cannot setup emission!");
            return;
        }
        
        // Enable emission keyword
        instanceMaterial.EnableKeyword("_EMISSION");
        
        // Set emission color (HDR intensity)
        // In URP, emission intensity is set through color values, values greater than 1.0 create HDR effects
        Color hdrEmissionColor = emissionColor * emissionIntensity;
        
        if (instanceMaterial.HasProperty(emissionColorProperty))
        {
            instanceMaterial.SetColor(emissionColorProperty, hdrEmissionColor);
            Debug.Log($"OrbEnergyFlow: Set emission color to {hdrEmissionColor} with intensity {emissionIntensity}");
        }
        else
        {
            Debug.LogWarning($"OrbEnergyFlow: Material does not have property {emissionColorProperty}");
        }
        
        // Handle material surface type and blend mode
        if (instanceMaterial.HasProperty("_Surface"))
        {
            float surfaceType = instanceMaterial.GetFloat("_Surface");
            bool isTransparent = surfaceType > 0.5f;
            
            if (forceOpaqueForEmission)
            {
                // Force opaque mode for stronger emission effect
                instanceMaterial.SetFloat("_Surface", 0f); // Opaque
                instanceMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                instanceMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                instanceMaterial.SetInt("_ZWrite", 1);
                instanceMaterial.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
                instanceMaterial.renderQueue = -1; // Geometry queue
                Debug.Log("OrbEnergyFlow: Forced opaque mode for better emission visibility");
            }
            else if (isTransparent && useAdditiveBlending)
            {
                // Use additive blending for stronger glow effect
                // Maintain transparency but use additive blending
                instanceMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                instanceMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One); // One for additive
                instanceMaterial.SetInt("_BlendOp", (int)UnityEngine.Rendering.BlendOp.Add);
                Debug.Log("OrbEnergyFlow: Applied additive blending for better emission visibility");
            }
        }
        
        // Enable real-time emission in global illumination
        instanceMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        
        // Ensure material participates in Bloom post-processing
        // In URP, this is typically achieved by setting sufficiently high emission values
        // Also ensure BaseColor has some brightness
        if (instanceMaterial.HasProperty("_BaseColor"))
        {
            Color baseColor = instanceMaterial.GetColor("_BaseColor");
            // Increase BaseColor brightness to enhance glow effect
            Color enhancedBaseColor = new Color(
                Mathf.Max(baseColor.r, emissionColor.r * 0.5f),
                Mathf.Max(baseColor.g, emissionColor.g * 0.5f),
                Mathf.Max(baseColor.b, emissionColor.b * 0.5f),
                baseColor.a
            );
            instanceMaterial.SetColor("_BaseColor", enhancedBaseColor);
        }
    }
    
    void Update()
    {
        if (instanceMaterial == null) return;

        // UV scrolling
        currentOffset += scrollSpeed * Time.deltaTime;
        instanceMaterial.SetTextureOffset(mainTextureProperty, currentOffset);

        // Rotate object
        if (transform != null)
        {
            transform.Rotate(rotationAxis.normalized, rotationSpeed * Time.deltaTime, Space.World);
        }
    }
    
    void OnDestroy()
    {
        // Clean up material instance
        if (instanceMaterial != null && objectRenderer != null)
        {
            Destroy(instanceMaterial);
        }
    }
}


