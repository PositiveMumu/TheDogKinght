// ****************************************************
//     文件：EnemyController.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/4/21 22:5:47
//     功能：敌人控制器
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator ani;
    private EnemyStates enemyState;
    

    private bool isWalk;
    private bool isChase;
    private bool isFollow;
    
    [Header("Basic Setting")] 
    
    public float sightRadius;

    public bool isGuard;

    public float IdleTime;

    private float remainIdleTime;
    
    private float runSpeed;
    
    private GameObject attackTarget;
    

    [Header("Patrol State")] 
    
    public float PatrolRange;

    private Vector3 wayPoint;

    private Vector3 guardPoint;
    
    
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        runSpeed = agent.speed;
        guardPoint = transform.position;
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
        SwitchStates();
        SwitchAnimator();
    }

    private void SwitchAnimator()
    {
        ani.SetBool("Walk",isWalk);
        ani.SetBool("Chase",isChase);
        ani.SetBool("Follow",isFollow);
    }

    private void SwitchStates()
    {
        if (FoundPlayer())
        {
            enemyState = EnemyStates.CHASE;
        }
        switch (enemyState)
        {
            case EnemyStates.GUARD:
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
                    agent.destination = attackTarget.transform.position;
                }
                break;
            case EnemyStates.DEAD:
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.blue;
        Gizmos.DrawWireSphere(transform.position,sightRadius);
        Gizmos.DrawWireSphere(transform.position,PatrolRange);
    }
}
