// ****************************************************
//     文件：SaveManager.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/11 16:35:41
//     功能：玩家数据保存类
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoSingleton<SaveManager>
{
    private string sceneName="Level";

    public string SceneName
    {
        get { return PlayerPrefs.GetString(sceneName); }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
        }
    }

    public void SavePlayerData()
    {
        if (GameManager.Instance.playerStats != null)
            Save(GameManager.Instance.playerStats.characterData,GameManager.Instance.playerStats.name);
    }
    
    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStats.characterData,GameManager.Instance.playerStats.name);
    }

    public void Save(object obj, string key)
    {
        string jsonData = JsonUtility.ToJson(obj, true);
        PlayerPrefs.SetString(key,jsonData);
        PlayerPrefs.SetString(sceneName,SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }
    
    public void Load(object obj, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key),obj);
        }
    }
}
