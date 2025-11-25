using UnityEngine;

public class Key : MonoBehaviour
{
    public string playerKeyType;  // jenis key player pegang

    private void OnTriggerEnter2D(Collider2D other)
    {
        Chest chest = other.GetComponent<Chest>();
        if (chest != null)
        {
            if (chest.keyType == playerKeyType)
            {
                chest.OpenChest();
            }
            else
            {
                Debug.Log("Wrong key!");
            }
        }
    }
}
