using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private string missionNumber;
    [SerializeField] private bool isCompleted;
    [SerializeField] private int playerLives = 3;

    private string currentScene;
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }
    public void LoseLife()
    {
        playerLives--;

        if (playerLives <= 0)
        {
            RestartLevel();
        }
    }
    public void RestartLevel()
    {
        playerLives = 3;
        SceneManager.LoadScene(currentScene);
    }
    public void CompleteLevel()
    {
        isCompleted = true;
        NextLevel();
    }
    public void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    //for UI
    public int GetLives()
    {
        return playerLives;
    }
    public bool IsCompleted()
    {
        return isCompleted;
    }
    public int GetLevelNumber()
    {
        return levelNumber;
    }
}

//TRIGGER BASE CLASS
public abstract class LevelTrigger : MonoBehaviour
{
    // The Level object this trigger will communicate with
    protected Level currentLevel;

    protected virtual void Start()
    {
        currentLevel = FindObjectOfType<Level>();
    }

    // Each trigger will define what happens when player enters
    protected abstract void OnPlayerTrigger(Collider2D other);
}