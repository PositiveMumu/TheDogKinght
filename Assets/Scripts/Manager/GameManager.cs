// ****************************************************
//     文件：GameManager.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/4/25 17:2:7
//     功能：游戏管理类
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public CharacterStats playerStats;

    private event Action GameEnd;
    
    public void RegisterPlayer(CharacterStats player)
    {
        playerStats = player;

        CinemachineFreeLook cfl = FindObjectOfType<CinemachineFreeLook>();
        if (cfl != null)
        {
            cfl.Follow = player.transform.GetChild(2);
            cfl.LookAt = player.transform.GetChild(2);
        }
    }

    public void RegisterGameEndEvent(Action method)
    {
        GameEnd += method;
    }

    public void RemoveGameEndEvent(Action method)
    {
        GameEnd -= method;
    }

    public void Notify()
    {
        GameEnd?.Invoke();
    }
    
    protected override void OnDestroy()
    {
        SaveManager.Instance.SavePlayerData();
        base.OnDestroy();
    }
}
