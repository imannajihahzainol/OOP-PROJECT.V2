using UnityEngine;
using Assembly_CSharp;

public class Gun : Weapons
{
    [Header("Gun Settings")]
    public int bullets = 6;
    public int gunDamage = 50;
    public ParticleSystem muzzleFlash;
    public AudioSource shootSound;
    public Sprite gunSprite;
    [Tooltip("Rotation of the gun when equipped (in degrees).")]
    public float equippedRotationZ = 270f;

    protected void Awake()
    {
        Initialize("Gun");
        Debug.Log("Gun initialized.");
    }

    public void Shoot(Zombie target)
    {
        if (bullets <= 0)
        {
            Debug.Log("Out of bullets!");
            return;
        }

        bullets--;
        Debug.Log($"Shooting at {target.name}. Bullets left: {bullets}");

        if (target != null && !target.IsDead)
        {
            target.TakeDamage(gunDamage);
            Debug.Log($"Shot {target.name} for {gunDamage} damage!");
        }
        if (muzzleFlash != null) muzzleFlash.Play();
        if (shootSound != null) shootSound.Play();

        Animator animator = GetComponent<Animator>();
        if (animator != null) animator.SetTrigger("Shoot");
    }

    public override void Attack(Zombie target)
    {
        Debug.Log("Gun Attack method called.");
        Shoot(target);
    }

    public override void Equip(Player player)
    {
        base.Equip(player);
        Debug.Log("Gun equipped visually.");
        EquipVisually(player);
    }

    private void EquipVisually(Player player)
    {
        Transform weaponHolder = player.transform.Find("WeaponHolder");
        if (weaponHolder == null)
        {
            weaponHolder = new GameObject("WeaponHolder").transform;
            weaponHolder.SetParent(player.transform);
            weaponHolder.localPosition = new Vector3(0.5f, 0, 0);
        }

        foreach (Transform child in weaponHolder)
        {
            Destroy(child.gameObject);
        }

        GameObject gunVisual = new GameObject("Gun_Visual");
        gunVisual.transform.SetParent(weaponHolder);
        gunVisual.transform.localPosition = Vector3.zero;
        gunVisual.transform.localRotation = Quaternion.Euler(0, 0, equippedRotationZ);

        SpriteRenderer gunSpriteRenderer = gunVisual.AddComponent<SpriteRenderer>();
        if (gunSprite != null)
        {
            gunSpriteRenderer.sprite = gunSprite;
        }
        else
        {
            Sprite sprite = Resources.Load<Sprite>("Sprites/Weapons/Gun");
            if (sprite != null)
            {
                gunSpriteRenderer.sprite = sprite;
            }
            else
            {
                Debug.LogError("Gun sprite not found in Resources/Sprites/Weapons/");
            }
        }
        gunSpriteRenderer.sortingLayerName = "Player";
        gunSpriteRenderer.sortingOrder = 1;
    }
}
