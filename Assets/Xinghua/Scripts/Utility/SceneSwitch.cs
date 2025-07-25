using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    PlayerInputManager player;
    public PlayerUpgradeProfile upgradeProfile;
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
    }
    private void OnDisable()
    {
        if (player != null)
        {
            player.OnUpgradeInput -= LoadScene;
        }
    }
    public void LoadScene()
    {
        //  Debug.Log("scene current:" + SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "XHProtoGym")
        {
            SceneManager.LoadScene("XHUpgradeScene");
        }
        else if (SceneManager.GetActiveScene().name == "XHUpgradeScene")
        {
            SceneManager.LoadScene("XHProtoGym");
        }
    }
    public void LoadSceneByName(string name)
    {
        upgradeProfile.ResetProfile();
        Debug.Log("scene name:" + name);
        SceneManager.LoadScene(name);

    }
}
