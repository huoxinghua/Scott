using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GunState
{
    Idle,
    Firing,
    Reloading,
    Switching
}

public class Gun : MonoBehaviour
{
    public GunState currentState;

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
    private WeaponController weaponController;
    public int currentAmmo;
    private int leftAmmo;
    //upgrade 
    public float spreadAmount;
    private int bulletsPerShot;
    private float damage;
    private int magzaineSize;
    public float shootCooldown;//it is little different to fire rate, but this easy to set in logic
    private float recoilAmount;
    private float reloadSpeed = 1f;
    //event
    public event Action OnShoot;

    private void Awake()
    {

        gunAnimator = GetComponent<Animator>();
        playerAnimator = GetComponentInParent<Animator>();
        weaponController = GetComponentInParent<WeaponController>();
    }
    private void Start()
    {
        currentState = GunState.Idle;
        currentAmmo = gunData.maxMagazineSize;
        leftAmmo = currentAmmo;
    }
    private void SetOriginalData()
    {

        Debug.Log(this.gunData.type + "currentAmmo when enable:" + currentAmmo);
        magzaineSize = gunData.maxMagazineSize;
        damage = this.gunData.damage;
        shootCooldown = this.gunData.shootCooldown;//this is not idea for upgrade 
        bulletsPerShot = gunData.bulletPerShot;
        spreadAmount = gunData.spreadAmount;
        recoilAmount = gunData.recoilAmount;
    }
    private void OnEnable()
    {
        currentState = GunState.Idle;
        // currentAmmo = leftAmmo;
        SetOriginalData();
        ApplyUpgradeBonuses();
    }
    private void OnDisable()
    {
        SaveLeftAmmoBeforeChangeGun();

    }

    private void SaveLeftAmmoBeforeChangeGun()//if player change to another weapon save
    {
        leftAmmo = currentAmmo;
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
        SetGunUpgradeBulletsPerShot(upgrade.GetBonus(BonusType.ShotsPerShoot));
    }


