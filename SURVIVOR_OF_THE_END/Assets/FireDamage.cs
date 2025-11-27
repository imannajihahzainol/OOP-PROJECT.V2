using UnityEngine;

public class FireDamage : MonoBehaviour
{
    [Header("Fire Damage Settings")]
    public int damageAmount = 1;  // Amount of damage to apply

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                if (player.isImmune)
                {
                    Debug.Log("Player is immune to fire!");
                    return;
                }
                // Apply fire damage
                player.TakeDamage(damageAmount);
                Debug.Log($"Player took {damageAmount} fire damage!");
            }
        }
    }
}
