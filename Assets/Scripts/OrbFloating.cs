using UnityEngine;

/// <summary>
/// Makes the energy orb float and sway slightly
/// </summary>
public class OrbFloating : MonoBehaviour
{
    [Header("Floating Settings")]
    [SerializeField] private bool enableFloating = true;
    [SerializeField] private float floatingSpeed = 1f;
    [SerializeField] private float floatingAmplitude = 0.3f;
    
    [Header("Swaying Settings")]
    [SerializeField] private bool enableSwaying = true;
    [SerializeField] private float swaySpeed = 0.8f;
    [SerializeField] private float swayAmount = 0.1f;
    
    private Vector3 startPosition;
    private float floatTimer;
    private float swayTimerX;
    private float swayTimerZ;
    
    void Start()
    {
        startPosition = transform.localPosition;
        
        // Randomly initialize timers to avoid synchronization of multiple orbs
        floatTimer = Random.Range(0f, Mathf.PI * 2);
        swayTimerX = Random.Range(0f, Mathf.PI * 2);
        swayTimerZ = Random.Range(0f, Mathf.PI * 2);
    }
    
    void Update()
    {
        Vector3 newPosition = startPosition;
        
        // Vertical floating
        if (enableFloating)
        {
            floatTimer += Time.deltaTime * floatingSpeed;
            float yOffset = Mathf.Sin(floatTimer) * floatingAmplitude;
            newPosition.y += yOffset;
        }
        
        // Horizontal swaying
        if (enableSwaying)
        {
            swayTimerX += Time.deltaTime * swaySpeed;
            swayTimerZ += Time.deltaTime * swaySpeed * 0.7f; // Different frequencies create more natural movement
            
            float xOffset = Mathf.Sin(swayTimerX) * swayAmount;
            float zOffset = Mathf.Cos(swayTimerZ) * swayAmount;
            
            newPosition.x += xOffset;
            newPosition.z += zOffset;
        }
        
        transform.localPosition = newPosition;
    }
    
    /// <summary>
    /// Reset starting position
    /// </summary>
    public void ResetStartPosition()
    {
        startPosition = transform.localPosition;
    }
}


