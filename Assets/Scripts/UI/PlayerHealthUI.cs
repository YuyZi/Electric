using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    Text levelText;
    Image healthSlider;
    Image expSlider;
    Image elecSlider;
    Text bulletText;
    private void Awake()
    {   DontDestroyOnLoad(this);
        //��������� ���϶��±��Ϊ0��1��2
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        elecSlider = transform.GetChild(2).GetChild(0).GetComponent<Image>();
        levelText = transform.GetChild(3).GetComponent<Text>();
        bulletText = transform.GetChild(4).GetComponent<Text>();
    }
    private void Update()
    {
        
        //bug  ��ɫ������ʱ�ᱨ���޷���ȡ״̬
        if (GameManager.Instance.playerStats != null)
        {
            //�����ı������Լ��ı���ʽΪ00
            levelText.text = "Level  " + GameManager.Instance.playerStats.characterData.currentLevel.ToString("00");

            bulletText.text =GameManager.Instance.playerStats.characterData.currentBullets.ToString("00") + "/"
             + GameManager.Instance.playerStats.characterData.readyBullets.ToString("00");
            UpdateHealth();
            UpdateExp();
            //TODO:��������
            //UpdateElec();
        }

    } 
    private void UpdateHealth()
    {
        //��ðٷֱ�
        float sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth / 
         GameManager.Instance.playerStats.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    } 
    private void UpdateExp()
    {
        //��ðٷֱ�
        float sliderPercent = (float)GameManager.Instance.playerStats.characterData.currentExp / 
         GameManager.Instance.playerStats.characterData.baseExp;
        expSlider.fillAmount = sliderPercent;
    }
    //TODO:CD��ȴд������
    // public void UpdateCoolDown()
    // {

    // }
    // private void UpdateElec()
    // {
    //     //��ðٷֱ�
    //     float sliderPercent = (float)GameManager.Instance.playerStats.characterData.currentElectric /
    //      GameManager.Instance.playerStats.characterData.maxElectric;
    //     expSlider.fillAmount = sliderPercent;
    // }
}
