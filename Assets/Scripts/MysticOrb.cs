using UnityEngine;

/// <summary>
/// Mystic Orb Controller - Controls the rotation, pulsing, and effects of the energy orb
/// </summary>
public class MysticOrb : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Transform innerSphere;
    [SerializeField] private float innerRotationSpeed = 20f;
    [SerializeField] private Vector3 innerRotationAxis = new Vector3(0, 1, 0.3f);
    
    [Header("Pulsing Settings")]
    [SerializeField] private bool enablePulsing = true;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseAmplitude = 0.15f;
    [SerializeField] private float baseScale = 1f;
    
    [Header("Glow Settings")]
    [SerializeField] private Material glowMaterial;
    [SerializeField] private float glowIntensityMin = 2f;
    [SerializeField] private float glowIntensityMax = 5f;
    [SerializeField] private string emissionColorProperty = "_EmissionColor";
    
    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem[] particleSystems;
    
    private Color baseEmissionColor;
    private float pulseTimer;
    
    void Start()
    {
        if (glowMaterial != null)
        {
            // Save base emission color
            baseEmissionColor = glowMaterial.GetColor(emissionColorProperty);
        }
        
        // Start all particle systems
        if (particleSystems != null)
        {
            foreach (var ps in particleSystems)
            {
                if (ps != null)
                {
                    ps.Play();
                }
            }
        }
    }
    
    void Update()
    {
        // Update timer
        pulseTimer += Time.deltaTime;
        
        // Rotate inner sphere
        if (innerSphere != null)
        {
            innerSphere.Rotate(innerRotationAxis.normalized, innerRotationSpeed * Time.deltaTime, Space.World);
        }
        
        // Pulsing effect
        if (enablePulsing)
        {
            float pulse = Mathf.Sin(pulseTimer * pulseSpeed) * pulseAmplitude;
            float currentScale = baseScale + pulse;
            transform.localScale = Vector3.one * currentScale;
            
            // Synchronize glow intensity
            if (glowMaterial != null)
            {
                float normalizedPulse = (Mathf.Sin(pulseTimer * pulseSpeed) + 1f) / 2f;
                float glowIntensity = Mathf.Lerp(glowIntensityMin, glowIntensityMax, normalizedPulse);
                glowMaterial.SetColor(emissionColorProperty, baseEmissionColor * glowIntensity);
            }
        }
    }
    
    /// <summary>
    /// Set the color of the energy orb
    /// </summary>
    public void SetOrbColor(Color color)
    {
        baseEmissionColor = color;
        if (glowMaterial != null)
        {
            glowMaterial.SetColor(emissionColorProperty, color * glowIntensityMin);
        }
    }
    
    /// <summary>
    /// Set rotation speed
    /// </summary>
    public void SetRotationSpeed(float speed)
    {
        innerRotationSpeed = speed;
    }
    
    /// <summary>
    /// Set pulse speed
    /// </summary>
    public void SetPulseSpeed(float speed)
    {
        pulseSpeed = speed;
    }
}


