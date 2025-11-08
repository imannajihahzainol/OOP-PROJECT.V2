using UnityEngine;

public class RunningZombie : Zombie
{
    public float sprintSpeed = 5f;
    public float detectionRange = 8f; // how far it can see the player

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange && isGrounded)
        {
            // Move only on the X axis (stays on ground)
            Vector2 target = new Vector2(player.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, target, sprintSpeed * Time.deltaTime);

            // Face player
            if (player.position.x > transform.position.x)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);

            // Attack if close enough
            AttackPlayer();
        }
        else
        {
            Idle();
        }
    }

    public override void AttackPlayer()
    {
        base.AttackPlayer();
        Debug.Log(name + " Zombies attacks the player!");
    }

    void Idle()
    {
        // You can add idle animation later
        // For now, just stop moving
    }
}