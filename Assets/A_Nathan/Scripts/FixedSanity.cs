using UnityEngine;
using UnityEngine.SceneManagement;

public class FixedSanity : MonoBehaviour
{
    public float currentSanity;
    public float maxSanity;
    public float baseMaxSanity;
    public float sanityMins;
    public float sanityGainedOnKill;

    public static FixedSanity instance;
    [SerializeField] SOSanity sanity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += OnSceneChanged;

        }
        else
        {
            Destroy(gameObject);
        }
        ResetSanityData();
    }
    public void ResetSanityData()
    {
        currentSanity = sanity.currentSanity;
        maxSanity = sanity.maxSanity;
        baseMaxSanity = sanity.baseMaxSanity;
        sanityMins = sanity.sanityMins;
        sanityGainedOnKill = sanity.sanityGainedOnKill;
    }
    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        string newSceneName = newScene.name;



        switch (newSceneName)
        {
            case "XHMainMenu":
                ResetSanityData();
                break;

            case "XHProtoGym":

                break;

            case "XHUpgradeScene":

                break;

            default:

                break;
        }
    }
        // Update is called once per frame
        void Update()
    {
        
    }
}
