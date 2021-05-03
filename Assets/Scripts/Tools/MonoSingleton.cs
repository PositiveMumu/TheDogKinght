// ****************************************************
//     文件：MonoSingleton.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/4/25 16:58:18
//     功能：单例模式基类
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T:MonoSingleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance != null)
                return instance;
            return null;
        }
    }

    protected void Awake()
    {
        if(instance!=null)
            Destroy(gameObject);
        instance = (T)this;
    }
    
    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
