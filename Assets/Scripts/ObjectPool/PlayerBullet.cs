using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    //����
    CharacterStats characterStats;
    private void OnTriggerEnter(Collider enemy) 
    {
        characterStats = GameObject.Find("Player(Clone)").GetComponent<CharacterStats>();
        if(enemy.gameObject.CompareTag("Enemy"))
        {   
            //TODO:�����ж� ȡ����ҿ��ƽű��е�Hit
            //��ʱ����
            var targetStats = enemy.gameObject.GetComponent<CharacterStats>();
             //��������Ŀ����ܻ�Ŀ�귵��ֵ
            targetStats.TakeDamge(characterStats, targetStats);
        }
    }
    public void OnCollisionEnter(Collision enemy) 
    {

    }
}
