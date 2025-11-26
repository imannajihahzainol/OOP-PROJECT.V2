using UnityEngine;

public class LivesManager : MonoBehaviour
{
    public static LivesManager Instance; // Singleton

    [Header("Settings")]
    public int maxLives = 3;
    public int currentLives;

    [Header("UI References")]
    public GameObject[] hearts; // Move your hearts array here!

    //private void Start()
    //{
    //    // Initialize the hearts immediately when game loads
    //    currentLives = maxLives;
    //    UpdateUI();
    //}

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        // 1. Set lives to max (3)
        currentLives = maxLives;

        // 2. Force the hearts to appear
        UpdateUI();
    }
    public void ResetLives()
    {
        currentLives = maxLives;
        UpdateUI();
    }

    //public void LoseLife(int damage)
    //{
    //    currentLives -= damage;
    //    UpdateUI();

    //    if (currentLives <= 0)
    //    {
    //        // Tell the Boss (GameManager) that we died
    //        GameManager.Instance.EndGame();
    //    }
    //}

    /*public void LoseLife(int damage)
    {
        currentLives -= damage;
        UpdateUI(); // This deletes the heart visually

        if (currentLives <= 0)
        {
            // No lives left? Game Over.
            GameManager.Instance.EndGame();
        }
        else
        {
            // Still have lives? Respawn!
            // calling your existing Restart/Respawn logic in GameManager
            GameManager.Instance.RestartLevel();
        }
    }*/

    // Inside LivesManager.cs

    public void LoseLife(int damage)
    {
        currentLives -= damage;
        UpdateUI(); // This updates the visual hearts

        if (currentLives <= 0)
        {
            // 1. Game Over: Tell the GameManager to display the panel and pause
            GameManager.Instance.EndGame();

            // We can optionally destroy the player object here if needed, but the Game Over panel should pause the game flow.
        }
        else
        {
            // 2. Player still has lives: Tell the GameManager to move the player back to the respawn point
            GameManager.Instance.RestartLevel();
        }
    }

    public void HealLife(int amount)
    {
        currentLives += amount;
        if (currentLives > maxLives) currentLives = maxLives;
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentLives) hearts[i].SetActive(true);
            else hearts[i].SetActive(false);
        }
    }
}