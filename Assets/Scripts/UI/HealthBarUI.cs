// ****************************************************
//     文件：HealthBarUI.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/4 15:52:44
//     功能：血条管理类
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthBarPre;
    public Transform headBarPos;
    public bool alwaysVisible;
    public float visibleTime;


    private CharacterStats currentStats;
    private Image Slider;
    private Transform uiBar;
    private Transform cam;
    private float timeLeft;
    
    void Awake()
    {
        currentStats = GetComponent<CharacterStats>();
        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }

    private void OnEnable()
    {
        cam = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                uiBar = Instantiate(healthBarPre, canvas.transform).transform;
                Slider = uiBar.GetChild(0).GetComponent<Image>();
                uiBar.gameObject.SetActive(alwaysVisible);
            }
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
        {
            Destroy(uiBar.gameObject);
            return;
        }
        uiBar.gameObject.SetActive(true);
        timeLeft = visibleTime;
        Slider.fillAmount = (float)currentHealth / maxHealth;
    }

    private void LateUpdate()
    {
        if (uiBar != null)
        {
            uiBar.position = headBarPos.position;
            uiBar.forward = -cam.forward;

            if (timeLeft <= 0 && !alwaysVisible)
            {
                uiBar.gameObject.SetActive(false);
            }
            else
            {
                timeLeft -= Time.deltaTime;
            }
        }

        
    }
}
