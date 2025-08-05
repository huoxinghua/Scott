using System.Collections;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private PlayerInputManager inputManager;
    public float shootInterval = 5f;

    private Coroutine gunShakeCoroutine;
    private Coroutine continuousShootingCoroutine;
    private Animator playerAnimator;
    private Gun gun;
    private Gun currentGun;
    private WeaponController weaponController;
    private void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
        playerAnimator = GetComponentInChildren<Animator>();
        weaponController = GetComponentInChildren<WeaponController>();
        gun = GetComponentInChildren<Gun>();
        currentGun = gun;
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
    public void UpGradeReloadAnimationSpeed(Animator gunAnimator, Gun currentGun)
    {
        var bonusSpeed = currentGun.GetReloadSpeed();
        gunAnimator.speed = bonusSpeed;
        playerAnimator.speed = bonusSpeed;
    }
    public float reloadSpeed = 1f;
    private void GunReload()
    {
        currentGun = weaponController.currentGun;
       // Debug.Log("currentGun in gunRoload :" + currentGun.currentAmmo);
        Animator gunAnimator = currentGun.GetComponent<Animator>();
        // Debug.Log(gunAnimator.name + " :" + "reload");
        if (!currentGun.CheckFullAmmo())
        {
            //upgrade animation speed here
            UpGradeReloadAnimationSpeed(gunAnimator,currentGun);
            playerAnimator.SetBool("isReload", true);
            gunAnimator.SetBool("isReload", true);

            gun.Reload();
        }
        else
        {
            Debug.Log("full ammo");
        }
    }
    private void HandleShoot(bool isAuto)
    {
        var currentGun = weaponController.GetCurrentGun();
        Animator gunAnimator = currentGun.GetComponent<Animator>();
        if (currentGun != null && !currentGun.CheckEmptyAmmo())
        {

            // Debug.Log("gunAnimation in Handle shoot:" + gunAnimator.name);
            if (gunAnimator != null)
            {
                gunAnimator.SetBool("Automatic", true);
            }
            // Debug.Log("Handle shoot :" + isAuto);
            currentGun.Shoot();
            playerAnimator.SetBool("Automatic", true);
        }
    }

    private void HandleShootStartedInput()
    {
        if (gun.CheckEmptyAmmo())
        {
            Debug.Log("current ammo empty need reload");
            return;
        }

        isAutoShooting = false;
        if (continuousShootingCoroutine != null)
        {
            StopCoroutine(continuousShootingCoroutine);
        }
        continuousShootingCoroutine = StartCoroutine(ContinuousShootingRoutine());
    }

    private void HandleShootCanceledInput()
    {
        //animation
        playerAnimator.SetBool("Automatic", false);
        Animator gunAnimator = gun.gameObject.GetComponent<Animator>();
        GetComponent<Animator>();
        gunAnimator.SetBool("Automatic", false);

        if (continuousShootingCoroutine != null)
        {
            StopCoroutine(continuousShootingCoroutine);
            continuousShootingCoroutine = null;
        }
        isAutoShooting = false;

        gun.shoot = 0;
    }
    public bool isAutoShooting = false;
    private IEnumerator ContinuousShootingRoutine()
    {
        var currentGun = weaponController.GetCurrentGun();
        var type = currentGun.gunData.type;
        if (currentGun != null && type == GunType.Automatic)
        {

            playerAnimator.SetBool("Automatic", true);

            //isAutoShooting = true;
            while (true)
            {
                if (gun.CheckEmptyAmmo())
                {
                    playerAnimator.SetBool("Automatic", false);
                    break;
                }

                HandleShoot(true);
                yield return new WaitForSeconds(currentGun.shootCooldown);
            }

        }
        else
        {
            HandleShoot(false);
            yield return new WaitForSeconds(currentGun.shootCooldown); // this is for single shoot
        }

    }
    //player !!!!animation event
    public void OnLoadFinish()
    {
        playerAnimator.SetBool("isReload", false);
        ResetAnimationSpeed();
    }
    public void ResetAnimationSpeed()
    {
        currentGun = weaponController.currentGun;
        Animator gunAnimator = currentGun.GetComponent<Animator>();
        gunAnimator.speed = 1f;
        playerAnimator.speed = 1f;
    }

}
