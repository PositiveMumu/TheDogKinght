// ****************************************************
//     文件：Rock.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/4 10:4:30
//     功能：石头武器类
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public enum RockStates
{
    HitPlayer,
    HitEnemy,
    HitNothing
}

public class Rock : MonoBehaviour
{
    
    
    [Header("Basic Setting")] 
    
    public float force = 15;
    public int damage = 10;
    public GameObject breakEffect;
    
    public GameObject target;
    public RockStates rockState;
    
    private Rigidbody rb;
    private Vector3 direction;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rockState = RockStates.HitPlayer;
        rb.velocity=Vector3.one;
        FlyToTarget();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude < 1f)
        {
            rockState = RockStates.HitNothing;
        }
    }

    private void FlyToTarget()
    {
        direction = (target.transform.position - transform.position+Vector3.up).normalized;
        rb.AddForce(force*direction,ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (rockState)
        {
            case RockStates.HitPlayer:
                if (other.collider.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = force * direction;
                    
                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage);

                    rockState = RockStates.HitNothing;
                }
                break;
            case RockStates.HitEnemy:
                if (other.gameObject.GetComponent<GolemController>())
                {
                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage);
                    Instantiate(breakEffect, transform.position, quaternion.identity);
                    Destroy(this.gameObject);
                }
                break;
            case RockStates.HitNothing:
                break;
        }
    }
}
