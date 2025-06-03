using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BhopPerformanceMonitor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool showPerformanceInfo = true;
    [SerializeField] private float updateRate = 0.5f;
      [Header("UI References - Manuel olarak atayýn")]
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private TextMeshProUGUI frameTimeText;
    [SerializeField] private TextMeshProUGUI memoryText;
    [SerializeField] private TextMeshProUGUI jumpCountText;
    [SerializeField] private TextMeshProUGUI airTimeText;
    
    private BhopMovement bhopMovement;
    private float lastUpdateTime;
    private float frameTime;
    private int frameCount;
    private float fps;
    
    // Performance tracking
    private int jumpCount = 0;
    private float totalAirTime = 0f;
    private float currentAirTime = 0f;
    private bool wasGrounded = true;
    
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
        
        // Initial values
        InvokeRepeating("CalculateFPS", 1f, 1f);
    }
    
    void Update()
    {
        if (!showPerformanceInfo || bhopMovement == null) return;
        
        // Frame time hesaplama
        frameTime += Time.unscaledDeltaTime;
        frameCount++;
          // Air time tracking
        bool isGrounded = bhopMovement.IsGrounded();
        if (!isGrounded && wasGrounded)
        {
            // Yeni jump baþladý
            jumpCount++;
            currentAirTime = 0f;
        }
        else if (!isGrounded)
        {
            // Havada
            currentAirTime += Time.deltaTime;
        }
        else if (isGrounded && !wasGrounded)
        {
            // Yere indi
            totalAirTime += currentAirTime;
        }
        
        wasGrounded = isGrounded;
        
        // Update rate kontrolü
        if (Time.time - lastUpdateTime >= updateRate)
        {
            UpdatePerformanceInfo();
            lastUpdateTime = Time.time;
        }
        
        // Klavye kýsayollarý
        if (Input.GetKeyDown(KeyCode.F4))
        {
            TogglePerformanceInfo();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetStats();
        }
    }
    
    void CalculateFPS()
    {
        fps = frameCount / frameTime;
        frameCount = 0;
        frameTime = 0f;
    }
    
    void UpdatePerformanceInfo()
    {
        if (!showPerformanceInfo) return;
        
        // FPS bilgisi
        if (fpsText != null)
        {
            fpsText.text = "FPS: " + fps.ToString("F0");
            
            // FPS renk kodlamasý
            if (fps >= 60f)
                fpsText.color = Color.green;
            else if (fps >= 30f)
                fpsText.color = Color.yellow;
            else
                fpsText.color = Color.red;
        }
        
        // Frame time bilgisi
        if (frameTimeText != null)
        {
            float avgFrameTime = fps > 0 ? 1000f / fps : 0f;
            frameTimeText.text = "Frame Time: " + avgFrameTime.ToString("F1") + "ms";
            frameTimeText.color = Color.white;
        }
        
        // Memory bilgisi
        if (memoryText != null)
        {
            float memoryMB = System.GC.GetTotalMemory(false) / (1024f * 1024f);
            memoryText.text = "Memory: " + memoryMB.ToString("F1") + "MB";
            memoryText.color = Color.cyan;
        }
        
        // Jump count
        if (jumpCountText != null)
        {
            jumpCountText.text = "Jumps: " + jumpCount.ToString();
            jumpCountText.color = Color.magenta;
        }
        
        // Air time
        if (airTimeText != null)
        {
            float avgAirTime = jumpCount > 0 ? totalAirTime / jumpCount : 0f;
            airTimeText.text = "Avg Air Time: " + avgAirTime.ToString("F2") + "s";
            airTimeText.color = Color.yellow;
        }
    }
    
    // Performance UI'yi aç/kapat
    public void TogglePerformanceInfo()
    {
        showPerformanceInfo = !showPerformanceInfo;
        
        // Tüm UI elementlerini aktif/deaktif et
        SetUIElementsActive(showPerformanceInfo);
        
        Debug.Log("Performance Info: " + (showPerformanceInfo ? "Açýk" : "Kapalý"));
    }
    
    void SetUIElementsActive(bool active)
    {
        if (fpsText != null) fpsText.gameObject.SetActive(active);
        if (frameTimeText != null) frameTimeText.gameObject.SetActive(active);
        if (memoryText != null) memoryText.gameObject.SetActive(active);
        if (jumpCountText != null) jumpCountText.gameObject.SetActive(active);
        if (airTimeText != null) airTimeText.gameObject.SetActive(active);
    }
    
    // Ýstatistikleri sýfýrla
    public void ResetStats()
    {
        jumpCount = 0;
        totalAirTime = 0f;
        currentAirTime = 0f;
        
        Debug.Log("Performance istatistikleri sýfýrlandý!");
    }
    
    // Public property'ler diðer scriptler için
    public int JumpCount { get { return jumpCount; } }
    public float TotalAirTime { get { return totalAirTime; } }
    public float AverageAirTime { get { return jumpCount > 0 ? totalAirTime / jumpCount : 0f; } }
    public float CurrentFPS { get { return fps; } }
}
