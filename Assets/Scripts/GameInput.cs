
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractHandler;
    public event EventHandler OnInteractAlternateHandler;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateHandler?.Invoke(this,EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractHandler?.Invoke(this,EventArgs.Empty);
    }

    public Vector2 GetPlayerMoveInput()
    {
        if (playerInputActions == null) { 
            return Vector2.zero;
        }
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
