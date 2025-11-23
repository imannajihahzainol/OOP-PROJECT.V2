using UnityEngine;

namespace Assembly_CSharp
{
    public class TeleportPotion : Potion
    {
        public Transform teleportTarget;

        protected void Awake()
        {
            Initialize("Teleport Potion");
        }

        public override void ApplyEffect(PlayerMovement player)
        {
            if (player == null || teleportTarget == null) return;

            player.transform.position = teleportTarget.position;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;

            Debug.Log("Teleport Potion used! Player teleported.");
        }
    }
}

