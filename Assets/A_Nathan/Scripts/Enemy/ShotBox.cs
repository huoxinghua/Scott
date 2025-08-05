using UnityEngine;

public class ShotBox : MonoBehaviour, IDamageable, IRagDollable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject baseDmged;
    void Start()
    {
        
    }
    public void DamagePos(Transform pos)
    {
        baseDmged.GetComponent<IRagDollable>().DamagePos(pos);
    }
    public void TakeDamage(float dmg)
    {
        baseDmged.GetComponent<IDamageable>().TakeDamage(dmg);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
