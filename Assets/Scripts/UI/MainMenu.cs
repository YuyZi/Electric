using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{   
    Button startBtn,continueBtn,quitBtn;
    PlayableDirector playableDirector;
    private void Awake()
    {
        //获取组件
        startBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();
        //添加事件
        startBtn.onClick.AddListener(PlayTimeline);
        continueBtn.onClick.AddListener(LoadGame);
        quitBtn.onClick.AddListener(QuitGame);

        playableDirector = FindObjectOfType<PlayableDirector>();
        playableDirector.stopped+=NewGame;

    }   
    void PlayTimeline()
    {
        playableDirector.Play();
        AudioController.Instance.AudioPlay("点击");
        AudioController.Instance.AudioPlay("拉闸");
    }
    void NewGame(PlayableDirector director)
    {
        //清除原有数据
        PlayerPrefs.DeleteAll();
        //切换场景 生成角色
        SceneController.Instance.TransitionToFirstScene();

    }
    void LoadGame()
    {
        AudioController.Instance.AudioPlay("点击");
        //转换场景 生成角色
        SceneController.Instance.TransitionToLoadGame();
    }
    
    void QuitGame()
    {
        AudioController.Instance.AudioPlay("点击");
        Application.Quit();
    }
}
