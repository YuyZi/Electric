using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO currentData;
    public DialogueData_SO nextData;
    bool canTalk =false;
    // private void Awake() 
    // {
    //     currentData =(DialogueData_SO) Resources.Load("Game Data/Dialogue/New Talk.asset");
    // }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player")&& currentData!=null)
        {
            canTalk = true;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        //离开时关闭对话框
        if(other.CompareTag("Player"))
        {
            canTalk = false;
            DialogueUI.Instance.dialoguePanel.SetActive(false);
        }
    }
    private void Update() 
    {
        if(canTalk && Input.GetKeyDown(KeyCode.E))
        {
            OpenDialogue();
        }
    }
    void OpenDialogue()
    {
        //打开UI面板
        //传入对话信息
        DialogueUI.Instance.UpdateDialogueData(currentData);
        DialogueUI.Instance.UpdateMainDialogue(currentData.dialoguePieces[0]);
    }
}
