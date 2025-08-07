using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankEnemy : MonoBehaviour , IDamageable, IRagDollable
{
    [SerializeField] float moveSpeed;
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
    public NavMeshAgent agent;
    GameObject playerObj;
    Transform playerTransform;
    List<int> agentTypeIdList = new List<int>();
    bool hasJumped = false;
    [SerializeField] Ragdoll ragDollScript;
    Transform hitPoint;
    [SerializeField] Material disperseShaderBody;
    [SerializeField] Renderer bodyRend;

    [SerializeField] Material disperseShaderTeeth;
    [SerializeField] Renderer teethRend;
    [SerializeField] string paraName;
    bool doDecay = false;
    float decayProgress = 3f;
    [SerializeField] float DecaySpeed;
    public enum EnemyState
    {
        Moving = 0,
        Attacking = 1,
        Dead = 2
    }
    public void OnDestroy()
    {

    }
    public void DamagePos(Transform hitPos)
    {
        hitPoint = hitPos;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && currentState != EnemyState.Dead)
        {
            currentState = EnemyState.Dead;

            GetComponent<CapsuleCollider>().enabled = false;
            agent.SetDestination(transform.position);
            agent.ResetPath();
            agent.isStopped = true;
            // agent.enabled = false;
            ragDollScript.AvtivateRagdoll((transform.position - playerTransform.position).normalized, hitPoint.InverseTransformPoint(hitPoint.position), 10f);
            enemySpawn.EnemyWasKilled();
            StartCoroutine(DecayBody());

            //proper death later
            //  Destroy(gameObject);
        }
    }
    IEnumerator DecayBody()
    {
        yield return new WaitForSeconds(5f);
        doDecay = true;
        yield return new WaitForSeconds(7);
        Destroy(gameObject);
    }
    public void DecayShader()
    {
     /*   bodyRend.material = disperseShaderBody;
        teethRend.material = disperseShaderTeeth;
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        MaterialPropertyBlock mpbt = new MaterialPropertyBlock();

        bodyRend.GetPropertyBlock(mpb);

        decayProgress -= Time.deltaTime * DecaySpeed;
        mpb.SetFloat(paraName, decayProgress);
        bodyRend.SetPropertyBlock(mpb);

        teethRend.GetPropertyBlock(mpbt);
        mpbt.SetFloat(paraName, decayProgress);
        teethRend.SetPropertyBlock(mpbt);*/
    }
    public void Awake()
    {
        animator = transform.GetComponentInChildren<Animator>();
        currentHealth = maxHealth;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    virtual public void Start()
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

        if (SpeedChange < 0)
        {
            moveSpeed -= (moveSpeed * Mathf.Abs(SpeedChange));
        }
        else
        {
            moveSpeed += (moveSpeed * SpeedChange);
        }
        agent.speed = moveSpeed;
        //   Debug.Log(agent.agentTypeID);
        GenerateAgentIdList();
        agent.agentTypeID = agentTypeIdList[Random.Range(0, agentTypeIdList.Count)];
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
        if (agent.isOnOffMeshLink)
        {
            if (!hasJumped)
            {
                animator.SetTrigger("Jump");
            }

            hasJumped = true;
        }
        else
        {
            hasJumped = false;
        }
        if (currentState == EnemyState.Dead)
        {
            return;
        }
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
            animator.SetInteger("AtkNumber", Random.Range(0, 2));
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
            // Debug.Log("hitPlayer");
            //xh code this can been used already
            /*    PlayerHealth playerHealth = FindAnyObjectByType<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }*/
            //xh code end
            if (playerObj.GetComponent<IDamageable>() != null)
            {
                playerObj.GetComponent<IDamageable>().TakeDamage(attackDamage);
            }
        }
    }
    public void OnAttackFinish()
    {
        isAttacking = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (doDecay)
        {
            DecayShader();
        }
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
            case EnemyState.Dead:
                break;
            default:
                Debug.Log("Unknown state.");
                break;
        }
        if (currentState != EnemyState.Dead)
        {
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
}
