using UnityEngine;
using System.Collections.Generic; // Added for completeness

namespace Assembly_CSharp
{
    // --- ITEM BASE CLASS ---
    // Note: All custom constructors have been removed.
    // Initialization must happen in Initialize() or via Unity Inspector.
    public class Item : MonoBehaviour
    {
        [Header("Item Data")]
        public string itemName;
        public string itemType;
        public bool isCollected = false; // Default to false

        // Placeholder for initialization, can be called by subclasses
        protected virtual void Initialize(string name, string type)
        {
            itemName = name;
            itemType = type;
        }

        public void Collect()
        {
            isCollected = true;
        }

        public void Use(PlayerMovement player)
        {
            if (!isCollected || player == null)
                return;

            player.PickUpItem(this);
            player.ApplyItemEffect(this);

            // Item is consumed upon use (depending on game rules)
            // If the item should be destroyed, this is usually handled by the Player
            // or by the ApplyItemEffect logic. For now, we'll just set collected to false.
            isCollected = false;
        }

        public void Drop()
        {
            isCollected = false;
        }
    }

    // --- CHEST CLASS ---
    public class Chest : MonoBehaviour // Added MonoBehaviour since it's a scene object
    {
        public bool hasKey = false;
        public bool isOpened = false;

        // Custom constructor removed. Rely on Inspector or Awake.

        public void OpenChest()
        {
            if (hasKey)
            {
                isOpened = true;
                // Logic to spawn item here
            }
        }

        public void GiveKey()
        {
            hasKey = false;
        }
    }

    // --- POTION HIERARCHY ---
    // This must now be an Abstract class OR a component base class. 
    // We'll keep it as a base component for specific potions.
    public abstract class Potion : Item
    {
        // Potion initialization: set the type
        protected override void Initialize(string name, string type = "Potion")
        {
            base.Initialize(name, type);
        }

        public abstract void ApplyEffect(PlayerMovement player);
    }

    public class Booster : Potion
    {
        public float speedIncrease = 1f;
        public float jumpIncrease = 1f;
        public float damageIncrease = 1f;

        protected void Awake()
        {
            Initialize("Booster Potion");
        }

        public override void ApplyEffect(PlayerMovement player)
        {
            if (player == null) return;
            player.IncreaseSpeed(speedIncrease);
            player.IncreaseJump(jumpIncrease);
            player.IncreaseDamage(damageIncrease);
        }
    }

    public class Immune : Potion
    {
        public float immunityDuration = 5f;

        protected void Awake()
        {
            Initialize("Immunity Potion");
        }

        public override void ApplyEffect(PlayerMovement player)
        {
            if (player == null) return;
            // Note: I used 'immunityDuration' from the public field here.
            player.SetImmunity(true, immunityDuration);
        }
    }

    public class SizeUp : Potion
    {
        public float sizeMultiplier = 0.5f;

        protected void Awake()
        {
            Initialize("Size-Up Potion");
        }

        public override void ApplyEffect(PlayerMovement player)
        {
            if (player == null) return;
            player.IncreaseSize(sizeMultiplier);
        }
    }

    public class Heal : Potion
    {
        public int healAmount = 20;

        protected void Awake()
        {
            Initialize("Heal Potion");
        }

        public override void ApplyEffect(PlayerMovement player)
        {
            if (player == null) return;
            player.Heal(healAmount);
        }
    }

    // --- WEAPONS HIERARCHY ---
    public abstract class Weapons : Item
    {
        // Weapons initialization: set the type
        protected override void Initialize(string name, string type = "Weapon")
        {
            base.Initialize(name, type);
        }

        public abstract void Attack(Zombie target);

        public virtual void Equip(PlayerMovement player)
        {
            // The GameManager or PlayerMovement should handle the reference, not the base class.
            // We'll change this to reference the PlayerMovement directly.
            // Assuming currentWeapon is now public in PlayerMovement for simplicity in this structure.
            player.currentWeapon = this;
            Debug.Log($"Equipped weapon: {itemName}");
        }
    }

    public class Sword : Weapons
    {
        public float slashSpeed = 1.0f;
        public int swordDamage = 50; // Added for clarity

        protected void Awake()
        {
            Initialize("Sword");
        }

        public void Slash(Zombie target)
        {
            if (target != null && !target.IsDead())
            {
                target.TakeDamage(swordDamage);
            }

            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Slash");
                animator.speed = slashSpeed;
            }
        }

        public override void Attack(Zombie target)
        {
            Slash(target);
        }

    }

    public class Gun : Weapons
    {
        public int bullets = 6;
        public int gunDamage = 50; // Added for clarity
        public ParticleSystem muzzleFlash;
        public AudioSource shootSound;

        protected void Awake()
        {
            Initialize("Gun");
        }

        public void Shoot(Zombie target)
        {
            if (bullets <= 0) return;
            bullets--;

            if (target != null && !target.IsDead())
            {
                target.TakeDamage(gunDamage);
            }

            if (muzzleFlash != null) muzzleFlash.Play();
            if (shootSound != null) shootSound.Play();

            Animator animator = GetComponent<Animator>();
            if (animator != null) animator.SetTrigger("Shoot");
        }

        public override void Attack(Zombie target)
        {
            Shoot(target);
        }
    }
}