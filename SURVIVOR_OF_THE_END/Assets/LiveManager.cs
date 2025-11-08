using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSystem
{
    internal class Level
    {
        // Attributes
        private int levelNumber;
        private string mission;
        private bool isCompleted;

        private int playerLives = 3; // main character starts with 3 lives
        private int currentLives;

        // Constructor
        public Level(int levelNumber, string mission)
        {
            this.levelNumber = levelNumber;
            this.mission = mission;
            this.isCompleted = false;
            this.currentLives = playerLives;

            Debug.Log($"Level {levelNumber} started. Mission: {mission}");
        }

        // Method: restartLevel()
        public void restartLevel()
        {
            Debug.Log("Restarting current level...");
            currentLives = playerLives;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Method: completeLevel()
        public void completeLevel()
        {
            isCompleted = true;
            Debug.Log($"Level {levelNumber} completed! Moving to next level...");
            nextLevel();
        }

        // Method: nextLevel()
        public void nextLevel()
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            Debug.Log($"Loading next level: Scene {nextSceneIndex}");
            SceneManager.LoadScene(nextSceneIndex);
        }

        // Method: loseLife()
        public void loseLife()
        {
            currentLives--;
            Debug.Log($"Player lost a life! Remaining lives: {currentLives}");

            if (currentLives <= 0)
            {
                Debug.Log("All lives lost! Restarting level...");
                restartLevel();
            }
        }

        // Method: resetLives() — optional if you want manual reset
        public void resetLives()
        {
            currentLives = playerLives;
            Debug.Log("Lives reset to full!");
        }

        // Getter methods
        public int getCurrentLives() => currentLives;
        public int getLevelNumber() => levelNumber;
        public string getMission() => mission;
        public bool getIsCompleted() => isCompleted;
    }
}