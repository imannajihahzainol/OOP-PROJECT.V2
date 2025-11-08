using Assembly_CSharp;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
    {
    public Weapons currentWeapon;

    public static GameManager Instance { get; private set; }

        public int currentLevel;
        public bool isGameOver;

        public List<Level> levels = new List<Level>();

        private void Awake()
        {
            // Ensure only one GameManager exists (Singleton Pattern)
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            StartGame();
        }

        // Start the game from level 1
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

        // End the game
        public void endGame()
        {
            isGameOver = true;
            Debug.Log("Game Over!");
        }

        // Proceed to next level
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

        // Restart current level
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
