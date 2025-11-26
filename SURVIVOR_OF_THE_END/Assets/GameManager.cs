/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int currentLevel;
    public bool isGameOver;
    public Transform respawnPoint; // Add this line
    public List<Level> levels = new List<Level>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager created and set to persist.");
        }
        else
        {
            Debug.Log("GameManager already exists, destroying duplicate.");
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        StartGame();
    }
    public void StartGame()
    {
        isGameOver = false;
        currentLevel = 1;
        Debug.Log("Game Started!");
        if (levels.Count > 0)
        {
            levels[currentLevel - 1].RestartLevel();
        }
        else
        {
            Debug.LogWarning("No levels assigned to GameManager!");
        }
    }
    // Add this method to set the respawn point
    public void SetRespawnPoint(Transform point)
    {
        respawnPoint = point;
        Debug.Log("Respawn point set to: " + point.position);
    }
    public void endGame()
    {
        isGameOver = true;
        Debug.Log("Game Over!");
    }
    public void nextLevel()
    {
        if (isGameOver)
        {
            Debug.Log("Cannot continue. Game is already over.");
            return;
        }
        if (levels.Count == 0)
        {
            Debug.LogWarning("No levels assigned to GameManager!");
            return;
        }
        levels[currentLevel - 1].CompleteLevel();
        currentLevel++;
        if (currentLevel > levels.Count)
        {
            Debug.Log("Congratulations! All levels completed.");
            endGame();
        }
        else
        {
            Debug.Log($"Proceeding to Level {currentLevel}");
            levels[currentLevel - 1].RestartLevel();
        }
    }
    public void RestartLevel()
    {
        if (isGameOver)
        {
            Debug.Log("Cannot restart. Game is over.");
            return;
        }
        if (levels.Count == 0)
        {
            Debug.LogWarning("No levels assigned to GameManager!");
            return;
        }
        Debug.Log($"Restarting Level {currentLevel}");
        levels[currentLevel - 1].RestartLevel();
    }
}*/

/*using Assembly_CSharp;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Friend's Settings")]
    public Weapons currentWeapon;
    public Transform respawnPoint;
    public List<Level> levels = new List<Level>();

    [Header("UI References")]
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject[] hearts;

    [Header("Game State")]
    public int currentLevel;
    public bool isGameOver;

    [Header("Player Stats")]
    public int maxLives = 3;
    public int currentLives;

    private void Awake()
    {
        // FIX: Remove DontDestroyOnLoad so the buttons reset correctly when scene reloads
        Instance = this;
    }

    private void Start()
    {
        // Pause game at start so player sees the Start Menu
        Time.timeScale = 0;

        if (startPanel != null) startPanel.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void StartGame()
    {
        isGameOver = false;
        currentLevel = 1;
        currentLives = maxLives;

        // UI Updates
        if (startPanel != null) startPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        UpdateLivesUI();

        Time.timeScale = 1; // Unpause the game

        // Now the Player can move and JUMP (handled by your teammate's script)
        if (levels.Count > 0)
        {
            levels[currentLevel - 1].RestartLevel();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isGameOver) return;

        currentLives -= damage;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            EndGame();
        }
    }

    public void UpdateLivesUI()
    {
        if (hearts == null) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentLives) hearts[i].SetActive(true);
            else hearts[i].SetActive(false);
        }
    }

    public void EndGame()
    {
        isGameOver = true;
        Time.timeScale = 0; // Pause game
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // --- YOUR FRIEND'S LEVEL CODE BELOW ---

    public void SetRespawnPoint(Transform point)
    {
        respawnPoint = point;
    }

    public void NextLevel()
    {
        if (isGameOver) return;
        if (levels.Count == 0) return;

        levels[currentLevel - 1].CompleteLevel();
        currentLevel++;

        if (currentLevel > levels.Count) EndGame();
        else levels[currentLevel - 1].RestartLevel();
    }

    public void RestartLevel()
    {
        if (isGameOver) return;
        if (levels.Count > 0) levels[currentLevel - 1].RestartLevel();
    }
}*/

using GameSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public GameObject startPanel;
    public GameObject gameOverPanel;
    // NOTE: We REMOVED the "hearts" array from here.

    [Header("Friend's Settings")]
    // If "Weapons" gives an error, remove the next line, otherwise keep it:
    // public Weapons currentWeapon; 
    public Transform respawnPoint;
    public List<Level> levels = new List<Level>();

    [Header("Game State")]
    public int currentLevel = 1;
    public bool isGameOver = false;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 0; // Pause at start
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // Tell the Doctor to reset the hearts
        LivesManager.Instance.ResetLives();
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    //public void RestartGame()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}

    // Add this to GameManager.cs
    public void RestartLevel()
    {
        if (isGameOver) return;

        // OPTION A: If you are using your friend's Level system
        if (levels.Count > 0)
        {
            levels[currentLevel - 1].RestartLevel();
            // Note: Ask your friend if their RestartLevel() reloads the scene. 
            // If it does, tell them to change it to just move the player!
        }
        // OPTION B: The Simple Teleport (Use this if you don't have the level list yet)
        else
        {
            // Find the player
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // Check if we have a respawn point set
            if (player != null && respawnPoint != null)
            {
                player.transform.position = respawnPoint.position;
                Debug.Log("Teleported player to Respawn Point");
            }
            else
            {
                Debug.LogWarning("Cannot Respawn! Missing Player or RespawnPoint.");
                // Fallback: Reload scene (but this resets hearts)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

        // If we are running in the Editor, stop playing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // If we are in the real game, close the window
            Application.Quit();
#endif
    }
}