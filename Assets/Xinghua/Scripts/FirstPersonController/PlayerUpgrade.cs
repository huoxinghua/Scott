using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    public List<ModuleConfig> equippedModules = new List<ModuleConfig>();


    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerInputManager inputManager;
    private PodiumManager podiumManager;
    public event Action OnUIInput;
    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
      
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
        if (PodiumManager.Instance != null)
        {
            PodiumManager.Instance.OnConfirmUpgradePodium -= EquipModule;

        }
    }
    public bool isInteract = false;
    private void HandleInteract()
    {
        isInteract = true;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        UpgradePodium upgradeObj = other.GetComponent<UpgradePodium>();
        if (upgradeObj != null)
        {
            upgradeObj.TryInteract();
            OnUIInput?.Invoke();
          
        }
    }
    private void OnTriggerExit(Collider other)
    {
        UpgradePodium upgradeObj = other.GetComponent<UpgradePodium>();
        if (upgradeObj != null)
        {
            upgradeObj.EndInteract();
        }
    }
    public void EquipModule(ModuleConfig module)
    {
        Debug.Log("PlayerUpgrade EquipModule: " + module.name);
        equippedModules.Add(module);
        Debug.Log("PlayerUpgrade EquipModule count: " + equippedModules.Count);
        ApplyBonuses();
       // inputManager.OnUIClose();
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
