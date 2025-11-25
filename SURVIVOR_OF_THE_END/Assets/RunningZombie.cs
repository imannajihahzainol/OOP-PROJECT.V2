using UnityEngine;

public class RunningZombie : Zombie
{
    [Header("Running Zombie Stats")]
    public int runSpeed = 3;
    public float chaseRange = 5f;
    public Transform playerTransform;

    private Vector3 originalPosition;

    protected override void Start()
    {
        base.Start();
        originalPosition = transform.position;

        if (playerTransform == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerTransform = playerObject.transform;
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void Update()
    {
        if (playerTransform != null)
        {
            ChasePlayer(playerTransform);
        }
    }

    public override void ChasePlayer(Transform player)
    {
        if (!isGrounded) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Chase player
        if (distanceToPlayer <= chaseRange)
        {
            Vector2 target = player.position;
            transform.position = Vector2.MoveTowards(transform.position, target, runSpeed * Time.deltaTime);
        }
        else
        {
            // Return to start point
            transform.position = Vector2.MoveTowards(transform.position, originalPosition, runSpeed * Time.deltaTime);
        }

        // Flip sprite
        Vector3 scale = transform.localScale;
        scale.x = player.position.x > transform.position.x
                  ? Mathf.Abs(scale.x)
                  : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}
