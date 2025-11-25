using UnityEngine;
using Assembly_CSharp;

public class Zombie : MonoBehaviour
{
    [Header("Zombie Stats")]
    public int zombieHealth = 1;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;
    public bool IsDead;

    protected Transform player;
    protected PlayerMovement playerMovement;

    protected virtual void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            playerMovement = p.GetComponent<PlayerMovement>();
        }
    }

    protected virtual void FixedUpdate() { }

    // ZOMBIE TAKES DAMAGE
    public virtual void TakeDamage(int amount)
    {
        zombieHealth -= amount;
        if (zombieHealth <= 0)
            Destroy(gameObject);
    }

    // ZOMBIE ATTACK PLAYER
    public virtual void AttackPlayer()
    {
        if (player == null || playerMovement == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                playerMovement.TakeDamage(1);   // ✔ using your playerMovement code
                lastAttackTime = Time.time;
            }
        }
    }
    public virtual void ChasePlayer(Transform targetPlayer)
    {
        // handled in child classes
    }
}
