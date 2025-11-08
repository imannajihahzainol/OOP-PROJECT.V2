using Assets;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assembly_CSharp
{
    // ITEMS 
    public class Item
    {
        public string itemName;
        public string itemType;
        public bool isCollected;

        public Item(string name, string type)
        {
            itemName = name;
            itemType = type;
            isCollected = false;
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
    public class Chest
    {
        public bool hasKey;
        public bool isOpened;

        public Chest(bool hasKey, bool isOpened)
        {
            this.hasKey = hasKey;
            this.isOpened = isOpened;
        }

        public void OpenChest()
        {
            if (hasKey)
            {
                isOpened = true;
            }
        }

        public void GiveKey()
        {
            hasKey = false;
        }
    }
    public abstract class Potion : Item
    { // Potion is one of the items. Hence, abstraction is used here.
        public Potion(string name) : base(name, "Potion") { }
        public abstract void ApplyEffect(PlayerMovement player);
    }
    public class HealPotion : Potion
    { //While, potions have several functions. For this one, it is to heal
        private int healAmount;

        public HealPotion(int amount) : base("Heal Potion")
        {
            healAmount = amount;
        }

        public override void ApplyEffect(PlayerMovement player)
        {
            player.Heal(healAmount);
        }
    }
    public class BoosterPotion : Potion
    {
        private float speedBoost;

        public BoosterPotion(float boost) : base("Booster Potion")
        {
            speedBoost = boost;
        }

        public override void ApplyEffect(PlayerMovement player)
        {
            player.IncreaseSpeed(speedBoost);
        }
    }
    public class SizeUpPotion : Potion
    {
        private float scaleAmount;

        public SizeUpPotion(float size) : base("Size-Up Potion")
        {
            scaleAmount = size;
        }

        public override void ApplyEffect(PlayerMovement player)
        {
            player.IncreaseSize(scaleAmount);
        }
    }

    public class ImmunePotion : Potion
    {
        public ImmunePotion() : base("Immunity Potion") { }

        public override void ApplyEffect(PlayerMovement player)
        {
            player.SetImmunity(true);
        }
    }
    public abstract class Weapons : Item
    {
        public Weapons(string name) : base(name, “Weapons”) { }
        public abstract void attack(target : Zombie);
    }
    public class Sword : Weapons
    {
        public float slashSpeed = 1.0f;

        public void Slash(Zombie target)
        {
            if (target != null && !target.IsDead())
            {
                target.TakeDamage(50); // you can set damage however you want
            }

            // Play slash animation if attached to GameObject
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Slash");
                animator.speed = slashSpeed;
            }
        }

        public class Gun : Weapons
        {
            public int bullets = 6;

            public ParticleSystem muzzleFlash;
            public AudioSource shootSound;

            public void Shoot(Zombie target)
            {
                if (bullets <= 0) return;

                bullets--;

                if (target != null && !target.IsDead())
                {
                    target.TakeDamage(50); // set your gun damage
                }

                if (muzzleFlash != null) muzzleFlash.Play();
                if (shootSound != null) shootSound.Play();

                Animator animator = GetComponent<Animator>();
                if (animator != null) animator.SetTrigger("Shoot");
            }

        }
        public class StaticZombie : Zombie
        {
            public float detectionRange = 5f;

            void Update()
            {
                // Find player by tag if exists
                GameObject playerObj = GameObject.FindWithTag("Player");
                if (playerObj == null) return;

                Transform player = playerObj.transform;
                float distance = Vector2.Distance(transform.position, player.position);

                if (distance <= detectionRange)
                {
                    ChasePlayer(player);
                }
                else
                {
                    GuardArea();
                }
            }

            public void GuardArea()
            {
                Debug.Log(name + " is guarding its area.");
            }


        }
        public class RunningZombie : Zombie
        {
            public float sprintSpeed = 5f;

            void Update()
            {
                GameObject playerObj = GameObject.FindWithTag("Player");
                if (playerObj == null) return;

                Transform player = playerObj.transform;
                ChasePlayer(player);
            }

            public override void AttackPlayer()
            {
                Debug.Log(name + " sprints and attacks the player!");
            }

            public void Sprint()
            {
                Debug.Log(name + " is sprinting at speed " + sprintSpeed);
            }

        }
        public class FloatingZombie : Zombie
        {
            public float floatHeight = 0.5f;
            private float floatSpeed = 2f;
            private Vector3 startPos;

            void Start()
            {
                startPos = transform.position;
            }

            void Update()
            {
                // Floating animation (up-down motion)
                transform.position = new Vector3(
                    transform.position.x,
                    startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight,
                    transform.position.z
                );

                GameObject playerObj = GameObject.FindWithTag("Player");
                if (playerObj == null) return;

                Transform player = playerObj.transform;
                ChasePlayer(player);
            }

            public void FloatAround()
            {
                Debug.Log(name + " is floating around at height " + floatHeight);
            }
        }
        public class BossZombie : Zombie
        {
            public int attackPower = 50;
            public int rewards = 100;

            void Update()
            {
                GameObject playerObj = GameObject.FindWithTag("Player");
                if (playerObj == null) return;

                Transform player = playerObj.transform;
                ChasePlayer(player);
            }

            public override void AttackPlayer()
            {
                Debug.Log(name + " performs a powerful attack with " + attackPower + " power!");
            }

            public void SpecialAttack()
            {
                Debug.Log(name + " uses its special attack and drops " + rewards + " reward points!");
            }

        }
    }
}
