using UnityEngine;
using UnityEngine.Tilemaps;

public class HideOnTouch : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<TilemapRenderer>().enabled = false; // hides tilemap
            // Optional: remove collision so player can walk through
            TilemapCollider2D col = GetComponent<TilemapCollider2D>();
            if (col != null)
                col.enabled = false;
        }
    }
}

