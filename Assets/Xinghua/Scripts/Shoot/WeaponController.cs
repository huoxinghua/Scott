using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Transform shootStartPoint;//fx
    [SerializeField] public WeaponSO[] weapons;
    private Queue<GameObject> guns = new Queue<GameObject>();
  //  private GameObject currentWeapon;
   // public Transform weaponContainer;
    public Gun currentGun;
   // private int currentIndex = 0;
    [SerializeField] public GameObject crossHair;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private void Awake()
    {
        // shootStartPoint = transform.GetChild(0);
    }
    private void Start()
    {
        currentGun = GetComponentInChildren<Gun>();
        spawnPosition = currentGun.transform.position;
        spawnRotation = currentGun.transform.rotation;
        guns.Enqueue(currentGun.gameObject);
        var another = Instantiate(weapons[1].gunPrefab, transform.position, Quaternion.identity);
        another.gameObject.SetActive(false);
        another.transform.SetParent(transform);
        another.transform.position = spawnPosition;
        another.transform.rotation = spawnRotation;

        guns.Enqueue(another);
    }
    public void EquipWeapon()
    {
        if (currentGun != null)
        {
            currentGun.gameObject.SetActive(false);

            if (!guns.Contains(currentGun.gameObject))
            {
                guns.Enqueue(currentGun.gameObject);
            }
        }
        var newWeapon = guns.Dequeue();
        newWeapon.SetActive(true);
        currentGun = newWeapon.GetComponent<Gun>();
    }

}
