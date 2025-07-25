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

    public float DamageBonus;
    public float MagazineBonus;
    public float FireRateBonus;
    public float SpreadAmountBonus;
    public float RecoilBonus;
    public float ReloadSpeedBonus;
    public float MoveSpeedBonus;
    public float SanityBonus;
    public int ShotsPerShootBonus;
}
