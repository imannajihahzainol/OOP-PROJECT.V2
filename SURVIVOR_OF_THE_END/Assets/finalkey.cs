using UnityEngine;

public class finalkey : MonoBehaviour
{
    public GameObject survivedPanel;   // assign your SURVIVED UI here

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            survivedPanel.SetActive(true);  // show SURVIVED UI
            Destroy(gameObject);            // remove key from scene
            Time.timeScale = 0f;            // freeze game (optional)
        }
    }

}
