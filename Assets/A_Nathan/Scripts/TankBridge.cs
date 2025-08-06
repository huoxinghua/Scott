using UnityEngine;

public class TankBridge : MonoBehaviour
{
    [SerializeField] TankEnemy tEnemy;
    void Start()
    {

    }
    public void OnAttemptHit()
    {
        tEnemy.OnAttemptHit();
    }
    public void OnAttackFinish()
    {
        tEnemy.OnAttackFinish();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
