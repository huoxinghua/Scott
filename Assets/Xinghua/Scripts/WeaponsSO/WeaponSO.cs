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
    public bool isAutoShoot = true;
    [Header("Crosshair Settings")]
    public Color crosshairNormalColor = Color.white;
    public Color crosshairEnemyColor = Color.red;
    public float crosshairMoveScale = 1.2f;
    public float crosshairIdleScale = 1f;
    public float crosshairFlashDuration = 0.1f;
    public GameObject crosshairCanves;
}
