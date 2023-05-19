using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�����Ӽ��˵�    ���һЩ����  �����ơ�      ֮��������½��в鵽���Լ����õĲ˵�
[CreateAssetMenu(fileName ="New Data",menuName ="Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{  
    //д�����״̬
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
    //��ɱ����ֵ
    [Header("Kill")]
    public int killPoint;
    //��ҵȼ����
    [Header("Level")]
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public float levelBuff;
    //�������ӱ���
    public float LevelMultiplier
    {
        //���Ϊ2�� ��Ϊ2-1=1�� ������һ��buff
        get { return 1 + (currentLevel - 1) * levelBuff; }
    }
    //����ֵ����
    public void UpdateExp(int point)
    {
        currentExp += point;
        if (currentExp >= baseExp)
            LevelUp();
    }
    //����
    private void LevelUp()
    {
        AudioController.Instance.AudioPlay("����");
        //�������ݵķ���       clamp ֵ ��С ���  ���������Ǽ�
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
        //������������  �����һ�׶���Ҫ�ľ���ֵ
        baseExp += (int)(baseExp * LevelMultiplier);
        //Ѫ������   multiplierΪ�ٷֱ�  level buffΪ�̶�ֵ
        maxHealth = (int)(maxHealth * LevelMultiplier);
        //������Ѫ
        currentHealth = maxHealth;
        //����������
        currentDefence +=(int)(baseDefence*LevelMultiplier);
        Debug.Log("Up"+currentLevel+currentHealth+currentExp);
        //��������
        // if(currentElectric>maxElectric)
        //     currentElectric+=1;

    }
}
