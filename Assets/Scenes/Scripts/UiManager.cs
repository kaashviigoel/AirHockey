using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UiManager : MonoBehaviour
{
    public enum GameMode
    {
        FirstToMaxScore,
        Timed
    }

    [Header("Mode")]
    public GameMode currentMode = GameMode.FirstToMaxScore;
    public float timedMatchLength = 60f;
    float timeRemaining;
    public TextMeshProUGUI timerText;

    [Header("Canvases")]
    public GameObject CanvasMenu;
    public GameObject CanvasGame;
    public GameObject CanvasRestart; 

    [Header("Restart Texts")]
    public GameObject RedWinTxt;
    public GameObject BlueWinTxt;
    public GameObject DrawTxt;

    [Header("Other")]
    public ScoreScript scoreScript;
    public PuckScript puckScript;
    public PlayerMovement redPlayer;
    public PlayerMovement bluePlayer;

    void Start()
    {
        if (CanvasMenu != null) CanvasMenu.SetActive(true);
        if (CanvasGame != null) CanvasGame.SetActive(false);
        if (CanvasRestart != null) CanvasRestart.SetActive(false);

        Time.timeScale = 0f;

        if (timerText != null)
            timerText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (currentMode != GameMode.Timed) return;
        if (CanvasRestart != null && CanvasRestart.activeSelf) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0f) timeRemaining = 0f;

        UpdateTimerUI();

        if (timeRemaining <= 0f)
        {
            EndTimedGame();
        }
    }

    public void SetModeFirstToMaxScore()
    {
        currentMode = GameMode.FirstToMaxScore;
        StartGame();
    }

    public void SetModeTimed()
    {
        currentMode = GameMode.Timed;
        StartGame();
    }

    void StartGame()
    {
        if (CanvasMenu != null) CanvasMenu.SetActive(false);
        if (CanvasRestart != null) CanvasRestart.SetActive(false);
        if (CanvasGame != null) CanvasGame.SetActive(true);

        Time.timeScale = 1f;

        if (currentMode == GameMode.Timed)
        {
            timeRemaining = timedMatchLength;
            if (timerText != null)
            {
                timerText.gameObject.SetActive(true);
                UpdateTimerUI();
            }
        }
        else
        {
            if (timerText != null)
                timerText.gameObject.SetActive(false);
        }

        if (scoreScript != null) scoreScript.ResetScores();
        if (puckScript != null) puckScript.ManualReset();
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;
        int seconds = Mathf.CeilToInt(timeRemaining);
        int minutes = seconds / 60;
        int secs = seconds % 60;
        timerText.text = $"{minutes:00}:{secs:00}";
    }

    void EndTimedGame()
    {
        Time.timeScale = 0f;

        int red = scoreScript != null ? scoreScript.CurrentRedScore : 0;
        int blue = scoreScript != null ? scoreScript.CurrentBlueScore : 0;

        if (CanvasGame != null) CanvasGame.SetActive(false);
        if (CanvasRestart != null) CanvasRestart.SetActive(true);

        if (red > blue)
        {
            if (RedWinTxt != null) RedWinTxt.SetActive(true);
            if (BlueWinTxt != null) BlueWinTxt.SetActive(false);
            if (DrawTxt != null) DrawTxt.SetActive(false);
        }
        else if (blue > red)
        {
            if (RedWinTxt != null) RedWinTxt.SetActive(false);
            if (BlueWinTxt != null) BlueWinTxt.SetActive(true);
            if (DrawTxt != null) DrawTxt.SetActive(false);
        }
        else
        {
            if (RedWinTxt != null) RedWinTxt.SetActive(false);
            if (BlueWinTxt != null) BlueWinTxt.SetActive(false);
            if (DrawTxt != null) DrawTxt.SetActive(true);
        }
    }

    public void ShowRestartCanvas(bool redWon)
    {
        if (currentMode != GameMode.FirstToMaxScore)
            return;

        Debug.Log("ShowRestartCanvas called, redWon = " + redWon);

        if (CanvasGame != null) CanvasGame.SetActive(false);
        if (CanvasRestart != null) CanvasRestart.SetActive(true);

        Time.timeScale = 0f;

        if (redWon)
        {
            if (RedWinTxt != null) RedWinTxt.SetActive(true);
            if (BlueWinTxt != null) BlueWinTxt.SetActive(false);
            if (DrawTxt != null) DrawTxt.SetActive(false);
        }
        else
        {
            if (RedWinTxt != null) RedWinTxt.SetActive(false);
            if (BlueWinTxt != null) BlueWinTxt.SetActive(true);
            if (DrawTxt != null) DrawTxt.SetActive(false);
        }
    }

    public void OnRestartButton()
    {
        Debug.Log("OnRestartButton CLICKED");
        Time.timeScale = 1f;
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    public void OnBackToMenuButton()
    {
        Time.timeScale = 0f;

        if (CanvasMenu != null) CanvasMenu.SetActive(true);
        if (CanvasGame != null) CanvasGame.SetActive(false);
        if (CanvasRestart != null) CanvasRestart.SetActive(false);
    }
}
