using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//������չ���������̳�������     �ұ���Ϊ��̬�������
public static class ExtensionMethod 
{    //const����  ���ڱȽϴ�С
    private const float dotThreshold = 0.5f;
    //�Ƿ����Ŀ��        
    //��չд����                    this ����Ӷ�Ӧ����  ���Ÿ��������Ҫ���õı���
   public  static bool IsFacingTarget(this Transform transform,Transform target )
    {
        //��÷�������  ��������λ��
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();
                                //�泯����
        float dot = Vector3.Dot(transform.forward, vectorToTarget);
         //bug: ���۶�Զ���ᱻ����    
        return dot >= dotThreshold;
    }
}
