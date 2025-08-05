using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHealth : MonoBehaviour, IDamageable
{  
    private float health;
    public float maxHealth=100f;
    [SerializeField] SOSanity sanityData;
    [SerializeField] Volume ppVol;
    private void Start()
    {
        maxHealth = sanityData.maxSanity;
        health = sanityData.maxSanity;
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
        sanityData.currentSanity -= a;
        if (sanityData.currentSanity <= 0)
        {
            //playerDies
        }
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
    public void DecreaseSanityOvertime()
    {
        sanityData.currentSanity -= (Time.deltaTime / 0.6f) / sanityData.sanityMins;
    }
    public void SanityOnKill()
    {
        //May change to a percent based system. Currently based on minutes gained
        sanityData.currentSanity += (sanityData.sanityGainedOnKill * 100) / sanityData.sanityMins;
        Mathf.Clamp(sanityData.currentSanity, 0, sanityData.maxSanity);
    }
    public void DamagedSanity(float dmg)
    {
        
    }
    public void HandlePostProcess()
    {
        if (sanityData.currentSanity > sanityData.maxSanity / 2)
        {
            ppVol.weight = Mathf.Lerp(0, 0.2f, 1 - (sanityData.currentSanity - sanityData.maxSanity / 2) / (sanityData.maxSanity / 2));
        }
        else if (sanityData.currentSanity > sanityData.maxSanity / 5)
        {
            ppVol.weight = Mathf.Lerp(0.2f, 0.5f, 1 - ((sanityData.currentSanity - (sanityData.maxSanity / 5))) / (sanityData.maxSanity / (3 + 1 / 3)));
        }
        else
        {
            ppVol.weight = Mathf.Lerp(0.5f, 1f, 1 - (sanityData.currentSanity) / (sanityData.maxSanity / 5));
        }
    }
    // Update is called once per frame
    void Update()
    {
        DecreaseSanityOvertime();
        HandlePostProcess();
    }

}
