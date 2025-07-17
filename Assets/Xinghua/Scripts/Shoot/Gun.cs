using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class Gun : MonoBehaviour
{
    private int shoot = 0;
    private float lastShootTime = 0f;
    public WeaponSO gunData;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Coroutine shakeCoroutine;
    private ParticleSystem muzzleFlash; 
    [SerializeField] private Vector3 shakeRotationAmount = new Vector3(2f, 2f, 1f);
    [SerializeField] private float shakePositionAmount = 0.05f;
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private LayerMask lm;
     private CrosshairController crosshairController;

    public float spreadAmount = 0.02f;
    private void Awake()
    {
        crosshairController = GetComponent<CrosshairController>();
    }
    private void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }
    private void StartGunShake()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(GunShakeOnce());
    }

    public void Shoot()
    {
        Debug.Log("Shoot");
        StartGunShake();
        float offsetX = Random.Range(-spreadAmount, spreadAmount);
        float offsetY = Random.Range(-spreadAmount, spreadAmount);
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f+ offsetX, 0.5f +offsetY, 0));
 
        RaycastHit hit;

     
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
        {
            muzzleFlash.Play();
            Debug.Log("shoot effect: " + muzzleFlash);
        }
        
     
        // Debug.DrawRay(ray.origin, ray.direction * gunData.range, Color.red, 1.0f);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity,~lm))
        {
            Vector3 offsetPos = hit.point + hit.normal * 0.001f;
            Quaternion rotation = Quaternion.LookRotation(hit.normal);
            rotation *= Quaternion.Euler(0f, 180f, 0f);
            Camera.main.GetComponent<CameraShake>().Shake();
            if (hit.collider.GetComponent<IDamageable>() == null && !TooCloseToOtherHoles(offsetPos))
            {
                var objHole = Instantiate(gunData.holeFX, offsetPos, rotation);
                objHole.transform.SetParent(hit.collider.transform);
                objHole.tag = "BulletHole";
                Destroy(objHole, 5f);
            }
            // Debug.Log("Hit " + hit.collider.name + shoot + "times");

            if (Time.time - lastShootTime > gunData.shootCooldown)
            {
            
                shoot++;
               // crosshairController.PlayShootAnimation();
              

           

               // var objFX = Instantiate(gunData.cube, offsetPos, rotation);
            
              //  Destroy(objFX, 0.5f);
              //  Debug.Log("Hit " + hit.collider.name + shoot + "times");
                lastShootTime = Time.time;
            }
         

            var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(gunData.damage);
                Debug.Log(gunData.name + "gun damage apply:" + gunData.damage);
            }
        }

    }
    bool TooCloseToOtherHoles(Vector3 pos)
    {
        Collider[] nearby = Physics.OverlapSphere(pos, 0.05f);
        foreach (var c in nearby)
        {
            if (c.CompareTag("BulletHole"))
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator GunShakeOnce()
    {
        float shakeStrength = 0.01f;
        transform.localPosition = originalPosition + Random.insideUnitSphere * shakeStrength;
        yield return new WaitForSeconds(0.05f);
        transform.localPosition = originalPosition;
    }
    private IEnumerator GunShake()
    {

        float shakeTime = 0.1f;
        float elapsed = 0f;
        Vector3 upwardShakeDirection = Vector3.up;
        while (elapsed < shakeTime)
        {
            elapsed += Time.deltaTime;

          //  transform.localPosition = originalPosition + upwardShakeDirection * shakePositionAmount;
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakePositionAmount;
            yield return null;
        }


        transform.localPosition = originalPosition;

    }
    public void OnShootSoundPlay()
    {
         SoundManager.Instance.PlaySFX("BaseGunShoot", 1f);
    }
}

