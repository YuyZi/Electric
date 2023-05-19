using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionUI : MonoBehaviour
{
    public Text optionText;
    Button thisBtn;
    //所匹配的对话
    DialoguePiece currentPiece;
    //获取下一步的ID
    string nextPieceID;
    private void Awake() 
    {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(OnOptionClick);
    }

    public void UpdateOption(DialoguePiece piece , DialogueOption option)
    {
        currentPiece = piece;
        optionText.text =option.text;
        nextPieceID = option.targetID;
    }
    public void OnOptionClick()
    {
        //如果下一步为空
        if(nextPieceID=="")
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);
            return;
        }
        else
        {
            // DialogueUI.Instance.UpdateMainDialogue(DialogueUI.Instance.currentData.dialoguePieces[nextPieceID]);
            DialogueUI.Instance.UpdateMainDialogue(DialogueUI.Instance.currentData.dialogueIndex[nextPieceID]);
        }
    }
}
