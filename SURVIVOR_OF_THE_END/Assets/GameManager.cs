using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public GameObject startPanel;
    public GameObject gameOverPanel;

    [Header("Friend's Settings")]
    // public Weapons currentWeapon; // Keep/Remove based on your needs
    public Transform respawnPoint;
    public List<Level> levels = new List<Level>();

    [Header("Game State")]
    public int currentLevel = 1;
    public bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        // else Destroy(gameObject); // Consider adding this if you move to multiple scenes
    }

    private void Start()
    {
        // 1. Pause at start
        Time.timeScale = 0;

        // 2. Display ONLY the Start Panel. Check for null to prevent URE error.
        if (startPanel != null) startPanel.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // Handle the UnassignedReferenceException if panels are not set
        if (startPanel == null) Debug.LogError("startPanel is NOT assigned in the Inspector!");
    }

    public void StartGame()
    {
        isGameOver = false;

        // 1. Unpause the game
        Time.timeScale = 1;

        // 2. Hide ALL UI panels
        if (startPanel != null) startPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // 3. Reset lives
        if (LivesManager.Instance != null)
        {
            LivesManager.Instance.ResetLives();
        }
    }

    public void EndGame()
    {
        isGameOver = true;

        // 1. Pause game
        Time.timeScale = 0;

        // 2. Show ONLY the Game Over Panel
        if (startPanel != null) startPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        Debug.Log("Game Over! Panel Display Attempted.");
    }

    // function LivesManager needs to call for respawn
    public void RestartLevel()
    {
        if (isGameOver) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && respawnPoint != null)
        {
            player.transform.position = respawnPoint.position;
            Debug.Log("Teleported player to Respawn Point");
        }
        else if (levels.Count > 0)
        {
            // If you are using the Level object system
            levels[currentLevel - 1].RestartLevel();
        }
        else
        {
            Debug.LogWarning("Cannot Respawn! Missing Player, RespawnPoint, and Level list.");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Unpause the game before reloading

        // Reloads the currently active scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}