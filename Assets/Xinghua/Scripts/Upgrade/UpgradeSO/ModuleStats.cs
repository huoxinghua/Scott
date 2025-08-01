using UnityEngine;

public enum ModuleType
{
    Good,        
    Evil,    
    Neutral,    
}

[System.Serializable]
public class ModuleStats
{
    public ModuleType Type;
    [Header("Random Panel")]
    public GameObject panel;
    public string name;
    public Sprite sprite;
    public string Description;
    [Header("Player")]
   //bonus
    public float SanityBonus;
    public float MoveSpeedBonus;
    [Header("Weapon")]
    public float DamageBonus ;
   
    public float FireRateBonus ;
    public float SpreadAmountBonus ;
    public float RecoilBonus ;
    public float ReloadSpeedBonus;

    public int MagazineBonus;
    public int  ShotsPerShootBonus ;
}
