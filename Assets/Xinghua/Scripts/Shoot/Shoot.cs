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
        if (!weapon.isSwitchingGun)
        {

            weapon.isSwitchingGun = true;
            if (weapon != null)
            {
                weapon.EquipWeapon();
            }
            else
            {
                Debug.Log("weapon is null");
            }
        }
    }
    public void UpGradeReloadAnimationSpeed(Animator gunAnimator, Gun currentGun)
    {
        var bonusSpeed = currentGun.GetReloadSpeed();
        gunAnimator.SetFloat("reloadSpeed", bonusSpeed);
        playerAnimator.SetFloat("reloadSpeed", bonusSpeed);

    }
    public float reloadSpeed = 1f;
    private void GunReload()
    {
        currentGun = weaponController.currentGun;

        if (currentGun.currentState != GunState.Idle || isAutoShooting)
        {
           // Debug.Log("currentGun GunState: " + currentGun.currentState + "isAutoShoot:" + isAutoShooting);
            return;
        }

       

        if (!currentGun.CheckFullAmmo())
        {
            Animator gunAnimator = currentGun.GetComponent<Animator>();
            //upgrade animation speed here
            UpGradeReloadAnimationSpeed(gunAnimator, currentGun);
            playerAnimator.SetBool("isReload", true);
            gunAnimator.SetBool("isReload", true);
          
   
            currentGun.Reload();
        }
        else
        {
            Debug.Log("full ammo "+currentGun.currentAmmo);
        }
    }

    
    private void HandleShoot( )
    {
        var currentGun = weaponController.GetCurrentGun();
        Animator gunAnimator = currentGun.GetComponent<Animator>();
        if (currentGun != null && !currentGun.CheckEmptyAmmo())
        {
            if (gunAnimator != null)
            {
                gunAnimator.SetBool("isShoot", true);
            }
            currentGun.Shoot();

            playerAnimator.SetBool("Automatic", true);
        }
    }

    private void HandleShootStartedInput()
    {
        var currentGun = weaponController.GetCurrentGun();

        if (currentGun.CheckEmptyAmmo())
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
      
        isAutoShooting = false;
        //animation
        playerAnimator.SetBool("Automatic", false);
        Animator gunAnimator = gun.gameObject.GetComponent<Animator>();
        GetComponent<Animator>();
        gunAnimator.SetBool("isShoot", false);

        if (continuousShootingCoroutine != null)
        {
            StopCoroutine(continuousShootingCoroutine);
            continuousShootingCoroutine = null;
        }
        isAutoShooting = false;

        currentGun.shoot = 0;
        currentGun.OnShootFinish();
    }
    public bool isAutoShooting = false;
    private IEnumerator ContinuousShootingRoutine()
    {
        var currentGun = weaponController.GetCurrentGun();
        var type = currentGun.gunData.type;
        if (currentGun != null && type == GunType.Automatic)
        {

            playerAnimator.SetBool("Automatic", true);

            isAutoShooting = true;
            while (true)
            {
                if (currentGun.CheckEmptyAmmo())
                {
                    playerAnimator.SetBool("Automatic", false);
                    break;
                }

                HandleShoot();
                yield return new WaitForSeconds(currentGun.shootCooldown);
            }

        }
        else
        {
            HandleShoot();
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
        playerAnimator.SetFloat("isReload", 1f);
        gunAnimator.SetFloat("isReload", 1f);
        Debug.Log("player animation speed:" + playerAnimator.speed);

    }
 

}
