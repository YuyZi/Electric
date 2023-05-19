using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    //血条预制体
    public GameObject healthUIPrefab;
    //生成位置
    public Transform BarPoint;
    //是否长久可见
    public bool alwaysVisable;
    //可视化时间
    public float visableTime;
    //计时器，剩余显示时间
    private float timeLeft;
    Image healthSlider;
    //血条位置
    Transform UIBar;
    //保持与摄像机反向
    Transform cam;
    CharacterStats currentStats;
    private void Awake()
    {
        currentStats = GetComponent<CharacterStats>();
        //修补生成方法
        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }
    private void OnEnable()
    {
        cam = Camera.main.transform;
        //FindObjectsOfType<Canvas>  多个UI  找到全部的Canvas  FindObjectsOfType<Canvas>注意Objects
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            //可能存在的弊端，如场景中还有其他Canvas也为WorldSpace模式可能会出错，可以选择创建单独变量，同样拖拽拿到该变量，或者使用名字来判断
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                //实例化         拿到血条坐标参数
                UIBar = Instantiate(healthUIPrefab, canvas.transform).transform;
                //获取子物体     的Image组件
                healthSlider = UIBar.GetChild(0).GetComponent<Image>();
                //设置为可见
                UIBar.gameObject.SetActive(alwaysVisable);

                 
            }
        }
    }
    private void UpdateHealthBar(int currentHealth, int Maxhealth)
    {
        if (currentHealth <= 0)
        {
            Destroy(UIBar.gameObject);
        }
        //受伤后显示血条
        UIBar.gameObject.SetActive(true);
        timeLeft = visableTime;
        //获取百分比
        float sliderPercent = (float)currentHealth / Maxhealth;
        //更改参数实现扣血
        healthSlider.fillAmount = sliderPercent;
    }
    //跟随
    private void LateUpdate()
    {
        //防止血条消失报错
        if (UIBar != null)
        {
            UIBar.position = BarPoint.position;
            //对准摄像机    
            UIBar.forward = -cam.forward;
            if (timeLeft <= 0 && !alwaysVisable)
            {
                UIBar.gameObject.SetActive(false);

            }
            else
            {
                timeLeft -= Time.deltaTime;
            }
        }
    }
}
