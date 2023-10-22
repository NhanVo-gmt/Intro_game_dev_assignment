using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;

    public void UpdateScore(int addScore)
    {
        score += addScore;
        scoreText.SetText($"Score: {score}");
    }
}
