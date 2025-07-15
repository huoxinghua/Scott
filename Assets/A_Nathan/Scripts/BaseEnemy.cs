using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class BaseEnemy : MonoBehaviour , IDamageable
{
    [SerializeField]float moveSpeed;
    [SerializeField] float attackDistance;
    [SerializeField] float attackDistBuffer;
    [SerializeField] float attackDamage;
    [SerializeField] float attackSpeed;
    [SerializeField] float maxHealth;
    float currentHealth;

    Animator animator;
    public EnemySpawn enemySpawn;

    [SerializeField] float speedPercentVariation;
   public bool canAttack = false;
    public bool isAttacking = false;
    public EnemyState currentState;
    NavMeshAgent agent;
    GameObject playerObj;
    Transform playerTransform;
    List<int> agentTypeIdList = new List<int>();
    public enum EnemyState
    {
        Moving = 0,
        Attacking = 1
    }
    public void OnDestroy()
    {
        enemySpawn.EnemyWasKilled();
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            //proper death later
            Destroy(gameObject);
        }
    }
    public void Awake()
    {
        animator = transform.GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
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
        float SpeedChange = Random.Range(-speedPercentVariation, speedPercentVariation);

        if(SpeedChange < 0)
        {
      moveSpeed -= (moveSpeed * Mathf.Abs(SpeedChange));
        }
        else
        {
       moveSpeed += (moveSpeed * SpeedChange);
        }
        agent.speed = moveSpeed;
        Debug.Log(agent.agentTypeID);
        GenerateAgentIdList();
        agent.agentTypeID = agentTypeIdList[Random.Range(0,agentTypeIdList.Count)];
        agent.stoppingDistance = attackDistance;
        currentState = EnemyState.Moving;
    }
    public void GenerateAgentIdList()
    {
        agentTypeIdList.Clear();
        for (int i = 0; i < NavMesh.GetSettingsCount(); i++)
        {
            agentTypeIdList.Add(NavMesh.GetSettingsByIndex(i).agentTypeID);
        }
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
            animator.SetFloat("Speed", agent.velocity.magnitude / moveSpeed);
        }
       // Debug.Log(agent.velocity.magnitude / moveSpeed);
        
    }
    public void Attacking()
    {
        animator.SetFloat("Speed", 0);
        if (!isAttacking)
        {
            animator.SetInteger("AtkNumber", Random.Range(0, 3));
            animator.SetTrigger("Attack");
            isAttacking = true;
           
        }
    }
    //likely needs a better way and or needs event from animator
   /* IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attackSpeed);
       
        isAttacking = false;
    }*/
    public void OnAttemptHit()
    {
        if (canAttack)
        {
            Debug.Log("hitPlayer");

        }
    }
    public void OnAttackFinish()
    {
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
