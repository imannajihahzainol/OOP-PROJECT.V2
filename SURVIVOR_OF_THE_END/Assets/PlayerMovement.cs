using Assembly_CSharp;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public Weapons currentWeapon;
    public Transform respawnPoint;

    // ---------------- PLAYER STATS ----------------
    [Header("Player Stats")]
    public int lives = 3;
    public int health = 100;

    [Header("Quest Stats")]
    public int bonesCollected = 0; // Tracks how many bones collected

    [Header("Immunity")]
    public bool isImmune = false;        // Potion immunity (for slime only)
    public float immunityTimer = 0f;

    // ---------------- DAMAGE SYSTEM ----------------
    [Header("Damage Settings")]
    public float invincibilityDuration = 1f; // i-frames after taking damage
    public float knockbackForce = 5f;
    public float flashSpeed = 0.1f;

    private bool isInvincible = false;     // i-frame system
    private SpriteRenderer spriteRenderer;

    // ---------------- MOVEMENT ----------------
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float climbSpeed = 3f;

    // ---------------- GROUND CHECK ----------------
    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    private PlayerControls controls;
    private Vector2 moveInput;

    private bool isGrounded;
    private bool isAttacking;
    private bool isClimbing = false;
    private bool isOnLadder = false;
    private float originalGravity;

    private int groundLayer;
    private int playerLayer;
    public TextMeshProUGUI boneText;

    // -------------------------------------------------------------
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalGravity = rb.gravityScale;

        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => jump();
        controls.Player.Attack.performed += ctx => attack();

        groundLayer = LayerMask.NameToLayer("Ground");
        playerLayer = gameObject.layer;
    }

    void OnEnable() => controls.Player.Enable();
    void OnDisable() => controls.Player.Disable();

    // -------------------------------------------------------------
    // BASIC MOVEMENT
    public void moveLeft()
    {
        rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    public void moveRight()
    {
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    public void jump()
    {
        if (isGrounded && !isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // -------------------------------------------------------------
    // ATTACK
    public void attack()
    {
        if (isAttacking) return;

        isAttacking = true;
        Debug.Log("Player Attacked!");
        Invoke(nameof(_resetAttack), 0.5f);
    }

    private void _resetAttack() => isAttacking = false;

    // -------------------------------------------------------------
    // UNIVERSAL DAMAGE FUNCTION
    public void takeDamage(int amount)
    {
        // Block only slime damage through the immune potion
        if (isImmune)
        {
            Debug.Log("Player is immune! Slime damage blocked.");
            return;
        }

        // Normal i-frame invincibility still works
        if (isInvincible) return;

        lives -= amount;
        Debug.Log("Player took damage! Lives left: " + lives);

        if (lives <= 0)
        {
            Debug.Log("Player died!");
            Respawn();
            return;
        }

        StartCoroutine(DamageEffects());
    }


    // -------------------------------------------------------------
    // SPECIAL SLIME DAMAGE FUNCTION (uses isImmune)
    public void TakeSlimeDamage(int amount)
    {
        if (isImmune)
        {
            Debug.Log("Slime damage ignored (IMMUNE!)");
            return;
        }
        takeDamage(amount);
    }


    // -------------------------------------------------------------
    // RESPAWN SYSTEM
    public void Respawn()
    {
        Debug.Log("Respawn point position: " + respawnPoint.position);
        Debug.Log("Moving player to: " + respawnPoint.position);
        lives = 3;
        transform.position = respawnPoint.position;
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(DamageEffects());
    }

    // -------------------------------------------------------------
    // DAMAGE EFFECTS / I-FRAMES
    private System.Collections.IEnumerator DamageEffects()
    {
        isInvincible = true;

        float direction = transform.localScale.x > 0 ? -1 : 1;
        rb.linearVelocity = new Vector2(direction * knockbackForce, rb.linearVelocity.y);

        for (float i = 0; i < invincibilityDuration; i += flashSpeed * 2)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(flashSpeed);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(flashSpeed);
        }

        isInvincible = false;
    }

    // -------------------------------------------------------------
    void FixedUpdate() => HandleMovement();

    private void HandleMovement()
    {
        // LADDER
        if (isOnLadder && Mathf.Abs(moveInput.y) > 0.1f)
        {
            isClimbing = true;
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * climbSpeed);
            Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, true);
            return;
        }

        if (isClimbing && isOnLadder)
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, 0f);
            return;
        }

        // NORMAL
        isClimbing = false;
        rb.gravityScale = originalGravity;

        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, false);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (moveInput.x > 0.1f) moveRight();
        else if (moveInput.x < -0.1f) moveLeft();
        else rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // IMMUNITY TIMER UPDATE
        if (isImmune)
        {
            immunityTimer -= Time.deltaTime;
            if (immunityTimer <= 0)
            {
                isImmune = false;
                Debug.Log("Immunity expired.");
            }
        }
    }

    // -------------------------------------------------------------
    // TRIGGERS
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // --- EXISTING LADDER CODE ---
        if (collision.CompareTag("Ladder"))
            isOnLadder = true;

        // --- EXISTING BONE CODE ---
        // ... (Bone code remains the same) ...

        // --- EXISTING DOG SUBMISSION ---
        // ... (Dog code remains the same) ...

        // ------------------ NEW CODE FOR PICKUP ------------------
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            // NEW: Use the Collect and Use methods defined in the Item class.
            item.Collect(); // Mark the item as collected
            item.Use(this); // Apply the item's effect (which equips the weapon)
            Destroy(collision.gameObject); // Remove the item from the scene
        }
    
}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = false;
            isClimbing = false;
            rb.gravityScale = originalGravity;
            Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, false);
        }
    }

    // -------------------------------------------------------------
    // ITEM SYSTEM
    public void PickUpItem(Item item)
    {
        if (item != null)
            Debug.Log("Picked up: " + item.itemName);
    }

    public void ApplyItemEffect(Item item)
    {
        if (item is Potion potion)
        {
            potion.ApplyEffect(this);
        }
        else if (item is Weapons weapon)
        {
            weapon.Equip(this);
        }
    }

    // POTION EFFECTS --------------------------------
    public void Heal(int amount)
    {
        health = Mathf.Min(100, health + amount);
        Debug.Log("Player Healed: " + health);
    }

    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;
        Debug.Log("Speed Increased: " + moveSpeed);
    }

    public void IncreaseJump(float amount)
    {
        jumpForce += amount;
        Debug.Log("Jump Increased: " + jumpForce);
    }

    public void IncreaseSize(float amount)
    {
        transform.localScale += new Vector3(amount, amount, 0);
        Debug.Log("Player Size Increased");
    }

    // IMMUNITY EFFECT
    public void SetImmunity(bool value, float duration)
    {
        isImmune = value;

        if (value)
        {
            immunityTimer = duration;
            Debug.Log("IMMUNITY ACTIVATED!");
        }
        else Debug.Log("IMMUNITY OFF");
    }
}
