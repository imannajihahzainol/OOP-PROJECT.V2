using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    public int damage = 50;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Sword touched: {other.gameObject.name}, Tag: {other.tag}");

        if (other.CompareTag("Zombie"))
        {
            Zombie zombie = other.GetComponent<Zombie>();

            if (zombie != null && !zombie.IsDead)
            {
                zombie.TakeDamage(damage);
                Debug.Log($"✅ Sword dealt {damage} damage to {zombie.name}!");
            }
            else
            {
                Debug.LogWarning("❌ Zombie component not found or zombie already dead!");
            }
        }
    }
}