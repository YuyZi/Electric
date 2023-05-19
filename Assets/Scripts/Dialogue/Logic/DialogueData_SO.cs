using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Dialogue",menuName ="Dialogue/Dialogue Data")]
public class DialogueData_SO :ScriptableObject
{
    public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>(); 
    //利用字典匹配ID和内容
    public Dictionary<string,DialoguePiece> dialogueIndex = new Dictionary<string, DialoguePiece>();

#if UNITY_EDITOR
//仅在编辑器内执行导致打包游戏后字典空了
//当数据被更改时调用
    private void OnValidate() 
    {
        dialogueIndex.Clear();
        foreach(var piece in dialoguePieces)
        {
            if(!dialogueIndex.ContainsKey(piece.ID))
            {
                dialogueIndex.Add(piece.ID,piece);
            }
        }
    }
#else
//保证在打包执行的游戏里第一时间获得对话的所有字典匹配 
    void Awake()
    {
        dialogueIndex.Clear();
        foreach (var piece in dialoguePieces)
        {
            if (!dialogueIndex.ContainsKey(piece.ID))
                dialogueIndex.Add(piece.ID, piece);
        }
    }
#endif
}
