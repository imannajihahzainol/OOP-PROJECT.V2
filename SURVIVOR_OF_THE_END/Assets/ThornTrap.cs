using UnityEngine;

public class ThornTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    public int damageAmount = 1;          // Amount of life to remove
    public float damageCooldown = 1f;     // Time before trap can deal damage again

    private bool canDamage = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canDamage) return;

        if (collision.CompareTag("Player"))
        {
            canDamage = false;

            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                Debug.Log("Thorn Trap hit player for 1 life!");
                player.takeDamage(damageAmount);
                Debug.Log($"Thorn Trap triggered! Dealt {damageAmount} damage to player.");
            }

            Invoke(nameof(ResetDamage), damageCooldown);
        }
    }

    private void ResetDamage()
    {
        canDamage = true;
    }
}
