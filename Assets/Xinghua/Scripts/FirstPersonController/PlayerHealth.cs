using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{  
    private float health;
    public float maxHealth=100f;
    FixedSanity fSanity;
    [SerializeField] Volume ppVol;
    [SerializeField] Image hpBar;
    [SerializeField] float healPercentPerWave;
    public void Awake()
    {
        fSanity = FixedSanity.instance;
    }
    private void Start()
    {
        maxHealth = fSanity.maxSanity;
        health = fSanity.maxSanity;
        ApplyUpgrade();
        HealBetweenWave();
    }
    private void ApplyUpgrade()
    {
      //  Debug.Log("before health:" + health);
        var bonus = UpgradeManager.Instance.GetBonus(BonusType.Sanity);
        health = health *(1+ bonus);
        fSanity.maxSanity = fSanity.maxSanity * (1 + bonus);
       // Debug.Log("after health:"+ health);
    }
    public void HealBetweenWave()
    {
        fSanity.currentSanity += fSanity.maxSanity * healPercentPerWave;
        fSanity.currentSanity = Mathf.Clamp(fSanity.currentSanity, 0, fSanity.maxSanity);
    }
    public void SetHealth(float value)
    {
        health = value;
        Debug.Log("player set new health to:" + health);
    }
    public void TakeDamage(float a)
    {
        fSanity.currentSanity -= a;
        if (fSanity.currentSanity <= 0)
        {
            //playerDies
        }
        // Debug.Log("player take damage");
        if (health > a)
        {
            health -= a;
           // Debug.Log("player current health:" + health);
        }
        else
        {
            health = 0;
            // Debug.Log("player die");
            SceneManager.LoadScene("GameOverScene");
        }
    }
    public void DecreaseSanityOvertime()
    {
        fSanity.currentSanity -= (Time.deltaTime / 0.6f) / fSanity.sanityMins;
    }
    public void SanityOnKill()
    {
        //May change to a percent based system. Currently based on minutes gained
        fSanity.currentSanity += (fSanity.sanityGainedOnKill * 100) / fSanity.sanityMins;
        fSanity.currentSanity = Mathf.Clamp(fSanity.currentSanity, 0, fSanity.maxSanity);
    }
    public void DamagedSanity(float dmg)
    {
        
    }
    public void HandlePostProcess()
    {
        if (fSanity.currentSanity > fSanity.maxSanity / 2)
        {
            ppVol.weight = Mathf.Lerp(0, 0.2f, 1 - (fSanity.currentSanity - fSanity.maxSanity / 2) / (fSanity.maxSanity / 2));
        }
        else if (fSanity.currentSanity > fSanity.maxSanity / 5)
        {
            ppVol.weight = Mathf.Lerp(0.2f, 0.5f, 1 - ((fSanity.currentSanity - (fSanity.maxSanity / 5))) / (fSanity.maxSanity / (3 + 1 / 3)));
        }
        else
        {
            ppVol.weight = Mathf.Lerp(0.5f, 1f, 1 - (fSanity.currentSanity) / (fSanity.maxSanity / 5));
        }
    }
    // Update is called once per frame
    void Update()
    {
        DecreaseSanityOvertime();
        HandlePostProcess();
        hpBar.fillAmount = fSanity.currentSanity / fSanity.maxSanity;
    }

}
