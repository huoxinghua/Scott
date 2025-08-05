using Unity.VisualScripting;
using UnityEngine;

public class HeadShotBox : MonoBehaviour, IDamageable, IRagDollable
{
    [SerializeField] GameObject baseDmged;
    [SerializeField] float headShotMult;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void DamagePos(Transform pos)
    {
        baseDmged.GetComponent<IRagDollable>().DamagePos(pos);
    }
    public void TakeDamage(float dmg)
    {
        Debug.Log("HEADSHOT");
        baseDmged.GetComponent<IDamageable>().TakeDamage(dmg*headShotMult);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
