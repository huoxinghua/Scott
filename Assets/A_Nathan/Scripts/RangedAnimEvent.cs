using UnityEngine;

public class RangedAnimEvent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] RangedEnemy rEnemy;
    void Start()
    {

    }
    public void OnAttemptHit()
    {
        Debug.Log("!");
        rEnemy.OnAttemptHit();
    }
    public void OnAttackFinish()
    {
        Debug.Log("?");
        rEnemy.OnAttackFinish();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
