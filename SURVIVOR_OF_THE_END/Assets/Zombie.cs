using UnityEngine;

public class Zombie : MonoBehaviour
{
    [Header("Zombie Settings")]
    public int maxHealth = 2;
    protected int currentHealth;
    protected Transform player;

    public bool IsDead
    {
        get { return currentHealth <= 0; }
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
        Debug.Log($"{name} initialized with {currentHealth} health.");
    }





    public virtual void TakeDamage(int damage)
    {
        Debug.Log($"{name} taking {damage} damage. Current health before damage: {currentHealth}");
        currentHealth -= damage;
        Debug.Log($"{name} current health after damage: {currentHealth}");
        if (IsDead)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log($"{name} died!");
        Destroy(gameObject);
    }

    protected virtual void AttackPlayer()
    {
        
    }

    public virtual void FixedUpdate() { }

    public virtual void ChasePlayer(Transform player) { }
}
