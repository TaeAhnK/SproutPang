using System.Collections;
using UnityEngine;

public enum CatcherState
{
    Home,
    Watching,
    Caught,
    End
}

public class CatcherConfig
{
    public const float MinWatchingTime = 2f;
    public const float MaxWatchingTime = 4f;
    public const float MinHomeTime = 3f;
    public const float MaxHomeTime = 8f;
    public const float GracePeriod = 0.4f;
    public const float CatcherSpeed = 3f;
    public static Vector3 CatcherDest = new Vector3(-6.94f, -3.3f, 0f);
}

public class Catcher : MonoBehaviour
{
    // Components
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public CatcherState state { get; private set; }
    private Coroutine vigilanceCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void Start()
    {
        spriteRenderer.enabled = false;
        state = CatcherState.Home;
        vigilanceCoroutine = StartCoroutine(Vigilance());
    }

    private void Update()
    {
        // Caught Move Animation
        if (state == CatcherState.Caught)
        {
            float step = CatcherConfig.CatcherSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, CatcherConfig.CatcherDest, step);

            if (Vector3.Distance(transform.position, CatcherConfig.CatcherDest) < 0.001f)
            {
                SoundManager.Instance.PlaySound(SoundType.caught);
                GameManager.Instance.UpdateGameState(GameState.GameOver);
            }
        }
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Caught)
        {
            OnCaught();
        }
        else if (state == GameState.GameOver)
        {
            OnGameOver();
        }
    }

    public void OnCaught()
    {
        // Quit Vigilance
        if (vigilanceCoroutine != null)
        {
            StopCoroutine(vigilanceCoroutine);
            vigilanceCoroutine = null;
        }

        spriteRenderer.enabled = true;
        state = CatcherState.Caught;
        animator.SetBool("IsRunning", true);
    }

    public void OnGameOver()
    {
        state = CatcherState.End;
        animator.SetBool("IsRunning", false);

        // Quit Vigilance
        if (vigilanceCoroutine != null)
        {
            StopCoroutine(vigilanceCoroutine);
            vigilanceCoroutine = null;

            spriteRenderer.enabled = false;
        }
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private IEnumerator Vigilance()
    {
        while (state != CatcherState.Caught && state != CatcherState.End)
        {
            // Home
            spriteRenderer.enabled = false;
            state = CatcherState.Home;
            SoundManager.Instance.PlaySound(SoundType.door);

            yield return new WaitForSeconds(UnityEngine.Random.Range(CatcherConfig.MinWatchingTime, CatcherConfig.MaxWatchingTime));

            // Watching
            spriteRenderer.enabled = true;
            SoundManager.Instance.PlaySound(SoundType.door);
            yield return new WaitForSeconds(CatcherConfig.GracePeriod);

            state = CatcherState.Watching;

            yield return new WaitForSeconds(UnityEngine.Random.Range(CatcherConfig.MinWatchingTime, CatcherConfig.MaxWatchingTime));
        }
    }
}
