using System.Collections.Generic;
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
}