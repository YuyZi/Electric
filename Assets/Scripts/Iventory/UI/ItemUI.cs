using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//孙级  最低级 即UI与Text文本
public class ItemUI : MonoBehaviour
{
    public Image icon =null;
    public Text amount = null;
    //可读 可写
    public InventoryData_SO Bag{    get ;   set ;}
    //初始值为0，避免设置时错位 设置初始值为-1
    public int Index {get; set;}    =   -1;
    //更新数据
    public void SetupItemUI(ItemData_SO item,int itemAmount)
    {
        if(itemAmount==0)
        {
            Bag.items[Index].itemData=null;
            icon.gameObject.SetActive(false);
            return;
        }
        if(item !=null)
        {
            icon.sprite = item.itemIcon;
            amount.text = itemAmount.ToString();
            //更新完毕数据后显示图片
            icon.gameObject.SetActive(true);

        }
        else
        {
            icon.gameObject.SetActive(false);
        }
    }

    //快捷获取物品
    public ItemData_SO GetItem()
    {
        return Bag.items[Index].itemData;
    }
}
