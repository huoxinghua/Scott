using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUpgradeProfile", menuName = "GameData/PlayerUpgradeProfile")]
public class PlayerUpgradeProfile : ScriptableObject
{
    public List<ModuleConfig> equippedUpgrades = new List<ModuleConfig>();

    public void AddUpgrade(ModuleConfig upgrade)
    {
        if (!equippedUpgrades.Contains(upgrade))
        {
            equippedUpgrades.Add(upgrade);
        }
    }
    public void ResetProfile()
    {
        equippedUpgrades.Clear();
    }
}
