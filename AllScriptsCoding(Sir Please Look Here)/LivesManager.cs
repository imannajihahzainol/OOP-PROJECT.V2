using UnityEngine;

public class LivesManager : MonoBehaviour
{
    public static LivesManager Instance; // Singleton

    [Header("Settings")]
    public int maxLives = 3;
    public int currentLives;

    [Header("UI References")]
    public GameObject[] hearts; // Move your hearts array here!

   

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

   

    public void LoseLife(int damage)
    {
        // 1. DEDUCT LIFE and UPDATE UI (CRITICAL FIX)
        currentLives -= damage; // <--- THIS LINE MUST BE PRESENT
        UpdateUI();             // <--- THIS LINE MUST BE PRESENT

        Debug.Log("Life Lost. Current Lives: " + currentLives);

        if (currentLives <= 0)
        {
            // 2. TRIGGER GAME OVER
            if (GameManager.Instance != null)
            {
                GameManager.Instance.EndGame();
            }
        }
        else
        {
            // 3. TRIGGER RESPAWN
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RestartLevel();
            }
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