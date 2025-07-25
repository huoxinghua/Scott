using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.STP;

public class PodiumManager : MonoBehaviour
{
    public static PodiumManager Instance;
    public event Action<ModuleConfig> OnConfirmUpgradePodium;
    private UpgradePodium[] podiums ;
    [SerializeField]private GameObject canves;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        podiums = transform.GetComponentsInChildren<UpgradePodium>();
    }
    public void ConfirmUpgrade(UpgradePodium upgradePodium)
    {
        Debug.Log("ConfirmUpgrade:"+ upgradePodium);
        var config = upgradePodium.config;
        OnConfirmUpgradePodium?.Invoke(config);
        Debug.Log("ConfirmUpgrade");
        HideOption();
    }
    public void HideOption()//need not hide the collider
    {
      /*  for (int i = 0; i < podiums.Length; i++)
        {
            podiums[i].gameObject.SetActive(false);
        }*/
        canves.SetActive(false);
    }

}
