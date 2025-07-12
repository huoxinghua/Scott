using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Weapon/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public GameObject gunPrefab;
    public int ammoCapacity;
    public float fireRate;
    public float damage;
}
