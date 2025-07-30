using UnityEngine;

[CreateAssetMenu(fileName = "SOSanity", menuName = "Scriptable Objects/SOSanity")]
public class SOSanity : ScriptableObject
{
    public float currentSanity;
    public float maxSanity;
    public float baseMaxSanity;
    public float sanityMins;
    public float sanityGainedOnKill;
}
