using UnityEngine;

namespace Assembly_CSharp
{
    public class BoosterPotion : Potion
    {
        public float boosterDuration = 3f;
        [Header("Boost Values")]
        public float speedIncrease = 3f;
        public float jumpIncrease = 3f;

        protected void Awake()
        {
            Initialize("Booster Potion");
        }

        public override void ApplyEffect(PlayerMovement player)
        {
            if (player == null)
            {
                Debug.LogWarning("Booster tried to apply effect but PlayerMovement is missing.");
                return;
            }

            // Apply boosts
            player.IncreaseSpeed(speedIncrease);
            player.IncreaseJump(jumpIncrease);

            Debug.Log($"Booster applied! Speed +{speedIncrease}, Jump +{jumpIncrease}");
        }
    }
}
