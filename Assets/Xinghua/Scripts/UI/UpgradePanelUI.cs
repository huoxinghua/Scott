using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
  
    public void Set(ModuleConfig config)
    {
        icon.sprite = config.stats.sprite;
        titleText.text = config.stats.name;
        descriptionText.text = config.stats.Description;
    }
}
