using UnityEngine;
using Assembly_CSharp;

public class Sword : Weapons
{

    [Header("Sword Settings")]
    public int swordDamage = 50;       // Damage dealt by the sword
    public float slashSpeed = 1.0f;     // Speed of the slash animation
    public Sprite swordSprite;         // Sprite for the sword (optional, can be loaded from Resources)
    [Tooltip("Sudut rotasi Z (dalam darjah) pedang apabila ia dipegang.")]
    public float equippedRotationZ = 90f;

    protected void Awake()
    {
        Initialize("Sword");
    }


    // Tambah di bahagian atas, dalam class Sword
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Sword collided with: {other.gameObject.name}, Tag: {other.tag}");

        if (other.CompareTag("Zombie"))
        {
            Zombie zombie = other.GetComponent<Zombie>();

            if (zombie != null && !zombie.IsDead)
            {
                zombie.TakeDamage(swordDamage);
                Debug.Log($"SUCCESS! Sword dealt {swordDamage} damage to {zombie.name}!");
            }
            else
            {
                Debug.LogWarning("Zombie component not found or zombie is dead!");
            }
        }
    }

    // Method to attack a zombie
    public void Slash(Zombie target)
    {
        if (target != null && !target.IsDead)
        {
            target.TakeDamage(swordDamage);
            Debug.Log($"Sword dealt {swordDamage} damage to {target.name}!");
        }
        // Play slash animation if available
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Slash");
            animator.speed = slashSpeed;
        }
    }

    // Override the Attack method from the Weapons class
    public override void Attack(Zombie target)
    {

        Debug.Log("Sword slash method called.");
        Slash(target);
    }

    // Override the Equip method to handle visuals
    public override void Equip(PlayerMovement player)
    {
        base.Equip(player);
        EquipVisually(player);
    }

    // Method to visually equip the sword to the player
    private void EquipVisually(PlayerMovement player)
    {
        Transform weaponHolder = player.transform.Find("WeaponHolder");
        if (weaponHolder == null)
        {
            weaponHolder = new GameObject("WeaponHolder").transform;
            weaponHolder.SetParent(player.transform);
            weaponHolder.localPosition = new Vector3(0.5f, 0, 0); // Position at the player's hand
        }
        // Clear any existing weapon
        foreach (Transform child in weaponHolder)
        {
            Destroy(child.gameObject);
        }
        // Create a visual representation of the sword
        GameObject swordVisual = new GameObject("Sword_Visual");
        swordVisual.transform.SetParent(weaponHolder);
        swordVisual.transform.localPosition = Vector3.zero;
        SpriteRenderer swordSpriteRenderer = swordVisual.AddComponent<SpriteRenderer>();
        swordVisual.transform.localRotation = Quaternion.Euler(0, 0, equippedRotationZ);
        if (swordSprite != null)
        {
            swordSpriteRenderer.sprite = swordSprite;
        }
        else
        {
            // Load the sprite from Resources if not set in the Inspector
            Sprite sprite = Resources.Load<Sprite>("Sprites/Weapons/Sword");
            if (sprite != null)
            {
                swordSpriteRenderer.sprite = sprite;
            }
            else
            {
                Debug.LogError("Sword sprite not found in Resources/Sprites/Weapons/");
            }
        }
        swordSpriteRenderer.sortingLayerName = "Player";
        swordSpriteRenderer.sortingOrder = 1;
        Debug.Log("Sword equipped visually!");
    }

    // Inside Sword.cs

    public void ActivateHitbox()
    {
        // If you had animation, this is where you'd trigger it.
        // Without animation, this is where you'd manually ENABLE the Collider.

        // For now, let's log to confirm the call worked:
        Debug.Log("Sword Hitbox Activation called!");

        // If you want the Collider to only be active during the attack:
        // GetComponent<Collider2D>().enabled = true;
        // You would then need to disable it after a short delay (e.g., 0.1s).
    }


}
