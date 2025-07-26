using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    [HideInInspector]
    public PlayerUpgradeProfile profile;

    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerInputManager inputManager;
    private Gun currentGun;
    public event Action OnUIInput;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        inputManager = GetComponent<PlayerInputManager>();
        currentGun = GetComponentInChildren<Gun>(false);

        var profile = Resources.Load<PlayerUpgradeProfile>("Data/PlayerUpgradeProfile");

    }
    private void OnEnable()
    {
        if (inputManager != null)
        {
            inputManager.OnInteractInput += HandleInteract;
        }

    }
    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.OnInteractInput -= HandleInteract;
        }

    }
    private void Start()
    {
        ApplyBonuses();
    }
    private void ApplyBonuses()
    {
        Debug.Log("Apply bonuses");
        float totalHealthBonus = 1f;
        float totalMoveSpeedBonus = 1f;

        float totalDamageBonus = 1f;
        int totalMagzineBonus = 1;
        float totalFireRateBonus = 1f;
        float totalSpreadAmountBonus = 1f;
        float totalRecoilBonus = 1f;
        float totalReloadSpeedBonus = 1f;
        int totalShotsPerShootBonus = 1;
       

        foreach (var m in profile.equippedUpgrades)
        {
            //guns
            if (m.stats.DamageBonus != 1)
            {
                totalDamageBonus = m.stats.DamageBonus;
                currentGun.SetGunUpgradeDamage(totalDamageBonus);
            }

            if (m.stats.MagazineBonus != 1)
            {
                totalMagzineBonus = m.stats.MagazineBonus;
                currentGun.SetGunUpgradeMagazine(totalMagzineBonus);
            }

            if (m.stats.FireRateBonus != 1)
            {
                totalFireRateBonus = m.stats.FireRateBonus;
                currentGun.SetGunUpgradeFireRate(totalFireRateBonus);
            }

            if (m.stats.SpreadAmountBonus != 1)
            {
                totalSpreadAmountBonus = m.stats.SpreadAmountBonus;
                currentGun.SetGunUpgradeSpreadAmount(totalSpreadAmountBonus);
            }

            if (m.stats.RecoilBonus != 1)
            {
                totalRecoilBonus = m.stats.RecoilBonus;
                currentGun.SetGunUpgradeRecoil(totalRecoilBonus);
            }

            if (m.stats.ReloadSpeedBonus != 1)
            {
                totalReloadSpeedBonus = m.stats.ReloadSpeedBonus;
                currentGun.SetGunUpgradeReloadSpeed(totalReloadSpeedBonus);
            }

            if (m.stats.ShotsPerShootBonus != 1)
            {
                totalShotsPerShootBonus = m.stats.ShotsPerShootBonus;
                currentGun.SetGunUpgradeShotsPerShoot(totalShotsPerShootBonus);
            }
        }

        playerHealth.SetBonusHealth(totalHealthBonus);
        playerMovement.SetBonusSpeed(totalMoveSpeedBonus);

       // currentGun.SetGunBonus(currentGun.gunData.type, gunBonus);

    }
    public bool isInteract = false;
    private void HandleInteract()
    {
        Debug.Log("get interact input");
        TryInteractPodium();
    }
    private bool isInRange = false;
    private void OnTriggerEnter(Collider other)
    {
        UpgradePodium upgradePodium = other.GetComponent<UpgradePodium>();
        if (upgradePodium != null)
        {
            PodiumManager.Instance.ShowInteractE();
            upgradePodium.ShowPanel();
            isInRange = true;
            PodiumManager.Instance.SetCurrentUpgradeOptinon(upgradePodium.config);

        }
    }
    private void TryInteractPodium()
    {
        Debug.Log("TryInteractPodium");
        if (isInRange)
        {
            PodiumManager.Instance.ShowButton();

            OnUIInput?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        UpgradePodium upgradePodium = other.GetComponent<UpgradePodium>();
        if (upgradePodium != null)
        {
            upgradePodium.HidePanel();
            PodiumManager.Instance.HideInteractE();
            PodiumManager.Instance.HideButton();
            isInRange = false;
        }
    }
    public void EquipModule(ModuleConfig module)
    {

        inputManager.OnUIClose();
    }

}
