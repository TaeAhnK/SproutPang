using System;
using System.Collections;
using UnityEngine;

public enum VegetableType
{
    Wheat,
    Tomato,
    Carrot,
    Eggplant
}

public enum VegetableState
{
    None,
    Lv1,
    Lv2,
    Riped
}

public static class VegetableStateExtensions
{
    public static VegetableState NextState(this VegetableState currentState)
    {
        switch (currentState)
        {
            case VegetableState.None:
                return VegetableState.Lv1;
            case VegetableState.Lv1:
                return VegetableState.Lv2;
            case VegetableState.Lv2:
            case VegetableState.Riped:
                return VegetableState.Riped;
            default:
                throw new ArgumentOutOfRangeException(nameof(currentState), currentState, null);
        }
    }
}

public static class VegetableConfig
{
    public const int VegNum = 4;
    public const float MinGrowTime = 5f;
    public const float MaxGrowTime = 10f;
}

public class Vegetable : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites = new Sprite[4];
    [SerializeField] public GameObject particle;
    private SpriteRenderer _spriteRenderer;

    public VegetableType type;
    public VegetableState state { get; private set; }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _spriteRenderer.sprite = sprites[1];
        state = VegetableState.Lv1;

        StartCoroutine(Grow());
    }

    private IEnumerator Grow()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(VegetableConfig.MinGrowTime, VegetableConfig.MaxGrowTime));

        state = state.NextState();
        _spriteRenderer.sprite = sprites[(int)state];

        if (state != VegetableState.Riped)
        {
            StartCoroutine(Grow());
        }
    }

    public void Pop()
    {
        VegetableParticleManager.Instance.PlayParticle(type, transform.position, Quaternion.identity);
        // GameObject effect = Instantiate(particle, transform.position, Quaternion.identity);
        // Destroy(effect, 1f);
    }
}