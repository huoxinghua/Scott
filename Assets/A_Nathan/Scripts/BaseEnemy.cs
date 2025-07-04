using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField]float moveSpeed;
    [SerializeField] float attackDistance;
    [SerializeField] float attackDistBuffer;
    [SerializeField] float attackDamage;
    [SerializeField] float attackSpeed;
    [SerializeField] float health;

   public bool canAttack = false;
    public bool isAttacking = false;
    public EnemyState currentState;
    NavMeshAgent agent;
    GameObject playerObj;
    Transform playerTransform;
    public enum EnemyState
    {
        Moving = 0,
        Attacking = 1
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameObject.Find("FirstPersonController") != null)
        {
            playerObj = GameObject.Find("FirstPersonController");
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.Log("notFound");
        }
        agent = GetComponent<NavMeshAgent>();   
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackDistance;
        currentState = EnemyState.Moving;
    }
    public void Moving()
    {
        agent.SetDestination(playerTransform.position);
        if (isAttacking)
        {
            agent.speed = 0;
        }
        else
        {
            agent.speed = moveSpeed;
        }
    }
    public void Attacking()
    {
        if (!isAttacking)
        {
            StartCoroutine(AttackCoroutine());
            isAttacking = true;
        }
    }
    //likely needs a better way and or needs event from animator
    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attackSpeed);
        if (canAttack)
        {
            Debug.Log("hitPlayer");
            
        }
        isAttacking = false;
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Moving:
                Moving();
              //  Debug.Log("EnemyIsMoving");
                break;

            case EnemyState.Attacking:
                Attacking();
              //  Debug.Log("EnemyIsAttacking");
                break;

            default:
                Debug.Log("Unknown state.");
                break;
        }
        if (Vector3.Distance(transform.position, playerTransform.position) < attackDistance + attackDistBuffer)
        {
            currentState = EnemyState.Attacking;
            canAttack = true;
        }
        else
        {
            currentState = EnemyState.Moving;
            canAttack = false;
        }
    }
}
