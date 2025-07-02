using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Weapon/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public GameObject gunPrefab;
    public float damage;
}
