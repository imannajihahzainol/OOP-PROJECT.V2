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

    public virtual void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void Update()
    {
        if (playerTransform != null)
        {
            ChasePlayer(playerTransform);
            AttackPlayer(); // Optional: attacks automatically
        }
    }

    public override void ChasePlayer(Transform player)
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        // Chase player if within range
        if (distanceToPlayer <= chaseRange)
        {
            Vector2 target = player.position;
            transform.position = Vector2.MoveTowards(transform.position, target, runSpeed * Time.deltaTime);
        }
        else
        {
            // Return to original position
            transform.position = Vector2.MoveTowards(transform.position, originalPosition, runSpeed * Time.deltaTime);
        }
        // Flip sprite to face player
        Vector3 scale = transform.localScale;
        scale.x = player.position.x > transform.position.x
                  ? Mathf.Abs(scale.x)
                  : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    // Add this method for damage on contact
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(1);
                Debug.Log("Player took damage from RunningZombie!");
            }
        }
    }
}
