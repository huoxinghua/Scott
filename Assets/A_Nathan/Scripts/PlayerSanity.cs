using UnityEngine;
using UnityEngine.Rendering;

public class PlayerSanity : MonoBehaviour
{
    // [SerializeField] SOSanity sanityData;
    FixedSanity fSanity;
    [SerializeField] Volume ppVol;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void Awake()
    {
        fSanity = FixedSanity.instance;
    }
    public void DecreaseSanityOvertime()
    {
        fSanity.currentSanity -= (Time.deltaTime / 0.6f) / fSanity.sanityMins;
    }
    public void SanityOnKill()
    {
        //May change to a percent based system. Currently based on minutes gained
        fSanity.currentSanity += (fSanity.sanityGainedOnKill*100) / fSanity.sanityMins;
        Mathf.Clamp(fSanity.currentSanity, 0, fSanity.maxSanity);
    }
    public void DamagedSanity(float dmg)
    {
        fSanity.currentSanity -= dmg;
        if(fSanity.currentSanity <= 0)
        {
            //playerDies
        }
    }
    public void HandlePostProcess()
    {
        if(fSanity.currentSanity > fSanity.maxSanity / 2)
        {
            ppVol.weight = Mathf.Lerp(0, 0.2f, 1-(fSanity.currentSanity- fSanity.maxSanity/2) / (fSanity.maxSanity/2));
        }
        else if(fSanity.currentSanity > fSanity.maxSanity / 5)
        {
            ppVol.weight = Mathf.Lerp(0.2f, 0.5f, 1 - ((fSanity.currentSanity - (fSanity.maxSanity/5))) / (fSanity.maxSanity / (3 + 1 / 3)));
        }
        else
        {
            ppVol.weight = Mathf.Lerp(0.5f, 1f, 1 - (fSanity.currentSanity ) / (fSanity.maxSanity /5));
        }
    }
    // Update is called once per frame
    void Update()
    {
        DecreaseSanityOvertime();
        HandlePostProcess();
    }
}
