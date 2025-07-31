using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.STP;
using Random = UnityEngine.Random;

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
    public event Action OnUpgradeConfirm;
    [SerializeField] private SceneSwitch sceneSwitch;
    private List<ModuleConfig> configs = new List<ModuleConfig>();

    private List<ModuleConfig> goodConfigs = new List<ModuleConfig>();
    private List<ModuleConfig> neutralConfigs = new List<ModuleConfig>();
    private List<ModuleConfig> evilConfigs = new List<ModuleConfig>();

    [SerializeField] private RectTransform goodUpgradeParent;
    [SerializeField] private RectTransform neutralUpgradeParent;
    [SerializeField] private RectTransform evilUpgradeParent;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
         goodConfigs = Resources.LoadAll<ModuleConfig>("Modules/Good").ToList();
         neutralConfigs = Resources.LoadAll<ModuleConfig>("Modules/Neutral").ToList();
         evilConfigs = Resources.LoadAll<ModuleConfig>("Modules/Evil").ToList();
       
    }
    /* private void OnEnable()
     {
         if (sceneSwitch != null)
         {
             sceneSwitch.OnUpgradeSceneLoad += GenerateRandomUpgradeOption;
         }
         else
         {
             Debug.Log("sceneSwitch is null");
         }

     }
     private void OnDisable()
     {
         if (sceneSwitch != null)
         {
             sceneSwitch.OnUpgradeSceneLoad -= GenerateRandomUpgradeOption;
         }
         else
         {
             Debug.Log("sceneSwitch is null");
         }
     }*/
    private void Start()
    {
        podiums = transform.GetComponentsInChildren<UpgradePodium>();
        interactPrompt.SetActive(false);
        cvonfirmButton.SetActive(false);
        cancelButton.SetActive(false);
        playerUpgrade = FindAnyObjectByType<PlayerUpgrade>();
        GenerateRandomUpgradeOption();

    }
    /*   private List<ModuleConfig> GetRandomConfigs()
       {
           var copyConfigs = configs;
           var result = new List<ModuleConfig>();
           for (int i = 0; i < 3 && copyConfigs.Count > 0; i++)
           {

               var index = Random.Range(0, configs.Count);
               Debug.Log("i" + i + "Configs count:" + configs.Count + "index" + index);
               var config = configs[index];
               result.Add(config);
               configs.Remove(config);
           }
           Debug.Log("result" + result.Count);
           configs = copyConfigs;
           return result;
       }*/
  
    private void GenerateRandomUpgradeOption()
    {
        Debug.Log("GenerateRandomUpgradeOption"+ goodConfigs.Count+neutralConfigs.Count+evilConfigs.Count);
        ModuleConfig randomGood = goodConfigs[Random.Range(0, goodConfigs.Count)];
        ModuleConfig randomNeutral = neutralConfigs[Random.Range(0, neutralConfigs.Count)];
        ModuleConfig randomEvil = evilConfigs[Random.Range(0, evilConfigs.Count)];
       /* Debug.Log("Good panel prefab: " + randomGood.stats.panel);
        Debug.Log("Neutral panel prefab: " + randomNeutral.stats.panel);
        Debug.Log("Evil panel prefab: " + randomEvil.stats.panel);*/

        Debug.Log(randomGood.stats.name + randomEvil.stats.Type + randomNeutral.name);
       

        var neutralPanel = Instantiate(randomNeutral.stats.panel, neutralUpgradeParent);
       // neutralPanel.GetComponent<UpgradePanelUI>().SetPanel(randomNeutral);

        var goodPanel = Instantiate(randomGood.stats.panel, goodUpgradeParent);
       // goodPanel.GetComponent<UpgradePanelUI>().SetPanel(randomGood);

        var evilPanel = Instantiate(randomEvil.stats.panel,evilUpgradeParent);
       // evilPanel.GetComponent<UpgradePanelUI>().SetPanel(randomEvil);
    }

   /* public void GenerateRandomUpgradeOption()
    {
        Debug.Log("GenerateRandomUpgradeOption");
        var upgradeConfigs = GetRandomConfigs();
        Debug.Log("upgradeConfigs count:" + upgradeConfigs.Count);
        foreach (var config in upgradeConfigs)
        {
            Debug.Log("config.stats.sprite:" + config.stats.sprite.name);
            Instantiate(config.stats.sprite);
        }

    }*/
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
            Debug.Log("playerUpgrade null");
        }
        HideOption();
        OnUpgradeConfirm?.Invoke();
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
    private UpgradePodium currentPanel = null;
    public void SetCurrentUpgradeOptinon(ModuleConfig config)
    {
        currentUpgradeOptinon = config;
    }



}
