using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public enum Score
    {
        RedScore,
        BlueScore
    }

    public TextMeshProUGUI RedScoreTxt;
    public TextMeshProUGUI BlueScoreTxt;
    public UiManager uiManager;

    #region Scores
    private int redScore;
    private int blueScore;

    [Header("Max score to win")]
    public int MaxScore = 5;

    // expose for timed mode
    public int CurrentRedScore => redScore;
    public int CurrentBlueScore => blueScore;

    private int RedScore
    {
        get { return redScore; }
        set
        {
            redScore = value;

            // Only auto-end in FirstToMaxScore mode
            if (uiManager != null &&
                uiManager.currentMode == UiManager.GameMode.FirstToMaxScore &&
                redScore == MaxScore)
            {
                uiManager.ShowRestartCanvas(true); // red wins
            }
        }
    }

    private int BlueScore
    {
        get { return blueScore; }
        set
        {
            blueScore = value;

            if (uiManager != null &&
                uiManager.currentMode == UiManager.GameMode.FirstToMaxScore &&
                blueScore == MaxScore)
            {
                uiManager.ShowRestartCanvas(false); // blue wins
            }
        }
    }
    #endregion

    public void Increment(Score whichScore)
    {
        if (whichScore == Score.RedScore)
        {
            RedScore++;
            if (RedScoreTxt != null)
                RedScoreTxt.text = RedScore.ToString();
        }
        else if (whichScore == Score.BlueScore)
        {
            BlueScore++;
            if (BlueScoreTxt != null)
                BlueScoreTxt.text = BlueScore.ToString();
        }
    }

    public void ResetScores()
    {
        redScore = 0;
        blueScore = 0;
        if (RedScoreTxt != null) RedScoreTxt.text = "0";
        if (BlueScoreTxt != null) BlueScoreTxt.text = "0";
    }
}
