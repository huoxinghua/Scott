using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.STP;
using SysRandom = System.Random;

public class PodiumManager : MonoBehaviour
{
    public static PodiumManager Instance;
    //[SerializeField] private GameObject buttonSoundPlayer;
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
    private SysRandom rng;
    private int rngSeed;
    public string name;
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
    
    private void Start()
    {
        podiums = transform.GetComponentsInChildren<UpgradePodium>();
        interactPrompt.SetActive(false);
        cvonfirmButton.SetActive(false);
        cancelButton.SetActive(false);
        playerUpgrade = FindAnyObjectByType<PlayerUpgrade>();
        GenerateRandomUpgradeOption();
    }
 
  private List<ModuleConfig> generateConfigs = new List<ModuleConfig>();
    public void GenerateRandomUpgradeOption()
    {
        rng = new SysRandom(Guid.NewGuid().GetHashCode());
        var index1 = rng.Next(goodConfigs.Count);
        var index2 = rng.Next(neutralConfigs.Count);    
        var index3 = rng.Next(evilConfigs.Count);
        ModuleConfig randomGood = goodConfigs[index1];
        generateConfigs.Add(randomGood);
        ModuleConfig randomNeutral = neutralConfigs[index2];
        generateConfigs.Add(randomNeutral);
        ModuleConfig randomEvil = evilConfigs[index3];
        generateConfigs.Add(randomEvil);

        var goodPanel = Instantiate(randomGood.stats.panel, goodUpgradeParent);
        goodPanel.GetComponent<UpgradePanelUI>().SetPanel(randomGood);

      
        var neutralPanel = Instantiate(randomNeutral.stats.panel, neutralUpgradeParent);
        neutralPanel.GetComponent<UpgradePanelUI>().SetPanel(randomNeutral);

     

        var evilPanel = Instantiate(randomEvil.stats.panel,evilUpgradeParent);
        evilPanel.GetComponent<UpgradePanelUI>().SetPanel(randomEvil);

       
    }

    public void ConfirmUpgrade()
    {
        OnConfirmUpgradePodium?.Invoke(currentUpgradeOptinon);
        if (currentUpgradeOptinon != null)
        {
            playerUpgrade.profile.AddUpgrade(currentUpgradeOptinon);

            // OnUpgradeConfirm?.Invoke();
            Debug.Log("ConfirmUpgrade" + currentUpgradeOptinon.name);
            UpgradeManager.Instance.upgradeTime++;
            UpgradeManager.Instance.ApplyUpgradeBonus(currentUpgradeOptinon);
        }
        else
        {
            Debug.Log("playerUpgrade null");
        }
        HideOption();
        OnUpgradeConfirm?.Invoke();
       /* if(buttonSoundPlayer != null)
        {
            buttonSoundPlayer.PlayClickSound();
        }*/
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
    public PodiumType currentType;
    public void SetCurrentUpgradeOptinon(PodiumType type)
    {

        foreach (var a in generateConfigs)
        {
            if (currentType == PodiumType.Good)
            {
                currentUpgradeOptinon = a;
            }
        }
        
    }



}
