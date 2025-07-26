using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    private Transform shootStartPoint;//fx
    [SerializeField] public WeaponSO[] weapons;
    private Queue<GameObject> guns = new Queue<GameObject>();
  //  private GameObject currentWeapon;
   // public Transform weaponContainer;
    public Gun currentGun;
   // private int currentIndex = 0;
   // [SerializeField] public GameObject crossHair;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;


    private Vector3 idleScale;
    private Vector3 moveScale;
    private Image crosshairImage;
    private void Awake()
    {
        
        // shootStartPoint = transform.GetChild(0);
    }
    private void Start()
    {
        currentGun = GetComponentInChildren<Gun>();
        spawnPosition = currentGun.transform.position;
        spawnRotation = currentGun.transform.rotation;
        var another = Instantiate(weapons[1].gunPrefab, transform.position, Quaternion.identity);
        another.gameObject.SetActive(false);
        another.transform.SetParent(transform);
        another.transform.position = spawnPosition;
        another.transform.rotation = spawnRotation;

        guns.Enqueue(another);
       
        var crosshair = Instantiate(currentGun.gunData.crosshairCanves);
        crosshairImage = crosshair.GetComponentInChildren<Image>();

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

            else
            {
                crosshairImage.color = currentGun.gunData.crosshairNormalColor;
            }
        }
    }
    public void EquipWeapon()
    {
        if (currentGun != null)
        {
            currentGun.gameObject.SetActive(false);

            if (!guns.Contains(currentGun.gameObject))
            {
                guns.Enqueue(currentGun.gameObject);
            }
        }
        var newWeapon = guns.Dequeue();
        newWeapon.SetActive(true);
        currentGun = newWeapon.GetComponent<Gun>();
    }

    //animation
    public void OnReLoadFinish()
    {
        Debug.Log("OnReLoadFinish");
        PlayerMovement playerMovement = GetComponentInParent<PlayerMovement>();
        playerMovement.playerAnim.SetBool("isReload",false);
        playerMovement.gunAnim.SetBool("isReload", false);
    }

}
