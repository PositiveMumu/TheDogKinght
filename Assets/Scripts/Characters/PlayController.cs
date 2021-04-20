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
    
    // Start is called before the first frame update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

    private void Start()
    {
        MouseManager.Instance.OnMouseClick += MoveToTarget;
    }

    public void MoveToTarget(Vector3 target)
    {
        agent.destination = target;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
