using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : MonoBehaviour, IDamageable, IRagDollable
{ 
[SerializeField]float moveSpeed;
[SerializeField] float attackDistance;
    [SerializeField] float minAttackDistance;
    [SerializeField] float attackDistBuffer;
[SerializeField] float attackDamage;
[SerializeField] float attackSpeed;
[SerializeField] float maxHealth;
    [SerializeField] Transform rayOrigin;
    [SerializeField] LayerMask lm;
    [SerializeField] Transform bulletOrigin;
    [SerializeField] GameObject projectilePrefab;
float currentHealth;

Animator animator;
public EnemySpawn enemySpawn;

[SerializeField] float speedPercentVariation;
public bool canAttack = false;
public bool isAttacking = false;
bool isHighArc;
public EnemyState currentState;
public NavMeshAgent agent;
GameObject playerObj;
Transform playerTransform;
List<int> agentTypeIdList = new List<int>();
bool hasJumped = false;
[SerializeField] Ragdoll ragDollScript;
Transform hitPoint;
    bool isAroundCorner = false;
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
        Debug.Log(currentHealth.ToString());
    if (currentHealth <= 0 && currentState != EnemyState.Dead)
    {
        currentState = EnemyState.Dead;

        GetComponent<CapsuleCollider>().enabled = false;
        agent.SetDestination(transform.position);
        agent.ResetPath();
        agent.isStopped = true;
        // agent.enabled = false;
      ragDollScript.AvtivateRagdoll((transform.position - playerTransform.position).normalized, hitPoint.InverseTransformPoint(hitPoint.position), 10f);
    if(enemySpawn != null)
            {
                enemySpawn.EnemyWasKilled();
            }
            StartCoroutine(DecayBody());

        //proper death later
        //  Destroy(gameObject);
    }
}
IEnumerator DecayBody()
{
    yield return new WaitForSeconds(15);
    Destroy(gameObject);
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
        playerTransform = playerObj.transform.GetChild(0);
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
    if(Random.Range(0,2) == 0)
        {
            isHighArc = true;
        }
        else
        {
            isHighArc = false;
        }
    agent.speed = moveSpeed;
    Debug.Log(agent.agentTypeID);
    GenerateAgentIdList();
    agent.agentTypeID = agentTypeIdList[Random.Range(0, agentTypeIdList.Count)];
    agent.stoppingDistance = minAttackDistance;
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
            animator.SetBool("isJump",true);
        }

        hasJumped = true;
    }
    else
    {
        hasJumped = false;
            animator.SetBool("isJump", false);
    }
    if (currentState == EnemyState.Dead)
    {
        return;
    }
    agent.SetDestination(playerTransform.position);
    if (isAttacking)
    {
        agent.speed = 0;
            animator.SetBool("isMove", false);
    }
    else
    {
        agent.speed = moveSpeed;
        animator.SetBool("isMove", true);
    }
    // Debug.Log(agent.velocity.magnitude / moveSpeed);

}
public void Attacking()
{
        animator.SetBool("isMove", false);
        if (!isAttacking)
    {
            if (isHighArc)
            {
                animator.SetBool("isProjectile", true);
            }
            else
            {
                animator.SetBool("isShoot", true);
            }
            //  animator.SetInteger("AtkNumber", Random.Range(0, 3));
            //     animator.SetTrigger("Attack");
            isAttacking = true;
            //StartCoroutine(ShootCor()); 
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
    
    
            GameObject tempBullet = Instantiate(projectilePrefab,bulletOrigin);
            tempBullet.GetComponent<REProjectile>().targetTransform = playerTransform;
            tempBullet.GetComponent<REProjectile>().shootHigh = isHighArc;
       // Debug.Log("hitPlayer");
       //xh code this can been used already
            /*    PlayerHealth playerHealth = FindAnyObjectByType<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }*/
            //xh code end

    
}

    //temp solution while waiting for anim
    IEnumerator ShootCor()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1);
        OnAttemptHit();
        yield return new WaitForSeconds(Random.Range(.5f,3f));
        OnAttackFinish();
    }
    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(Random.Range(0.2f,2f));
        isAttacking = false;
    }
public void OnAttackFinish()
{
        animator.SetBool("isProjectile", false);
        animator.SetBool("isShoot", false);
    StartCoroutine(ShootDelay());
}
// Update is called once per frame
IEnumerator GetAroundCorner()
    {
        isAroundCorner = true;
        yield return new WaitForSeconds(1.5f);
        agent.ResetPath();
    }
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
        if (currentState != EnemyState.Dead)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) < attackDistance + attackDistBuffer)
            {
                RaycastHit hit;
                if (Physics.Raycast(rayOrigin.position, (playerTransform.position - rayOrigin.position).normalized, out hit, Vector3.Distance(rayOrigin.position, playerTransform.position) * 1.1f, ~lm))
                {
                    Debug.DrawRay(rayOrigin.position, (playerTransform.position - rayOrigin.position).normalized, Color.green);
                    if (hit.collider.gameObject.name == "FirstPersonController")
                    {
                        //   Debug.Log("HitPlayer");
                        if (!isAroundCorner)
                        {
                            StartCoroutine(GetAroundCorner());
                        }
                        currentState = EnemyState.Attacking;
                        canAttack = true;
                    }
                    else
                    {
                        //    Debug.Log(hit.collider.gameObject.name);
                        currentState = EnemyState.Moving;
                        canAttack = false;
                        isAroundCorner = false;
                    }
                }

            }
            else
            {
                isAroundCorner = false;
                currentState = EnemyState.Moving;
                canAttack = false;
            }
        }
}
}
