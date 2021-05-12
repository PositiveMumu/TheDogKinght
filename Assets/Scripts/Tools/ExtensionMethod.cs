// ****************************************************
//     文件：ExtensionMethod.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/3 20:30:34
//     功能：Tranform拓展方法
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public static class ExtensionMethod
{
    private const float dotThreshold = 0.6f;
    public static bool IsFacingTarget(this Transform transform, Transform target)
    {
        var vectorToTarget = target.position - transform.position;
        
        vectorToTarget.Normalize();

        float dot = Vector3.Dot(vectorToTarget, transform.forward);

        return dot > dotThreshold ;
    }
}
