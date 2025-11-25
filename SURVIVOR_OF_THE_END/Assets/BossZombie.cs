using UnityEngine;
using System;

public class BossZombie : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public static event Action OnBossDead;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log($"Boss Zombie took {damage} damage: {currentHealth} HP left");

        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("Boss Zombie is dead!");
        OnBossDead?.Invoke();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerWeapon"))
            TakeDamage(1); // example damage
    }
}