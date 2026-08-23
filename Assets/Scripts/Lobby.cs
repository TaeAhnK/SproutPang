using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    private bool _isLoading = false;

    public void StartGame()
    {
        if (_isLoading) return;

        _isLoading = true;
        SceneManager.LoadSceneAsync("SproutPang");
    }

    public void OnClickHowToButton()
    {
        if (_isLoading) return;

        _isLoading = true;

        SceneManager.LoadSceneAsync("HowTo");
    }
}
