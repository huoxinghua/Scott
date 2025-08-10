using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class EnemySpawn : MonoBehaviour
{
    List<Transform> spawnList = new List<Transform>();
    [SerializeField] int maxSpawnPoints = 100;
    public int CurrentWave = 0;


    //better way than serialized?
    [SerializeField] GameObject BaseEnemy;
    [SerializeField] GameObject RangedEnemy;
    [SerializeField] GameObject TankEnemy;


    [SerializeField] int maxZombiesInScene;
    [SerializeField] float spawnSpeed = 4;
    [SerializeField] int firstWaveEnemyAmount;
    // [SerializeField] SOWave waveData;
    FixedWaves fWave;
    [SerializeField] PlayerHealth playerHealth;
    int EnemiesToSpawn;
    int EnemiesSpawned;
    int EnemiesKilled;

    public event Action WaveEnd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        EnemiesToSpawn = fWave.totalEnemyPerWave;
        //get all spawnpoints in scene (with a max possible in the scene)
        for (int i = 1; i < maxSpawnPoints; i++)
        {
            if(GameObject.Find("EnemySpawnLocation (" + i + ")") != null)
            {
                spawnList.Add(GameObject.Find("EnemySpawnLocation (" + i + ")").transform);
            }
            else
            {
                break;
            }
        }
        NextWave();
    }
    public void Awake()
    {
        fWave = FixedWaves.instance;
    }
    //start next wave
    public void NextWave()
    {
       
        CurrentWave = fWave.currentWave;
        EnemiesToSpawn = fWave.totalEnemyPerWave;
        EnemiesSpawned = 0;
        EnemiesKilled = 0;
        if(CurrentWave != 0)
        {
            EnemiesToSpawn+= 1 + (int)(1 * Mathf.Ceil(fWave.currentWave/3));
        }
        CurrentWave++;
        if(CurrentWave == fWave.waveToStartRangedEnemies)
        {
            fWave.baseEnemyChance -= fWave.rangedChanceIncrease;
            fWave.rangedEnemyChance += fWave.rangedChanceIncrease;
        }
        if(CurrentWave == fWave.waveToStartTanks)
        {
            fWave.baseEnemyChance -= (fWave.rangedChanceIncrease + fWave.tankChanceIncrease);
            fWave.tankEnemyChance += fWave.tankChanceIncrease;
            fWave.rangedEnemyChance += fWave.rangedChanceIncrease;
        }if(CurrentWave == fWave.waveToRampUp)
        {
            fWave.baseEnemyChance -= (fWave.tankChanceIncrease);
            fWave.tankEnemyChance += fWave.tankChanceIncrease;
        }
        fWave.currentWave = CurrentWave;
        fWave.totalEnemyPerWave = EnemiesToSpawn;
        TrySpawn();
    }
    //check if allowed to spawn. Wont spawn if too many have spawned at once, or all have been spawned
    public void TrySpawn()
    {
        if (EnemiesSpawned - EnemiesKilled < maxZombiesInScene && EnemiesSpawned < EnemiesToSpawn && this != null)//xh add :this != null
        {
            StartCoroutine(SpawnDelay());
        }
    }

    //spawn specifically the base enemy. Will need change once all the enemies are properly implemented
    public void SpawnBaseEnemy()
    {
        int rand = Random.Range(0, spawnList.Count);
        GameObject latestEnemy = Instantiate(BaseEnemy, spawnList[rand]);
        latestEnemy.GetComponent<BaseEnemy>().enemySpawn = this;
        EnemiesSpawned++;
        latestEnemy.transform.SetParent(null,true);
        TrySpawn();
    }
    public void SpawnRangedEnemy()
    {
        int rand = Random.Range(0, spawnList.Count);
        GameObject latestEnemy = Instantiate(RangedEnemy, spawnList[rand]);
        latestEnemy.GetComponent<RangedEnemy>().enemySpawn = this;
        EnemiesSpawned++;
        latestEnemy.transform.SetParent(null, true);
        TrySpawn();
    }
    public void SpawnTankEnemy()
    {
        int rand = Random.Range(0, spawnList.Count);
        GameObject latestEnemy = Instantiate(TankEnemy, spawnList[rand]);
        latestEnemy.GetComponent<TankEnemy>().enemySpawn = this;
        EnemiesSpawned++;
        latestEnemy.transform.SetParent(null, true);
        TrySpawn();
    }

    //called if enemyDies. Is not implemented as enemies cannot die. Is needed
    public void EnemyWasKilled()
    {
        playerHealth.SanityOnKill();
        EnemiesKilled++;
        if(EnemiesKilled >= EnemiesToSpawn)
        {
            if(fWave.currentWave >= 15)
            {
                    SceneManager.LoadScene("YouWonScene");

            }
            else
            {
                WaveEnd.Invoke();
            }
          
         //   NextWave();
        }
        else
        {
            TrySpawn();
        }
    }

    //Delay Spawning
    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(spawnSpeed);
        int rand = Random.Range(0, 100);
        if (rand <= fWave.baseEnemyChance)
        { 
            SpawnBaseEnemy();
        }else if(rand > fWave.baseEnemyChance && rand <= fWave.baseEnemyChance + fWave.rangedEnemyChance) 
        {
            SpawnRangedEnemy();
        }else if (rand > fWave.baseEnemyChance + fWave.rangedEnemyChance)
        {
            SpawnTankEnemy();
        }
        
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
