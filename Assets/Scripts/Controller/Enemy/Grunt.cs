using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : EnemyController
{
    [Header("Skil")]
    public float kickForce = 10;
    //技能攻击中的推开玩家
    public void KickOff()
    {   //将AttackTarget 设为protected  则子类可以访问
        if (attackTarget != null)
        {
            transform.LookAt(attackTarget.transform);
            //后退方向
            Vector3 direction = attackTarget.transform.position - transform.position;
            //量化方向-1 ,0 ,1
            direction.Normalize();
            //打断移动
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            //击飞  通常使用rigidbody，也可以直接使用navmashagent     反向击退
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            //让玩家眩晕
            //attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

        }
    }
}
