using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    private List<ModuleConfig> equippedModules = new List<ModuleConfig>();


    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerInputManager inputManager;

    public event Action OnUIInput;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        inputManager = GetComponent<PlayerInputManager>();


    }
    private void OnEnable()
    {
        if (inputManager != null)
        {
            inputManager.OnInteractInput += HandleInteract;
        }
        if (PodiumManager.Instance != null)
        {
            PodiumManager.Instance.OnConfirmUpgradePodium += EquipModule;
           
        }
        else
        {
            Debug.Log(" PodiumManager.Instance.null");
        }
    }
    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.OnInteractInput -= HandleInteract;
        }
        if (PodiumManager.Instance != null && inputManager != null)
        {
            PodiumManager.Instance.OnConfirmUpgradePodium -= EquipModule;

        }
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
        Debug.Log("PlayerUpgrade EquipModule: " + module.name);
        equippedModules.Add(module);
        Debug.Log("PlayerUpgrade EquipModule count: " + equippedModules.Count);
        ApplyBonuses();
        inputManager.OnUIClose();
    }
    public void ApplyBonuses()
    {
        if (playerHealth != null)
            playerHealth.SetBonusHealth(GetHealthBonus());
        // if (playerMovement != null)
        // playerMovement.SetBonusMoveSpeed(GetMoveSpeedBonus());

    }
    public float GetHealthBonus()
    {
        float result = 0;
        foreach (var m in equippedModules)
            result += m.stats.SanityBonus;
        return result;
    }
    public float GetMoveSpeedBonus()
    {
        float result = 0;
        foreach (var m in equippedModules)
            result += m.stats.MoveSpeedBonus;
        return result;
    }
}
