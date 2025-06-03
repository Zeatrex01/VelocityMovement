using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BhopMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private float runAcceleration = 14f;
    [SerializeField] private float runDeacceleration = 10f;
    [SerializeField] private float airAcceleration = 2.0f;
    [SerializeField] private float airDeacceleration = 2.0f;
    [SerializeField] private float airControl = 0.3f;
    [SerializeField] private float sideStrafeAcceleration = 50f;
    [SerializeField] private float sideStrafeSpeed = 1f;
    [SerializeField] private float friction = 8f; // Artýrýldý 6'dan 8'e - Daha hýzlý yavaþlama    [Header("Slope & Gravity Settings")]
    [SerializeField] private float slopeForce = 35f;  // Artýrýldý 20'den 35'e
    [SerializeField] private float slopeForceRayLength = 2.5f; // Artýrýldý 1.5'ten 2.5'e
    [SerializeField] private float maxSlopeAngle = 60f; // Artýrýldý 45'ten 60'a
    [SerializeField] private float minSlopeAngle = 5f; // Azaltýldý 10'dan 5'e
    [SerializeField] private bool enableSlopeSliding = true;
      [Header("Jump Settings")]
    [SerializeField] private float jumpSpeed = 8.0f;
    [SerializeField] private LayerMask groundLayers = 1;
    [SerializeField] private bool enableBhopQueue = false; // Bhop için space basýlý tutma      [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float mouseSpeedBoostMultiplier = 0.05f; // Artýrýldý 0.01'den 0.05'e
      [Header("Mobile Settings")]
    [SerializeField] private bool enableMobileSupport = true;
    [SerializeField] private float touchSensitivity = 1.5f;    // Core components - Inspector'dan atayýn
    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera playerCamera;
    private bool isGrounded;
    private bool wishJump = false;
    
    // Movement vectors
    private Vector3 playerVelocity;
    private Vector3 moveDirectionNorm = Vector3.zero;
    private Vector3 playerTopVelocity = Vector3.zero;
    
    // Mouse/Camera input
    private Vector2 lookInput;
    private float mouseMovementSpeed;
    private bool isMobile;
    
    // Mobile touch variables
    private Vector2 touchStartPos;
    private bool isTouching = false;
      // Bhop boost system
    private float speedBoost = 0f;
    private float maxSpeedBoost = 20f;
    private float boostDecay = 5f; // Artýrýldý 2'den 5'e - Daha hýzlý yavaþlama
    
    // Jump timing
    private float lastJumpTime = 0f;
    private float jumpCooldown = 0.1f;    // UI Debug variables
    public float CurrentSpeed { get; private set; }
    public float TopSpeed { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public float MouseMovementSpeed { get; private set; }
    public float CurrentSlopeAngle { get; private set; }  // Yeni eklendi
    public bool IsSlopeSliding { get; private set; }      // Yeni eklendi

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController component bulunamadý! Script otomatik olarak ekleyecek.");
            controller = gameObject.AddComponent<CharacterController>();
        }
        
        // CharacterController ayarlarý
        controller.height = 2f;
        controller.radius = 0.5f;
        controller.center = new Vector3(0, 0, 0);
        
        // Detect if we're on mobile
        isMobile = Application.isMobilePlatform && enableMobileSupport;
        
        // Find camera
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
                playerCamera = FindObjectOfType<Camera>();
        }
        
        // Lock cursor on PC
        if (!isMobile)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        // Initialize velocity
        playerVelocity = Vector3.zero;
        
        Debug.Log("BhopMovement baþlatýldý! Space ile zýplayabilirsiniz.");
    }    void Update()
    {
        // Set top speed
        if (playerVelocity.magnitude > TopSpeed)
            TopSpeed = playerVelocity.magnitude;
          
        // Update current speed
        CurrentSpeed = new Vector3(playerVelocity.x, 0f, playerVelocity.z).magnitude;
        
        // Update debug info
        MoveInput = moveDirectionNorm;
        MouseMovementSpeed = mouseMovementSpeed;
        
        // Handle input
        HandleInput();
        HandleMouseMovement();
          // Handle Bhop speed boost decay
        if (speedBoost > 0f)
        {
            speedBoost -= boostDecay * Time.deltaTime;
            speedBoost = Mathf.Max(0f, speedBoost);
        }
        
        // Extra velocity decay when no input (passive slowdown)
        if (moveDirectionNorm.magnitude < 0.1f && isGrounded)
        {
            // Player is not moving, apply extra slowdown
            Vector3 horizontalVel = new Vector3(playerVelocity.x, 0, playerVelocity.z);
            if (horizontalVel.magnitude > moveSpeed)
            {
                // Apply extra friction for speeds above normal
                float extraFriction = 3f; // Ek yavaþlama katsayýsý
                Vector3 frictionForce = -horizontalVel.normalized * extraFriction * Time.deltaTime;
                playerVelocity.x += frictionForce.x;
                playerVelocity.z += frictionForce.z;
                
                // Minimum speed sýnýrý
                Vector3 newHorizontalVel = new Vector3(playerVelocity.x, 0, playerVelocity.z);
                if (newHorizontalVel.magnitude < moveSpeed)
                {
                    float ratio = moveSpeed / newHorizontalVel.magnitude;
                    playerVelocity.x *= ratio;
                    playerVelocity.z *= ratio;
                }
            }
        }
          // Debug speed display (sadece F5 tuþuna basýldýðýnda, F4 UI toggle için kullanýlýyor)
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Debug.Log($"=== BHOP DEBUG ===");
            Debug.Log($"Speed: {CurrentSpeed:F1} | Boost: {speedBoost:F1} | Grounded: {isGrounded} | WishJump: {wishJump}");
            Debug.Log($"Velocity: {playerVelocity} | Input: {moveDirectionNorm}");
            Debug.Log($"Mouse Speed: {mouseMovementSpeed:F2} | Top Speed: {TopSpeed:F1}");
            Debug.Log($"Boost Decay: {boostDecay} | Friction: {friction} | No Input: {moveDirectionNorm.magnitude < 0.1f}");
        }
    }    void FixedUpdate()
    {
        // Improved ground check
        bool controllerGrounded = controller.isGrounded;
        bool raycastGrounded = Physics.Raycast(transform.position, Vector3.down, controller.height * 0.5f + 0.1f, groundLayers);
        isGrounded = controllerGrounded || raycastGrounded;
        
        // Jump handling - burada yapýlmalý çünkü physics burada
        if (wishJump && isGrounded)
        {
            playerVelocity.y = jumpSpeed;
            wishJump = false;
            Debug.Log("JUMP EXECUTED! Velocity.y = " + playerVelocity.y);
        }
          // Handle slope sliding if enabled - Slope sliding should work even when airborne!
        if (enableSlopeSliding)
        {
            ApplySlopeSliding();
        }
        
        // Handle movement based on whether we're grounded or not
        if (isGrounded)
            GroundMove();
        else
            AirMove();
        
        // Apply the movement
        controller.Move(playerVelocity * Time.fixedDeltaTime);
        
        // Debug ground check
        Debug.DrawRay(transform.position, Vector3.down * (controller.height * 0.5f + 0.1f), isGrounded ? Color.green : Color.red);
    }    void HandleInput()
    {
        // Jump input - iki mod: normal veya bhop
        if (enableBhopQueue)
        {
            // Bhop mode: space basýlý tutarak arka arkaya zýpla
            if (Input.GetKey(KeyCode.Space))
            {
                wishJump = true;
            }
        }
        else
        {
            // Normal mode: sadece yerdeyken space ile zýpla
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                wishJump = true;
                Debug.Log("SPACE PRESSED! Single jump mode");
            }
        }
        
        // Movement input
        Vector2 moveInput = Vector2.zero;
        
        if (!isMobile)
        {
            // PC input
            moveInput.x = Input.GetAxis("Horizontal");
            moveInput.y = Input.GetAxis("Vertical");
        }
        else
        {
            // Mobile touch input
            HandleTouchInput();
            // moveInput will be set in HandleTouchInput
            return;
        }
        
        // Convert to movement direction
        if (moveInput.magnitude > 1f)
            moveInput = moveInput.normalized;
        
        moveDirectionNorm = moveInput;
    }
      void HandleTouchInput()
    {
        Vector2 moveInput = Vector2.zero;
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            // First touch for movement
            switch (touch.phase)
            {                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    isTouching = true;
                    // Jump on touch begin
                    wishJump = true;
                    break;
                    
                case TouchPhase.Moved:
                    if (isTouching)
                    {
                        Vector2 touchDelta = touch.position - touchStartPos;
                        moveInput.x = Mathf.Clamp(touchDelta.x / (Screen.width * 0.3f), -1f, 1f);
                        moveInput.y = Mathf.Clamp(touchDelta.y / (Screen.height * 0.3f), -1f, 1f);
                    }
                    break;
                    
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isTouching = false;
                    moveInput = Vector2.zero;
                    wishJump = false;
                    break;
            }
        }
        
        // Convert to movement direction
        if (moveInput.magnitude > 1f)
            moveInput = moveInput.normalized;
        
        moveDirectionNorm = moveInput;
    }    void HandleMouseMovement()
    {
        if (!isMobile)
        {
            // PC mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            
            // Calculate mouse movement speed for bhop boost
            mouseMovementSpeed = (Mathf.Abs(mouseX) + Mathf.Abs(mouseY));
            
            // Update look rotation
            lookInput.x += mouseX;
            lookInput.y -= mouseY;
            lookInput.y = Mathf.Clamp(lookInput.y, -90f, 90f);
            
            // Apply rotation
            if (playerCamera != null)
            {
                // Player horizontal rotation
                transform.rotation = Quaternion.Euler(0f, lookInput.x, 0f);
                
                // Camera vertical rotation
                playerCamera.transform.localRotation = Quaternion.Euler(lookInput.y, 0f, 0f);
            }
        }
        else
        {
            // Mobile touch look (second touch)
            if (Input.touchCount > 1)
            {
                Touch lookTouch = Input.GetTouch(1);
                if (lookTouch.phase == TouchPhase.Moved)
                {
                    Vector2 touchDelta = lookTouch.deltaPosition * touchSensitivity * Time.deltaTime;
                    
                    // Calculate movement speed for mobile
                    mouseMovementSpeed = touchDelta.magnitude * 10f;
                    
                    lookInput.x += touchDelta.x;
                    lookInput.y -= touchDelta.y;
                    lookInput.y = Mathf.Clamp(lookInput.y, -90f, 90f);
                    
                    if (playerCamera != null)
                    {
                        // Player horizontal rotation
                        transform.rotation = Quaternion.Euler(0f, lookInput.x, 0f);
                        
                        // Camera vertical rotation
                        playerCamera.transform.localRotation = Quaternion.Euler(lookInput.y, 0f, 0f);
                    }
                }
            }
        }
          // Apply bhop speed boost based on mouse movement (works both grounded and airborne)
        if (mouseMovementSpeed > 0.1f)
        {
            float mouseBoost = mouseMovementSpeed * mouseSpeedBoostMultiplier;
            
            // More boost when airborne (classic bhop), less when grounded
            if (!isGrounded)
                mouseBoost *= 2f; // Double boost in air
            
            speedBoost += mouseBoost;
            speedBoost = Mathf.Clamp(speedBoost, 0f, maxSpeedBoost);
            
            // Debug mouse boost
            if (Input.GetKey(KeyCode.F5))
            {
                Debug.Log($"MOUSE BOOST: Speed={mouseMovementSpeed:F2} | Boost={mouseBoost:F2} | Total={speedBoost:F2} | Grounded={isGrounded}");
            }
        }
    }    // Ground movement with Quake-style physics
    void GroundMove()
    {
        Vector3 wishdir;
        
        // Do not apply friction if the player is queueing up the next jump
        if (!wishJump)
            ApplyFriction(1.0f);
        else
            ApplyFriction(0);

        // Calculate movement direction
        wishdir = new Vector3(moveDirectionNorm.x, 0, moveDirectionNorm.y);
        wishdir = transform.TransformDirection(wishdir);
        wishdir.Normalize();
        moveDirectionNorm = new Vector2(wishdir.x, wishdir.z);

        var wishspeed = moveDirectionNorm.magnitude;
        wishspeed *= moveSpeed + speedBoost;

        Accelerate(wishdir, wishspeed, runAcceleration);

        // Apply small downward force to keep grounded, but don't override jump
        if (playerVelocity.y <= 0f)
            playerVelocity.y = -2f;
    }

    // Air movement with strafe jumping
    void AirMove()
    {
        Vector3 wishdir;
        float wishvel = airAcceleration;
        float accel;

        // Calculate movement direction
        wishdir = new Vector3(moveDirectionNorm.x, 0, moveDirectionNorm.y);
        wishdir = transform.TransformDirection(wishdir);

        float wishspeed = wishdir.magnitude;
        wishspeed *= moveSpeed + speedBoost;

        wishdir.Normalize();
        moveDirectionNorm = new Vector2(wishdir.x, wishdir.z);

        // CPM Air control
        float wishspeed2 = wishspeed;
        if (Vector3.Dot(playerVelocity, wishdir) < 0)
            accel = airDeacceleration;
        else
            accel = airAcceleration;

        // If the player is ONLY strafing left or right
        if (moveDirectionNorm.y == 0 && moveDirectionNorm.x != 0)
        {
            if (wishspeed > sideStrafeSpeed)
                wishspeed = sideStrafeSpeed;
            accel = sideStrafeAcceleration;
        }

        Accelerate(wishdir, wishspeed, accel);
        if (airControl > 0)
            AirControl(wishdir, wishspeed2);

        // Apply gravity
        playerVelocity.y -= 20f * Time.fixedDeltaTime;
    }

    // Apply acceleration in a given direction
    void Accelerate(Vector3 wishdir, float wishspeed, float accel)
    {
        float addspeed, accelspeed, currentspeed;

        currentspeed = Vector3.Dot(playerVelocity, wishdir);
        addspeed = wishspeed - currentspeed;
        if (addspeed <= 0)
            return;
        accelspeed = accel * Time.fixedDeltaTime * wishspeed;
        if (accelspeed > addspeed)
            accelspeed = addspeed;

        playerVelocity += accelspeed * wishdir;
    }

    // Air control for better strafe jumping
    void AirControl(Vector3 wishdir, float wishspeed)
    {
        float zspeed;
        float speed;
        float dot;
        float k;

        // Can't control movement if not moving forward or backward
        if (Mathf.Abs(moveDirectionNorm.y) < 0.001f || Mathf.Abs(wishspeed) < 0.001f)
            return;

        zspeed = playerVelocity.y;
        playerVelocity.y = 0;
        /* Next two lines are equivalent to idTech's VectorNormalize() */
        speed = playerVelocity.magnitude;
        playerVelocity.Normalize();

        dot = Vector3.Dot(playerVelocity, wishdir);
        k = 32;
        k *= airControl * dot * dot * Time.fixedDeltaTime;

        // Change direction while slowing down
        if (dot > 0)
        {
            playerVelocity = playerVelocity * speed + wishdir * k;
            playerVelocity.Normalize();
        }

        playerVelocity *= speed;
        playerVelocity.y = zspeed;
    }

    // Apply friction
    void ApplyFriction(float t)
    {
        Vector3 vec = playerVelocity; // Equivalent to: VectorCopy();
        float speed;
        float newspeed;
        float control;
        float drop;

        vec.y = 0.0f;
        speed = vec.magnitude;
        drop = 0.0f;

        /* Only if the player is on the ground then apply friction */
        if (isGrounded)
        {
            control = speed < runDeacceleration ? runDeacceleration : speed;
            drop = control * friction * Time.fixedDeltaTime * t;
        }

        newspeed = speed - drop;
        if (newspeed < 0)
            newspeed = 0;
        if (speed > 0)
            newspeed /= speed;

        playerVelocity.x *= newspeed;
        playerVelocity.z *= newspeed;
    }
      // Public methods for UI or other scripts
    public float GetCurrentSpeed()
    {
        return CurrentSpeed;
    }
    
    public float GetSpeedBoost()
    {
        return speedBoost;
    }
    
    public bool IsGrounded()
    {
        return isGrounded;
    }
    
    public Vector3 GetVelocity()
    {
        return playerVelocity;
    }
    
    // Debug visualization
    void OnDrawGizmos()
    {
        if (Application.isPlaying && controller != null)
        {
            // Draw velocity vector
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + playerVelocity.normalized * 2f);
            
            // Draw movement direction
            Gizmos.color = Color.red;
            Vector3 moveDir = new Vector3(moveDirectionNorm.x, 0, moveDirectionNorm.y);
            moveDir = transform.TransformDirection(moveDir);
            Gizmos.DrawLine(transform.position, transform.position + moveDir * 2f);
              // Draw speed boost indicator
            if (speedBoost > 0f)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position + Vector3.up * 2f, speedBoost / maxSpeedBoost);
            }
              // Draw slope detection ray (if slope sliding is enabled)
            if (enableSlopeSliding)
            {
                Gizmos.color = IsSlopeSliding ? Color.red : Color.green;
                Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
                Gizmos.DrawLine(rayOrigin, rayOrigin + Vector3.down * slopeForceRayLength);
                
                // Draw slope angle indicator
                if (IsSlopeSliding)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.5f, 0.3f);
                }
            }
        }
    }    // Slope sliding physics for downhill movement
    private void ApplySlopeSliding()
    {
        // Reset slope sliding status
        IsSlopeSliding = false;
        CurrentSlopeAngle = 0f;
        
        // Raycast to detect ground slope
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        
        // Use longer raycast distance for better slope detection
        float rayDistance = Mathf.Max(slopeForceRayLength, 3f);
        
        // DEBUG: Her zaman slope bilgisi göster
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayDistance, groundLayers))
        {
            // Calculate slope angle
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            CurrentSlopeAngle = slopeAngle; // Debug için
            
            // DEBUG: Slope bilgisini sürekli göster
            if (Input.GetKey(KeyCode.F5))
            {
                Debug.Log($"SLOPE DEBUG - Hit: {hit.collider.name} | Angle: {slopeAngle:F1}° | Normal: {hit.normal} | Min: {minSlopeAngle}° | Max: {maxSlopeAngle}° | Distance: {hit.distance:F2}");
            }
            
            // Check if slope is steep enough for sliding (lowered minimum)
            if (slopeAngle >= minSlopeAngle && slopeAngle <= maxSlopeAngle)
            {
                IsSlopeSliding = true; // Debug için
                
                // Calculate slope direction (downhill)
                Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;
                
                // Apply stronger sliding force based on slope steepness
                float slopeFactor = (slopeAngle - minSlopeAngle) / (maxSlopeAngle - minSlopeAngle);
                
                // Increased base force and made it frame-rate independent
                float baseForce = isGrounded ? slopeForce : slopeForce * 0.7f; // Slightly less when airborne
                Vector3 slideForce = slopeDirection * baseForce * slopeFactor;
                
                // DEBUG: Sliding force bilgisi
                if (Input.GetKey(KeyCode.F5))
                {
                    Debug.Log($"SLOPE SLIDING ACTIVE! - Force: {slideForce.magnitude:F2} | Factor: {slopeFactor:F2} | Direction: {slopeDirection} | Grounded: {isGrounded}");
                    Debug.Log($"Velocity BEFORE: Speed={playerVelocity.magnitude:F2} Vec={playerVelocity}");
                }
                  // Add slope sliding to player velocity with MUCH stronger effect
                Vector3 velocityBefore = playerVelocity;
                float forceMultiplier = Time.fixedDeltaTime * 15f; // Increased from 5f to 15f for more noticeable effect
                
                // Apply force differently based on whether grounded or airborne
                if (isGrounded)
                {
                    // When grounded, apply full force
                    playerVelocity.x += slideForce.x * forceMultiplier;
                    playerVelocity.z += slideForce.z * forceMultiplier;
                }
                else
                {
                    // When airborne, apply even more force for acceleration
                    playerVelocity.x += slideForce.x * forceMultiplier * 1.5f;
                    playerVelocity.z += slideForce.z * forceMultiplier * 1.5f;
                    
                    // Add downward force for better slope contact
                    playerVelocity.y -= 8f * Time.fixedDeltaTime;
                }
                
                // DEBUG: Velocity deðiþimi
                if (Input.GetKey(KeyCode.F5))
                {
                    Vector3 velocityChange = playerVelocity - velocityBefore;
                    Debug.Log($"Velocity AFTER: Speed={playerVelocity.magnitude:F2} | Change: {velocityChange.magnitude:F2} Vec={velocityChange}");
                }
                
                // Optional: Reduce friction on slopes for more realistic sliding
                if (slopeAngle > 25f && isGrounded)
                {
                    // Apply reduced friction on steeper slopes
                    float frictionReduction = slopeFactor * 0.7f;
                    Vector3 currentHorizontalVel = new Vector3(playerVelocity.x, 0, playerVelocity.z);
                    Vector3 reducedFriction = currentHorizontalVel * (1f - frictionReduction * Time.fixedDeltaTime);
                    playerVelocity.x = reducedFriction.x;
                    playerVelocity.z = reducedFriction.z;
                    
                    if (Input.GetKey(KeyCode.F5))
                    {
                        Debug.Log($"FRICTION REDUCTION: {frictionReduction:F2} applied");
                    }
                }
            }
            else
            {
                // DEBUG: Neden sliding yok?
                if (Input.GetKey(KeyCode.F5))
                {
                    if (slopeAngle < minSlopeAngle)
                        Debug.Log($"NO SLIDING - Angle {slopeAngle:F1}° TOO FLAT (min: {minSlopeAngle}°)");
                    else if (slopeAngle > maxSlopeAngle)
                        Debug.Log($"NO SLIDING - Angle {slopeAngle:F1}° TOO STEEP (max: {maxSlopeAngle}°)");
                }
            }
        }
        else
        {
            // DEBUG: Raycast miss
            if (Input.GetKey(KeyCode.F5))
            {
                Debug.Log($"SLOPE RAYCAST MISS - Origin: {rayOrigin} | Length: {rayDistance} | Layer: {groundLayers.value}");
            }        }
    }
}
