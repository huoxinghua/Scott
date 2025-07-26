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

    public float DamageBonus = 1f;
    public int MagazineBonus =1;
    public float FireRateBonus =1f;
    public float SpreadAmountBonus =1f;
    public float RecoilBonus =1f;
    public float ReloadSpeedBonus = 1f;
    public float MoveSpeedBonus =1f;
    public float SanityBonus = 1f;
    public int  ShotsPerShootBonus = 1;
}
