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
    public string name;
    public Sprite sprite;
    public GameObject panel;
    public string Description;
    [Header("Player")]
   
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
