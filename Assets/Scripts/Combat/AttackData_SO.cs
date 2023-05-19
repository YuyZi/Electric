using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    //��ս������Χ
    public float attackRange;
    //Զ�̹�����Χ
    public float skillRange;
    //CD��ȴʱ��
    public float collDown;
    //�˺���ֵ����С ���
    public int minDamge;
    public int maxDamge;
    //�������
    //�����ӳɰٷֱ�
    public float criticalMultilier;
    //������        1Ϊ�ٷְٱ���  ͬ��
    public float criticalChance;
    //���Ա��      ͬ���ʹ�õ���
    public void ApplyWeaponData(AttackData_SO weapon)
    {
        attackRange = weapon.attackRange;
        skillRange = weapon.skillRange;
        minDamge = weapon.minDamge;
        maxDamge = weapon.maxDamge;
        collDown = weapon.collDown;
        criticalChance = weapon.criticalChance;
        criticalMultilier = weapon.criticalMultilier;

    }

}
