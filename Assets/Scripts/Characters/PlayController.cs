// ****************************************************
//     文件：PlayController.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/4/20 22:37:36
//     功能：玩家控制类
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PlayController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator ani;
    private CharacterStats characterStats;

    private GameObject AttackTarget;
    private float lastTimeAttack;

    private bool isDead;

    private float stopDistance;

    // Start is called before the first frame update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();

        stopDistance = agent.stoppingDistance;
    }

    private void Start()
    {
        MouseManager.Instance.OnGroundClick += MoveToTarget;
        MouseManager.Instance.OnAttackClick += EventAttack;
    }

    void Update()
    {
        isDead = characterStats.CurrentHealth <= 0;
        
        if(isDead)
            GameManager.Instance.Notify();
        SetPlayerMoveAnimation();

        lastTimeAttack -= Time.deltaTime;
        
    }
    
    public void MoveToTarget(Vector3 target)
    {
        if(isDead) return;
        if(ani.GetCurrentAnimatorClipInfo(0)[0].clip.name=="Attack01"||
           ani.GetCurrentAnimatorClipInfo(0)[0].clip.name=="Attack02")
            ani.SetTrigger("StopAttack");

        StopAllCoroutines();
        agent.isStopped = false;
        agent.stoppingDistance = stopDistance;
        agent.destination = target;
    }

    private void EventAttack(GameObject target)
    {
        if(isDead) return;
        if (target != null)
        {
            AttackTarget = target;
            characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
            StartCoroutine(AttackToTarget());
        }
    }

    private IEnumerator AttackToTarget()
    {
        agent.isStopped = false;
        agent.stoppingDistance = characterStats.attackData.attackRange;
        while (Vector3.Distance(transform.position, AttackTarget.transform.position)>characterStats.attackData.attackRange)
        {
            agent.destination = AttackTarget.transform.position;
            yield return null;
        }
        
        transform.LookAt(AttackTarget.transform);
        agent.isStopped = true;

        if (lastTimeAttack <= 0)
        {
            ani.SetBool("Critical", characterStats.isCritical);
            ani.SetTrigger("Attack");
            lastTimeAttack = characterStats.attackData.coolDown;
        }

    }

    private void SetPlayerMoveAnimation()
    {
        ani.SetFloat("Speed",agent.velocity.sqrMagnitude);
        ani.SetBool("Death",isDead);
    }
    

    //Animation Event
    void Hit()
    {
        if (AttackTarget.GetComponent<Rock>()&&AttackTarget.GetComponent<Rock>().rockState==RockStates.HitNothing)
        {
            AttackTarget.GetComponent<Rock>().rockState = RockStates.HitEnemy;
            AttackTarget.GetComponent<Rigidbody>().velocity=Vector3.one;
            AttackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20f, ForceMode.Impulse);
        }
        else
        {
            var targetStats = AttackTarget.GetComponent<CharacterStats>();
            characterStats.TakeDamage(targetStats);
        }
    }
}
