using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    // Components
    [SerializeField] InputActionAsset playerInput;
    
    // Input Actions
    private InputAction selectAction;
    private InputAction clickAction;

    public event Action Click;

    public Vector2 Selected => selectAction.ReadValue<Vector2>();

    private void Awake()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;

        var playerActionMap = playerInput.FindActionMap("Player");
        selectAction = playerActionMap.FindAction("Position");
        clickAction = playerActionMap.FindAction("Click");

        clickAction.performed += OnClick;

        selectAction.Enable();
        clickAction.Enable();
    }

    private void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                OnPlaying();
                break;
            case GameState.Caught:
                OnCaught();
                break;
            //case GameState.GameOver:
            //    OnGameOver();
            //    break;
            default:
                break;
        }
    }

    private void OnPlaying()
    {
        // Enalble Inputs on Start Game
        selectAction.Enable();
        clickAction.Enable();
    }
    private void OnCaught()
    {
        // Disable Inputs on Caught
        selectAction.Disable();
        clickAction.Disable();
    }

    //private void OnGameOver()
    //{

    //}

    private void OnDestroy()
    {
        clickAction.performed -= OnClick;

        selectAction.Disable();
        clickAction.Disable();
    }

    public void OnClick(InputAction.CallbackContext obj) => Click?.Invoke();
}
