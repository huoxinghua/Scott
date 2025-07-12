using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private WeaponSO gunData;
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
                damageable.TakeDamage(gunData.damage);
                Debug.Log(gunData.name +"gun damage apply:" + gunData.damage);
            }
        }

    }
}

