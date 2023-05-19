using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    //受伤
    CharacterStats characterStats;
    private void OnTriggerEnter(Collider enemy) 
    {
        characterStats = GameObject.Find("Player(Clone)").GetComponent<CharacterStats>();
        if(enemy.gameObject.CompareTag("Enemy"))
        {   
            //TODO:受伤判断 取代玩家控制脚本中的Hit
            //临时数据
            var targetStats = enemy.gameObject.GetComponent<CharacterStats>();
             //给定攻击目标和受击目标返回值
            targetStats.TakeDamge(characterStats, targetStats);
        }
    }
    public void OnCollisionEnter(Collision enemy) 
    {

    }
}
