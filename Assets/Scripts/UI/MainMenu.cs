// ****************************************************
//     文件：MainMenu.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/11 22:27:20
//     功能：主界面管理类
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Button newBtn;
    private Button continueBtn;
    private Button quitBtn;

    private PlayableDirector playableDirector;
    private void Awake()
    {
        newBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();
        playableDirector = FindObjectOfType<PlayableDirector>();
        
        newBtn.onClick.AddListener(NewGameWithTimeline);
        continueBtn.onClick.AddListener(LoadGameWithTimeline);
        quitBtn.onClick.AddListener(QuitGame);
    }

    private void NewGameWithTimeline()
    {
        playableDirector.Play();
        playableDirector.stopped -= NewGame;
        playableDirector.stopped -= LoadGame;

        playableDirector.stopped += NewGame;
    }
    
    private void LoadGameWithTimeline()
    {
        if(SaveManager.Instance.SceneName.Equals("")) return;
        playableDirector.Play();
        playableDirector.stopped -= NewGame;
        playableDirector.stopped -= LoadGame;

        playableDirector.stopped += LoadGame;
    }

    private void NewGame(PlayableDirector director)
    {
        PlayerPrefs.DeleteAll();
        SceneController.Instance.LoadGameScene("GameScene");
    }

    private void LoadGame(PlayableDirector director)
    {
        SceneController.Instance.LoadGameScene(SaveManager.Instance.SceneName);
    }
    
    private void QuitGame()
    {
        Application.Quit();
    }
}
