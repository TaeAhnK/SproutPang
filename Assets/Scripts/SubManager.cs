using UnityEngine;

public abstract class SubManager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindAnyObjectByType<T>();
                if (!instance)
                {
                    var go = new GameObject(typeof(T).Name + " Auto-generated");
                    instance = go.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        instance = this as T;
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this as T;
        }

        GameManager.OnGameStateChanged += OnGameStateChanged;
    }
    
    protected void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                OnPlaying();
                break;
            case GameState.Caught:
                OnCaught();
                break;
            case GameState.GameOver:
                OnGameOver();
                break;
            default:
                break;
        }
    }

    protected virtual void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    protected virtual void OnPlaying() { }
    protected virtual void OnCaught() { }
    protected virtual void OnGameOver() { }
}