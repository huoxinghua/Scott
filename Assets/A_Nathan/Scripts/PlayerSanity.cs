using UnityEngine;
using UnityEngine.Rendering;

public class PlayerSanity : MonoBehaviour
{
    [SerializeField] SOSanity sanityData;
    [SerializeField] Volume ppVol;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void DecreaseSanityOvertime()
    {
        sanityData.currentSanity -= (Time.deltaTime / 0.6f) / sanityData.sanityMins;
    }
    public void SanityOnKill()
    {
        //May change to a percent based system. Currently based on minutes gained
        sanityData.currentSanity += (sanityData.sanityGainedOnKill*100) / sanityData.sanityMins;
        Mathf.Clamp(sanityData.currentSanity, 0, sanityData.maxSanity);
    }
    public void DamagedSanity(float dmg)
    {
        sanityData.currentSanity -= dmg;
        if(sanityData.currentSanity <= 0)
        {
            //playerDies
        }
    }
    public void HandlePostProcess()
    {
        if(sanityData.currentSanity > sanityData.maxSanity / 2)
        {
            ppVol.weight = Mathf.Lerp(0, 0.2f, 1-(sanityData.currentSanity-sanityData.maxSanity/2) / (sanityData.maxSanity/2));
        }
        else if(sanityData.currentSanity > sanityData.maxSanity / 5)
        {
            ppVol.weight = Mathf.Lerp(0.2f, 0.5f, 1 - ((sanityData.currentSanity - (sanityData.maxSanity/5)) / (sanityData.maxSanity / (3 + 1 / 3)));
        }
        else
        {
            ppVol.weight = Mathf.Lerp(0.5f, 1f, 1 - (sanityData.currentSanity ) / (sanityData.maxSanity /5));
        }
    }
    // Update is called once per frame
    void Update()
    {
        DecreaseSanityOvertime();
        HandlePostProcess();
    }
}
