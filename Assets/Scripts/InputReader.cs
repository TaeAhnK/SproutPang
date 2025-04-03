using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : SubManager<InputReader>
{
    // Components
    [SerializeField] InputActionAsset playerInput;
    
    // Input Actions
    private InputAction selectAction;
    private InputAction clickAction;

    public event Action Click;

    public Vector2 Selected => selectAction.ReadValue<Vector2>();

    protected override void Awake()
    {
        base.Awake();

        var playerActionMap = playerInput.FindActionMap("Player");
        selectAction = playerActionMap.FindAction("Position");
        clickAction = playerActionMap.FindAction("Click");

        clickAction.performed += OnClick;

        EnableInputs();
    }
    
    protected override void OnPlaying()
    {
        // Enable Inputs on Start Game
        EnableInputs();
    }
    protected override void OnCaught()
    {
        // Disable Inputs on Caught
        DisableInputs();
    }

    protected override void OnGameOver()
    {
        DisableInputs();
    }
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        clickAction.performed -= OnClick;
        DisableInputs();
    }

    private void EnableInputs()
    {
        selectAction.Enable();
        clickAction.Enable();
    }

    private void DisableInputs()
    {
        selectAction.Disable();
        clickAction.Disable();
    }
    
    public void OnClick(InputAction.CallbackContext obj) => Click?.Invoke();
}
