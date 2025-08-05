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
    
    public bool isInteract = false;
    private void HandleInteract()
    {
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
  /*  public void EquipModule(ModuleConfig module)
    {
        inputManager.OnUIClose();
    }*/

}
