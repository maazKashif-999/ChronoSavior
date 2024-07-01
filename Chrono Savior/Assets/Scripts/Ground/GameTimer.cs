using System;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private Text timerText;
    [SerializeField] private Text highScoreText;
    private float startTime;
    private float pausedTime;
    private static float highScore = 0;
    private float lastUpdateTime = 0;
    private const string HIGH_SCORE_KEY = "GroundHighScore";

    void Start()
    {
        startTime = Time.time;
        highScore = PlayerPrefs.GetFloat(HIGH_SCORE_KEY,0);
        UpdateHighScoreText();
        UpdateTimerText(0);
    }

    void Update()
    {
        // if (!Player.Instance.AreEnemiesFrozen())
        // {
            float currentTime = Time.time - startTime - pausedTime;
            if (Mathf.FloorToInt(currentTime) != Mathf.FloorToInt(lastUpdateTime))
            {
                UpdateTimerText(currentTime);
                lastUpdateTime = currentTime;
            }
        // }
        // else
        // {
            // pausedTime += Time.unscaledDeltaTime;
        // } //TAHA NAY KAHA COMMENT KARDO URAO MAT
    }

    public void StopTimer()
    {
        float finalTime = Time.time - startTime - pausedTime;
        if (finalTime > highScore)
        {
            highScore = finalTime;
            PlayerPrefs.SetFloat(HIGH_SCORE_KEY, highScore);
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
