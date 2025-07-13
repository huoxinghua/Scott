using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Weapon/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public GameObject gunPrefab;
    public int ammoCapacity;
    public float fireRate;
    public float damage;
    public float range;
    public float shootCooldown;
    public GameObject cube;
    public GameObject holeFX;
}
