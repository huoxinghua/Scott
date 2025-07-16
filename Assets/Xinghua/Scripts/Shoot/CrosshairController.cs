using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class CrosshairController : MonoBehaviour
{
    private Gun gun;
    private Image crosshairImage;
    private GameObject crosshair;
    WeaponSO gunData;

    private Vector3 idleScale;
    private Vector3 moveScale;
    private Coroutine animCoroutine;
    private PlayerMovement player;
    private Transform originalRotation;
    private Transform targetRotation;
    private void Awake()
    {
        gun = GetComponent<Gun>();
        gunData = gun.gunData;

        crosshair = Instantiate(gunData.crosshairCanves);
        crosshairImage = crosshair.GetComponentInChildren<Image>();
      //  crosshairImage.rectTransform.sizeDelta = new Vector2(100f, 100f);

        player = FindAnyObjectByType<PlayerMovement>();

    }

    private void Start()
    {
        idleScale = Vector3.one * gunData.crosshairIdleScale;
        moveScale = new Vector3(2, 2, 2) * gunData.crosshairMoveScale;

        crosshairImage.color = gunData.crosshairNormalColor;

        crosshairImage.rectTransform.localScale = idleScale;
        if (crosshairImage != null)
        {
            gunData.crosshairCanves.SetActive(true);
        }
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
                crosshairImage.color = gunData.crosshairEnemyColor;
                Debug.Log(" crosshairImage.color:" + crosshairImage.color);
            }

            else
            {
                crosshairImage.color = gunData.crosshairNormalColor;
            }
        }
    }
    Vector3 targetScale;
    private void UpdateCrosshairScale()
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


    }
    public void ChangeScale(Vector3 target)
    {
        crosshairImage.rectTransform.localScale = Vector3.Lerp(crosshairImage.rectTransform.localScale, target, Time.deltaTime * 10f);
    }

    public void PlayShootAnimation()
    {
        Animator animator = crosshair.GetComponentInChildren<Animator>();
        if (animator != null)
        {

            animator.SetTrigger("isRotate");
            targetScale = idleScale;
            ChangeScale(targetScale);
        }
        else
        {
            Debug.Log("animator = null");
        }
        
    }

}