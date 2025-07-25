using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    [HideInInspector]
    public PlayerUpgradeProfile profile;

    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerInputManager inputManager;
    public event Action OnUIInput;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        inputManager = GetComponent<PlayerInputManager>();
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
    private void Start()
    {
        ApplyBonuses();
    }
    private void ApplyBonuses()
    {
        float totalHealthBonus = 1f;
        float totalMoveSpeedBonus = 1f;
        foreach (var m in profile.equippedUpgrades)
        {
            totalHealthBonus = m.stats.SanityBonus;
            totalMoveSpeedBonus = m.stats.MoveSpeedBonus;
        }
        playerHealth.SetBonusHealth(totalHealthBonus);
        playerMovement.SetBonusSpeed(totalMoveSpeedBonus);

    }
    public bool isInteract = false;
    private void HandleInteract()
    {
        Debug.Log("get interact input");
        TryInteractPodium();
    }
   private bool isInRange =false ;
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
        Debug.Log("TryInteractPodium");
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
            isInRange=false;
        }
    }
    public void EquipModule(ModuleConfig module)
    {

        inputManager.OnUIClose();
    }
 
}
