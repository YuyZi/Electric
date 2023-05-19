using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : SingletonMono<SaveManager>
{
    //��������Ĭ��Ϊ��
    string sceneName="";
    public string SceneName
    { get
        {
            return PlayerPrefs.GetString(sceneName);
        }
    }

    public override void OnInit()
    {
       base.OnInit();
       //SavePlayerData();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.Instance.TransitionToLoadMain();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SavePlayerData();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            LoadPlayerData();
        }

    }
    // protected override void Awake()
    // {
    //     base.Awake();
    // }
    //��
    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    } 
    //��
    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }


    //              �������� ��ֵ
    public void Save(Object data , string key)
    {   
        //������ʱ����  ��ʽת��          prettyPrint
        var jsonData = JsonUtility.ToJson(data,true);
        //��ֵд��ϵͳ
        PlayerPrefs.SetString(key, jsonData);
        //���浱ǰ������
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);
        //д�����
        PlayerPrefs.Save();
    }
    public void Load(Object data, string key)
    {
        //�������
        if (PlayerPrefs.HasKey(key))
        {
            //��ȡ֮ǰ�洢�����ֲ����ظ�����
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
}
