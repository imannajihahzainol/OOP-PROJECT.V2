using UnityEngine;

public class SizeUp : MonoBehaviour
{
    [Header("Size-Up Potion Settings")]
    public float sizeMultiplier = 0.5f;
    public string groundTag = "Ground"; // Tag of your Tilemap or ground object

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (player != null)
            {
                // Increase player size
                player.IncreaseSize(sizeMultiplier);
                Debug.Log($"Player size increased by {sizeMultiplier}!");

                // Disable the ground tilemap or object
                GameObject groundObj = GameObject.FindGameObjectWithTag(groundTag);

                if (groundObj != null)
                {
                    groundObj.SetActive(false);
                    Debug.Log("Ground is now disabled for boss fight.");
                }
                else
                {
                    Debug.LogWarning("GROUND OBJECT WITH TAG 'Ground' NOT FOUND!");
                }

                // Destroy potion
                Destroy(gameObject);
            }
        }
    }
}