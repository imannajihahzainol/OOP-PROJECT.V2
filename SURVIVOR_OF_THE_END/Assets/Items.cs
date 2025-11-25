using UnityEngine;

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

    // -------------------- POTIONS --------------------

    public abstract class Potion : Item
    {
        protected override void Initialize(string name, string type = "Potion")
        {
            base.Initialize(name, type);
        }

        public abstract void ApplyEffect(PlayerMovement player);
    }


    // -------------------- WEAPONS --------------------

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


}
