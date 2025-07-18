using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    private PlayerInputManager inputManager;
    public float shootInterval = 5f;
    
    private Coroutine gunShakeCoroutine;
    private Coroutine continuousShootingCoroutine;
    private void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
    }
    private void OnEnable()
    {
        if (inputManager != null)
        {

            inputManager.OnShootStarted += HandleShootStartedInput;
            inputManager.OnShootCanceled += HandleShootCanceledInput;

           // inputManager.OnChangeWeaponInput += ChangeWeapon;
        }
        else
        {
            Debug.Log("input manager is null ");
        }
    }
    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.OnShootStarted -= HandleShootStartedInput;
            inputManager.OnShootCanceled -= HandleShootCanceledInput;

           // inputManager.OnChangeWeaponInput += ChangeWeapon;
        }
        else
        {
            Debug.Log("input manager is null ");
        }
    }
    Weapon weapon;
    private void ChangeWeapon()
    {
         weapon = GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
            weapon.EquipWeapon();
        }
        else
        {
            Debug.Log("weapon is null");
        }
    }

    private void HandleShoot()
    {
        Gun gun = GetComponentInChildren<Gun>();
        if (gun != null)
        {
            gun.Shoot();
        }
        else
        {
            Debug.Log("gun is null");
        }

    }

    private void HandleShootStartedInput()
    {
        isAutoShooting = false;
        if (continuousShootingCoroutine != null)
        {
            StopCoroutine(continuousShootingCoroutine); 
        }
        continuousShootingCoroutine = StartCoroutine(ContinuousShootingRoutine());
    }

    private void HandleShootCanceledInput()
    {
      
        if (continuousShootingCoroutine != null)
        {
            StopCoroutine(continuousShootingCoroutine);
            continuousShootingCoroutine = null; 
        }
        isAutoShooting = false;
    }
    public bool isAutoShooting = false;
    private IEnumerator ContinuousShootingRoutine()
    {

        isAutoShooting = true;
        while (true)
        {

            HandleShoot();
            CameraShake camShake = Camera.main.GetComponentInParent<CameraShake>();
            camShake.Shake();
            yield return new WaitForSeconds(shootInterval);
        }

        /*HandleShoot();
        yield return new WaitForSeconds(shootInterval); */// this is for single shoot

    }
}
