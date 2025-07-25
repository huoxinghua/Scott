using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerUpgradeProfile", menuName = "GameData/PlayerUpgradeProfile")]
public class PlayerUpgradeProfile : ScriptableObject
{
    public List<ModuleConfig> equippedUpgrades = new List<ModuleConfig>();

    public void AddUpgrade(ModuleConfig upgrade)
    {
        if (!equippedUpgrades.Contains(upgrade))
            equippedUpgrades.Add(upgrade);

        Debug.Log("profile upgrade:" + equippedUpgrades.Count);

    }
    public void ResetProfile()
    {
        equippedUpgrades.Clear();
    }
}
