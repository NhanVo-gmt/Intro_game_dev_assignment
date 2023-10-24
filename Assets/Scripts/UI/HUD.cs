using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private CountdownUI countDownUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GhostScareUI ghostScareUI;
    [SerializeField] private TextMeshProUGUI gameTimerText;
    [SerializeField] private CanvasGroup gameOver;
    [SerializeField] private LifeUI lifeUI;

    public void ShowCountDown()
    {
        countDownUI.Show();
    }

    public void UpdateCountDown(float time)
    {
        countDownUI.UpdateText(time);
    }

    public void HideCountDown()
    {
        countDownUI.Hide();
    }

    public void ShowGhostTimerUI()
    {
        ghostScareUI.Show();
    }

    public void UpdateGameTimerUI(float timer)
    {
        int fraction = (int)((timer * 100) % 100);
        int second = (int)timer % 100;
        int minute = 0;
        gameTimerText.SetText($"{TimeString(minute)}:{TimeString(second)}:{TimeString(fraction)}");
    }

    string TimeString(int time)
    {
        return time < 10 ? $"0{time}" : time.ToString();
    }

    public void UpdateGhostTimerUI(float timer)
    {
        ghostScareUI.UpdateTime(timer);
    }

    public void HideGhostTimerUI()
    {
        ghostScareUI.Hide();
    }

    public void UpdateScore(int score)
    {
        scoreText.SetText($"Score: {score}");
    }

    public void ShowGameOver()
    {
        gameOver.alpha = 1f;
    }

    public void UpdateLifeUI(int lives)
    {
        lifeUI.UpdateLives(lives);
    }
}
