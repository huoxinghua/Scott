using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Transform shootStartPoint;//fx
    [SerializeField] private WeaponSO[] weapons;
    private GameObject currentWeapon;
    private int currentIndex = 0;

    public float damageAmount;

    [SerializeField] private float shootSpeed;
    private void Awake()
    {
        // shootStartPoint = transform.GetChild(0);
    }

    public void EquipWeapon()
    {
        if (currentWeapon != null)
        {
            // currentWeapon.SetActive(false);
            Destroy(currentWeapon);

        }
        currentIndex = (currentIndex + 1) % weapons.Length;
       // Debug.Log("current index: " + currentIndex);

        currentWeapon = Instantiate(weapons[currentIndex].gunPrefab, transform.position, Quaternion.identity);
        Debug.Log("current weapon: " + currentWeapon.name);
        currentWeapon.transform.SetParent(transform, transform);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;

    }
    public void Shoot()
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
                damageable.TakeDamage(damageAmount);
            }
        }

    }
}
