using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text timerText;
    public Text highScoreText;

    private float startTime;
    private float pausedTime;
    private static float highScore = 0;
    private float lastUpdateTime = 0;

    void Start()
    {
        startTime = Time.time;
        UpdateHighScoreText();
        UpdateTimerText(0);
    }

    void Update()
    {
        if (!Player.Instance.AreEnemiesFrozen())
        {
            float currentTime = Time.time - startTime - pausedTime;
            if (Mathf.FloorToInt(currentTime) != Mathf.FloorToInt(lastUpdateTime))
            {
                UpdateTimerText(currentTime);
                lastUpdateTime = currentTime;
            }
        }
        else
        {
            pausedTime += Time.unscaledDeltaTime;
        }
    }

    public void StopTimer()
    {
        float finalTime = Time.time - startTime - pausedTime;
        if (finalTime > highScore)
        {
            highScore = finalTime;
            UpdateHighScoreText();
        }
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = "Highest Score: " + FormatTime(highScore);
    }

    private void UpdateTimerText(float time)
    {
        timerText.text = "Score: " + FormatTime(time);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
