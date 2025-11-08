using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class GameManager
    {
        public int currentLevel { get; private set; }
        public bool isGameOver { get; private set; }

        public List<Level> levels = new List<Level>();

        public GameManager()
        {
            currentLevel = 1;
            isGameOver = false;
        }

        public void startGame()
        {
            isGameOver = false;
            currentLevel = 1;

            Console.WriteLine("Game Started!");
            levels[currentLevel - 1].restartLevel();
        }

        public void endGame()
        {
            isGameOver = true;
            Console.WriteLine("Game Over!");
        }

        public void nextLevel()
        {
            if (isGameOver)
            {
                Console.WriteLine("Cannot continue. Game is already over.");
                return;
            }

            // mark current level complete
            levels[currentLevel - 1].completeLevel();

            currentLevel++;

            if (currentLevel > levels.Count)
            {
                Console.WriteLine("Congratulations! All levels completed.");
                endGame();
            }
            else
            {
                Console.WriteLine("Proceeding to Level " + currentLevel);
                levels[currentLevel - 1].restartLevel();
            }
        }

        public void RestartLevel()
        {
            if (isGameOver)
            {
                Console.WriteLine("Cannot restart. Game is over.");
                return;
            }

            Console.WriteLine("Restarting Level " + currentLevel);
            levels[currentLevel - 1].restartLevel();
        }
    }
}
