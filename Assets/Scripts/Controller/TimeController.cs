using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : SingletonMono<TimeController>
{   
    //�ı�
    Text timeText;
    Canvas timeCanvas;
    //��ʼʱ��
    protected int START_HOUR ;
    //ÿСʱ������
    private const int MINUTES_PER_HOUR = 60;
    // ��Ϸ��һ���� �������� ����ʵʱ��
    private const int GAME_MINUTES_PER_REAL_SECOND = 1;
    //��ʼʱ��      ��ΪЭ��ͨ��ʱ��
    public  DateTime startTime;
    //������ʱ��
    private int elapsedGameMinutes;
    //������ʱ��
    private DateTime lastUpdateTime;
    //�ϴθ���ʱ�ľ���ʵ����
    private int lastElapsedRealSeconds;
    public int currentHour;
    public int currentMinute;
    public override void OnInit()
    {   
        DontDestroyOnLoad(this.transform.parent);
        START_HOUR =(int) UnityEngine.Random.Range(6,16);
        //�ı�����Panel������
        // GameObject showtime=(GameObject) Resources.Load("Prefabs/UI/Text.prefab");
        // timeText = showtime.GetComponent<Text>();
        timeText = GetComponentInChildren<Text>();
        //��ʼʱ��Ϊ��ǰʱ��UTC
        startTime = DateTime.UtcNow;
        elapsedGameMinutes = 0;
        //��ʼ���ϴθ���ʱ��Ϊ��ʼʱ��
        lastUpdateTime = startTime;
        //��ʼ���ϴθ���ʱ�ľ���ʵ������Ϊ0
        lastElapsedRealSeconds = 0;
        WeatherController.Instance.InitializeLight(START_HOUR);
    }


    private void FixedUpdate()
    { 
        UpdateTime();
    }

    //�ṩ�ⲿ���ʵ�ǰ��Ϸ��ʱ��Ľӿ�
    public int ElapsedGameMinutes
    {
        get { return elapsedGameMinutes; }
        //�ⲿ����
        // TimeController timeController = FindObjectOfType<TimeController>();
        // int currentGameTime = timeController.ElapsedGameMinutes;
        //����ֵΪ���� ������Ϸ��ʼһ�������ķ�����
    }
    
    void UpdateTime()
    {   
        //�����Կ�ʼ��������������ʱ��
        TimeSpan timeSinceStart = DateTime.UtcNow - startTime;
        //ת��Ϊ��
        int elapsedRealSeconds = (int)timeSinceStart.TotalSeconds;
        //ֻ�е�����ʵ�����������仯ʱ�Ž��и���
        if (elapsedRealSeconds != lastElapsedRealSeconds)
        {
            //���㾭������Ϸ�ڷ�����
            elapsedGameMinutes = (int)((float)elapsedRealSeconds / GAME_MINUTES_PER_REAL_SECOND);
            //�����ϴθ���ʱ��
            lastUpdateTime = DateTime.UtcNow;
            //�����ϴθ���ʱ�ľ���ʵ������
            lastElapsedRealSeconds = elapsedRealSeconds;
        }
        else
        {
            //���û�з����仯��ֱ�ӷ���
            return;
        }
        //���㵱ǰ��Ϸ��ʱ��   �����ķ���/ ÿСʱ������  ���ھ�����Сʱ  Ȼ����ϳ�ʼʱ��
        currentHour = (elapsedGameMinutes / MINUTES_PER_HOUR) + START_HOUR;
        //���������һ��Сʱ
        if(elapsedGameMinutes%MINUTES_PER_HOUR==0)
        {
            WeatherController.Instance.ChangeLight();
        }
        if(currentHour==17)
        {   //17�㿪ʼ����
            WeatherController.Instance.ChangeSkybox();
        }
        //WeatherController.Instance.ChangeRotation(elapsedGameMinutes/60);
        //24��������һ�� Ϊ00
        if(currentHour==24)
            currentHour = 00;
        
        currentMinute = elapsedGameMinutes % MINUTES_PER_HOUR;

        //��ʱ����ָ����ʽ��ʾ��UI��
        timeText.text = String.Format("{0:00}:{1:00}", currentHour, currentMinute);
        //�Զ��ҵ��ı�
    }
}