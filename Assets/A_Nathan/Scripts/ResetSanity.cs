using UnityEngine;

public class ResetSanity : MonoBehaviour
{
    [SerializeField] SOSanity sanityData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sanityData.maxSanity = sanityData.baseMaxSanity;
        sanityData.currentSanity = sanityData.maxSanity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
