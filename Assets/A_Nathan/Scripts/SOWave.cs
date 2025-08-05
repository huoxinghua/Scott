using UnityEngine;

[CreateAssetMenu(fileName = "SOWave", menuName = "Scriptable Objects/SOWave")]
public class SOWave : ScriptableObject
{
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
    
}
