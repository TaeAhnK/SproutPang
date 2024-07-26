using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SproutPang");
    }

    public void OnClickHowToButton()
    {
        SceneManager.LoadScene("HowTo");
    }
}
