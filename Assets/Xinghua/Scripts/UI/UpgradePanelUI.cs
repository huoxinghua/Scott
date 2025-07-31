using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    private void Start()
    {
        icon = GetComponentInChildren<Image>();
    }
    public void SetPanel(ModuleConfig config)
    {
        Debug.Log("set panel");
//        icon.sprite = config.stats.sprite;
        titleText.SetText(config.stats.name);
        descriptionText.SetText(config.stats.Description);
    }
}
