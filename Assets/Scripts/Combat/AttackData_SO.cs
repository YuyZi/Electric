using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    //近战攻击范围
    public float attackRange;
    //远程攻击范围
    public float skillRange;
    //CD冷却时间
    public float collDown;
    //伤害阈值（最小 最大）
    public int minDamge;
    public int maxDamge;
    //暴击相关
    //暴击加成百分比
    public float criticalMultilier;
    //暴击率        1为百分百暴击  同理
    public float criticalChance;
    //属性变更      同理可使用叠加
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
