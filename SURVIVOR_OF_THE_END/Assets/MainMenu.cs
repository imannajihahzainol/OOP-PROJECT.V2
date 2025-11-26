using UnityEngine;
using UnityEngine.SceneManagement; // Required to change scenes

public class MainMenu : MonoBehaviour
{
    // Call this function when "Start" is clicked
    public void PlayGame()
    {
        // This loads the next scene in your Build Settings (usually Level 1)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        // OR: If you know the specific scene name, use:
        // SceneManager.LoadScene("Level1");
    }

    // Call this function when "Exit" or "End" is clicked
    /*public void QuitGame()
    {
        Debug.Log("QUIT GAME!"); 
        Application.Quit();      
    }*/

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
