using System;
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


    #region upgrade
    private float bonusHealth = 0;
    public void SetBonusHealth(float bonus)
    {
        if (bonus == 0) return;
        Debug.Log("before upgrade health:" + currentHealth +"bonus:" +bonus);
        float totalbonus = 0f;
        totalbonus += bonus;
        currentHealth = currentHealth * (1+totalbonus);
        Debug.Log("after upgrade health:" + currentHealth);
    }


    #endregion
}
