using UnityEngine;

public class ThornTrap : MonoBehaviour
{
    public int damageAmount = 1;
    public float damageCooldown = 1f;
    private bool canDamage = true;
    
        private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canDamage)
        {
            canDamage = false;
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.takeDamage(damageAmount);
                Debug.Log($"Thorn Trap triggered! Dealt {damageAmount} damage to player.");
            }
            Invoke(nameof(ResetDamage), damageCooldown);
        }
    }

   
    void ResetDamage()
    {
        canDamage = true;
    }
}
