using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBtn : MonoBehaviour
{
    public KeyCode actionKecy;
    //按下对应按键使用物品
    SlotHolder currentSlotHolder;
    void Awake()
    {
        currentSlotHolder = GetComponent<SlotHolder>();
    }
    private void Update() 
    {
        //按下按键且格子中有数据
        if(Input.GetKeyDown(actionKecy) && currentSlotHolder.itemUI.GetItem())
        {
            currentSlotHolder.UseItem();
        }
    }
}
