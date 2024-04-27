using TMPro;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pausedText;
    [SerializeField] GameObject button;

    private enum GameState { active, paused }

    private static GameState gameState = GameState.active;

    private static bool endedGame = false;

    public static PauseGame Singleton;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseScreen(true);
        }
        if (endedGame)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LevelManager.Singleton.RestartGame();
                endedGame = false;
            }
        }
    }

    public void PauseScreen(bool showText)
    {
        switch (gameState)
        {
            case GameState.active:
                Time.timeScale = 0;

                if (showText)
                {
                    pausedText.enabled = true;
                    button.SetActive(true);
                }

                gameState = GameState.paused;
                break;
            case GameState.paused:
                Time.timeScale = 1;

                pausedText.enabled = false;
                button.SetActive(false);

                gameState = GameState.active;
                break;
        }
    }

    public static void EndedGame()
    {
        endedGame = true;
    }

    public static void ClearSingleton()
    {
        Singleton = null;
    }
}
