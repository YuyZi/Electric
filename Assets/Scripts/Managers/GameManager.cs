using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : SingletonMono<GameManager>
{
    //相机追踪    想要哪个相机选择哪个属性，如追踪相机属性为FreeLook 
    private CinemachineFreeLook followCamera;
    //获取角色信息      设为public 方便其他对象访问  集中管理
    public CharacterStats playerStats;
    //设置列表
    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();
    // //加载时不删除该物体
    // protected override void Awake()
    // {
    //     base.Awake();
    //     DontDestroyOnLoad(this);
    // }
    public override void OnInit()
    {
        base.OnInit();
    }
    //观察者模式反向注册  生成时告诉所有对象
    public void RigisterPlayer(CharacterStats player)
    {
        playerStats = player;
        //查找相机
        followCamera = FindObjectOfType<CinemachineFreeLook>();
        if (followCamera != null)
        {   //获得玩家子物体中需要追踪的点
            followCamera.Follow = playerStats.transform.GetChild(7);
            followCamera.LookAt = playerStats.transform.GetChild(7);
        }
    }

    //敌人生成时加入列表，死亡时移除
    public void AddObserver(IEndGameObserver observer)
    {
        //敌人启用时才判断，不用担心重复，所以不用事先判断是否已有
        endGameObservers.Add(observer);
    }
    public void RemoveObserver(IEndGameObserver observer)
    {
        //敌人启用时才判断，不用担心重复，所以不用事先判断是否已有
        endGameObservers.Remove(observer);
    }
    //观察者广播      保证调用接口的脚本都能执行各自的方法
    public void NotifyObservers()
    {
        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }

    //找到出生入口坐标
    public Transform GetEnterance()
    {
        //遍历获得所有传送门出点
        foreach(var item in FindObjectsOfType<TransitionDestination>())
        {   //进行匹配并返回值
            if (item.destinationTag == TransitionDestination.DestinationTag.Entrance)
                return item.transform;
        }
        return null;
    }
}
