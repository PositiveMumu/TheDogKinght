// ****************************************************
//     文件：PlayerHealthUI.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/9 22:53:34
//     功能：主界面玩家UI控制类
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    private Text levelText;
    private Image healthSlider;
    private Image expSlider;
    
    void Awake()
    {
        levelText = transform.GetChild(0).GetComponent<Text>();
        healthSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(2).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        levelText.text = "Level " + GameManager.Instance.playerStats.characterData.currentLevel;
        UpdateHealth();
        UpdateExp();
    }

    void UpdateHealth()
    {
        float sliderPercent = (float) GameManager.Instance.playerStats.CurrentHealth /
                              GameManager.Instance.playerStats.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    void UpdateExp()
    {
        float sliderPercent = (float) GameManager.Instance.playerStats.characterData.currentExp /
                              GameManager.Instance.playerStats.characterData.baseExp;
        expSlider.fillAmount = sliderPercent;
    }
    
}
