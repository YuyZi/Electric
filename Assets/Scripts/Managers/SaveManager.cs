using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : SingletonMono<SaveManager>
{
    //场景名，默认为空
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
    //存
    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    } 
    //读
    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }


    //              数据类型 键值
    public void Save(Object data , string key)
    {   
        //设置临时变量  格式转换          prettyPrint
        var jsonData = JsonUtility.ToJson(data,true);
        //将值写入系统
        PlayerPrefs.SetString(key, jsonData);
        //保存当前场景名
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);
        //写入磁盘
        PlayerPrefs.Save();
    }
    public void Load(Object data, string key)
    {
        //如果存在
        if (PlayerPrefs.HasKey(key))
        {
            //获取之前存储的数字并返回给对象
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
}
