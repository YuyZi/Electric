using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bullet : SingletonMono<Bullet>
{   
    //子弹父类
    [SerializeField]
    float moveSpeed=10f;
    [SerializeField]
    Transform initialDirection;
    Vector3 moveDirection;
    private void OnEnable() 
    {   
        initialDirection =  GameObject.Find("Player(Clone)/BulletPoint").GetComponent<Transform>();
        // 将目标游戏对象在世界空间中的前方方向转换为当前游戏对象的局部空间中的坐标
        moveDirection =transform .InverseTransformDirection(initialDirection.forward);
        StartCoroutine(MoveDirectly()); 
    }

    //子弹移动
    IEnumerator MoveDirectly()
    {   initialDirection = null;
        //循环 启用时会不停移动
        while (gameObject.activeSelf)
        {   
            if(moveDirection!=null)
            {
            //该方法不依赖刚体组件 可以减少性能消耗
            transform.Translate(moveDirection*moveSpeed*Time.fixedDeltaTime);
            yield return null;
            } 
            else 
            {
                yield return null;
            }

        }
    }

}
