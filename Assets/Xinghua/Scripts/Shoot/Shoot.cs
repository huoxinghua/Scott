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
    private void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
        playerAnimator = GetComponentInChildren<Animator>();
        gun = GetComponentInChildren<Gun>();

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
        if (gun != null)
        {
            gun.OnReload += GunReload;
        }
    }

    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.OnShootStarted -= HandleShootStartedInput;
            inputManager.OnShootCanceled -= HandleShootCanceledInput;

            inputManager.OnChangeWeaponInput -= ChangeWeapon;
            inputManager.OnGunReloadInput += GunReload;
        }
        else
        {
            Debug.Log("input manager is null ");
        }
        if (gun != null)
        {
            gun.OnReload -= GunReload;
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
        if (!gun.CheckFullAmmo())
        {
            playerAnimator.SetBool("isReload", true);
            gun.Reload();
        }
    }
    private void HandleShoot(bool isAuto)
    {
        Gun gun = GetComponentInChildren<Gun>(false);
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
        if (gun.CheckEmptyAmmo()) return;
       
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
        Gun gun = GetComponentInChildren<Gun>(false);
        var type = gun.gunData.type;
        if (gun != null && type == GunType.Automatic)
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
                yield return new WaitForSeconds(shootInterval);
            }
            
        }
        else
        {
            HandleShoot(false);
            yield return new WaitForSeconds(shootInterval); // this is for single shoot
        }

    }
}
