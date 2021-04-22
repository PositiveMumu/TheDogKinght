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

public class PlayController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator ani;

    private GameObject AttackTarget;
    private float lastTimeAttack;

    // Start is called before the first frame update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
    }

    private void Start()
    {
        MouseManager.Instance.OnGroundClick += MoveToTarget;
        MouseManager.Instance.OnAttackClick += EventAttack;
    }

    public void MoveToTarget(Vector3 target)
    {
        if(ani.GetCurrentAnimatorClipInfo(0)[0].clip.name=="Attack01")
            ani.SetTrigger("StopAttack");

        StopAllCoroutines();
        agent.isStopped = false;
        agent.destination = target;
    }

    private void EventAttack(GameObject target)
    {
        if (target != null)
        {
            AttackTarget = target;
            StartCoroutine(AttackToTarget());
        }
    }

    private IEnumerator AttackToTarget()
    {
        agent.isStopped = false;
        
        //攻击距离固定，后期可以修改
        while (Vector3.Distance(transform.position, AttackTarget.transform.position)>1.5f)
        {
            agent.destination = AttackTarget.transform.position;
            yield return null;
        }
        
        transform.LookAt(AttackTarget.transform);
        agent.isStopped = true;

        if (lastTimeAttack <= 0)
        {
            ani.SetTrigger("Attack");
            lastTimeAttack = 0.5f;
        }

    }

    private void SetPlayerMoveAnimation()
    {
        ani.SetFloat("Speed",agent.velocity.sqrMagnitude);
    }
    

    // Update is called once per frame
    void Update()
    {
        SetPlayerMoveAnimation();

        lastTimeAttack -= Time.deltaTime;
        
    }
}
