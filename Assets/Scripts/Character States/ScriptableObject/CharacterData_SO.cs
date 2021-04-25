// ****************************************************
//     文件：CharacterData_SO.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/4/25 11:13:28
//     功能：角色基础数据
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data",menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")] 
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;
}
