using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//创建子集菜单    添加一些变量  “名称”      之后可以在新建中查到到自己设置的菜单
[CreateAssetMenu(fileName ="New Data",menuName ="Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{  
    //写入基本状态
    [Header("Stats Info")]
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;
    [Header("Player")]
    // public int maxElectric;
    // public int currentElectric;
    public int currentBullets;
    public int readyBullets;
    public int magazineClipSize;
    //击杀经验值
    [Header("Kill")]
    public int killPoint;
    //玩家等级相关
    [Header("Level")]
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public float levelBuff;
    //属性增加比例
    public float LevelMultiplier
    {
        //如果为2级 则为2-1=1级 即加上一级buff
        get { return 1 + (currentLevel - 1) * levelBuff; }
    }
    //经验值增加
    public void UpdateExp(int point)
    {
        currentExp += point;
        if (currentExp >= baseExp)
            LevelUp();
    }
    //升级
    private void LevelUp()
    {
        AudioController.Instance.AudioPlay("升级");
        //提升数据的方法       clamp 值 最小 最大  不超过最大登记
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
        //基础经验增加  获得下一阶段需要的经验值
        baseExp += (int)(baseExp * LevelMultiplier);
        //血量增幅   multiplier为百分比  level buff为固定值
        maxHealth = (int)(maxHealth * LevelMultiplier);
        //升级回血
        currentHealth = maxHealth;
        //防御力增加
        currentDefence +=(int)(baseDefence*LevelMultiplier);
        Debug.Log("Up"+currentLevel+currentHealth+currentExp);
        //电量增加
        // if(currentElectric>maxElectric)
        //     currentElectric+=1;

    }
}
