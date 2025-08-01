using System;
using System.Collections.Generic;
using UnityEngine;
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    private float totalHealthBonus;
    float totalMoveSpeedBonus;

    float totalDamageBonus;
    int totalMagzineBonus;
    float totalFireRateBonus;
    float totalSpreadAmountBonus;
    float totalRecoilBonus;
    float totalReloadSpeedBonus;
    int totalShotsPerShootBonus;

    public Dictionary<BonusType, float> bonusTable = new Dictionary<BonusType, float>();
    private ModuleConfig currentConfig;
    public bool isUpgradeSceneStart = false;

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitBonusTable();
    }
    private void InitBonusTable()
    {
        foreach (BonusType type in Enum.GetValues(typeof(BonusType)))
        {
            bonusTable[type] = 0f;
           // Debug.Log(type.ToString() + bonusTable[type]);
        }
    }
    public void AddBonus(BonusType type,float value)
    {
        if(bonusTable.ContainsKey(type) && value != 0f)
        {
            bonusTable[type] += value;
            Debug.Log(type.ToString() + bonusTable[type]);
        }
     
    }
    public float GetBonus(BonusType type)
    {
        if (bonusTable.ContainsKey(type))
        {
            return bonusTable[type];
        }
        return 0f;
    }

    public void ApplyUpgradeBonus(ModuleConfig config)
    {
        currentConfig = config;

        AddBonus(BonusType.Sanity, config.stats.SanityBonus);
        AddBonus(BonusType.MoveSpeed, config.stats.MoveSpeedBonus);
        AddBonus(BonusType.Damage, config.stats.DamageBonus);
        AddBonus(BonusType.FireRate, config.stats.FireRateBonus);
        AddBonus(BonusType.Spread, config.stats.SpreadAmountBonus);
        AddBonus(BonusType.Recoil, config.stats.RecoilBonus);
        AddBonus(BonusType.ReloadSpeed, config.stats.ReloadSpeedBonus);
        AddBonus(BonusType.Magazine, config.stats.MagazineBonus);
        AddBonus(BonusType.ShotsPerShoot, config.stats.ShotsPerShootBonus);

    }


}
