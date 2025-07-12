using UnityEngine;

public class Shoot : MonoBehaviour
{
    private PlayerInputManager inputManager;
    private void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
    }
    private void OnEnable()
    {
        if (inputManager != null)
        {

            inputManager.OnShootInput += HandleShoot;
            inputManager.OnChangeWeaponInput += ChangeWeapon;
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
            inputManager.OnShootInput -= HandleShoot;
            inputManager.OnChangeWeaponInput += ChangeWeapon;
        }
        else
        {
            Debug.Log("input manager is null ");
        }
    }
    private void ChangeWeapon()
    {
        Weapon weapon = GetComponentInChildren<Weapon>();
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
}
