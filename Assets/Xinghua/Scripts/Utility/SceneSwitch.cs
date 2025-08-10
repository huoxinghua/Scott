using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    PlayerInputManager player;
    public PlayerUpgradeProfile upgradeProfile;
    [SerializeField] EnemySpawn eSpawn;

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
        if (eSpawn != null)
        {
            eSpawn.WaveEnd += LoadScene;
        }

       
    }
    private void OnDisable()
    {
        if (player != null)
        {
            player.OnUpgradeInput -= LoadScene;
        }
        if(eSpawn !=null)
        {
            eSpawn.WaveEnd -= LoadScene;
        }

      
    }
    public void LoadScene()
    {
         
        if (SceneManager.GetActiveScene().name == "XHProtoGym")
        {
            SceneManager.LoadScene("XHUpgradeScene");

        }
        else if (SceneManager.GetActiveScene().name == "XHUpgradeScene")
        {
            SceneManager.LoadScene("XHProtoGym");

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
    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }
}
