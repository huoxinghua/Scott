using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    PlayerInputManager player;
    public PlayerUpgradeProfile upgradeProfile;
    public event Action OnUpgradeSceneLoad;

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerInputManager>();
    }
    private void OnEnable()
    {
        if (player != null)
        {
            player.OnUpgradeInput += LoadScene;
        }
    /*    if(PodiumManager.Instance!= null)
        {
            PodiumManager.Instance.OnUpgradeConfirm += LoadScene;
        }
        else
        {
            Debug.Log("podiumManager is null");
        }*/
    }
    private void OnDisable()
    {
        if (player != null)
        {
            player.OnUpgradeInput -= LoadScene;
        }
    /*    if (PodiumManager.Instance != null)
        {
            PodiumManager.Instance.OnUpgradeConfirm -= LoadScene;
        }
        else
        {
            Debug.Log("podiumManager is null");
        }*/
    }
    public void LoadScene()
    {
         
        if (SceneManager.GetActiveScene().name == "XHProtoGym")
        {
            SceneManager.LoadScene("XHUpgradeScene");
            //UpgradeManager.Instance.isUpgradeSceneStart = true;
        }
        else if (SceneManager.GetActiveScene().name == "XHUpgradeScene")
        {
            SceneManager.LoadScene("XHProtoGym");
          //  UpgradeManager.Instance.isUpgradeSceneStart = false;
        }
        else
        {
            Debug.Log("podiumManager is null");
        }
    }
    public void LoadSceneByName(string name)//just for main menu to level 
    {
        upgradeProfile.ResetProfile();
        SceneManager.LoadScene(name);

    }
}
