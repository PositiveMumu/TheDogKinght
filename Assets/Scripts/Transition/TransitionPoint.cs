// ****************************************************
//     文件：TransitionPoint.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/11 14:39:42
//     功能：传送门类
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionType
{
    SameScene,
    DifferentScene
}

public class TransitionPoint : MonoBehaviour
{
    [Header("Transition Info")] 
    
    public string SceneName;

    public TransitionType transitionType;

    public DestinationTag destinationTag;
    
    private bool canTrans;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canTrans)
        {
            SceneController.Instance.TransitionToDestination(this);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            canTrans = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            canTrans = false;
    }
}
