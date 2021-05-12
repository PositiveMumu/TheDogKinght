// ****************************************************
//     文件：FadeUI.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/12 21:37:1
//     功能：渐变UI管理类
// *****************************************************

using System;
using System.Collections;
using UnityEngine;

public class FadeUI : MonoBehaviour
{
    public float fadeInDuration = 2f;
    public float fadeOutDuration = 2f;
        
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator FadeInScene(float time)
    {
        while (canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            
            yield return null;
        }
        
        Destroy(gameObject);
    }

    public IEnumerator FadeOutScene(float time)
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            
            yield return null;
        }
    }
}
