// ****************************************************
//     文件：AttackData_SO.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/4/25 11:31:43
//     功能：攻击数据
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack",menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;
    public float skillRange;
    public float coolDown;
    public float minDamage;
    public float maxDamage;
    public float criticalMultiplier;
    public float criticalChance;
}
