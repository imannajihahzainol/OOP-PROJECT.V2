using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Key Requirements")]
    public string keyType;

    [Header("Chest State")]
    public bool hasKey = false;
    public bool isOpened = false;

    public void OpenChest()
    {
        if (!isOpened)
        {
            isOpened = true;
            Debug.Log($"Chest opened! Spawning loot...");
            // You can add animation, sound, or item spawn logic here later
        }
        else
        {
            Debug.Log("This chest is already open.");
        }
    }

    public void GiveKey()
    {
        hasKey = false;
    }
}
