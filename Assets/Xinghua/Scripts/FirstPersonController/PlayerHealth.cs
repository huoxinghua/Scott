using UnityEngine;

public class PlayerHealth : MonoBehaviour,IDamageable
{
   [SerializeField]private float maxHealth = 100;
   private float currentHealth;
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float a)
    {
       // Debug.Log("player take damage");
        if (currentHealth > a)
        {
            currentHealth -= a;
            Debug.Log("player current health:"+ currentHealth);
        }
        else
        {
            currentHealth = 0;
           // Debug.Log("player die");
        }
    }

}
