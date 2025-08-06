using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    private Transform shootStartPoint;//fx
    [SerializeField] public WeaponSO[] weapons;
    public List<GameObject> guns = new List<GameObject>();
    public Gun currentGun;
    [SerializeField] private Gun startingGun;
    [SerializeField] Transform gunParent;
    private Animator playerAnim;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    public bool isShotGun;
    private Vector3 idleScale;
    private Vector3 moveScale;
    private Image crosshairImage;

    private void Awake()
    {

        shootStartPoint = transform.GetChild(0);
        playerAnim = GetComponent<Animator>();
    }
    private void Start()
    {
        currentGun = startingGun;
        isShotGun = false;
        playerAnim.SetBool("isShotgun", false);

        spawnPosition = currentGun.transform.position;
        spawnRotation = currentGun.transform.rotation;


        var crosshair = Instantiate(currentGun.gunData.crosshairCanves);
        crosshairImage = crosshair.GetComponentInChildren<Image>();


    }

    private void Update()
    {
        UpdateCrosshairColor();

    }
    public bool isCrossHairActive = false;
    private void UpdateCrosshairColor()
    {

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider.GetComponent<IDamageable>() != null)
            {
                crosshairImage.color = currentGun.gunData.crosshairEnemyColor;
                isCrossHairActive = true;
            }

            else
            {
                crosshairImage.color = currentGun.gunData.crosshairNormalColor;
                isCrossHairActive = false;
            }
        }
    }

    private void HandGunSwitchAnimation()
    {
        Debug.Log("player animation :" + playerAnim);
        if (isShotGun == false)
        {
            isShotGun = true;
            playerAnim.SetBool("isShotgun", true);
            playerAnim.SetBool("isSwitch", true);
        }
        else
        {
            isShotGun = false;
            playerAnim.SetBool("isShotgun", false);
            playerAnim.SetBool("isSwitch", true);
        }
    }

    public Gun GetCurrentGun()
    {
        return currentGun = gameObject.GetComponentInChildren<Gun>(false);
    }
    public void OnSwitchEnd()//anim event
    {
        foreach (var weapon in guns)
        {
            Gun gun = weapon.GetComponent<Gun>();
            if (gun != null && gun.gunData.type == GunType.SpreadShot && isShotGun)
            {
                weapon.SetActive(true);

            }
            else if (gun != null && gun.gunData.type == GunType.Automatic && !isShotGun)
            {
                weapon.SetActive(true);

            }
            else
            {
                weapon.SetActive(false);
            }

        }
    }


    public void EquipWeapon()
    {
       // currentGun.currentState = GunState.Switching;
        HandGunSwitchAnimation();
        
    }

    //animation
    public void OnReLoadFinish()
    {
        PlayerMovement playerMovement = GetComponentInParent<PlayerMovement>();
        playerMovement.playerAnim.SetBool("isReload", false);
        playerMovement.gunAnim.SetBool("isReload", false);


    }
    public void OnPlayerShotGunReloadFinish()
    {
        playerAnim.SetBool("isReload", false);
    }
    public void OnPlayerGunSwitchEnd()
    {
        currentGun.currentState = GunState.Idle;
    }
}
