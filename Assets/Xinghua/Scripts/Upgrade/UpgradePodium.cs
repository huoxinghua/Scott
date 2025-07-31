using UnityEngine;

public class UpgradePodium : MonoBehaviour
{
    private GameObject upgardePanel;
    public ModuleConfig config;

    private void Awake()
    {
        upgardePanel = gameObject.transform.GetChild(0).gameObject;
      
    }
    void Start()
    {
        if (upgardePanel != null)
        {
            upgardePanel.SetActive(false);
        }
    }
    public void ShowPanel()
    {
        if (upgardePanel != null)
        {
            upgardePanel.SetActive(true);
        }
    }
    public void HidePanel()
    {
        if (upgardePanel != null)
        {
            upgardePanel.SetActive(false);
        }
    }
}
