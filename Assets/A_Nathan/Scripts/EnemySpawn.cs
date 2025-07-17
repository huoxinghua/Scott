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


    [SerializeField] int maxZombiesInScene;
    [SerializeField] float spawnSpeed = 4;
    [SerializeField] int firstWaveEnemyAmount;
    int EnemiesToSpawn;
    int EnemiesSpawned;
    int EnemiesKilled;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemiesToSpawn = firstWaveEnemyAmount;
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
        EnemiesSpawned = 0;
        EnemiesKilled = 0;
        if(CurrentWave != 0)
        {
            EnemiesToSpawn++;
        }
        CurrentWave++;
        TrySpawn();
    }
    //check if allowed to spawn. Wont spawn if too many have spawned at once, or all have been spawned
    public void TrySpawn()
    {
        if (EnemiesSpawned - EnemiesKilled < maxZombiesInScene && EnemiesSpawned < EnemiesToSpawn)
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

    //called if enemyDies. Is not implemented as enemies cannot die. Is needed
    public void EnemyWasKilled()
    {
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
        
            SpawnBaseEnemy();
        
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
