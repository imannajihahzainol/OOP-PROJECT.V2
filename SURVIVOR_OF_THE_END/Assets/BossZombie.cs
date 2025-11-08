using UnityEngine;

public class BossZombie : Zombie
{
    [Header("Boss Stats")]
    public int attackPower = 50;
    public int rewards = 100;

    void Update()
    {
        if (player == null) return;

        ChasePlayer(player);
        AttackPlayer();
    }

    public override void AttackPlayer()
    {
        Debug.Log(name + " performs a powerful attack with " + attackPower + " power!");
        if (playerScript != null)
        {
            playerScript.takeDamage(attackPower);
        }
    }

    public void SpecialAttack()
    {
        Debug.Log(name + " uses its special attack and drops " + rewards + " reward points!");
    }

    protected override void Die()
    {
        Debug.Log(name + " is dead and drops " + rewards + " reward points!");
        base.Die();
    }
}