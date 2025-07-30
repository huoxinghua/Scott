using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.STP;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    public float originalHealth = 100f;
    public float originalSpeed = 5f;
    public float newHealth { get; set; }
    public float newSpeed { get; set; }
    private float totalHealthBonus;
    float totalMoveSpeedBonus;

    float totalDamageBonus;
    int totalMagzineBonus;
    float totalFireRateBonus;
    float totalSpreadAmountBonus;
    float totalRecoilBonus;
    float totalReloadSpeedBonus;
    int totalShotsPerShootBonus;

  public event Action<float> OnPlayerDataUpgradeConfirm;
    public event Action<float> OnPlayerSpeedUpgradeConfirm;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }

    }
 
  
    private void Start()
    {
        newHealth = originalHealth;
        newSpeed = originalSpeed;

        totalHealthBonus = 0f;
        totalMoveSpeedBonus = 0f;

        totalDamageBonus = 0f;
        totalMagzineBonus = 0;
        totalFireRateBonus = 0f;
        totalSpreadAmountBonus = 0f;
        totalRecoilBonus = 0f;
        totalReloadSpeedBonus = 0f;
        totalShotsPerShootBonus = 0;
        Debug.Log("upgrade manager start");
    }

    public void ApplyUpgrade(ModuleConfig config)
    {
      
        if (config.stats.SanityBonus != 0)
        {
            totalHealthBonus += config.stats.SanityBonus;
            SetBonusHealth();
            
        }
        if (config.stats.MoveSpeedBonus != 0)
        {
            totalMoveSpeedBonus += config.stats.MoveSpeedBonus;
            SetBonusSpeed();
        }
        //guns
        if (config.stats.MagazineBonus != 0)
        {
            totalMagzineBonus = config.stats.MagazineBonus;

        }

        if (config.stats.DamageBonus != 0)
        {
            totalDamageBonus = config.stats.DamageBonus;

        }

        if (config.stats.FireRateBonus != 0)
        {
            totalFireRateBonus = config.stats.FireRateBonus;

        }

        if (config.stats.SpreadAmountBonus != 0)
        {
            totalSpreadAmountBonus = config.stats.SpreadAmountBonus;

        }

        if (config.stats.RecoilBonus != 0)
        {
            totalRecoilBonus = config.stats.RecoilBonus;

        }
        if (config.stats.ReloadSpeedBonus != 0)
        {
            totalReloadSpeedBonus = config.stats.ReloadSpeedBonus;

        }
        if (config.stats.ShotsPerShootBonus != 0)
        {
            totalShotsPerShootBonus = config.stats.ShotsPerShootBonus;
        }

    }
    public void SetBonusHealth()
    {
        newHealth = originalHealth * (1 + totalHealthBonus);
        Debug.Log("after upgrade player health: "+ newHealth);
        OnPlayerDataUpgradeConfirm?.Invoke(newHealth);
    }
    public void SetBonusSpeed()
    {
        newSpeed = originalSpeed *(1+totalMoveSpeedBonus);
        Debug.Log("after upgrade player speed: "+ newHealth);
        OnPlayerSpeedUpgradeConfirm?.Invoke(newSpeed);
    }
    public void ApplyBonuses(ModuleConfig config)
    {
       
        if (config.stats.SanityBonus != 0)
        {
           
            // UpgradeManager.Instance.SetBonusHealth(UpgradeManager.Instance.totalHealthBonus);
        }
        if (config.stats.MoveSpeedBonus != 0)
        {
           
           // playerMovement.SetBonusSpeed(config.stats.SanityBonus);
            Debug.Log("player SetBonusSpeed");
        }
        //guns
        if (config.stats.MagazineBonus != 0)
        {
           
          //  foreach (var gun in guns)
          //  {
                //  gun.SetGunUpgradeMagazine(totalMagzineBonus);
          //  }
        }

        if (config.stats.DamageBonus != 0)
        {
          
           /* foreach (var gun in guns)
            {
                 gun.SetGunUpgradeDamage(totalDamageBonus);
            }*/
        }

        if (config.stats.FireRateBonus != 0)
        {
           
           /* foreach (var gun in guns)
            {
                  gun.SetGunUpgradeFireRate(totalFireRateBonus);
            }*/
        }

        if (config.stats.SpreadAmountBonus != 0)
        {
             
          /*  foreach (var gun in guns)
            {
                 gun.SetGunUpgradeSpreadAmount(totalSpreadAmountBonus);
            }*/
        }

        if (config.stats.RecoilBonus != 0)
        {
       
           /* foreach (var gun in guns)
            {
                 gun.SetGunUpgradeRecoil(totalRecoilBonus);
            }*/
        }

        if (config.stats.ReloadSpeedBonus != 0)
        {
          
          /*  foreach (var gun in guns)
            {
                 gun.SetGunUpgradeReloadSpeed(totalReloadSpeedBonus);
            }*/
        }

        if (config.stats.ShotsPerShootBonus != 0)
        {
           
          /*  foreach (var gun in guns)
            {
                  gun.SetGunUpgradeShotsPerShoot(totalShotsPerShootBonus);
            }*/

        }


    }
}
