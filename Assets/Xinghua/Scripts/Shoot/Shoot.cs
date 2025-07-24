using System.Collections;
using UnityEngine;

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

            inputManager.OnChangeWeaponInput += ChangeWeapon;
            inputManager.OnGunReloadInput += GunReload;
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

            inputManager.OnChangeWeaponInput -= ChangeWeapon;
            inputManager.OnGunReloadInput -= GunReload;
        }
        else
        {
            Debug.Log("input manager is null ");
        }
    }
    WeaponController weapon;
    private void ChangeWeapon()
    {
        weapon = GetComponentInChildren<WeaponController>();
        if (weapon != null)
        {
            weapon.EquipWeapon();
        }
        else
        {
            Debug.Log("weapon is null");
        }
    }
    private void GunReload()
    {
        Gun gun = GetComponentInChildren<Gun>();
        gun.Reload();
    }
    private void HandleShoot(bool isAuto)
    {
        Gun gun = GetComponentInChildren<Gun>();
        if (gun != null && isAuto == true)
        {
            gun.Shoot();
        }
        else if (gun != null && isAuto == false)
        {
            gun.FireMultiRayShot();
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
        Gun gun = GetComponentInChildren<Gun>(false);
        gun.shoot = 0;
    }
    public bool isAutoShooting = false;
    private IEnumerator ContinuousShootingRoutine()
    {

        isAutoShooting = true;
        while (true)
        {

            HandleShoot(true);

            yield return new WaitForSeconds(shootInterval);
        }

        /*  HandleShoot(false);

          yield return new WaitForSeconds(shootInterval); // this is for single shoot*/

    }
}
