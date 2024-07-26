using UnityEngine;

public enum BGMList
{
    MainTheme,
    EndTheme
}

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;
    public static BGMManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<BGMManager>();
                if (instance == null)
                {
                    var go = new GameObject(typeof(BGMManager).Name + " Auto-generated");
                    instance = go.AddComponent<BGMManager>();
                }
            }
            return instance;
        }
    }

    [SerializeField] private AudioSource mainTheme;
    [SerializeField] private AudioSource endTheme;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            GameManager.OnGameStateChanged += OnGameStateChanged;
        }
    }

    private void Start()
    {
        if (!mainTheme.isPlaying)
        {
            PlayBGM(BGMList.MainTheme);
        }
    }

    private void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                StopBGM(BGMList.EndTheme);
                if (!mainTheme.isPlaying)
                {
                    PlayBGM(BGMList.MainTheme);
                }
                break;
            case GameState.Caught:
                StopBGM(BGMList.MainTheme);
                break;
            case GameState.GameOver:
                StopBGM(BGMList.MainTheme);
                PlayBGM(BGMList.EndTheme);
                break;
            default:
                break;
        }
    }

    public void PlayBGM(BGMList bgm)
    {
        switch (bgm)
        {
            case BGMList.MainTheme:
                mainTheme.Play();
                break;
            case BGMList.EndTheme:
                endTheme.Play();
                break;
            default:
                break;
        }
    }

    public void StopBGM(BGMList bgm)
    {
        switch (bgm)
        {
            case BGMList.MainTheme:
                mainTheme.Stop();
                break;
            case BGMList.EndTheme:
                endTheme.Stop();
                break;
            default:
                break;
        }
    }

}
