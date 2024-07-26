using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HowTo : MonoBehaviour
{
    [SerializeField] private Sprite[] images;
    [SerializeField] private Button screen;
    private Image screenImage;
    private int index;

    private void Awake()
    {
        screenImage = screen.GetComponent<Image>();
    }

    private void Start()
    {
        index = 0;
        screenImage.sprite = images[index];
    }

    public void OnScreenClick()
    {
        index++;
        if (index < images.Length)
        {
            screenImage.sprite = images[index];
        }
        else
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
