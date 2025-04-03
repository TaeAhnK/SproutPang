using TMPro;
using UnityEngine;

public class UIManager : SubManager<UIManager>
{
    // Canvas Elements
    [SerializeField] private Canvas GamePlayUI;
    [SerializeField] private TextMeshProUGUI gamePlayScore;
    [SerializeField] private Canvas GameOverUI;
    [SerializeField] private TextMeshProUGUI gameOverScore;
    
    protected override void OnPlaying()
    {
        GamePlayUI.enabled = true;
        GameOverUI.enabled = false;
    }

    protected override void OnGameOver()
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
