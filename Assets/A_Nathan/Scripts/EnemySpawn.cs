using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
    [SerializeField] SOWave waveData;
    [SerializeField] PlayerHealth playerHealth;
    int EnemiesToSpawn;
    int EnemiesSpawned;
    int EnemiesKilled;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        EnemiesToSpawn = waveData.totalEnemyPerWave;
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
    //start next wave
    public void NextWave()
    {
        CurrentWave = waveData.currentWave;
        EnemiesToSpawn = waveData.totalEnemyPerWave;
        EnemiesSpawned = 0;
        EnemiesKilled = 0;
        if(CurrentWave != 0)
        {
            EnemiesToSpawn+= 1 + (int)(1 * Mathf.Ceil(waveData.currentWave/3));
        }
        CurrentWave++;
        if(CurrentWave == waveData.waveToStartRangedEnemies)
        {
            waveData.baseEnemyChance -= waveData.rangedChanceIncrease;
            waveData.rangedEnemyChance += waveData.rangedChanceIncrease;
        }
        if(CurrentWave == waveData.waveToStartTanks)
        {
            waveData.baseEnemyChance -= (waveData.rangedChanceIncrease + waveData.tankChanceIncrease);
            waveData.tankEnemyChance += waveData.tankChanceIncrease;
            waveData.rangedEnemyChance += waveData.rangedChanceIncrease;
        }if(CurrentWave == waveData.waveToRampUp)
        {
            waveData.baseEnemyChance -= (waveData.tankChanceIncrease);
            waveData.tankEnemyChance += waveData.tankChanceIncrease;
        }
        waveData.currentWave = CurrentWave;
        waveData.totalEnemyPerWave = EnemiesToSpawn;
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
        latestEnemy.GetComponent<BeefCake>().enemySpawn = this;
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
            NextWave();
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
        if (rand <= waveData.baseEnemyChance)
        { 
            SpawnBaseEnemy();
        }else if(rand > waveData.baseEnemyChance && rand <= waveData.baseEnemyChance + waveData.rangedEnemyChance) 
        {
            SpawnRangedEnemy();
        }else if (rand > waveData.baseEnemyChance + waveData.rangedEnemyChance)
        {
            SpawnTankEnemy();
        }
        
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
