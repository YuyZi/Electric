using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonMono<PoolManager>
{ 
    // List<Pool> playerProjectilePools;
    // ArrayList arrayList = new ArrayList();
    public List<Pool> poolList = new List<Pool>();
    //使用字典 键值对匹配       游戏对象 和对象对应的池
    static  Dictionary<GameObject , Pool> dictionary;
    public override void OnInit()
    {
        base.OnInit();
        poolList.Add(new Pool ( Resources.Load<GameObject>("Prefabs/Tool/Bullet"), 10));
        poolList.Add(new Pool ( Resources.Load<GameObject>("Prefabs/Tool/SpecialBullet"), 10));
        //poolList.Add(new Pool ( Resources.Load<GameObject>("Prefabs/Character/Slime"), 10));
        dictionary =new Dictionary<GameObject, Pool>();
        // Initialize(playerProjectilePools);
        //实例化
        Initialize(poolList);
    }
    #if UNITY_EDITOR
    private void OnDestroy() 
    {
        //传入对象池数组，其他对象池数组也需要在这里调用
        CheckPoolSize(poolList);
    }
    #endif
    void CheckPoolSize(List<Pool> pools)
    {
        foreach(var pool in pools)
        {   
            //如果实际尺寸大于初始化尺寸
            if(pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning(string.Format("Pool: {0} has a runtime size {1} bigger than its initial size{2}!",
                pool.Prefab.name,pool.RuntimeSize,pool.Size));
            }
        }
    }
    public void Initialize(List<Pool> pools)
    {
        foreach(var pool in pools)
        {   //预处理指令        仅指定环境下编译执行
            #if    UNITY_EDITOR
            //防止添加相同键    判断是否存在该键    如果有相同键则跳过循环
            if(dictionary.ContainsKey(pool.Prefab))
            {   
                Debug.LogError("Same Prefab In Multiple Pools! Prefab:"+pool.Prefab);
                continue;
            }
            #endif
            //添加键值对    传入对象池预制体  和池本身      初始化池会自动添加键值对
            dictionary.Add(pool.Prefab , pool);
            //新建游戏对象作为父级 
            Transform poolParent = new GameObject("Pool:"+ pool.Prefab.name).transform;
            //脚本挂在对象成为其父级对象
            poolParent.parent =transform;
            pool.Initialize(poolParent);
        }
    }
    ///<summary>
    /// <para> Return a specified<paramref name = "prefab"></paramref>gameObject in the pool.</para>
    /// <para>根据传入的<paramref name = "prefab"></paramref>参数,返回对象池中预备好的游戏对象。</para>
    /// </summary>
    /// <param name = "prefab">
    /// <para>Specified gameObject prefab.</para> 
    /// <para>指定的游戏对象预制体</para>
    /// </param>
    /// <returns>
    /// <para>Prepared gameObject in the pool.</para>
    /// <para>对象池中预备好的游戏对象</para>
    /// </returns>
    public  GameObject Release(GameObject prefab)
    {   
        #if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager Could Not Find Prefab:"+prefab.name);
            return null;
        }
        #endif
        //取得池中预备好的对象
        return dictionary[prefab].PreparedObject();
    }
    ///<summary>
    /// <para> Return a specified<paramref name = "prefab"></paramref>gameObject in the pool.</para>
    /// <para>根据传入的<paramref name = "prefab"></paramref>参数,返回对象池中预备好的游戏对象。</para>
    /// </summary>
    /// <param name = "prefab">
    /// <para>Specified gameObject prefab.</para> 
    /// <para>指定的游戏对象预制体</para>
    /// </param>
    /// <param name ="position">
    /// <para>Specified Release Position.</para>
    /// <para>指定释放位置</para>
    /// </param>
    /// <returns></returns>
    public  GameObject Release(GameObject prefab, Vector3 position)
    {   
        #if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager Could Not Find Prefab:"+prefab.name);
            return null;
        }
        #endif
        //取得池中预备好的对象
        return dictionary[prefab].PreparedObject(position);
    }
    ///<summary>
    /// <para> Return a specified<paramref name = "prefab"></paramref>gameObject in the pool.</para>
    /// <para>根据传入的<paramref name = "prefab"></paramref>参数,返回对象池中预备好的游戏对象。</para>
    /// </summary>
    /// <param name = "prefab">
    /// <para>Specified gameObject prefab.</para> 
    /// <para>指定的游戏对象预制体</para>
    /// </param>
    /// <param name ="position">
    /// <para>Specified Release Position.</para>
    /// <para>指定释放位置</para>
    /// </param>
    /// <param name ="rotation">
    /// <para>Specified Release Rotation.</para>
    /// <para>指定的旋转值</para>
    /// </param>
    /// <returns></returns>
    public  GameObject Release(GameObject prefab,Vector3 position ,Quaternion rotation)
    {   
        #if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager Could Not Find Prefab:"+prefab.name);
            return null;
        }
        #endif
        //取得池中预备好的对象
        return dictionary[prefab].PreparedObject(position ,rotation);
    }
    ///<summary>
    /// <para> Return a specified<paramref name = "prefab"></paramref>gameObject in the pool.</para>
    /// <para>根据传入的<paramref name = "prefab"></paramref>参数,返回对象池中预备好的游戏对象。</para>
    /// </summary>
    /// <param name = "prefab">
    /// <para>Specified gameObject prefab.</para> 
    /// <para>指定的游戏对象预制体</para>
    /// </param>
    /// <param name ="position">
    /// <para>Specified Release Position.</para>
    /// <para>指定释放位置</para>
    /// </param>
    /// <param name ="rotation">
    /// <para>Specified Release Rotation.</para>
    /// <para>指定的旋转值</para>
    /// </param>
    /// <param name ="localScale">
    /// <para>Specified Release LocalScale.</para>
    /// <para>指定的缩放值</para>
    /// </param>
    /// <returns></returns>
    public  GameObject Release(GameObject prefab,Vector3 position ,Quaternion rotation,Vector3 localScale)
    {   
        #if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager Could Not Find Prefab:"+prefab.name);
            return null;
        }
        #endif
        //取得池中预备好的对象
        return dictionary[prefab].PreparedObject(position ,rotation,localScale);
        
    }
    
}
