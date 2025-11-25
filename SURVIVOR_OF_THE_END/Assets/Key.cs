using UnityEngine;
using UnityEngine.SceneManagement;

public class Key : MonoBehaviour
{
    [Header("Next Scene Settings")]
    public string nextSceneName; // Assign this in Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collected the key!");

            // Load next scene
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogError("Next scene name NOT assigned!");
            }

            Destroy(gameObject);
        }
    }
}

