using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Inventory",menuName ="Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(ItemData_SO newItemData, int amount)
    {
        bool found = false;
        //如果可以堆叠且背包中已存在该物体
        if(newItemData.stackable)
        {
            foreach(var item in items)
            {
                if(item.itemData==newItemData)
                {
                    item.amount+=amount;
                    found = true;
                    break;
                }
            }
        }
        //背包中不存在物品，找到最近的空格 加入物品
        for(int i =0;i<items.Count;i++)
        {
            if(items[i].itemData==null && !found)
            {
                items[i].itemData=newItemData;
                items[i].amount=amount;
                break;
            }
        }


    }


}
[System.Serializable]
public class InventoryItem
{
    public ItemData_SO itemData;
    public int amount;
}