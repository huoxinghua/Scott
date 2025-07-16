using UnityEngine;

[CreateAssetMenu(fileName = "SOEnemyData", menuName = "Scriptable Objects/SOEnemyData")]
public class SOEnemyData : ScriptableObject
{
    public float moveSpeed;
    public float attackDistance;
    public float attackDistBuffer;
    public float attackSpeed;
    public float damage;
    public float health;
}
