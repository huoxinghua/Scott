using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    public event Action<Vector2> OnMoveInput;
    public event Action OnJumpInput;
    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Move.performed += HandleMove;
        inputActions.Player.Move.canceled += HandleMove;
        inputActions.Player.Jump.performed += HandleJump;
        inputActions.Player.Jump.canceled += HandleJump;
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    private void HandleMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        OnMoveInput?.Invoke(moveInput);
    }
    private void HandleJump(InputAction.CallbackContext context)
    {
       if (context.performed)
        {
            OnJumpInput?.Invoke();
        }
    }

}

