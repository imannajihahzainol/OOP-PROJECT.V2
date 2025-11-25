using UnityEngine;

public class BossZombie : Zombie
{
    [Header("Boss Stats")]
    public int maxHealth = 10;
    private int currentHealth;

    public int attackPower = 2;

    private bool isAwake = false;

    [Header("Respawn & Tilemap Reset")]
    public Transform playerSpawnPoint;    // where the player should return to
    public GameObject oldGrid;            // your ORIGINAL tilemap grid
    public GameObject bossGrid;           // tilemap used for boss fight
    public GameObject survived;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerMovement = player.GetComponent<PlayerMovement>();

        currentHealth = maxHealth;
        isAwake = true;

        Debug.Log("Boss Zombie has AWAKENED!");
    }


    private void OnDisable()
    {
        isAwake = false;
    }

    protected override void FixedUpdate()
    {
        if (!isAwake) return;  // do nothing if inactive
        if (player != null)
        {
            ChasePlayer(player);
            AttackPlayer();
        }
    }

    public override void TakeDamage(int amount)
    {
        if (!isAwake) return;

        currentHealth -= amount;
        Debug.Log($"Boss Zombie took {amount} damage, HP left: {currentHealth}");
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Boss Zombie is dead!");
        Destroy(gameObject);
        survived.SetActive(true);
    }

    public override void AttackPlayer()
    {
        if (!isAwake || playerMovement == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            playerMovement.TakeDamage(attackPower);
            Debug.Log("Boss attacks player for " + attackPower);

            // Check if player is dead
            if (playerMovement.lives <= 0)
            {
                ResetLevel();
            }
        }
    }


    public override void ChasePlayer(Transform targetPlayer)
    {
        if (!isAwake) return;

        // Example: move directly toward player
        float speed = 2f;
        transform.position = Vector2.MoveTowards(transform.position, targetPlayer.position, speed * Time.fixedDeltaTime);

        // Flip sprite to face player
        Vector3 scale = transform.localScale;
        scale.x = player.position.x > transform.position.x
                  ? Mathf.Abs(scale.x)
                  : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private void ResetLevel()
    {
        Debug.Log("Player died in boss fight — resetting level.");

        // 1. teleport player to the spawn location
        player.position = playerSpawnPoint.position;

        // 2. switch back tilemaps
        if (oldGrid != null) oldGrid.SetActive(true);
        if (bossGrid != null) bossGrid.SetActive(false);

        // 3. reset player size (OPTIONAL)
        player.localScale = Vector3.one;

        // 4. reset boss
        this.gameObject.SetActive(false);   // hide boss so it doesn’t attack immediately
        isAwake = false;

        // OPTIONAL: restore boss HP when reactivated later
        currentHealth = maxHealth;

    }
}
