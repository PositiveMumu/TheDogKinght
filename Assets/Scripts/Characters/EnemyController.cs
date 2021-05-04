// ****************************************************
//     文件：EnemyController.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/4/21 22:5:47
//     功能：敌人控制器
// *****************************************************

using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]

public class EnemyController : MonoBehaviour
{
    
    
    private NavMeshAgent agent;
    private Animator ani;
    protected CharacterStats characterStats;
    private EnemyStates enemyState;
    

    private bool isWalk;
    private bool isChase;
    private bool isFollow;
    private bool isDead;

    private bool playerDead;
    
    [Header("Basic Setting")] 
    
    public float sightRadius;

    public bool isGuard;

    public float IdleTime;

    private float remainIdleTime;
    private float lastAttackTime;
    
    private float runSpeed;
    
    protected GameObject attackTarget;
    

    [Header("Patrol State")] 
    
    public float PatrolRange;

    private Vector3 wayPoint;

    private Vector3 guardPoint;
    private Quaternion guardQuaternion;
    
    
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        
        runSpeed = agent.speed;
        guardPoint = transform.position;
        guardQuaternion = transform.rotation;
    }

    private void Start()
    {
        if (isGuard)
        {
            enemyState = EnemyStates.GUARD;
        }
        else
        {
            enemyState = EnemyStates.PATROL;
            GetNewWayPoint();
        }
    }

    private void Update()
    {
        if (characterStats.CurrentHealth <= 0)
        {
            isDead = true;
        }
        if(!playerDead)
        {
            SwitchStates();
            SwitchAnimator();
            lastAttackTime -= Time.deltaTime;
        }
        
        //todo 添加场景切换后修改
        GameManager.Instance.RegisterGameEndEvent(IEndGameMethod);
    }

    // private void OnEnable()
    // {
    //     GameManager.Instance.RegisterGameEndEvent(IEndGameMethod);
    // }

    private void OnDisable()
    {
        if(!GameManager.IsInitialized) return;
        GameManager.Instance.RemoveGameEndEvent(IEndGameMethod);
    }

    private void SwitchAnimator()
    {
        ani.SetBool("Walk",isWalk);
        ani.SetBool("Chase",isChase);
        ani.SetBool("Follow",isFollow);
        ani.SetBool("Critical",characterStats.isCritical);
        ani.SetBool("Death",isDead);
    }

    private void SwitchStates()
    {
        if (isDead)
        {
            enemyState = EnemyStates.DEAD;
        }
        else if (FoundPlayer())
        {
            enemyState = EnemyStates.CHASE;
        }
        switch (enemyState)
        {
            case EnemyStates.GUARD:
                isChase = false;

                if (transform.position != guardPoint)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPoint;

                    if (Vector3.SqrMagnitude(guardPoint - transform.position) <= agent.stoppingDistance)
                    {
                        isWalk = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation, guardQuaternion, 0.01f);
                    }
                }
                break;
            case EnemyStates.PATROL:
                isChase = false;
                agent.speed = runSpeed * 0.5f;
                if (Vector3.Distance(wayPoint, transform.position) <= 1f)
                {
                    isWalk = false;
                    if (remainIdleTime > 0)
                        remainIdleTime -= Time.deltaTime;
                    else
                        GetNewWayPoint();
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }
                break;
            case EnemyStates.CHASE:
                isWalk = false;
                isChase = true;
                agent.speed = runSpeed;
                if (!FoundPlayer())
                {
                    isFollow = false;
                    if (remainIdleTime > 0)
                    {
                        agent.destination = transform.position;
                        remainIdleTime -= Time.deltaTime;
                    }
                    else if(isGuard)
                    {
                        enemyState = EnemyStates.GUARD;
                    }
                    else
                    {
                        enemyState = EnemyStates.PATROL;
                        GetNewWayPoint();
                    }
                    
                }
                else
                {
                    isFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                }
                
                if (TargetInAttackRange() || TargetInSkillRange())
                {
                    isFollow = false;
                    agent.isStopped = true;

                    if (lastAttackTime < 0)
                    {
                        lastAttackTime = characterStats.attackData.coolDown;
                        
                        //暴击判断
                        characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
                        //执行攻击
                        Attack();
                    }
                }

                break;
            case EnemyStates.DEAD:
                agent.radius = 0;
                GetComponent<Collider>().enabled = false;
                Destroy(gameObject, 2f);
                break;
        }
    }

    private bool FoundPlayer()
    {
        Collider[] temp= Physics.OverlapSphere(transform.position, sightRadius);
        foreach (var VARIABLE in temp)
        {
            if (VARIABLE.CompareTag("Player"))
            {
                attackTarget = VARIABLE.gameObject;
                return true;
            }
                
        }

        attackTarget = null;
        return false;
    }

    private void GetNewWayPoint()
    {
        remainIdleTime = IdleTime;
        
        float randomX = Random.Range(-PatrolRange, PatrolRange);
        float randomZ = Random.Range(-PatrolRange, PatrolRange);

        Vector3 randomPoint = new Vector3(guardPoint.x + randomX, transform.position.y, guardPoint.z + randomZ);

        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, PatrolRange, 1) ? hit.position : transform.position;
    }

    private bool TargetInAttackRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <=
                   characterStats.attackData.attackRange;
        return false;
    }

    private bool TargetInSkillRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <=
                   characterStats.attackData.skillRange;
        return false;
    }

    private void Attack()
    {
        transform.LookAt(attackTarget.transform);
        
        if (TargetInAttackRange())
        {
            ani.SetTrigger("Attack");
        }
        else if(TargetInSkillRange())
        {
            ani.SetTrigger("Skill");
        }
    }
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.blue;
        Gizmos.DrawWireSphere(transform.position,sightRadius);
        Gizmos.DrawWireSphere(transform.position,PatrolRange);
    }
    
    //Animation Event
    private void Hit()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
        
            characterStats.TakeDamage(targetStats);
        }
    }
    
    //PlayerDeadEvent

    private void IEndGameMethod()
    {
        playerDead = true;
        ani.SetBool("Win",true);
        isChase = false;
        isWalk = false;
        attackTarget = null;
    }
}
