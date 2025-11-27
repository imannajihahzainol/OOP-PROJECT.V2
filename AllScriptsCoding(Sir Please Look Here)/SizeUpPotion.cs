using UnityEngine;

public class SizeUpPotion : MonoBehaviour
{
    public float sizeMultiplier = 1.5f;
    public LevelManager levelManager; // assign in Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        collision.transform.localScale *= sizeMultiplier;

        if (levelManager != null)
            levelManager.PlayerGotSizePotion();

        Destroy(gameObject);
    }
}