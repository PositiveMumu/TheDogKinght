// ****************************************************
//     文件：GruntController.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/3 18:55:17
//     功能：兽人战士控制器
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GruntController : EnemyController
{
    [Header("Skill")] 
    
    public float kickForce = 15;

    public void Kickoff()
    {
        transform.LookAt(attackTarget.transform);

        Vector3 direction = attackTarget.transform.position - transform.position;
        direction.Normalize();

        attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
        attackTarget.GetComponent<NavMeshAgent>().velocity = kickForce * direction;
        attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
    }
}
