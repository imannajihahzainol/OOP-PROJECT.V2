using UnityEngine;

namespace Assembly_CSharp
{
    public class Heal : Potion
    {
        public int healAmount = 1;

        protected void Awake() => Initialize("Heal Potion");

        public override void ApplyEffect(PlayerMovement player)
        {
            if (player == null) return;
            LivesManager.Instance.HealLife(healAmount);
            Debug.Log($"Heal Potion used! Restored {healAmount} health.");

        }
    }
}



