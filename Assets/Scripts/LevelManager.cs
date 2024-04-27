using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Team { left, right }

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject crateScorePrefab;

    [SerializeField] Canvas worldSpaceCanvas;

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI player1ScoreText;
    [SerializeField] TextMeshProUGUI player2ScoreText;
    [SerializeField] TextMeshProUGUI gameOverText;

    private float player1Score = 0;
    private float player2Score = 0;

    private float timestepsInSecond;
    private int elapsedTime = 0;
    private int elapsedSeconds = 0;

    public static LevelManager Singleton;

    public static Dictionary<Team, Color> colorDict = new Dictionary<Team, Color>();

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }

        colorDict.Add(Team.left, Color.red);
        colorDict.Add(Team.right, Color.blue);
    }

    private void Start()
    {
        timestepsInSecond = 1 / Time.fixedDeltaTime;

        timerText.text = $"{elapsedSeconds}";

        player1ScoreText.SetText($"{player1Score}");
        player2ScoreText.SetText($"{player2Score}");
    }

    private void FixedUpdate()
    {
        elapsedTime++;
        if (elapsedTime % timestepsInSecond == 0)
        {
            elapsedSeconds++;
            timerText.SetText($"{elapsedSeconds}");
        }

        if (player1Score >= 250)
        {
            EndGame(Team.right);
        }
        else if (player2Score >= 250)
        {
            EndGame(Team.left);
        }
    }

    public void SetPlayerScore(Team crateId, float score)
    {
        if (crateId == Team.left)
        {
            player1Score += score;
            player1ScoreText.SetText($"{player1Score}");
        }
        else
        {
            player2Score += score;
            player2ScoreText.SetText($"{player2Score}");
        }
    }

    public void EndGame(Team playerId)
    {
        if (playerId == Team.left)
        {
            gameOverText.SetText($"Player{(int)Team.right + 1}\nWins!");
        }
        else
        {
            gameOverText.SetText($"Player{(int)Team.left + 1}\nWins!");
        }

        gameOverText.enabled = true;

        PauseGame.EndedGame();
        PauseGame.Singleton.PauseScreen(false);
    }

    public void RestartGame()
    {
        gameOverText.enabled = false;

        ClearManager();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ClearManager()
    {
        colorDict.Clear();
        elapsedTime = 0;
        Time.timeScale = 1;
    }

    public static void ClearSingleton()
    {
        Singleton = null;
        PauseGame.ClearSingleton();
    }

    public int GetElapsedSeconds()
    {
        return elapsedSeconds;
    }

    public TextMeshProUGUI GetTimerText()
    {
        return timerText;
    }

    public TextMeshProUGUI SpawnCrateScore(Vector2 position)
    {
        GameObject crateScore = Instantiate(crateScorePrefab, position, Quaternion.identity,
                                            worldSpaceCanvas.gameObject.transform);
        return crateScore.GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator DestroyCrateScore(GameObject crateScore, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(crateScore);
    }
}