    private void StartGunShake()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(GunShakeOnce());

    }
    public void Shoot()
    {
        if (currentState != GunState.Idle || currentAmmo <= 0) return;
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
        currentState = GunState.Firing;
        // StartGunShake();

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
           // Camera.main.GetComponent<CameraShake>().Shake();
            FilterBulletHole(offsetPos,rotation,hit);


            // Debug.Log("Hit " + hit.collider.name + shoot + "times");

            if (Time.time - lastShootTime > gunData.shootCooldown)
            {
                shoot++;
                CameraShake camShake = Camera.main.GetComponentInParent<CameraShake>();
                camShake.Shake();


                currentAmmo--;
                if (currentAmmo <= 0)
                {
                    gunAnimator.SetBool("isShoot", false);
                    playerAnimator.SetBool("Automatic", false);
                }

                //  crosshairController.PlayShootAnimation();

               
                lastShootTime = Time.time;
            }

            HandleDamage(hit, rotation);
            OnShoot?.Invoke();

        }

    }
    private void FilterBulletHole(Vector3 offsetPos, Quaternion rotation,RaycastHit hit)
    {
        if (!weaponController.isCrossHairActive)
        {
            var objHole = Instantiate(gunData.holeFX, offsetPos, rotation);
          
            objHole.transform.SetParent(hit.collider.transform);
            objHole.tag = "BulletHole";
            Destroy(objHole, 1f);
        }
    }
 
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
        if (currentState != GunState.Idle) return;
        Debug.Log(this.name + "reload ");
        currentState = GunState.Reloading;
        currentAmmo = magzaineSize;
        Debug.Log("after reload ammo:"+currentAmmo);
    }


    public void FireMultiRayShot()
    {
        Debug.Log("FireMultiRayShot");
        currentState = GunState.Firing;
        currentAmmo -= bulletsPerShot;
        Debug.Log(this.gunData.type + "currentAmmo:" + currentAmmo);
        for (int i = 0; i < bulletsPerShot; i++)
        {
            float offsetX = Random.Range(-spreadAmount,spreadAmount);
            float offsetY = Random.Range(-spreadAmount,spreadAmount);

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f + offsetX, 0.5f + offsetY, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~lm))
            {

                Vector3 hitPos = hit.point + hit.normal * 0.001f;
                Quaternion rotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(0f, 180f, 0f);


                CameraShake camShake = Camera.main.GetComponentInParent<CameraShake>();
                camShake.Shake();
               
                
                CheckEmptyAmmo();
                currentState = GunState.Firing;
                if (hit.collider.GetComponent<IDamageable>() == null)
                {

                    var hole = GameObject.Instantiate(gunData.holeFX, hitPos, rotation);
                    gunAnimator.SetBool("isShoot", true);
                    hole.transform.SetParent(hit.collider.transform);
                    GameObject.Destroy(hole, 5f);
                }

                HandleDamage( hit, rotation);
            }
        }
    }
    private void HandleDamage(RaycastHit hit, Quaternion rotation)
    {
        var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
        var ragDollable = hit.collider.gameObject.GetComponent<IRagDollable>();
        if (damageable != null)
        {
            ragDollable.DamagePos(hit.transform);
            var bloodFX = Instantiate(gunData.bloodPrefab, hit.transform.position, rotation);
            Debug.Log("play blood fx:" + bloodFX.name);
            Destroy(bloodFX, 0.5f);
            damageable.TakeDamage(damage);
            //Debug.Log(gunData.name + "gun damage apply:" + gunData.damage);
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
        SoundManager.Instance.PlaySFX("BaseGunShoot", 0.02f);
    }



    public void SetGunUpgradeDamage(float bonus)
    {
        if (bonus == 0) return;
        //Debug.Log(this.gunData.type+" :gun before damage:" + damage +"bones"+bonus);
        damage = damage * (1 + bonus);
        //Debug.Log(this.gunData.type + ":gun after damage:" + damage);
    }

    public void SetGunUpgradeMagazine(float bonus)
    {
        if (bonus == 0) return;
        magzaineSize = (int)(magzaineSize * (1 + bonus));
    }

    public void SetGunUpgradeFireRate(float bonus)
    {
        if (bonus == 0) return;
        shootCooldown = shootCooldown * (1 + bonus);
    }
    public void SetGunUpgradeSpreadAmount(float bonus)
    {
        if (bonus == 0) return;
        spreadAmount = spreadAmount * (1 + bonus);
    }
    public void SetGunUpgradeRecoil(float bonus)
    {
        if (bonus == 0) return;
        recoilAmount =recoilAmount * (1 + bonus);
       // OnRecoilAmountUpgrade?.Invoke(recoilAmount);
        var cam = Camera.main;
        PlayerLook camSc = cam.GetComponent<PlayerLook>();
        camSc.UpgradeRecoilAmount(recoilAmount);
}
    public void SetGunUpgradeReloadSpeed(float bonus)
    {
        if (bonus == 0) return;
       
        reloadSpeed = reloadSpeed * (1 + bonus);
        SetReloadSpeed(reloadSpeed);
    }
    public int shotTimes = 2;
    public void SetGunUpgradeBulletsPerShot(float bonus)
    {
        if(bonus == 0)return;
     
       // Debug.Log(this.gunData.type+" :gun before bullets per shot:" + bulletsPerShot +"bones"+bonus);
      
        bulletsPerShot = (int)(bulletsPerShot * (1 + bonus));
        magzaineSize = bulletsPerShot * shotTimes;
        currentAmmo = magzaineSize;
       // Debug.Log(this.gunData.type + ":gun after bulletsPerShot:" + bulletsPerShot);
    }

    public void OnGunReloadFinish()
    {
        currentState = GunState.Idle;
        Debug.Log("reload finish");
        gunAnimator.SetBool("isReload", false);
        currentAmmo = magzaineSize;
        gunAnimator.speed = 1;
        playerAnimator.speed = 1;
      
    }
    public void OnShootFinish()
    {
        currentState = GunState.Idle;
        gunAnimator.SetBool("isShoot", false);
      
    }
    public void OnPlayReloadSoundAR()
    {
        Debug.Log("reload sound play");
        SoundManager.Instance.PlaySFX("ARReload",0.8f);
    }
  
    public float GetReloadSpeed()
    {
        return reloadSpeed;
    }
    private void SetReloadSpeed(float bonusSpeed)
    {
        reloadSpeed = bonusSpeed;
    }
}

