using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//使用DOTween
using DG.Tweening;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;
    
    [Header("Basic Elements")]
    public Image icon;
    public Text mainText;
    public Button nextButton;
    public GameObject dialoguePanel;
    [Header("Option")]
    public RectTransform optionPanel;
    public OptionUI optionPrefab;
    [Header("Data")]
    public DialogueData_SO currentData;
    //默认播放
    int currentIndex =0;
    [Header("Tips")]
    public GameObject tipsPanel;
    public Button tipsButton;
    private void Awake() 
    {   
        //TODO:改为UI单例 框架  自动生成UI
        Instance = this;     
        nextButton.onClick.AddListener(ContinueDialogue); 
        //tipsPanel.SetActive(true);
        tipsButton.onClick.AddListener(CloseDialogue);
    }   
    void ContinueDialogue()
    {
        if(currentIndex<currentData.dialoguePieces.Count)
            //点击按键后更新文本
            UpdateMainDialogue(currentData.dialoguePieces[currentIndex]);
        else
            dialoguePanel.SetActive(false);
    }
    void CloseDialogue()
    {
        tipsPanel.SetActive(false);
    }
    //更新数据
    public void UpdateDialogueData(DialogueData_SO data)
    {
        currentData=data;
        //保证每次都重头开始
        currentIndex = 0;
    }
    //更新对话框
    public void UpdateMainDialogue(DialoguePiece piece)
    {
        dialoguePanel.SetActive(true);
        currentIndex++;
        if(piece.image!=null)
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else
        {
            icon.enabled= false;
        }
        //清空原有对话，更改文本内容
        mainText.text="";
        //mainText.text=piece.text;
        // 文本内容 以及需要的显示时间
        mainText.DOText(piece.text,1f);
        //如果没有其他可选项
        if(piece.options.Count==0 && currentData.dialoguePieces.Count>0)
        {
            nextButton.interactable = true;
            nextButton.gameObject.SetActive(true);
            nextButton.transform.GetChild(0).gameObject.SetActive(true);

        }
        else
        {
            //导致自动调整发生偏移  改为关闭文本
            // nextButton.gameObject.SetActive(false);
            nextButton.interactable = false;
            nextButton.transform.GetChild(0).gameObject.SetActive(false);
        }

        //创建options
        CreateOptions(piece);
    }

    void CreateOptions(DialoguePiece piece)
    {
        //销毁原有子物体 再重新创建
        //如果现有选择大于1 则删除所有
        if(optionPanel.childCount > 0)
        {
            for(int i = 0;i<optionPanel.childCount;i++)
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }
        }
        //如果当前对话有可选项则重新创建 
        for(int i = 0;i<piece.options.Count;i++)
        {
            //生成按钮并设定父级
            var option = Instantiate(optionPrefab,optionPanel);
            //传入变量
            option.UpdateOption(piece,piece.options[i]);
        }
    }
}   
