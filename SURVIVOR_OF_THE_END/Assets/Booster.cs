using UnityEngine;

namespace Assembly_CSharp
{
    public class Booster : Potion
    {
        public float boosterDuration = 3f;
        [Header("Boost Values")]
        public float speedIncrease = 5f;
        public float jumpIncrease = 5f;

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
