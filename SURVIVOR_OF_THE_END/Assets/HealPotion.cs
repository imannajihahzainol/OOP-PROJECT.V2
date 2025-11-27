using UnityEngine;

namespace Assembly_CSharp
{
    public class HealPotion : Potion
    {
        public int healAmount = 1;

        protected void Awake() => Initialize("Heal Potion");

        public override void ApplyEffect(Player player)
        {
            if (player == null) return;
            LivesManager.Instance.HealLife(healAmount);
            Debug.Log($"Heal Potion used! Restored {healAmount} health.");

        }
    }
}



