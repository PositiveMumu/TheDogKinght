// ****************************************************
//     文件：CharacterStats.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/4/25 11:23:22
//     功能：角色数据处理类
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO characterData;

    public AttackData_SO attackData;

    [HideInInspector]
    public bool isCritical;
    #region Read from CharacterData_SO

    public int MaxHealth
    {
        get
        {
            if (characterData != null)
                return characterData.maxHealth;
            return 0;
        }
        set
        {
            if (characterData != null)
                characterData.maxHealth = value;
        }
    }
    
    public int CurrentHealth
    {
        get
        {
            if (characterData != null)
                return characterData.currentHealth;
            return 0;
        }
        set
        {
            if (characterData != null)
                characterData.currentHealth = value;
        }
    }
    
    public int BaseDefence
    {
        get
        {
            if (characterData != null)
                return characterData.baseDefence;
            return 0;
        }
        set
        {
            if (characterData != null)
                characterData.baseDefence = value;
        }
    }
    
    public int CurrentDefence
    {
        get
        {
            if (characterData != null)
                return characterData.currentDefence;
            return 0;
        }
        set
        {
            if (characterData != null)
                characterData.currentDefence = value;
        }
    }

    #endregion

    #region Character Combat

    public void TakeDamage(CharacterStats denfencer)
    {
        int damage = Mathf.Max(CurrentDamage() - denfencer.CurrentDefence, 0);
        denfencer.CurrentHealth = Mathf.Max(denfencer.CurrentHealth - damage, 0);

        if (isCritical)
        {
            denfencer.GetComponent<Animator>().SetTrigger("Hit");
        }
        
        //TODO Update UI
        //TODO Update Experience
        
    }

    private int CurrentDamage()
    {
        float coreDamage = Random.Range(attackData.minDamage, attackData.maxDamage);

        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
        }

        return (int) coreDamage;
    }

    #endregion
}
