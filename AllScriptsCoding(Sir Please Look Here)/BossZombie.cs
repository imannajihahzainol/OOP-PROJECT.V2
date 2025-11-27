using UnityEngine;

public class BossZombie : Zombie
{
    [Header("Boss Stats")]
    public int bossMaxHealth = 10;
    public int attackPower = 2;
    public float attackRange = 1.5f;
    public float moveSpeed = 2f;

    private bool isAwake = false;

    [Header("Respawn & Tilemap Reset")]
    public Transform playerSpawnPoint;
    public GameObject oldGrid;
    public GameObject bossGrid;
    public GameObject survived;

    private Player playerMovement;                                   

    protected override void Start()
    {
        base.Start();

        // Override base health with boss health
        maxHealth = bossMaxHealth;
        currentHealth = maxHealth;

        // Cache player & movement script
        if (player != null)
            playerMovement = player.GetComponent<Player>();

        isAwake = true;
        Debug.Log("Boss Zombie has AWAKENED!");
    }

    private void OnDisable()
    {
        isAwake = false;
    }

    public override void FixedUpdate()
    {
        if (!isAwake)
            return;

        if (player != null)
        {
            ChasePlayer(player);
            AttackPlayer();
        }
    }

    public override void TakeDamage(int amount)
    {
        if (!isAwake) return;

        base.TakeDamage(amount);   // uses Zombie.cs damage system
    }

    protected override void AttackPlayer()
    {
        if (!isAwake || playerMovement == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            playerMovement.TakeDamage(attackPower);
            Debug.Log($"Boss attacks player for {attackPower}");
        }
    }

    public override void ChasePlayer(Transform targetPlayer)
    {
        if (!isAwake) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPlayer.position,
            moveSpeed * Time.fixedDeltaTime
        );

        // Flip sprite
        Vector3 scale = transform.localScale;
        scale.x = (player.position.x > transform.position.x)
                  ? Mathf.Abs(scale.x)
                  : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public override void Die()
    {
        Debug.Log("Boss Zombie is dead!");

        if (survived != null)
            survived.SetActive(true);

        Destroy(gameObject);
    }
}
