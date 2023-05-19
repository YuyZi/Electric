using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    public GameObject slime;
    public  Vector3 cubeScale;
    private void Awake() 
    {
        InvokeRepeating("Create", 0f, 10f);
    }
    void Update()
    {
    }
    //显示范围  （OnDrawGizmos为一直显示，Selected则为选中目标时显示
    void OnDrawGizmosSelected()
    {  
        //设置颜色
        Gizmos.color = Color.blue;
        //画一个f方块圈来定义范围     需要使用Wire 否则为实心球体    (中心点坐标Vector3值，范围)  
        Gizmos.DrawWireSphere(transform.position, 7.5f);      //自身为中心，视野范围为半径
    }
    public void Create()
    {   
        Vector3 randomPosition = new Vector3(
        Random.Range(transform.position.x - cubeScale.x / 2, transform.position.x + cubeScale.x / 2),
        Random.Range(transform.position.y - cubeScale.y / 2, transform.position.y + cubeScale.y / 2),
        Random.Range(transform.position.z - cubeScale.z / 2, transform.position.z + cubeScale.z / 2)
        );
    PoolManager.Instance.Release(slime,transform.position,Quaternion.identity);
    }

}
