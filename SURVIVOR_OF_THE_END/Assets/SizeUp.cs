using UnityEngine;

public class SizeUp : MonoBehaviour
{
    [Header("Size-Up Potion Settings")]
    public float sizeMultiplier = 0.5f; // How much the player's size will increase

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.IncreaseSize(sizeMultiplier);
                Debug.Log($"Player size increased by {sizeMultiplier}!");
                Destroy(gameObject); // Remove the potion after pickup
            }
        }
    }
}