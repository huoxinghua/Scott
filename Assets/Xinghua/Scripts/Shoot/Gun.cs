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
    // [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private LayerMask lm;
    private Animator gunAnimator;
    private Animator playerAnimator;
    // private CrosshairController crosshairController;

    public float spreadAmount = 0.02f;
    private int bulletsPerShot;
    private float damage;
    private int magzaineSize;
    public event Action OnShoot;
    public event Action OnReload;

    [Header("Ammo and Magazine")]
    public int currentAmmo;


    private void Awake()
    {
        //   crosshairController = GetComponent<CrosshairController>();
        gunAnimator = GetComponent<Animator>();
        playerAnimator = GetComponentInParent<Animator>();

    }
    private void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

    }
    private void SetOriginalData()
    {

        currentAmmo = gunData.maxMagazineSize;
        magzaineSize = gunData.maxMagazineSize;
        damage = this.gunData.damage;
        // Debug.Log(this.gunData.type + "start damage:" + damage+"so damage"+gunData.damage);
        bulletsPerShot = gunData.bulletPerShot;

    }
    private void OnEnable()
    {
        SetOriginalData();
        ApplyUpgradeBonuses();
    }
    private void ApplyUpgradeBonuses()
    {
        var upgrade = UpgradeManager.Instance;
        if (upgrade == null) return;

        SetGunUpgradeDamage(upgrade.GetBonus(BonusType.Damage));
        SetGunUpgradeMagazine(upgrade.GetBonus(BonusType.Magazine));
        SetGunUpgradeFireRate(upgrade.GetBonus(BonusType.FireRate));
        SetGunUpgradeSpreadAmount(upgrade.GetBonus(BonusType.Spread));
        SetGunUpgradeRecoil(upgrade.GetBonus(BonusType.Recoil));
        SetGunUpgradeReloadSpeed(upgrade.GetBonus(BonusType.ReloadSpeed));
        SetGunUpgradeShotsPerShoot(upgrade.GetBonus(BonusType.ShotsPerShoot));
    }


    private void StartGunShake()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(GunShakeOnce());

    }
    public void Shoot()
    {
        switch (gunData.type)
        {
            case GunType.Automatic:
                AutomaticShoot();
                break;
            case GunType.SpreadShot:
                FireMultiRayShot();
                break;
        }
    }
    public void AutomaticShoot()
    {
        // Debug.Log("AutomaticShoot");
        if (isReload || currentAmmo <= 0) return;
        Debug.Log("gun shake");
        // StartGunShake();
       // gunAnimator.SetBool("Automatic", true);
        // playerAnimator.SetBool("Automatic", true);
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
                if (currentAmmo <= 0)
                {
                    gunAnimator.SetBool("Automatic", false);
                    playerAnimator.SetBool("Automatic", false);
                }

                //  crosshairController.PlayShootAnimation();

               
                lastShootTime = Time.time;
            }


            var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            var ragDollable = hit.collider.gameObject.GetComponent<IRagDollable>();
            if (damageable != null)
            {
                ragDollable.DamagePos(hit.transform);
                var bloodFX = Instantiate(gunData.bloodPrefab, hit.transform.position, rotation);
                Debug.Log("play blood fx:"+ bloodFX.name);
                Destroy(bloodFX, 0.5f);
                damageable.TakeDamage(damage);
                //Debug.Log(gunData.name + "gun damage apply:" + gunData.damage);
            }
        }

    }
    public bool isReload = false;
    public bool isShoot = false;
    public bool CheckFullAmmo()
    {
        if (currentAmmo == magzaineSize)
        {

            return true;

        }
        return false;
    }
    public bool CheckEmptyAmmo()
    {
        if (currentAmmo <= 0)
        {

            return true;

        }
        return false;
    }
    public void Reload()
    {
        if (isShoot)return;
        Debug.Log(this.name + "reload ");
        isReload = true;
        //  ReloadAnimation();
        currentAmmo = magzaineSize;
        Debug.Log("after reload ammo:"+currentAmmo);
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
   /* private void ReloadAnimation()
    {
        Debug.Log("handle ReloadAnimation");
        if (gunAnimator != null)
        {
            gunAnimator.SetBool("isReload", true);
            isReload = true;
        }
        else
        {
            Debug.Log("gun anim null");
        }
    }*/

    public void FireMultiRayShot()
    {
        Debug.Log("FireMultiRayShot");

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
                currentAmmo-= bulletsPerShot;
                isShoot = true;
                if (hit.collider.GetComponent<IDamageable>() == null)
                {

                    var hole = GameObject.Instantiate(gunData.holeFX, hitPos, rotation);
                    gunAnimator.SetBool("isShoot", true);
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



    public void SetGunUpgradeDamage(float bonus)
    {
        //Debug.Log(this.gunData.type+" :gun before damage:" + damage +"bones"+bonus);
        damage = damage * (1 + bonus);
        //Debug.Log(this.gunData.type + ":gun after damage:" + damage);
    }

    public void SetGunUpgradeMagazine(float bonus)
    {
        magzaineSize = (int)(magzaineSize * (1 + bonus));
    }

    public void SetGunUpgradeFireRate(float bonus)
    {

    }
    public void SetGunUpgradeSpreadAmount(float bonus)
    {
        spreadAmount = spreadAmount * (1 + bonus);
    }
    public void SetGunUpgradeRecoil(float bonus)
    {

    }
    public void SetGunUpgradeReloadSpeed(float bonus)
    {

    }
    public void SetGunUpgradeShotsPerShoot(float bonus)
    {
        bulletsPerShot = (int)(bulletsPerShot * (1 + bonus));
    }

    public void OnReloadFinish()
    {
        Debug.Log("reload finish");
        gunAnimator.SetBool("isReload", false);
        currentAmmo = magzaineSize;
    }
    public void OnShootFinish()
    {
        gunAnimator.SetBool("isShoot", false);
        isShoot = false;
    }

}

