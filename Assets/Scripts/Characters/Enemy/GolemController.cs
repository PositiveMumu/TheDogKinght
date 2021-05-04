// ****************************************************
//     文件：GolemController.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/3 21:30:57
//     功能：石头人Boss控制类
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemController : EnemyController
{
    [Header("Skill")]
    public float kickForce = 25;

    public GameObject rockPre;
    public Transform handPos;
    
    public void Kickoff()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            
            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();

            targetStats.GetComponent<NavMeshAgent>().isStopped = true;
            targetStats.GetComponent<NavMeshAgent>().velocity = kickForce * direction;
            
            targetStats.GetComponent<Animator>().SetTrigger("Dizzy");

            characterStats.TakeDamage(targetStats);
        }
    }

    public void ThrowRock()
    {
        var rock = Instantiate(rockPre, handPos.position, Quaternion.identity);

        if (attackTarget != null)
        {
            rock.GetComponent<Rock>().target = attackTarget;
        }
        else
        {
            rock.GetComponent<Rock>().target=FindObjectOfType<PlayController>().gameObject;
        }
    }
}
