using UnityEngine;

public enum GunType
{
    Automatic,
    SpreadShot
}


[CreateAssetMenu(fileName = "WeaponSO", menuName = "Weapon/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public GunType type;
    public GameObject gunPrefab;
    public GameObject bloodPrefab;
 
 
    public GameObject cube;
    public GameObject holeFX;

    [Header("Crosshair Settings")]
    public Color crosshairNormalColor = Color.white;
    public Color crosshairEnemyColor = Color.red;
/*    public float crosshairMoveScale = 1.2f;
    public float crosshairIdleScale = 1f;*/
    public float crosshairFlashDuration = 0.1f;
    public GameObject crosshairCanves;
   
    public float fireRate;// use shootCooldown
    public float damage;
    public float shootCooldown;
    public float spreadAmount;
    public int bulletPerShot;
    public int maxMagazineSize = 30;
    public float recoilAmount;
    //public int ammoStore = 120;
}
