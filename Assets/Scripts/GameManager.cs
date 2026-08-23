using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Caught,
    GameOver
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindAnyObjectByType<GameManager>();
                if (!instance)
                {
                    var go = new GameObject(typeof(GameManager).Name + " Auto-generated");
                    instance = go.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    private bool _isLoading = false;

    // Game Elements
    //public Match3 match3;
    public Catcher catcher;

    // Game State
    public int score = 0;
    public GameState gameState { get; private set; }

    // Alert Center
    public static Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.Playing);
        score = 0;
        AddScore(0); // To Init Score UI
    }

    public void UpdateGameState(GameState state)
    {
        gameState = state;        

        OnGameStateChanged?.Invoke(gameState);
    }

    public void RestartGame()
    {
        if (_isLoading) return;
    
        _isLoading = true;
        SceneManager.LoadSceneAsync("SproutPang");
    }

    public void AddScore(int value)
    {
        score += value;
        UIManager.Instance.UpdateScore();
    }
}
