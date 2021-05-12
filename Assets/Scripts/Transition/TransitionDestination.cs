// ****************************************************
//     文件：TransitionDestination.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/11 14:39:58
//     功能：传送终点类
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DestinationTag
{
    ENTER,
    A,
    B
}

public class TransitionDestination : MonoBehaviour
{
    public DestinationTag destinationTag;
}
