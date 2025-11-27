using UnityEngine;
namespace Assembly_CSharp
{
    public class TeleportPotion : Potion
    {
    [Header("Teleport Settings")]
        public Transform teleportTarget; // Assign in Inspector

        protected void Awake()
        {
            Initialize("Teleport Potion");
        }

        public override void ApplyEffect(PlayerMovement player)
        {
            if (player == null || teleportTarget == null) return;

            // Teleport the player
            player.transform.position = teleportTarget.position;
            Debug.Log("Player teleported to: " + teleportTarget.position);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                ApplyEffect(player);
                Destroy(gameObject); // Remove potion after use
                Debug.Log("TeleportPotion collected!");
            }
        }
    }
}







