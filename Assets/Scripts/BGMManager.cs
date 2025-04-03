using UnityEngine;

public enum BGMList
{
    MainTheme,
    EndTheme
}

public class BGMManager : SubManager<BGMManager>
{
    [SerializeField] private AudioSource mainTheme;
    [SerializeField] private AudioSource endTheme;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (!mainTheme.isPlaying)
        {
            PlayBGM(BGMList.MainTheme);
        }
    }

    protected override void OnPlaying()
    {
        StopBGM(BGMList.EndTheme);
        if (!mainTheme.isPlaying)
        {
            PlayBGM(BGMList.MainTheme);
        }
    }

    protected override void OnCaught()
    {
        StopBGM(BGMList.MainTheme);
    }

    protected override void OnGameOver()
    {
        StopBGM(BGMList.MainTheme);
        PlayBGM(BGMList.EndTheme);
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
