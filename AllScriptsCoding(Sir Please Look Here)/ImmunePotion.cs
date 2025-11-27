using UnityEngine;

namespace Assembly_CSharp
{
    public class ImmunePotion : Potion
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
}
