using UnityEngine;

public class WaterDamage : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                if (player.isImmune)
                {
                    Debug.Log("Player is immune to water!");
                    return;
                }
                // Kill the player if not immune
                player.Respawn();
            }
        }
    }
}
