using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bullet : SingletonMono<Bullet>
{   
    //�ӵ�����
    [SerializeField]
    float moveSpeed=10f;
    [SerializeField]
    Transform initialDirection;
    Vector3 moveDirection;
    private void OnEnable() 
    {   
        initialDirection =  GameObject.Find("Player(Clone)/BulletPoint").GetComponent<Transform>();
        // ��Ŀ����Ϸ����������ռ��е�ǰ������ת��Ϊ��ǰ��Ϸ����ľֲ��ռ��е�����
        moveDirection =transform .InverseTransformDirection(initialDirection.forward);
        StartCoroutine(MoveDirectly()); 
    }

    //�ӵ��ƶ�
    IEnumerator MoveDirectly()
    {   initialDirection = null;
        //ѭ�� ����ʱ�᲻ͣ�ƶ�
        while (gameObject.activeSelf)
        {   
            if(moveDirection!=null)
            {
            //�÷���������������� ���Լ�����������
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
