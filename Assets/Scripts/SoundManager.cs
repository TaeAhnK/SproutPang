using UnityEngine;

public enum SoundType
{
    dig,
    door,
    harvest,
    caught
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SoundManager();
            }
            return instance;
        }
    }

    // Sounds
    [SerializeField] private AudioSource digSound;
    [SerializeField] private AudioSource doorSound;
    [SerializeField] private AudioSource harvestSound;
    [SerializeField] private AudioSource caughtSound;

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

    public void PlaySound(SoundType sound)
    {
        switch (sound)
        {
            case SoundType.dig:
                digSound.Play();
                break;
            case SoundType.door:
                doorSound.Play();
                break;
            case SoundType.harvest:
                harvestSound.Play();
                break;
            case SoundType.caught:
                caughtSound.Play();
                break;
            default:
                break;
        }
    }

}
