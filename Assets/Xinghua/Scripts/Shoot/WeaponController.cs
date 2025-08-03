using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    private Transform shootStartPoint;//fx
    [SerializeField] public WeaponSO[] weapons;
    public List<GameObject> guns = new List<GameObject>();
    //  private GameObject currentWeapon;
    // public Transform weaponContainer;
    public Gun currentGun;
    [SerializeField] private Gun startingGun;
    [SerializeField] Transform gunParent;
    // private int currentIndex = 0;
    // [SerializeField] public GameObject crossHair;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;


    private Vector3 idleScale;
    private Vector3 moveScale;
    private Image crosshairImage;


    private void Awake()
    {

        shootStartPoint = transform.GetChild(0);
    }
    private void Start()
    {
        currentGun = startingGun;
        // guns.Enqueue(currentGun.gameObject);
        spawnPosition = currentGun.transform.position;
        spawnRotation = currentGun.transform.rotation;
        //var another = Instantiate(weapons[1].gunPrefab, transform.position, Quaternion.identity);
        //  another.transform.SetParent(gunParent);

        /* another.gameObject.SetActive(false);

         another.transform.position = spawnPosition;
         another.transform.rotation = spawnRotation;

         guns.Enqueue(another);*/
    
        var crosshair = Instantiate(currentGun.gunData.crosshairCanves);
        crosshairImage = crosshair.GetComponentInChildren<Image>();
        Debug.Log("start gun have:" + guns.Count);
        /* idleScale = Vector3.one * currentGun.gunData.crosshairIdleScale;
         moveScale = new Vector3(2, 2, 2) * currentGun.gunData.crosshairMoveScale;*/
    }

    private void Update()
    {
        UpdateCrosshairColor();
        //UpdateCrosshairScale();
    }

    private void UpdateCrosshairColor()
    {

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider.GetComponent<IDamageable>() != null)
            {
                crosshairImage.color = currentGun.gunData.crosshairEnemyColor;

            }

            /*  else
              {
                  crosshairImage.color = currentGun.gunData.crosshairNormalColor;
              }*/
        }
    }
    public bool isShotGun = true;
    private void HandGunSwitchAnimation()
    {
        Animator playerAnim = GetComponent<Animator>();
   
        if (isShotGun == false)
        {
            playerAnim.SetBool("isShotgun", true);
            isShotGun = true;
        }
        else
        {
            playerAnim.SetBool("isShotgun", false);
            isShotGun = false;
        }
    }

    public Gun GetCurrentGun()
    {
        return currentGun;
    }
    public void EquipWeapon()
    {
        
        foreach (var gun in guns)
        {
            if (gun.gameObject.activeSelf == true)
            {
                gun.gameObject.SetActive(false);

            }
            else
            {
                 gun.SetActive(true);
                currentGun = gun.GetComponent<Gun>();
                HandGunSwitchAnimation();
            }
        }
    
        /*  if (guns.Count <= 1)
              return;
         Debug.Log("EquipWeapon"+guns.Count);
         foreach (var gun in guns)
         {
             Debug.Log("EquipWeapon:" + gun.name);
         }
          currentGun.gameObject.SetActive(false);
          guns.Enqueue(currentGun.gameObject);


          var next = guns.Dequeue();
          next.SetActive(true);
          currentGun = next.GetComponent<Gun>();*/
    }

    //animation
    public void OnReLoadFinish()
    {
        Debug.Log("OnReLoadFinish");
        PlayerMovement playerMovement = GetComponentInParent<PlayerMovement>();
        playerMovement.playerAnim.SetBool("isReload", false);
        playerMovement.gunAnim.SetBool("isReload", false);
        currentGun.isReload = false;
        Debug.Log("gun is reload?" + currentGun.isReload);
    }

}
