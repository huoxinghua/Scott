using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class FixedWaves : MonoBehaviour
{
    public static FixedWaves instance;

    public int currentWave;
    public int waveToStartRangedEnemies;
    public int waveToStartTanks;
    public int waveToRampUp;
    public int totalEnemyPerWave;
    public int startEnemyPerWave;

    public int baseEnemyChance;
    public int rangedEnemyChance;
    public int rangedChanceIncrease;
    public int tankEnemyChance;
    public int tankChanceIncrease;
    [SerializeField] SOWave wave;
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
        ResetWaveData();
    }
    public void ResetWaveData()
    {
    currentWave = wave.currentWave;
    waveToStartRangedEnemies = wave.waveToStartRangedEnemies;
        waveToStartTanks = wave.waveToStartTanks;
        waveToRampUp = wave.waveToRampUp;
    totalEnemyPerWave = wave.totalEnemyPerWave;
        startEnemyPerWave = wave.startEnemyPerWave;

    baseEnemyChance = wave.baseEnemyChance;
    rangedEnemyChance = wave.rangedEnemyChance;
    rangedChanceIncrease = wave.rangedChanceIncrease;
    tankEnemyChance = wave.tankEnemyChance;
    tankChanceIncrease = wave.tankChanceIncrease;
}
    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        string newSceneName = newScene.name;



        switch (newSceneName)
        {
            case "XHMainMenu":
                ResetWaveData();
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
