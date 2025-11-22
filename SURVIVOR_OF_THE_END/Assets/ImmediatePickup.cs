using UnityEngine;
using Assembly_CSharp; // This is necessary to access your Item, Potion, and Weapon classes

public class ImmediatePickup : MonoBehaviour
{
    private Item itemComponent;

    void Start()
    {
        // Get the generic 'Item' component (which is actually a Potion subclass)
        itemComponent = GetComponent<Item>();
        if (itemComponent == null)
        {
            Debug.LogError($"'{gameObject.name}' is missing an Item component (e.g., Heal.cs, Booster.cs). Pickup failed.");
            enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Check if the object we collided with is the player.
        if (other.CompareTag("Player"))
        {
            // 2. Get the PlayerMovement script from the player object.
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (player != null)
            {
                // The item must be marked as collected for the Item.Use() method to proceed.
                itemComponent.Collect();

                // 3. Immediately use the item. This single call triggers the entire effect chain:
                //    a. Item.Use() calls player.ApplyItemEffect()
                //    b. player.ApplyItemEffect() calls Potion.ApplyEffect() (Polymorphism)
                //    c. Potion.ApplyEffect() calls the specific method (e.g., player.Heal())
                itemComponent.Use(player);

                // 4. Destroy the physical object once it's used.
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Player object is missing the PlayerMovement script!");
            }
        }
    }
}