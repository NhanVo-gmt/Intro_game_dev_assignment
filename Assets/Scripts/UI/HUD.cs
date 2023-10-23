using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GhostScareUI ghostScareUI;

    [SerializeField] private List<Ghost> ghosts;
    
    private int score = 0;

    private void Start()
    {
        ghosts = GameObject.FindGameObjectsWithTag("Ghost").Select(i => i.GetComponent<Ghost>()).ToList();
    }

    private void OnEnable()
    {
        foreach (PowerPellet powerPellet in FindObjectsOfType<PowerPellet>())
        {
            powerPellet.OnActivated += ScareGhost;
        }

        ghostScareUI.On3sLeft += RecoverGhost;
        ghostScareUI.On0sLeft += NormalGhost;
    }
    
    private void ScareGhost()
    {
        foreach (Ghost ghost in ghosts)
        {
            ghost.Scare();
        }
        UpdateGhostTimerUI();
    }

    private void UpdateGhostTimerUI()
    {
        ghostScareUI.Show();
    }

    private void RecoverGhost()
    {
        foreach (Ghost ghost in ghosts)
        {
            ghost.Recover();
        }
    }
    
    private void NormalGhost()
    {
        foreach (Ghost ghost in ghosts)
        {
            ghost.Normal();
        }
    }

    public void UpdateScore(int addScore)
    {
        score += addScore;
        scoreText.SetText($"Score: {score}");
    }
}
