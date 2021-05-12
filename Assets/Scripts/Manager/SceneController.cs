// ****************************************************
//     文件：SceneController.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/11 14:59:48
//     功能：场景管理类
// *****************************************************

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneController : MonoSingleton<SceneController>
{
    public GameObject playerPrefabs;
    public FadeUI fadeCanvasPrefabs;

    private bool fadeFinished;
    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        fadeFinished = false;
        GameManager.Instance.RegisterGameEndEvent(LoadStartScene);
    }

    private void OnDisable()
    {
        GameManager.Instance.RemoveGameEndEvent(LoadStartScene);
    }

    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.SceneName, transitionPoint.destinationTag));
                break;
        }
    }

    IEnumerator Transition(string sceneName, DestinationTag destinationTag)
    {
        FadeUI fadeUI = Instantiate(fadeCanvasPrefabs);
        SaveManager.Instance.SavePlayerData();
        yield return StartCoroutine(fadeUI.FadeOutScene(2f));
        if (!SceneManager.GetActiveScene().name.Equals(sceneName))
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(playerPrefabs, GetDestination(destinationTag).transform.position,
                GetDestination(destinationTag).transform.rotation);
            SaveManager.Instance.LoadPlayerData();
        }
        else
        {
            GameObject player = GameManager.Instance.playerStats.gameObject;
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position,GetDestination(destinationTag).transform.rotation);
            player.GetComponent<NavMeshAgent>().enabled = true;
        }
        yield return StartCoroutine(fadeUI.FadeInScene(2f));
    }

    public void LoadGameScene(string sceneName)
    {
        StartCoroutine(TransitionToGameScene(sceneName));
    }
    
    private IEnumerator TransitionToGameScene(string sceneName)
    {
        FadeUI fadeUI = Instantiate(fadeCanvasPrefabs);
        if (sceneName != "")
        {
            yield return StartCoroutine(fadeUI.FadeOutScene(2f));
            yield return SceneManager.LoadSceneAsync(sceneName);
            foreach (var VARIABLE in FindObjectsOfType<TransitionDestination>())
            {
                if (VARIABLE.destinationTag == DestinationTag.ENTER)
                {
                    yield return Instantiate(playerPrefabs, VARIABLE.transform.position,
                        VARIABLE.transform.rotation);
                    break;
                }
            }
            yield return StartCoroutine(fadeUI.FadeInScene(2f));
        }
    }

    public void LoadStartScene()
    {
        if (!fadeFinished)
        {
            StartCoroutine(TransitionToStartScene());
            fadeFinished = true;
        }
        
    }
    
    private IEnumerator TransitionToStartScene()
    {
        FadeUI fadeUI = Instantiate(fadeCanvasPrefabs);
        yield return StartCoroutine(fadeUI.FadeOutScene(2f));
        yield return SceneManager.LoadSceneAsync("StartScene");
        PlayerPrefs.DeleteAll();
        yield return StartCoroutine(fadeUI.FadeInScene(2f));
    }

    private TransitionDestination GetDestination(DestinationTag destinationTag)
    {
        TransitionDestination[] temps = FindObjectsOfType<TransitionDestination>();

        foreach (var VARIABLE in temps)
        {
            if (VARIABLE.destinationTag == destinationTag)
                return VARIABLE;
        }

        return null;
    }
}
