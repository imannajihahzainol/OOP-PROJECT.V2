using UnityEngine;

public class SizeUp : MonoBehaviour
{
    public float sizeMultiplier = 1.5f;

    [Header("Boss Activation")]
    public GameObject bossZombieParent;   // assign the boss parent or the boss itself, inactive at start

    [Header("Tilemap Switching")]
    public GameObject currentGrid;        // active grid
    public GameObject newGrid;            // inactive grid to switch to

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // 1. Increase player size
        collision.transform.localScale *= sizeMultiplier;

        // 2. Activate boss
        bossZombieParent.SetActive(true);
        Debug.Log("Boss Zombie ACTIVATED!");

        // 3. Switch tilemaps
        if (currentGrid != null) currentGrid.SetActive(false);
        if (newGrid != null) newGrid.SetActive(true);

        // 4. Destroy the potion
        Destroy(gameObject);
    }
}
