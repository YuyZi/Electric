using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//父级   最高级  格子背景
public class ContainerUI : MonoBehaviour
{
    public SlotHolder[] slotHolders;

    //生成物品
    public void RefreshUI()
    {   
        //循环格子长度  从0 开始 匹配InventoryData_SO  items列表序号
        for(int i =0;i<slotHolders.Length;i++)
        {   
            //设置编号 从0 开始 设置编号  Index初始值为-1  设置后为0 方便匹配InventoryData中的items列表
            slotHolders[i].itemUI.Index = i;
            //拿到序号后更新
            slotHolders[i].UpdateItem();
        }
    }
    
}
