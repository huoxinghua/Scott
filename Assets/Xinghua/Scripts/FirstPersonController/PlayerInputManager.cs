using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public InputSystem_Actions inputActions;
    public event Action<Vector2> OnMoveInput;
    public event Action OnSprintInputStart;
    public event Action OnSprintInputCancel;
    public event Action OnJumpInput;
    public event Action<Vector2> OnLookInput;
    public event Action OnShootStarted;
    public event Action OnShootCanceled;
    public event Action OnChangeWeaponInput;

    public event Action OnGunReloadInput;
    public event Action OnAimInputStart;
    public event Action OnAimInputCancle;
    public event Action OnInteractInput;
    //temp
    public event Action OnUpgradeInput;
    public Vector2 LookInput { get; private set; }
    PlayerUpgrade playerUpgrade;
    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        playerUpgrade = GetComponent<PlayerUpgrade>();
    }
    public void OnUIOpen()
    {
        inputActions.Player.Disable();
        inputActions.UI.Enable();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnUIClose()
    {
        inputActions.UI.Disable();
        inputActions.Player.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        inputActions.Enable();
       
        inputActions.Player.Move.performed += HandleMove;
        inputActions.Player.Move.canceled += HandleMove;
        inputActions.Player.Jump.performed += HandleJump;
        inputActions.Player.Jump.canceled += HandleJump;
        inputActions.Player.Look.performed += HandleLook;
        inputActions.Player.Look.canceled += HandleLook;

        inputActions.Player.Sprint.started += HandleSprintStart;
        inputActions.Player.Sprint.canceled += HandleSprintCancel;

        inputActions.Player.Attack.started += HandleShootStarted;
        inputActions.Player.Attack.canceled += HandleShootCanceled;
        inputActions.Player.ChangeWeapon.performed += HandleChangeWeapon;
        inputActions.Player.ChangeWeapon.canceled += HandleChangeWeapon;
        inputActions.Player.GunReload.performed += HandleGunReload;
        inputActions.Player.GunReload.canceled += HandleGunReload;

        inputActions.Player.Aim.started += HandleAimStart;
        inputActions.Player.Aim.canceled += HandleAimCancle;
        inputActions.Player.Interact.performed += Interact;
        inputActions.Player.Interact.canceled += Interact;
        //test input temp
        inputActions.Player.Upgrade.performed += Upgrade;
        inputActions.Player.Upgrade.canceled += Upgrade;

        if (playerUpgrade != null)
        {
            playerUpgrade.OnUIInput += OnUIOpen;
        }
           
     
    }

    private void Upgrade(InputAction.CallbackContext context)
    {
        OnUpgradeInput?.Invoke();    
    }

    private void OnDisable()
    {
        inputActions.Disable();

        inputActions.Player.Move.performed -= HandleMove;
        inputActions.Player.Move.canceled -= HandleMove;
        inputActions.Player.Jump.performed -= HandleJump;
        inputActions.Player.Jump.canceled -= HandleJump;
        inputActions.Player.Look.performed -= HandleLook;
        inputActions.Player.Look.canceled -= HandleLook;

        inputActions.Player.Attack.started -= HandleShootStarted;
        inputActions.Player.Attack.canceled -= HandleShootCanceled;

        inputActions.Player.ChangeWeapon.performed -= HandleChangeWeapon;
        inputActions.Player.ChangeWeapon.canceled -= HandleChangeWeapon;
        inputActions.Player.GunReload.performed -= HandleGunReload;
        inputActions.Player.GunReload.canceled -= HandleGunReload;

        inputActions.Player.Aim.started -= HandleAimStart;
        inputActions.Player.Aim.canceled -= HandleAimCancle;
        inputActions.Player.Interact.performed -= Interact;
        inputActions.Player.Interact.canceled -= Interact;
        //test input
        inputActions.Player.Upgrade.performed += Upgrade;
        inputActions.Player.Upgrade.canceled += Upgrade;

        if (playerUpgrade != null)
        {
            playerUpgrade.OnUIInput -= OnUIOpen;
        } 
     
    }
    
    private void HandleGunReload(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("OnGunReloadInput");
            OnGunReloadInput?.Invoke();
        }
    }
    private void HandleAimStart(InputAction.CallbackContext context)
    {
        OnAimInputStart?.Invoke();
    }
    private void HandleAimCancle(InputAction.CallbackContext context)
    {

        OnAimInputCancle?.Invoke();
    }
    Vector2 moveInput;
    private void HandleMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        OnMoveInput?.Invoke(moveInput);
    }
    private void HandleSprintStart(InputAction.CallbackContext context)
    {
        OnSprintInputStart?.Invoke();
    }
    private void HandleSprintCancel(InputAction.CallbackContext context)
    {
        OnSprintInputCancel?.Invoke();
    }

    private void HandleJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnJumpInput?.Invoke();
        }
    }
    private void HandleLook(InputAction.CallbackContext context)
    {

        OnLookInput?.Invoke(context.ReadValue<Vector2>());
    }

    private void HandleShootStarted(InputAction.CallbackContext context)
    {
   /*     if (EventSystem.current.IsPointerOverGameObject())
        {
            OnUIOpen();
            return;
        }*/
           
        OnShootStarted?.Invoke();
    }

    private void HandleShootCanceled(InputAction.CallbackContext context)
    {
        OnShootCanceled?.Invoke();
    }
    private void HandleChangeWeapon(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            OnChangeWeaponInput?.Invoke();
        }
    }
    private void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("get interact input");
        OnInteractInput?.Invoke();
    }


}

