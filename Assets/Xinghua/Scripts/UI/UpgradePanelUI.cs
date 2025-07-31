using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    private void Start()
    {
        icon = GetComponentInChildren<Image>();
    }
    public void SetPanel(ModuleConfig config)
    {
        Debug.Log("set panel");
//        icon.sprite = config.stats.sprite;
        titleText.text = config.stats.name;
        descriptionText.text = config.stats.Description;
    }
}
