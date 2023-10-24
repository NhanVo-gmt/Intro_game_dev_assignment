using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public enum GameState
    {
        Start,
        Paused,
        Playing
    }

    [SerializeField] private HUD hub;

    private GameState currentState = GameState.Start;

    public Action OnStartGame;
    public Action OnPausedGame;
    public Action OnPlayGame;

    private List<Ghost> ghosts;
    
    [Header("Time Component")]
    private float timeBeforeStartGame = 3;

    private float scaredGhostTime = 10f;
    private float scaredGhostElapseTime;
    private bool isRecover = false;
    private bool isNormal = true;

    private float gameTimer = 0f;
    
    private int score = 0;
    private int lives = 3;
    
    private void Start()
    {
        ghosts = GameObject.FindGameObjectsWithTag("Ghost").Select(i => i.GetComponent<Ghost>()).ToList();
        hub.ShowCountDown();
    }

    private void OnEnable()
    {
        foreach (PowerPellet powerPellet in FindObjectsOfType<PowerPellet>())
        {
            powerPellet.OnActivated += ScareGhost;
        }
    }

    private void ScareGhost()
    {
        ghosts.ForEach(ghost => ghost.Scare());
        isRecover = false;
        isNormal = false;
        scaredGhostElapseTime = scaredGhostTime;
        
        hub.ShowGhostTimerUI();
    }
    
    private void RecoverGhost()
    {
        ghosts.ForEach(ghost => ghost.Recover());
    }
    
    private void NormalGhost()
    {
        ghosts.ForEach(ghost => ghost.Normal());
    }

    private void Update()
    {
        if (currentState == GameState.Start)
        {
            hub.UpdateCountDown(timeBeforeStartGame);
            timeBeforeStartGame -= Time.deltaTime;
            
            if (timeBeforeStartGame <= -1f)
            {
                currentState = GameState.Playing;
                hub.HideCountDown();
                OnPlayGame?.Invoke();
            }
        }
        else if (currentState == GameState.Playing)
        {
            gameTimer += Time.deltaTime;
            hub.UpdateGameTimerUI(gameTimer);
            if (isNormal) return;
            
            scaredGhostElapseTime -= Time.deltaTime;
            hub.UpdateGhostTimerUI(scaredGhostElapseTime);

            if (scaredGhostElapseTime <= 3f && !isRecover)
            {
                isRecover = true;
                RecoverGhost();
            }
            else if (scaredGhostElapseTime <= 0f)
            {
                isNormal = true;
                hub.HideGhostTimerUI();
                NormalGhost();
            }
        }
    }
    
    public void UpdateScore(int addScore)
    {
        score += addScore;
        hub.UpdateScore(score);
    }

    public void Die()
    {
        lives -= 1;
        
        hub.UpdateLifeUI(lives);
        
        if (lives <= 0)
        {
            StartCoroutine(GameOverCoroutine());
        }
    }

    IEnumerator GameOverCoroutine()
    {
        currentState = GameState.Paused;
        OnPausedGame?.Invoke();
        hub.ShowGameOver();
        
        Save();

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("StartScene");
    }

    private void Save()
    {
        int highScore = PlayerPrefs.GetInt("Score", 0);
        if (highScore < score)
        {
            PlayerPrefs.SetInt("Score", score);
            PlayerPrefs.SetFloat("GameTime", gameTimer);
        }
        else if (highScore == score)
        {
            if (PlayerPrefs.GetFloat("GameTime", 0f) > gameTimer)
            {
                PlayerPrefs.SetFloat("GameTime", gameTimer);
            }
        }
    }
}