using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    //Ѫ��Ԥ����
    public GameObject healthUIPrefab;
    //����λ��
    public Transform BarPoint;
    //�Ƿ񳤾ÿɼ�
    public bool alwaysVisable;
    //���ӻ�ʱ��
    public float visableTime;
    //��ʱ����ʣ����ʾʱ��
    private float timeLeft;
    Image healthSlider;
    //Ѫ��λ��
    Transform UIBar;
    //���������������
    Transform cam;
    CharacterStats currentStats;
    private void Awake()
    {
        currentStats = GetComponent<CharacterStats>();
        //�޲����ɷ���
        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }
    private void OnEnable()
    {
        cam = Camera.main.transform;
        //FindObjectsOfType<Canvas>  ���UI  �ҵ�ȫ����Canvas  FindObjectsOfType<Canvas>ע��Objects
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            //���ܴ��ڵı׶ˣ��糡���л�������CanvasҲΪWorldSpaceģʽ���ܻ��������ѡ�񴴽�����������ͬ����ק�õ��ñ���������ʹ���������ж�
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                //ʵ����         �õ�Ѫ���������
                UIBar = Instantiate(healthUIPrefab, canvas.transform).transform;
                //��ȡ������     ��Image���
                healthSlider = UIBar.GetChild(0).GetComponent<Image>();
                //����Ϊ�ɼ�
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
        //���˺���ʾѪ��
        UIBar.gameObject.SetActive(true);
        timeLeft = visableTime;
        //��ȡ�ٷֱ�
        float sliderPercent = (float)currentHealth / Maxhealth;
        //���Ĳ���ʵ�ֿ�Ѫ
        healthSlider.fillAmount = sliderPercent;
    }
    //����
    private void LateUpdate()
    {
        //��ֹѪ����ʧ����
        if (UIBar != null)
        {
            UIBar.position = BarPoint.position;
            //��׼�����    
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
