using System;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    [HideInInspector]
    public PlayerUpgradeProfile profile;

    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerInputManager inputManager;
    private Gun[] guns;
    public event Action OnUIInput;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        inputManager = GetComponent<PlayerInputManager>();
        guns = GetComponentsInChildren<Gun>();
        Debug.Log("gun number:" + guns.Length);
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
        float totalHealthBonus = 0f;
        float totalMoveSpeedBonus = 0f;

        float totalDamageBonus = 0f;
        int totalMagzineBonus = 0;
        float totalFireRateBonus = 0f;
        float totalSpreadAmountBonus = 0f;
        float totalRecoilBonus = 0f;
        float totalReloadSpeedBonus = 0f;
        int totalShotsPerShootBonus = 0;



        foreach (var m in profile.equippedUpgrades)
        {
            if (m.stats.SanityBonus != 0)
            {
                totalHealthBonus = m.stats.DamageBonus;
                playerHealth.SetBonusHealth(totalHealthBonus);
            }
            if (m.stats.MoveSpeedBonus != 0)
            {
                totalMoveSpeedBonus = m.stats.MoveSpeedBonus;

                playerMovement.SetBonusSpeed(totalMoveSpeedBonus);
            }
            //guns
            if (m.stats.MagazineBonus != 0)
            {
                totalMagzineBonus = m.stats.MagazineBonus;
                foreach (var gun in guns)
                {
                    gun.SetGunUpgradeMagazine(totalMagzineBonus);
                }
            }

            if (m.stats.DamageBonus != 0)
            {
                totalDamageBonus = m.stats.DamageBonus;
                foreach (var gun in guns)
                {
                    gun.SetGunUpgradeDamage(totalDamageBonus);
                }
            }

            if (m.stats.FireRateBonus != 0)
            {
                totalFireRateBonus = m.stats.FireRateBonus;
                foreach (var gun in guns)
                {
                    gun.SetGunUpgradeFireRate(totalFireRateBonus);
                }
            }

            if (m.stats.SpreadAmountBonus != 0)
            {
                totalSpreadAmountBonus = m.stats.SpreadAmountBonus;
                foreach (var gun in guns)
                {
                    gun.SetGunUpgradeSpreadAmount(totalSpreadAmountBonus);
                }
            }

            if (m.stats.RecoilBonus != 0)
            {
                totalRecoilBonus = m.stats.RecoilBonus;
                foreach (var gun in guns)
                {
                    gun.SetGunUpgradeRecoil(totalRecoilBonus);
                }
            }

            if (m.stats.ReloadSpeedBonus != 0)
            {
                totalReloadSpeedBonus = m.stats.ReloadSpeedBonus;
                foreach (var gun in guns)
                {
                    gun.SetGunUpgradeReloadSpeed(totalReloadSpeedBonus);
                }
            }

            if (m.stats.ShotsPerShootBonus != 0)
            {
                totalShotsPerShootBonus = m.stats.ShotsPerShootBonus;
                foreach (var gun in guns)
                {
                    gun.SetGunUpgradeShotsPerShoot(totalShotsPerShootBonus);
                }

            }
        }

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
