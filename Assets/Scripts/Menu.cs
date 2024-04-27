using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        LevelManager.Singleton.ClearManager();
        LevelManager.ClearSingleton();
        SceneManager.LoadScene(0);
    }
}
