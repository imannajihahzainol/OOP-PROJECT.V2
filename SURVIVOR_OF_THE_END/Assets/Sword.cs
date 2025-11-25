using UnityEngine;
using Assembly_CSharp; // Make sure this matches your namespace

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
}