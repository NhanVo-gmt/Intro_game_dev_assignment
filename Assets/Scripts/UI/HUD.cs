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
}
