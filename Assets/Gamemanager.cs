using System;
using UnityEngine;


public enum GameState
{
    Paused,
    Easy,
    Hard
}

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager Instance;

    [SerializeField] private GameState currentGameState;

    [SerializeField] private bool easyDone = false;
    [SerializeField] private bool hardDone = false;

    [SerializeField] private int score;

    public static event Action<int> OnChangeScore;

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public bool EasyDone
    {
        get { return easyDone; }
    }
    public bool HardDone
    {
        get { return hardDone; }
    }

    public GameState CurrentGameState
    {
        get { return currentGameState; }
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }


    private void Start()
    {
        currentGameState = GameState.Paused;
    }

    public void SwitchState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Paused:
                Soundmanager.Instance.StopBackgroundAudio();
                break;
            case GameState.Easy:
                Spawnmanager.Instance.ButtonSpawnTime = 3; //20 buttons per game -> 10 points goal
                Spawnmanager.Instance.StartSpawning();
                break;
            case GameState.Hard:
                Spawnmanager.Instance.ButtonSpawnTime = 0.75f; // 60 buttons per game -> 30 point goal
                Spawnmanager.Instance.StartSpawning();
                break;
        }
        currentGameState = gameState;
    }

    public void SetupEasyGame()
    {
        UIManager.Instance.OpenTestModalUI(
            "Gamerules", 
            "Please make sure the watch is well positioned.", 
            "During this game buttons will randomly appear. Your score and time will be shown at the top.", 
            "Your goal: 10 points.", 
            () => StartEasyGame());
    }

    public void SetupHardGame()
    {
        UIManager.Instance.OpenTestModalUI(
            "Gamerules",
            "Please make sure the watch is well positioned.", 
            "During this game buttons will randomly appear. Your score and time will be shown at the top.", 
            "Your goal: 30 points.", 
            () => StartHardGame());
    }

    public void PauseGame()
    {
        SwitchState(GameState.Paused);
    }

    public void StartEasyGame()
    {
        easyDone = true;
        SwitchState(GameState.Easy);
        ResetScore();
        Soundmanager.Instance.PlayBackgroundA();
    }

    public void StartHardGame()
    {
        hardDone = true;
        ResetScore();
        SwitchState(GameState.Hard);
        Soundmanager.Instance.PlayBackgroundB();
    }

    public void AddToScore(int amount)
    {
        score += amount;
        OnChangeScore.Invoke(score);
    }

    public void ResetScore()
    {
        score = 0;
        OnChangeScore.Invoke(score);
    }
}
