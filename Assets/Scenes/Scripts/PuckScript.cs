using System.Collections;
using UnityEngine;

public class PuckScript : MonoBehaviour
{
    [Header("Score manager (drag in Inspector or auto-find in Start)")]
    public ScoreScript scoreScriptInstance;

    [Header("Players (drag paddles here)")]
    public PlayerMovement redPlayer;
    public PlayerMovement bluePlayer;

    [Header("Puck reset settings")]
    public float resetDelay = 1f;
    public Vector2 startPosition = Vector2.zero;
    public float initialImpulse = 5f;

    public static bool WasGoal { get; private set; }

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (scoreScriptInstance == null)
        {
            scoreScriptInstance = FindObjectOfType<ScoreScript>();
            if (scoreScriptInstance == null)
            {
                Debug.LogError("PuckScript: ScoreScript instance not found in scene!");
            }
        }

        WasGoal = false;
        startPosition = rb.position;

        ManualReset();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (WasGoal) return;

        if (other.CompareTag("BlueGoal"))
        {
            if (scoreScriptInstance != null)
                scoreScriptInstance.Increment(ScoreScript.Score.RedScore);

            WasGoal = true;
            StartCoroutine(ResetPuck());
        }
        else if (other.CompareTag("RedGoal"))
        {
            if (scoreScriptInstance != null)
                scoreScriptInstance.Increment(ScoreScript.Score.BlueScore);

            WasGoal = true;
            StartCoroutine(ResetPuck());
        }
    }

    private IEnumerator ResetPuck()
    {
        yield return new WaitForSecondsRealtime(resetDelay);
        ManualReset();
    }

    public void ManualReset()
    {
        StopAllCoroutines();
        WasGoal = false;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.position = startPosition;

        if (redPlayer != null) redPlayer.ResetPosition();
        if (bluePlayer != null) bluePlayer.ResetPosition();

        Vector2 dir = Random.value > 0.5f ? Vector2.up : Vector2.down;
        dir.x = Random.Range(-0.5f, 0.5f);
        dir.Normalize();
        rb.AddForce(dir * initialImpulse, ForceMode2D.Impulse);
    }
}
