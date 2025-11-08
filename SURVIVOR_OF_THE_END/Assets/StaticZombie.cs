using UnityEngine;

public class StaticZombie : Zombie
{
    public float detectionRange = 5f;

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            ChasePlayer(player);
            AttackPlayer(); // Attack when near
        }
        else
        {
            GuardArea();
        }
    }

    public void GuardArea()

    {
        Debug.Log(name + " is guarding its area.");
    }
}