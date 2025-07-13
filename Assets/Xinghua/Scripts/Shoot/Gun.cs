using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    private int shoot = 0;
    private float lastShootTime = 0f;
    [SerializeField] private WeaponSO gunData;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Coroutine shakeCoroutine;

    [SerializeField] private Vector3 shakeRotationAmount = new Vector3(2f, 2f, 1f);
    [SerializeField] private float shakePositionAmount = 0.05f;
    [SerializeField] private float shakeDuration = 0.1f;

    public float spreadAmount = 0.02f;

    private void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }
    private void StartGunShake()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(GunShake());
    }
    public void Shoot()
    {
        float offsetX = Random.Range(-spreadAmount, spreadAmount);
        float offsetY = Random.Range(-spreadAmount, spreadAmount);
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f+ offsetX, 0.5f +offsetY, 0));

 
        RaycastHit hit;
       
           
        
       
        Debug.DrawRay(ray.origin, ray.direction * gunData.range, Color.red, 1.0f);
        if (Physics.Raycast(ray, out hit, gunData.range))
        {
            // Debug.Log("Hit " + hit.collider.name + shoot + "times");
       /*     Weapon weaponManager = GetComponentInParent<Weapon>();
            if (hit.collider.GetComponent<IDamageable>() != null)
            {
                weaponManager.hairCross.SetActive(true);
            }
            else
            {
                weaponManager.hairCross.SetActive(false);
            }*/
            if (Time.time - lastShootTime > gunData.shootCooldown)
            {
            
                shoot++;
               
                var objFX = Instantiate(gunData.cube, hit.point, Quaternion.identity);
                SoundManager.Instance.PlaySFX("BaseGunShoot", 1f);
                Destroy(objFX, 0.5f);
              //  Debug.Log("Hit " + hit.collider.name + shoot + "times");
                lastShootTime = Time.time;
                StartGunShake();
                CameraShake camShake = Camera.main.GetComponentInParent<CameraShake>();
                camShake.isShake = true;
            }

            var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
              
                damageable.TakeDamage(gunData.damage);
                Debug.Log(gunData.name + "gun damage apply:" + gunData.damage);
            }
        }

    }
    private IEnumerator GunShake()
    {

        float shakeTime = 0.1f;
        float elapsed = 0f;

        while (elapsed < shakeTime)
        {
            elapsed += Time.deltaTime;


            transform.localPosition = originalPosition + Random.insideUnitSphere * shakePositionAmount;
            transform.localRotation = originalRotation * Quaternion.Euler(
                Random.Range(-shakeRotationAmount.x, shakeRotationAmount.x),
                Random.Range(-shakeRotationAmount.y, shakeRotationAmount.y),
                Random.Range(-shakeRotationAmount.z, shakeRotationAmount.z)
            );
            yield return null;
        }


        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
    }
}

