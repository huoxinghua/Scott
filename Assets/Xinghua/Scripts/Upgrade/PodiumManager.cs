using System;
using UnityEngine;
using UnityEngine.Events;

public class PodiumManager : MonoBehaviour
{
    public static PodiumManager Instance;
    public event Action<ModuleConfig> OnConfirmUpgradePodium;
    public UnityEvent OnUpgrade;
    public UnityEvent OnUIClosePodium;
    private UpgradePodium[] podiums;

    public GameObject interactPrompt;
    public GameObject cvonfirmButton;
    public GameObject cancelButton;
    PlayerUpgrade playerUpgrade;
   // public event Action OnUpgradeConfirm;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        podiums = transform.GetComponentsInChildren<UpgradePodium>();
        interactPrompt.SetActive(false);
        cvonfirmButton.SetActive(false);
        cancelButton.SetActive(false);
        playerUpgrade = FindAnyObjectByType<PlayerUpgrade>();
        
    }
    public void ConfirmUpgrade()
    {
        OnConfirmUpgradePodium?.Invoke(currentUpgradeOptinon);
        if (currentUpgradeOptinon != null)
        {
            playerUpgrade.profile.AddUpgrade(currentUpgradeOptinon);
           
           // OnUpgradeConfirm?.Invoke();
            Debug.Log("ConfirmUpgrade" + currentUpgradeOptinon.name);
            UpgradeManager.Instance.ApplyUpgrade(currentUpgradeOptinon);
        }
        else
        {
            Debug.Log("playerUpgrade null" );
        }
        HideOption();
    }
    public void ShowInteractE()
    {
        interactPrompt.SetActive(true);
    }
    public void HideInteractE()
    {
        interactPrompt.SetActive(false);
    }
    public void ShowButton()
    {
        cvonfirmButton.SetActive(true);
        cancelButton.SetActive(true);
    }
    public void HideButton()
    {
        cvonfirmButton.SetActive(false);
        cancelButton.SetActive(false);
    }
    public void HideOption()
    {
        OnUIClosePodium?.Invoke();
        HideButton();
    }
    private ModuleConfig currentUpgradeOptinon = null;
    private UpgradePodium currentPanel= null;
    public void SetCurrentUpgradeOptinon(ModuleConfig config)
    {
        currentUpgradeOptinon = config;
    }


 
}
