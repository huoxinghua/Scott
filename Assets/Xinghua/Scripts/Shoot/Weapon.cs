using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Transform shootStartPoint;//fx
    [SerializeField] public WeaponSO[] weapons;
    private GameObject currentWeapon;
    public Gun currentGun;
    private int currentIndex = 0;
    [SerializeField]public GameObject crossHair;
    private void Awake()
    {
        // shootStartPoint = transform.GetChild(0);
    }
 
    private void Start()
    {
      currentGun = GetComponentInChildren<Gun>();
      //  EquipWeapon();
      //  crossHair.SetActive(false);
    }
    public void EquipWeapon()
    {
        if (currentWeapon != null)
        {
             currentWeapon.SetActive(false);
            Destroy(currentWeapon);

        }
        currentIndex = (currentIndex + 1) % weapons.Length;
        Debug.Log("current index: " + currentIndex);
        if (weapons[currentIndex] == null)
        {
            Debug.LogError($"weapons[{currentIndex}] is null");
            return;
        }
        currentWeapon = Instantiate(weapons[currentIndex].gunPrefab, transform.position, Quaternion.identity);
        currentGun =currentWeapon.GetComponent<Gun>();
        Debug.Log("current weapon: " + currentWeapon.name);
        currentWeapon.transform.SetParent(transform, transform);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
        crossHair.gameObject.SetActive(true);

    }
/*    public void Shoot()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red, 1.0f);
        if (Physics.Raycast(ray, out hit, 20f))
        {
            Debug.Log("Hit " + hit.collider.name);
      
            var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(currentGun.);
            }
        }

    }*/
}
