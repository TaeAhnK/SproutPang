using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<UIManager>();
                if (instance == null)
                {
                    var go = new GameObject(typeof(UIManager).Name + " Auto-generated");
                    instance = go.AddComponent<UIManager>();
                }
            }
            return instance;
        }
    }
    
    // Canvas Elements
    [SerializeField] private Canvas GamePlayUI;
    [SerializeField] private TextMeshProUGUI gamePlayScore;
    [SerializeField] private Canvas GameOverUI;
    [SerializeField] private TextMeshProUGUI gameOverScore;

    private void Awake()
    {
        instance = this;
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                OnPlaying();
                break;
            //case GameState.Caught:
            //    break;
            case GameState.GameOver:
                OnGameOver();
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }


    private void OnPlaying()
    {
        GamePlayUI.enabled = true;
        GameOverUI.enabled = false;
    }

    private void OnGameOver()
    {
        GamePlayUI.enabled = false;
        GameOverUI.enabled = true;
        gameOverScore.text = "SCORE : " + GameManager.Instance.score.ToString();
    }

    public void UpdateScore()
    {
        gamePlayScore.text = "SCORE : " + GameManager.Instance.score.ToString();
    }

}
