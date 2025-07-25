using UnityEngine;

public class UpgradePodium : MonoBehaviour
{
    private GameObject interactPrompt;

    /*  public GameObject UpgardePanel;
      public GameObject ConfirmButton;
      public GameObject CancelButton;*/

    public GameObject canves;

    public ModuleConfig config;
    PlayerUpgrade player;
    private void Awake()
    {
        interactPrompt = gameObject.transform.GetChild(0).gameObject;
        player = FindAnyObjectByType<PlayerUpgrade>();
    }
    void Start()
    {
        HideOption();
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
    }

    public void TryInteract()
    {
        Debug.Log("Interact upgrade area");
        ShowOption();
    }
    public void UpgradeConfirm()
    {
        isUpgradeConfirmed = true;
        PodiumManager podiumManager = GetComponentInParent<PodiumManager>();
        podiumManager.ConfirmUpgrade(this) ;
       // HideOption();
    }

    public void EndInteract()
    {
        canves.SetActive(false);
        interactPrompt.SetActive(false);

    }
    public void ShowOption()
    {
        canves.SetActive(true);
        interactPrompt.SetActive(true);
      

    }
    public void HideOption()
    {
        canves.SetActive(false);
        interactPrompt.SetActive(false);

    }
    public bool isUpgradeConfirmed = false;
}
