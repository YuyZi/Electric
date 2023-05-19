using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//因 未继承MONO 所以需要加上Serializable特性才能显示pool中的序列化字段
// [System.Serializable]
public class Pool
{
    //属性 返回变量值
    public GameObject Prefab { get { return prefab; } }
    //对象池尺寸
    public int Size => size;
    //运行时尺寸
    public int RuntimeSize => queue.Count;
    //预制体
    // [SerializeField]
    GameObject prefab;

    // [SerializeField]
    int size = 10;

    //使用队列方法  先进先出
    Queue<GameObject> queue;

    // 父级变量
    Transform parent;

    public Pool(GameObject prefab, int size){
        this.prefab = prefab;
        this.size = size;
    }


    //创建
    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();
        //父级 赋值
        this.parent = parent;
        for (var i = 0; i < size; i++)
        {
            //在队列末尾添加        入列
            queue.Enqueue(Copy());
        }
    }
    GameObject Copy()
    {
        //实例化                预制体生成时指定父级对象
        var obj = GameObject.Instantiate(prefab, parent);
        //取消启用
        obj.SetActive(false);
        //返回对象
        return obj;
    }
    //生成
    GameObject AvailableObjcet()
    {
        GameObject availableObject = null;
        //当队列元素大于0     且队列第一个元素非启用  否则入列会返回已启用的对象
        //  Peek  返回队列第一个元素 但不会移除他
        if (queue.Count > 0 && !queue.Peek().activeSelf)
        {
            availableObject = queue.Dequeue();
        }
        else
        {
            //出列
            availableObject = Copy();
        }
        //出列后马上入列 就不用单独写返回方法
        queue.Enqueue(availableObject);
        return availableObject;
    }
    //启用      方法重载
    public GameObject PreparedObject()
    {
        GameObject preparedObject = AvailableObjcet();
        preparedObject.SetActive(true);
        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObjcet();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = AvailableObjcet();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        GameObject preparedObject = AvailableObjcet();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;
        return preparedObject;
    }
    //返回       在生成后出列后马上入列 就不用单独写返回方法
    // public void Return(GameObject gameObject)
    // {
    //     queue.Enqueue(gameObject);


    // }
}
