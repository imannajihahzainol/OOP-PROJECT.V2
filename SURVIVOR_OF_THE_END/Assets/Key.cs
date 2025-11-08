using UnityEngine;

public class Key
{
    public string keyType;
    public bool isCollected;

    // Constructor (called only by Chest)
    public Key(string keyType)
    {
        this.keyType = keyType;
        this.isCollected = false;
    }

    // Collect the key from a chest
    public void Collect(PlayerMovement player)
    {
        if (!isCollected)
        {
            isCollected = true;
            Debug.Log($"{player.name} collected the {keyType} key from the chest!");
        }
        else
        {
            Debug.Log("This key is already collected.");
        }
    }

    // Use the key (optional logic if needed)
    public void UseKey(Chest chest)
    {
        if (isCollected && chest.keyType == this.keyType)
        {
            chest.OpenChest();
        }
        else
        {
            Debug.Log("Cannot use this key here.");
        }
    }
}