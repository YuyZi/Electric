using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    //自动禁用子弹
    //是否为禁用状态
    [SerializeField] bool destroyGameObject;
    //生命周期
    [SerializeField] float lifetime = 3f;
    //等待生命周期
    WaitForSeconds waitLifetime;

    void Awake()
    {
        waitLifetime = new WaitForSeconds(lifetime);
    }
    //函数运行顺序 Awake -- Onenable -- Start
    void OnEnable()
    {
        StartCoroutine(DeactivateCoroutine());
    }

    IEnumerator DeactivateCoroutine()
    {
        yield return waitLifetime;

        if (destroyGameObject)
        {
            //如果要摧毁游戏对象
            Destroy(gameObject);
        }
        else 
        {   //否则禁用
            gameObject.SetActive(false);
        }
    }
}
