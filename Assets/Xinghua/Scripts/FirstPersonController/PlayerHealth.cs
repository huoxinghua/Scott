using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{  
    private float health;
    public float maxHealth=100f;
 
    private void Start()
    {
        health = maxHealth;
        ApplyUpgrade();
    }
    private void ApplyUpgrade()
    {
      //  Debug.Log("before health:" + health);
        var bonus = UpgradeManager.Instance.GetBonus(BonusType.Sanity);
        health = health *(1+ bonus);
       // Debug.Log("after health:"+ health);
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
