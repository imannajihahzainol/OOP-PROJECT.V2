using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using Assembly_CSharp;


public class Zombie : MonoBehaviour
{
    [Header("Zombie Stats")]
    public int speed = 1;
    public int health = 100;
    public int damage = 10;

    [Header("Attack Settings")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    protected bool isGrounded;

    protected Transform player;
    protected PlayerMovement playerScript;
    private float lastAttackTime = 0f;

    protected virtual void Start()
    {
        // Find player automatically
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerScript = playerObj.GetComponent<PlayerMovement>();
        }
    }

    // Base chase behavior (can be overridden)
    public virtual void ChasePlayer(Transform player)
    {
        if (player == null || !isGrounded) return;

        // Move horizontally toward player (no flying through ground!)
        Vector2 target = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Flip to face player
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // Base attack logic
    public virtual void AttackPlayer()
    {
        if (playerScript == null || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log(name + " attacks player for " + damage + " damage!");
            playerScript.takeDamage(damage);
            lastAttackTime = Time.time;
        }
    }

    // Damage handling
    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log(name + " took " + amount + " damage. Remaining health: " + health);

        if (IsDead()) Die();
    }

    public virtual bool IsDead() => health <= 0;

    protected virtual void Die()
    {
        Debug.Log(name + " is dead!");
        Destroy(gameObject);
    }

    protected virtual void FixedUpdate()
    {
        CheckGround();
    }

    protected void CheckGround()
    {
        if (groundCheck != null)
            isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    }
  
}
