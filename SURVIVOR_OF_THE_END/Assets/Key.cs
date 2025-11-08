using UnityEngine;
using Assembly_CSharp;

public class Key
{
    public string keyType;
    public bool isCollected;
    public Key(string keyType)
    {
        this.keyType = keyType;
        this.isCollected = false;
    }

    public void Collect(PlayerMovement player)
    {
        if (!isCollected)
        {
            isCollected = true;
            Debug.Log($"{player.name} collected the {keyType} key!");
        }
        else
        {
            Debug.Log("This key is already collected.");
        }
    }

    public void UseKey(Chest chest)
    {
        if (isCollected && chest.keyType == this.keyType)
        {
            chest.OpenChest();
            isCollected = false; 
            Debug.Log($"Used {keyType} key to open the chest.");
        }
        else
        {
            Debug.Log("Cannot use this key here, or the key type doesn't match.");
        }
    }
}