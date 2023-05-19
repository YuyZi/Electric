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
        //��ȡ���
        startBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();
        //����¼�
        startBtn.onClick.AddListener(PlayTimeline);
        continueBtn.onClick.AddListener(LoadGame);
        quitBtn.onClick.AddListener(QuitGame);

        playableDirector = FindObjectOfType<PlayableDirector>();
        playableDirector.stopped+=NewGame;

    }   
    void PlayTimeline()
    {
        playableDirector.Play();
        AudioController.Instance.AudioPlay("���");
        AudioController.Instance.AudioPlay("��բ");
    }
    void NewGame(PlayableDirector director)
    {
        //���ԭ������
        PlayerPrefs.DeleteAll();
        //�л����� ���ɽ�ɫ
        SceneController.Instance.TransitionToFirstScene();

    }
    void LoadGame()
    {
        AudioController.Instance.AudioPlay("���");
        //ת������ ���ɽ�ɫ
        SceneController.Instance.TransitionToLoadGame();
    }
    
    void QuitGame()
    {
        AudioController.Instance.AudioPlay("���");
        Application.Quit();
    }
}
