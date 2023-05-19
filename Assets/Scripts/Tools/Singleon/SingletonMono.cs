using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//泛型  约束
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T mInstance;
    public static T Instance
    {
        get
        {   
            //不需要主动拖入场景 在调用时会自动生成
            if (mInstance == null)
            {   
                //生成新对象
                GameObject obj = new GameObject();
                //命名
                obj.name = typeof(T).ToString();
                //加载场景时不删除
                DontDestroyOnLoad(obj);
                //创造空对象 给予类型
                mInstance = obj.AddComponent<T>();
            }
            return mInstance;
        }
    }
    //继承且可重写的虚函数
    protected virtual void Awake()
    {
        OnInit();
    }
    public virtual void OnInit()
    {
        //DontDestroyOnLoad(this);
    }
}
