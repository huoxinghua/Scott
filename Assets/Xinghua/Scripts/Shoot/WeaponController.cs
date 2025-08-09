using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    private Transform shootStartPoint;//fx
    [SerializeField] public WeaponSO[] weapons;
    public List<GameObject> guns = new List<GameObject>();
    public Gun currentGun;

    [SerializeField] Transform gunParent;
    private Animator playerAnim;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    public bool isShotGun;
    private Vector3 idleScale;
    private Vector3 moveScale;
    private Image crosshairImage;

    public bool isSwitchingGun;
    [SerializeField] GameObject bulletCanves;
    [SerializeField] TMP_Text bulletTMP;
    private void Awake()
    {

        shootStartPoint = transform.GetChild(0);
        playerAnim = GetComponent<Animator>();
    }
    private void Start()
    {
        currentGun = GetComponentInChildren<AutoGun>();
        isShotGun = false;
     
        playerAnim.SetBool("isShotgun", false);

     /*   spawnPosition = currentGun.transform.position;
        spawnRotation = currentGun.transform.rotation;
*/

        var crosshair = Instantiate(currentGun.gunData.crosshairCanves);
        crosshairImage = crosshair.GetComponentInChildren<Image>();
        

    }

    private void Update()
    {
        UpdateCrosshairColor();
    }
    public void DisplayEmpty()
    {
        bulletTMP.text = 0 + "/" + currentGun.magzaineSize;
    }

    public void DisplayBullet(int value,int max)
    {
        // bulletTMP.text = currentGun.currentAmmo + "/" + currentGun.magzaineSize;
        bulletTMP.text = value + "/" + currentGun.magzaineSize;
    }
    public bool isCrossHairActive = false;
    private void UpdateCrosshairColor()
    {
        if (currentGun == null)
            return;
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




    public void OnSwitchEnd()//anim event
    {

            isSwitchingGun = false;

    }


    public void SwitchWeapon()
    {
      
        HandGunSwitchAnimation();
       
    }
    private void HandGunSwitchAnimation()
    {
        Debug.Log("isShotGun" + isShotGun);
        if (!isShotGun)
        {
         
            playerAnim.SetBool("isShotgun", true);
            isShotGun = true;
        }
        else
        {
           
            playerAnim.SetBool("isShotgun", false);
        }
    
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
        if(currentGun != null)
        {
            currentGun.currentState = GunState.Idle;
        }
        
    }
    private bool isARGun = false;
    public void OnARGunDown()
    {
        guns[0].gameObject.SetActive(false);
      
    
    }
    public void OnARGunUp()
    {
        guns[0].gameObject.SetActive(true);
        isSwitchingGun = false;
        isShotGun = false;
    }
    public void OnShotGunDown()
    {
        guns[1].gameObject.SetActive(false);
     
        
    }
    public void OnShotGunUp()
    {
        guns[1].gameObject.SetActive(true);
        isShotGun = true;
        isSwitchingGun = false;

    }
    //player movement sound
    public void OnPlayerStep1Sound()
    {
        if(SoundManager.Instance!= null)
        {
            Debug.Log("play walk sound");
            SoundManager.Instance.PlaySFX("Step1", 1f);
        }
    }
    public void OnPlayerStep2Sound()
    {
        if (SoundManager.Instance != null)
        {
            Debug.Log("play walk sound");
            SoundManager.Instance.PlaySFX("Step2", 1f);
        }
    }
}
