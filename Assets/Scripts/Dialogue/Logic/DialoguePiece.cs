using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePiece
{
    //利用ID识别语句
    public string ID;
    //人物图片或奖励图片
    public Sprite image;
    [TextArea]
    public string text;
    public List<DialogueOption> options = new List<DialogueOption>();
    

}
