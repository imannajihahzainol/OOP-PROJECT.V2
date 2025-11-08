using UnityEngine;
using System.Collections.Generic; 

namespace Assembly_CSharp
{
    public class Item : MonoBehaviour
    {
        [Header("Item Data")]
        public string itemName;
        public string itemType;
        public bool isCollected = false; 
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
            isCollected = false;
        }

        public void Drop()
        {
            isCollected = false;
        }
    }
    public abstract class Potion : Item
    {
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
    public abstract class Weapons : Item
    {
        protected override void Initialize(string name, string type = "Weapon")
        {
            base.Initialize(name, type);
        }

        public abstract void Attack(Zombie target);

        public virtual void Equip(PlayerMovement player)
        {
            player.currentWeapon = this;
            Debug.Log($"Equipped weapon: {itemName}");
        }
    }

    public class Sword : Weapons
    {
        public float slashSpeed = 1.0f;
        public int swordDamage = 50; 

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
        public int gunDamage = 50;  
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