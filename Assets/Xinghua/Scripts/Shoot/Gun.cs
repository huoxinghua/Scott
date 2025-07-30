using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class Gun : MonoBehaviour
{
    public int shoot = 0;
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
    private Animator gunAnimator;
    private Animator playerAnimator;
    // private CrosshairController crosshairController;

    public float spreadAmount = 0.02f;
    private int bulletsPerShot = 5;
    private float damage = 10f;
    private int magzaineSize = 2;
    public event Action OnShoot;
    public event Action OnReload;

    [Header("Ammo and Magazine")]
    public int currentAmmo;

    // public int reserveAmmo = 30;
    //Animation event
  

    private void Awake()
    {
        //   crosshairController = GetComponent<CrosshairController>();
        gunAnimator = GetComponent<Animator>();
        playerAnimator = GetComponentInParent<Animator>();
        Debug.Log("start gun animation" + gunAnimator);
    }
    private void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        currentAmmo = gunData.maxMagazineSize;
        magzaineSize = gunData.maxMagazineSize;//for upgrade
        damage = gunData.damage;
        magzaineSize = gunData.maxMagazineSize;
    }
    private void StartGunShake()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(GunShakeOnce());

    }

    public void Shoot()
    {
        StartGunShake();
        float offsetX = 0f;
        float offsetY = 0f;

        if (shoot > 0)
        {
            offsetX = Random.Range(-spreadAmount, spreadAmount);
            offsetY = Random.Range(-spreadAmount, spreadAmount);
        }
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f + offsetX, 0.5f + offsetY, 0));

        RaycastHit hit;


        muzzleFlash = GetComponentInChildren<ParticleSystem>();
        {
            muzzleFlash.Play();
            //  Debug.Log("shoot effect: " + muzzleFlash);
        }


        // Debug.DrawRay(ray.origin, ray.direction * gunData.range, Color.red, 1.0f);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~lm))
        {
            // Vector3 offsetPos = hit.point + hit.normal * 0.001f;
            Vector3 offsetPos = hit.point;
            if (shoot > 1)
            {
                offsetPos += hit.normal * 0.001f;
            }
            Quaternion rotation = Quaternion.LookRotation(hit.normal);
            rotation *= Quaternion.Euler(0f, 180f, 0f);
            Camera.main.GetComponent<CameraShake>().Shake();
            if (hit.collider.GetComponent<IDamageable>() == null)
            {
                var objHole = Instantiate(gunData.holeFX, offsetPos, rotation);
                OnShoot?.Invoke();
                objHole.transform.SetParent(hit.collider.transform);
                objHole.tag = "BulletHole";
                Destroy(objHole, 5f);
            }
            // Debug.Log("Hit " + hit.collider.name + shoot + "times");

            if (Time.time - lastShootTime > gunData.shootCooldown)
            {
                shoot++;
                /* CameraShake camShake = Camera.main.GetComponentInParent<CameraShake>();
                 camShake.Shake();*/


                currentAmmo--;
                // crosshairController.PlayShootAnimation();




                // var objFX = Instantiate(gunData.cube, offsetPos, rotation);

                //  Destroy(objFX, 0.5f);
                //  Debug.Log("Hit " + hit.collider.name + shoot + "times");
                lastShootTime = Time.time;
            }


            var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            var ragDollable = hit.collider.gameObject.GetComponent<IRagDollable>();
            if (damageable != null)
            {
                ragDollable.DamagePos(hit.transform);
                damageable.TakeDamage(damage);
                //Debug.Log(gunData.name + "gun damage apply:" + gunData.damage);
            }
        }

    }
   
    public bool CheckAmmo()
    {
        if (currentAmmo == gunData.maxMagazineSize)
        {
             return true;
            
        }
        return false;
    }
    public void Reload()
    {
       
        ReloadAnimation();
        currentAmmo = gunData.maxMagazineSize;

        /*        int neededAmmo = gunData.maxMagazineSize - currentAmmo;

                if (reserveAmmo <= 0)
                {
                    Debug.Log("no ammo to reload");
                    return;
                }

                int ammoToLoad = Mathf.Min(neededAmmo, reserveAmmo);
                currentAmmo += ammoToLoad;
                reserveAmmo -= ammoToLoad;
                Debug.Log("reload finish：" + currentAmmo + "/" + reserveAmmo);*/
    }
    private void ReloadAnimation()
    {
       if(gunAnimator != null)
        {
            gunAnimator.SetBool("isReload", true);
        }
       else
        {
            Debug.Log("gun anim null");
        }

        
    }
    public void FireMultiRayShot()
    {

        for (int i = 0; i < bulletsPerShot; i++)
        {
            float offsetX = Random.Range(-gunData.spreadAmount, gunData.spreadAmount);
            float offsetY = Random.Range(-gunData.spreadAmount, gunData.spreadAmount);

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f + offsetX, 0.5f + offsetY, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~lm))
            {

                Vector3 hitPos = hit.point + hit.normal * 0.001f;
                Quaternion rotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(0f, 180f, 0f);


                CameraShake camShake = Camera.main.GetComponentInParent<CameraShake>();
                camShake.Shake();

                if (hit.collider.GetComponent<IDamageable>() == null)
                {
                    var hole = GameObject.Instantiate(gunData.holeFX, hitPos, rotation);
                    hole.transform.SetParent(hit.collider.transform);
                    GameObject.Destroy(hole, 5f);
                }

                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(gunData.damage);
                    Debug.Log(gunData.name + "gun damage apply:" + gunData.damage);
                }
            }
        }
    }

    /*    bool TooCloseToOtherHoles(Vector3 pos)
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
        }*/

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


    public void SetGunUpgradeBonus(float bonus)
    {
        float totalBonus = 0f;
        totalBonus += bonus;
    }
    private float bonusCount = 0f;
    public void SetGunUpgradeDamage(float bonus)
    {
        float totalBonus = 0f;
        totalBonus += bonus;
        damage = damage * (1+ totalBonus);
    }

    public void SetGunUpgradeMagazine(float bonus)
    {
        float totalBonus = 0f;
        totalBonus += bonus;
       var temp =magzaineSize * (1 + totalBonus);
        magzaineSize = (int)temp;
    }


    public void SetGunUpgradeFireRate(float bonus)
    {

    }
    public void SetGunUpgradeSpreadAmount(float bonus)
    {
        float totalBonus = 0f;
        totalBonus += bonus;
        var temp = spreadAmount * (1 + totalBonus);
        spreadAmount = (int)temp;
    }
    public void SetGunUpgradeRecoil(float bonus)
    {

    }
    public void SetGunUpgradeReloadSpeed(float bonus)
    {
        //animation
    }
    public void SetGunUpgradeShotsPerShoot(int bonus)
    {
        float totalBonus = 0f;
        totalBonus += bonus;
        var temp = bulletsPerShot * (1 + totalBonus);
        bulletsPerShot = (int)temp;
    }

    /* public void SetGunBonus(GunType type,  Dictionary<string,float> values)
     {

         foreach (var (key,value) in values)
         {
             if(value == 1)continue;

             Debug.Log("key:"+ key +"value"  + value);
         }

     }*/
}

