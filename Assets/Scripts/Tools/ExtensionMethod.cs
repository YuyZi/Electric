using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//所有拓展方法均不继承其他类     且必须为静态方便调用
public static class ExtensionMethod 
{    //const常量  用于比较大小
    private const float dotThreshold = 0.5f;
    //是否面对目标        
    //拓展写法，                    this 后面加对应的类  逗号隔开后加需要调用的变量
   public  static bool IsFacingTarget(this Transform transform,Transform target )
    {
        //获得方向向量  （获得相对位置
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();
                                //面朝方向
        float dot = Vector3.Dot(transform.forward, vectorToTarget);
         //bug: 无论多远都会被击飞    
        return dot >= dotThreshold;
    }
}
