using UnityEngine;

public enum ModuleType
{
    Stock,        
    Magazine,    
    Barrel,    
    Trigger,     
    Receiver,    
    Accessory,    
    Armor,        
    Boots,       
    Medicine,     

}

[System.Serializable]
public class ModuleStats
{
    public ModuleType Type;
    public string Name;
    public string Description;
    public GameObject sprite;
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
