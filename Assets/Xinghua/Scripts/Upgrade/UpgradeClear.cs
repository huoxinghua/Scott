using UnityEngine;

public class UpgradeClear : MonoBehaviour
{
    [SerializeField]private PlayerUpgradeProfile profile;
    public void ClearUpgradeData()
    {
        profile.ResetProfile();
    }
}
