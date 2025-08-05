using UnityEngine;

public class ResetWaveMM : MonoBehaviour
{
    [SerializeField] SOWave wave;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        wave.totalEnemyPerWave = wave.startEnemyPerWave;
        wave.currentWave = 0;
        wave.tankEnemyChance = 0;
        wave.baseEnemyChance = 100;
        wave.rangedEnemyChance = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
