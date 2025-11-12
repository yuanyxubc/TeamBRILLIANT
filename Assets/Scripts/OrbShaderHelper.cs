using UnityEngine;

/// <summary>
/// Shader Helper Class - Provides common shader property names and helper methods
/// </summary>
public static class OrbShaderHelper
{
    // Common shader property names
    public static class Properties
    {
        public const string MainTexture = "_MainTex";
        public const string BaseColor = "_BaseColor";
        public const string EmissionColor = "_EmissionColor";
        public const string EmissionMap = "_EmissionMap";
        public const string Metallic = "_Metallic";
        public const string Smoothness = "_Smoothness";
        public const string BumpMap = "_BumpMap";
        public const string AlphaClip = "_AlphaClip";
        public const string Surface = "_Surface";
        public const string Blend = "_Blend";
        public const string Cutoff = "_Cutoff";
    }
    
    /// <summary>
    /// Set material to transparent mode
    /// </summary>
    public static void SetTransparent(Material material)
    {
        if (material == null) return;
        
        material.SetFloat(Properties.Surface, 1); // Transparent
        material.SetFloat(Properties.Blend, 0); // Alpha
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.renderQueue = 3000;
        
        material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
    }
    
    /// <summary>
    /// Set material to opaque mode
    /// </summary>
    public static void SetOpaque(Material material)
    {
        if (material == null) return;
        
        material.SetFloat(Properties.Surface, 0); // Opaque
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.renderQueue = -1;
        
        material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
    }
    
    /// <summary>
    /// Enable material emission
    /// </summary>
    public static void EnableEmission(Material material, Color emissionColor)
    {
        if (material == null) return;
        
        material.EnableKeyword("_EMISSION");
        material.SetColor(Properties.EmissionColor, emissionColor);
        material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
    }
    
    /// <summary>
    /// Disable material emission
    /// </summary>
    public static void DisableEmission(Material material)
    {
        if (material == null) return;
        
        material.DisableKeyword("_EMISSION");
        material.SetColor(Properties.EmissionColor, Color.black);
    }
    
    /// <summary>
    /// Smoothly interpolate emission intensity
    /// </summary>
    public static Color LerpEmission(Color colorA, Color colorB, float t)
    {
        return Color.Lerp(colorA, colorB, t);
    }
    
    /// <summary>
    /// Create HDR color (for strong emission)
    /// </summary>
    public static Color CreateHDRColor(Color baseColor, float intensity)
    {
        return baseColor * Mathf.Pow(2, intensity);
    }
}

/// <summary>
/// Material Animator - Used to dynamically modify material properties
/// </summary>
public class OrbMaterialAnimator : MonoBehaviour
{
    [Header("Material Reference")]
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private int materialIndex = 0;
    
    [Header("Color Animation")]
    [SerializeField] private bool animateColor = false;
    [SerializeField] private Gradient colorGradient;
    [SerializeField] private float colorSpeed = 1f;
    [SerializeField] private bool animateEmission = false;
    [SerializeField] private float emissionIntensityMin = 1f;
    [SerializeField] private float emissionIntensityMax = 5f;
    
    [Header("UV Animation")]
    [SerializeField] private bool animateUVScroll = false;
    [SerializeField] private Vector2 uvScrollSpeed = Vector2.one;
    
    [Header("Alpha Animation")]
    [SerializeField] private bool animateAlpha = false;
    [SerializeField] private float alphaSpeed = 1f;
    [SerializeField] private float alphaMin = 0.3f;
    [SerializeField] private float alphaMax = 1f;
    
    private Material instanceMaterial;
    private float animationTimer;
    private Vector2 uvOffset;
    private Color baseEmissionColor;
    
    void Start()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }
        
        if (targetRenderer != null)
        {
            // Create material instance
            Material[] materials = targetRenderer.materials;
            if (materialIndex < materials.Length)
            {
                instanceMaterial = materials[materialIndex];
                
                // Save base emission color
                if (instanceMaterial.HasProperty(OrbShaderHelper.Properties.EmissionColor))
                {
                    baseEmissionColor = instanceMaterial.GetColor(OrbShaderHelper.Properties.EmissionColor);
                }
            }
        }
    }
    
    void Update()
    {
        if (instanceMaterial == null) return;
        
        animationTimer += Time.deltaTime;
        
        // Color animation
        if (animateColor && colorGradient != null)
        {
            float t = (Mathf.Sin(animationTimer * colorSpeed) + 1f) / 2f;
            Color newColor = colorGradient.Evaluate(t);
            
            if (instanceMaterial.HasProperty(OrbShaderHelper.Properties.BaseColor))
            {
                instanceMaterial.SetColor(OrbShaderHelper.Properties.BaseColor, newColor);
            }
        }
        
        // Emission animation
        if (animateEmission)
        {
            float t = (Mathf.Sin(animationTimer * colorSpeed) + 1f) / 2f;
            float intensity = Mathf.Lerp(emissionIntensityMin, emissionIntensityMax, t);
            Color emissionColor = baseEmissionColor * intensity;
            
            OrbShaderHelper.EnableEmission(instanceMaterial, emissionColor);
        }
        
        // UV scrolling
        if (animateUVScroll)
        {
            uvOffset += uvScrollSpeed * Time.deltaTime;
            
            if (instanceMaterial.HasProperty(OrbShaderHelper.Properties.MainTexture))
            {
                instanceMaterial.SetTextureOffset(OrbShaderHelper.Properties.MainTexture, uvOffset);
            }
        }
        
        // Alpha animation
        if (animateAlpha)
        {
            float t = (Mathf.Sin(animationTimer * alphaSpeed) + 1f) / 2f;
            float alpha = Mathf.Lerp(alphaMin, alphaMax, t);
            
            if (instanceMaterial.HasProperty(OrbShaderHelper.Properties.BaseColor))
            {
                Color currentColor = instanceMaterial.GetColor(OrbShaderHelper.Properties.BaseColor);
                currentColor.a = alpha;
                instanceMaterial.SetColor(OrbShaderHelper.Properties.BaseColor, currentColor);
            }
        }
    }
    
    void OnDestroy()
    {
        // Clean up material instance
        if (instanceMaterial != null)
        {
            Destroy(instanceMaterial);
        }
    }
    
    /// <summary>
    /// Set color gradient
    /// </summary>
    public void SetColorGradient(Gradient gradient)
    {
        colorGradient = gradient;
    }
    
    /// <summary>
    /// Set emission intensity range
    /// </summary>
    public void SetEmissionIntensityRange(float min, float max)
    {
        emissionIntensityMin = min;
        emissionIntensityMax = max;
    }
}


