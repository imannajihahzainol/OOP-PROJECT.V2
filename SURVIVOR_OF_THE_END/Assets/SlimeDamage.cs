using UnityEngine;

public class SlimeDamage : MonoBehaviour
{
    public int damage = 3;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Only proceed if the collision is with the player
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Slime hit the player!");
            PlayerMovement player = collision.collider.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeSlimeDamage(damage);
                Debug.Log("Slime damage applied to player!");
            }
            else
            {
                Debug.LogError("PlayerMovement script not found on player!");
            }
        }
    }
}
