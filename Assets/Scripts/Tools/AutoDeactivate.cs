using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    //�Զ������ӵ�
    //�Ƿ�Ϊ����״̬
    [SerializeField] bool destroyGameObject;
    //��������
    [SerializeField] float lifetime = 3f;
    //�ȴ���������
    WaitForSeconds waitLifetime;

    void Awake()
    {
        waitLifetime = new WaitForSeconds(lifetime);
    }
    //��������˳�� Awake -- Onenable -- Start
    void OnEnable()
    {
        StartCoroutine(DeactivateCoroutine());
    }

    IEnumerator DeactivateCoroutine()
    {
        yield return waitLifetime;

        if (destroyGameObject)
        {
            //���Ҫ�ݻ���Ϸ����
            Destroy(gameObject);
        }
        else 
        {   //�������
            gameObject.SetActive(false);
        }
    }
}
