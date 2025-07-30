using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{  
    private float health;
    private void OnEnable()
    {
        if(UpgradeManager.Instance!=null)
        {
            UpgradeManager.Instance.OnPlayerDataUpgradeConfirm += SetHealth;
        }
    }
    private void OnDisable()
    {
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.OnPlayerDataUpgradeConfirm -= SetHealth;
        }
    }
    private void Start()
    {
        health = UpgradeManager.Instance.newHealth;
        Debug.Log("player start health:" + health);
    }
    public void SetHealth(float value)
    {
        health = value;
        Debug.Log("player set new health to:" + health);
    }
    public void TakeDamage(float a)
    {
        // Debug.Log("player take damage");
        if (health > a)
        {
            health -= a;
            Debug.Log("player current health:" + health);
        }
        else
        {
            health = 0;
            // Debug.Log("player die");
        }
    }


}
