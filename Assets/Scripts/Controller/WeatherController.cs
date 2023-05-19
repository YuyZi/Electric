using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : SingletonMono<WeatherController>
{
    //8.开始游戏 为30°      12.为90°
    //材质 17.-  03. 10h  由0-1  0.1       03.-07. 1-0   4h  0.25
    //TODO:下雨
    //下雨概率
    //float rain = 0.2f;
    //设置旋转角度
    int angleX = 15;
    //获取灯光x轴属性
    Transform lightTransform;
    //天空盒
    public Material skyboxMaterial;
    float elapsedTime;
    float duration=60f;
    public override void OnInit()
    {
        base.OnInit();
        //获取当前的天空盒材质
        skyboxMaterial = RenderSettings.skybox;
        skyboxMaterial.SetFloat("_CubemapTransition",0);
        //灯光 -90/270为天最黑  设为0点     初始值从-90开始
        //360°  一天24小时  一小时转15°
        lightTransform = GameObject.Find("Directional Light").transform;  
        // int start_Hour = TimeController.Instance.START_HOUR;
        // //初始灯光角度
        // lightTransform.rotation = Quaternion.Euler((-90 + start_Hour * angleX), 0, 0);
    }
    //初始化灯光
    public void InitializeLight(int start_Hour)
    {
        lightTransform.rotation = Quaternion.Euler((-90 + start_Hour * angleX), 0, 0);
    }
    //根据经过的时间转角度
    public void ChangeLight()
    {
        //防止更换场景后原有不可调用
        lightTransform = GameObject.Find("Directional Light").transform; 

        //更改场景光角度 实现亮度减少效果   //调整全局光来实现昼夜转15°
        lightTransform.Rotate(angleX,0,0); 
        //lightTransform.rotation = Quaternion.Euler(currentHour * angleX, 0, 0);
        //减少玩家生命值
        GameManager.Instance.playerStats.CurrentHealth-=10;
    }
    //天空盒变化
    public void ChangeSkybox()
    {
        StartCoroutine(CubemapTransitionSet(duration));
    }
    IEnumerator CubemapTransitionSet(float time)
    {
        while(elapsedTime<=duration)
        {
            elapsedTime+=Time.fixedDeltaTime;
            float setTransition = Mathf.Lerp(0, 1, elapsedTime / time);
            //FIXME:无法动态更改
            skyboxMaterial.SetFloat("_CubemapTransition",setTransition);
        }
        yield return null;
    }

    //下雨事件！！！
    // public void Rain()
    // {

    // }
}
