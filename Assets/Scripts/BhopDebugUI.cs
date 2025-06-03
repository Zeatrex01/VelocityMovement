using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BhopDebugUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool showDebugInfo = true;
    [SerializeField] private float updateRate = 0.1f;    [Header("UI References - Manuel olarak atayýn")]
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI boostText;
    [SerializeField] private TextMeshProUGUI groundedText;
    [SerializeField] private TextMeshProUGUI topSpeedText;
    [SerializeField] private TextMeshProUGUI velocityText;
    [SerializeField] private TextMeshProUGUI inputText;
    [SerializeField] private TextMeshProUGUI slopeText; // Yeni eklendi
    
    private BhopMovement bhopMovement;
    private float lastUpdateTime;
    
    void Start()
    {
        // BhopMovement component'ini bul
        bhopMovement = FindObjectOfType<BhopMovement>();
        if (bhopMovement == null)
        {
            Debug.LogWarning("BhopMovement script bulunamadý!");
            enabled = false;
            return;
        }
    }    
    void Update()
    {
        if (!showDebugInfo || bhopMovement == null) return;
        
        // Update rate kontrolü
        if (Time.time - lastUpdateTime >= updateRate)
        {
            UpdateDebugInfo();
            lastUpdateTime = Time.time;
        }
        
        // Klavye kýsayolu ile debug UI'yi aç/kapat
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ToggleDebugUI();
        }
    }
    
    void UpdateDebugInfo()
    {
        if (!showDebugInfo) return;
        
        // Speed bilgisi
        if (speedText != null)
        {
            float currentSpeed = bhopMovement.CurrentSpeed;
            speedText.text = "Speed: " + currentSpeed.ToString("F1") + " u/s";
            
            // Hýz renk kodlamasý
            if (currentSpeed < 5f)
                speedText.color = Color.white;
            else if (currentSpeed < 15f)
                speedText.color = Color.yellow;
            else
                speedText.color = Color.green;
        }
        
        // Speed boost bilgisi
        if (boostText != null)
        {
            float speedBoost = bhopMovement.MouseMovementSpeed;
            boostText.text = "Mouse Speed: " + speedBoost.ToString("F2");
            boostText.color = speedBoost > 0.1f ? Color.cyan : Color.gray;
        }
          // Grounded durumu
        if (groundedText != null)
        {
            bool isGrounded = bhopMovement.IsGrounded();
            groundedText.text = "Grounded: " + (isGrounded ? "Yes" : "No");
            groundedText.color = isGrounded ? Color.green : Color.red;
        }
        
        // Top speed
        if (topSpeedText != null)
        {
            topSpeedText.text = "Top Speed: " + bhopMovement.TopSpeed.ToString("F1");
            topSpeedText.color = Color.magenta;
        }
          // Velocity vector
        if (velocityText != null)
        {
            Vector3 velocity = bhopMovement.GetVelocity();
            velocityText.text = "Velocity: (" + velocity.x.ToString("F1") + ", " + velocity.y.ToString("F1") + ", " + velocity.z.ToString("F1") + ")";
            velocityText.color = Color.white;
        }
          // Input bilgisi
        if (inputText != null)
        {
            Vector2 moveInput = bhopMovement.MoveInput;
            string inputInfo = "Input: (" + moveInput.x.ToString("F1") + ", " + moveInput.y.ToString("F1") + ")";
            if (Input.GetKey(KeyCode.Space)) inputInfo += " [JUMP]";
            
            inputText.text = inputInfo;
            inputText.color = moveInput.magnitude > 0.1f ? Color.white : Color.gray;
        }
        
        // Slope sliding bilgisi (yeni eklendi)
        if (slopeText != null)
        {
            string slopeInfo = GetSlopeInfo();
            slopeText.text = slopeInfo;
            
            // Slope durumuna göre renk
            if (slopeInfo.Contains("SLIDING"))
                slopeText.color = Color.yellow;
            else if (slopeInfo.Contains("Flat"))
                slopeText.color = Color.green;
            else
                slopeText.color = Color.white;
        }
    }
    
    // Debug UI'yi aç/kapat
    public void ToggleDebugUI()
    {
        showDebugInfo = !showDebugInfo;
        
        // Tüm UI elementlerini aktif/deaktif et
        SetUIElementsActive(showDebugInfo);
        
        Debug.Log("Debug UI: " + (showDebugInfo ? "Açýk" : "Kapalý"));
    }
      void SetUIElementsActive(bool active)
    {
        if (speedText != null) speedText.gameObject.SetActive(active);
        if (boostText != null) boostText.gameObject.SetActive(active);
        if (groundedText != null) groundedText.gameObject.SetActive(active);
        if (topSpeedText != null) topSpeedText.gameObject.SetActive(active);
        if (velocityText != null) velocityText.gameObject.SetActive(active);
        if (inputText != null) inputText.gameObject.SetActive(active);
        if (slopeText != null) slopeText.gameObject.SetActive(active); // Yeni eklendi
    }    // Slope bilgisini al
    private string GetSlopeInfo()
    {
        if (bhopMovement == null)
            return "Ground: No Movement Script";
            
        float slopeAngle = bhopMovement.CurrentSlopeAngle;
        bool isSliding = bhopMovement.IsSlopeSliding;
        bool isGrounded = bhopMovement.IsGrounded();
        
        string groundStatus = isGrounded ? "GROUNDED" : "AIRBORNE";
        
        if (slopeAngle < 1f)
            return $"Ground: {groundStatus} - Flat Surface";
        else if (isSliding)
            return $"Ground: {groundStatus} - ?? SLIDING! ({slopeAngle:F1}°)";
        else if (slopeAngle >= 5f)
            return $"Ground: {groundStatus} - Slope ({slopeAngle:F1}° - Too Gentle)";
        else
            return $"Ground: {groundStatus} - Minor Slope ({slopeAngle:F1}°)";
    }
}
