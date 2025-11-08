using Assembly_CSharp;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Transform respawnPoint;

    [Header("Player Stats")]
    public int lives = 3;

    [Header("Damage Settings")]
    public float invincibilityDuration = 1f;
    public float knockbackForce = 5f;
    public float flashSpeed = 0.1f;

    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float climbSpeed = 3f;

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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
        spriteRenderer = GetComponent<SpriteRenderer>();

        controls = new PlayerControls();

        // Input Bindings
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Jump.performed += ctx => jump();
        controls.Player.Attack.performed += ctx => attack();

        groundLayer = LayerMask.NameToLayer("Ground");
        playerLayer = gameObject.layer;
    }

    void OnEnable() => controls.Player.Enable();
    void OnDisable() => controls.Player.Disable();

    public void moveLeft()
    {
        rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
        transform.localScale = new Vector3(-1, 1, 1);
    }

    public void moveRight()
    {
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void jump()
    {
        if (isGrounded && !isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    public void attack()
    {
        if (isAttacking) return;

        isAttacking = true;
        Debug.Log("Player Attacked!");

        Invoke(nameof(_resetAttack), 0.5f);
    }

    private void _resetAttack() => isAttacking = false;


    // Take Damage
    public void takeDamage(int amount)
    {
        if (isInvincible) return; // ignore damage during i-frames

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

    // Respawn Player
    private void Respawn()
    {
        lives = 3; // Reset lives

        transform.position = respawnPoint.position;

        rb.linearVelocity = Vector2.zero;

        StartCoroutine(DamageEffects()); 
    }

    private System.Collections.IEnumerator DamageEffects()
    {
        isInvincible = true;

        // Knockback 
        float direction = transform.localScale.x > 0 ? -1 : 1;
        rb.linearVelocity = new Vector2(direction * knockbackForce, rb.linearVelocity.y);

        // Flashing effect
        for (float i = 0; i < invincibilityDuration; i += flashSpeed * 2)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(flashSpeed);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(flashSpeed);
        }

        isInvincible = false;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Ladder climbing
        if (isOnLadder && Mathf.Abs(moveInput.y) > 0.1f)
        {
            isClimbing = true;
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * climbSpeed);

            Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, true);
            return;
        }

        // On ladder but idle
        if (isClimbing && isOnLadder)
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, 0f);
            return;
        }

        // Normal movement
        isClimbing = false;
        rb.gravityScale = originalGravity;

        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, false);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // Choose correct UML method
        if (moveInput.x > 0.1f)
            moveRight();
        else if (moveInput.x < -0.1f)
            moveLeft();
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    // Ladder Triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            isOnLadder = true;
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
    public void PickUpItem(Item item)
    {
        if (item == null) return;

        // Mark item as collected
        item.Collect();

        Debug.Log("Picked up: " + item.itemName);
    }
    public void ApplyItemEffect(Item item)
    {
        if (item == null) return;

        if (item is Potion potion)
        {
            potion.ApplyEffect(this);
            Debug.Log($"Applied potion effect: {item.itemName}");
        }
        else if (item is Weapons weapon)
        {
            weapon.Equip(this);
            Debug.Log($"Equipped weapon: {item.itemName}");
        }
    }

    public int health = 100;
    public float speed = 5f;
    public bool isImmune = false;
    public float size = 1f;

    public void Heal(int amount)
    {
        health += amount;
        if (health > 100) health = 100;
        Debug.Log("Player Healed: " + health);
    }

    public void IncreaseSpeed(float amount)
    {
        speed += amount;
        Debug.Log("Player Speed Increased: " + speed);
    }
    public void IncreaseJump(float amount)
    {
        jumpForce += amount;
        Debug.Log("Player Jump Increased: " + jumpForce);
    }

    public void IncreaseDamage(float amount)
    {
        // You can later connect this to your attack system.
        Debug.Log("Player Damage Increased by: " + amount);
    }


    public void IncreaseSize(float amount)
    {
        size += amount;
        transform.localScale += new Vector3(amount, amount, 0);
        Debug.Log("Player Size Increased: " + transform.localScale);
    }

    public void SetImmunity(bool value, float duration = 0f)
    {
        if (value)
        {
            isImmune = true;
            Debug.Log("Player Immunity: ON");

            // If duration > 0, start a coroutine to turn it off later
            if (duration > 0)
                StartCoroutine(RemoveImmunityAfterDelay(duration));
        }
        else
        {
            isImmune = false;
            Debug.Log("Player Immunity: OFF");
        }
    }
    private System.Collections.IEnumerator RemoveImmunityAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        isImmune = false;
        Debug.Log("Player Immunity expired!");
    }
    public virtual void Equip(PlayerMovement player)
    {
        equippedPlayer = player;
    }

    public virtual void Unequip()
    {
        equippedPlayer = null;
    }


}
