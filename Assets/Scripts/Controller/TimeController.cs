using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : SingletonMono<TimeController>
{   
    //文本
    Text timeText;
    Canvas timeCanvas;
    //开始时间
    protected int START_HOUR ;
    //每小时分钟数
    private const int MINUTES_PER_HOUR = 60;
    // 游戏中一分钟 ———— 显现实时间
    private const int GAME_MINUTES_PER_REAL_SECOND = 1;
    //开始时间      作为协调通用时间
    public  DateTime startTime;
    //经过的时间
    private int elapsedGameMinutes;
    //最后更新时间
    private DateTime lastUpdateTime;
    //上次更新时的经过实际秒
    private int lastElapsedRealSeconds;
    public int currentHour;
    public int currentMinute;
    public override void OnInit()
    {   
        DontDestroyOnLoad(this.transform.parent);
        START_HOUR =(int) UnityEngine.Random.Range(6,16);
        //文本处于Panel子物体
        // GameObject showtime=(GameObject) Resources.Load("Prefabs/UI/Text.prefab");
        // timeText = showtime.GetComponent<Text>();
        timeText = GetComponentInChildren<Text>();
        //开始时间为当前时间UTC
        startTime = DateTime.UtcNow;
        elapsedGameMinutes = 0;
        //初始化上次更新时间为开始时间
        lastUpdateTime = startTime;
        //初始化上次更新时的经过实际秒数为0
        lastElapsedRealSeconds = 0;
        WeatherController.Instance.InitializeLight(START_HOUR);
    }


    private void FixedUpdate()
    { 
        UpdateTime();
    }

    //提供外部访问当前游戏内时间的接口
    public int ElapsedGameMinutes
    {
        get { return elapsedGameMinutes; }
        //外部访问
        // TimeController timeController = FindObjectOfType<TimeController>();
        // int currentGameTime = timeController.ElapsedGameMinutes;
        //返回值为整数 代表游戏开始一来经过的分钟数
    }
    
    void UpdateTime()
    {   
        //计算自开始运行以来经过的时间
        TimeSpan timeSinceStart = DateTime.UtcNow - startTime;
        //转换为秒
        int elapsedRealSeconds = (int)timeSinceStart.TotalSeconds;
        //只有当经过实际秒数发生变化时才进行更新
        if (elapsedRealSeconds != lastElapsedRealSeconds)
        {
            //计算经过的游戏内分钟数
            elapsedGameMinutes = (int)((float)elapsedRealSeconds / GAME_MINUTES_PER_REAL_SECOND);
            //更新上次更新时间
            lastUpdateTime = DateTime.UtcNow;
            //更新上次更新时的经过实际秒数
            lastElapsedRealSeconds = elapsedRealSeconds;
        }
        else
        {
            //如果没有发生变化，直接返回
            return;
        }
        //计算当前游戏内时间   经过的分钟/ 每小时分钟数  等于经过的小时  然后加上初始时间
        currentHour = (elapsedGameMinutes / MINUTES_PER_HOUR) + START_HOUR;
        //如果经过了一个小时
        if(elapsedGameMinutes%MINUTES_PER_HOUR==0)
        {
            WeatherController.Instance.ChangeLight();
        }
        if(currentHour==17)
        {   //17点开始变天
            WeatherController.Instance.ChangeSkybox();
        }
        //WeatherController.Instance.ChangeRotation(elapsedGameMinutes/60);
        //24分钟重置一次 为00
        if(currentHour==24)
            currentHour = 00;
        
        currentMinute = elapsedGameMinutes % MINUTES_PER_HOUR;

        //将时间以指定格式显示在UI上
        timeText.text = String.Format("{0:00}:{1:00}", currentHour, currentMinute);
        //自动找到文本
    }
}