using UnityEngine;

public class AnimationBridge : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] BaseEnemy bEnemy;
    void Start()
    {
        
    }
    public void OnAttemptHit()
    {
        bEnemy.OnAttemptHit();
    }
    public void OnAttackFinish()
    {
        bEnemy.OnAttackFinish();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
