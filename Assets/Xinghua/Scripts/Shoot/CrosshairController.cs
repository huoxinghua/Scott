using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class CrosshairController : MonoBehaviour
{
    private Gun gun;
    
    private GameObject crosshair;
    WeaponSO gunData;

    private Coroutine animCoroutine;
    private PlayerMovement player;
    private Transform originalRotation;
    private Transform targetRotation;
    private void Awake()
    {
        gun = GetComponent<Gun>();
        gunData = gun.gunData;
        player = FindAnyObjectByType<PlayerMovement>();

    }
  
    Vector3 targetScale;
/*    private void UpdateCrosshairScale()
    {
        Debug.Log(" UpdateCrosshairScale:" + idleScale);
        if (player.isMoving == true)
        {
            targetScale = moveScale;
        }
        else
        {
            targetScale = idleScale;
        }
        ChangeScale(targetScale);

    }*/
 /*   public void ChangeScale(Vector3 target)
    {
        crosshairImage.rectTransform.localScale = Vector3.Lerp(crosshairImage.rectTransform.localScale, target, Time.deltaTime * 10f);
    }*/

    public void PlayShootAnimation()
    {
        Animator animator = crosshair.GetComponentInChildren<Animator>();
        if (animator != null)
        {

            animator.SetTrigger("isRotate");
            //targetScale = idleScale;
          //  ChangeScale(targetScale);
        }
        else
        {
            Debug.Log("animator = null");
        }
        
    }

}