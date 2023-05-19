using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//子级  中级  格子本身
//类型  
public enum SlotType{ BAG ,WEAPON,ACTION,Light}
//                                      判断鼠标点击次数的接口
public class SlotHolder : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType slotType;
    public ItemUI itemUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        //双击使用      使用取余数为0判断双击两下
        if(eventData.clickCount%2==0)
        {
            UseItem();
        }
    }
    //鼠标放置物品上
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemUI.GetItem())
        {
            InventoryManager.Instance.toolTip.SetupTooltip(itemUI.GetItem());
            InventoryManager.Instance.toolTip.gameObject.SetActive(true);
        }
    }
    //离开
    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.toolTip.gameObject.SetActive(false);
    }
    //使用物品
    public void UseItem()
    {  
        if(itemUI.GetItem()!=null)
         //判断物品类型      且数量不为0
        if(itemUI.GetItem().itemType == ItemType.Useable && itemUI.Bag.items[itemUI.Index].amount>0)
        {
            if(itemUI.Bag.items[itemUI.Index].itemData.itemName=="电池")
            {
                AudioController.Instance.AudioPlay("恢复");
                GameManager.Instance.playerStats.ApplyHealth(itemUI.GetItem().usableItemData.healthPoint);
            }
            else if(itemUI.Bag.items[itemUI.Index].itemData.itemName=="子弹")
            {
                 AudioController.Instance.AudioPlay("补充弹药");
                GameManager.Instance.playerStats.ApplyBullet(itemUI.GetItem().usableItemData.bulletPoint);
            }
            itemUI.Bag.items[itemUI.Index].amount-=1;
        }
        //更新背包
        UpdateItem();
        
    }
    public void UpdateItem()
    {
        //更新对应背包格子数据
        switch(slotType)
        {
            case SlotType.BAG:
                //传入数据库内容
                itemUI.Bag = InventoryManager.Instance.inventoryData;
                break;
            case SlotType.WEAPON:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
            //装备武器 切换武器
            //背包物品不为空
                if(itemUI.GetItem() !=null)
                {   
                    //切换弹药种类更换数值
                    GameManager.Instance.playerStats.SwitchBulletType(itemUI.GetItem());
                }
                else
                {   
                    //如果武器耐久度为0，自动摧毁后  数据为空
                    //TODO:弹药为空时切换为默认弹药
                    if(GameManager.Instance.playerStats!=null)
                        GameManager.Instance.playerStats.UnEquipBullet();
                }
                break;
            case SlotType.Light:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                if(itemUI.GetItem() !=null)
                {   
                    //背包数据不为空则直接生成灯光
                    GameManager.Instance.playerStats.SwitchLightEquip(itemUI.GetItem());
                }
                else
                {
                    if(GameManager.Instance.playerStats!=null)
                        GameManager.Instance.playerStats.UnEquipLight();
                }
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionData;
                break;
        }
        //将背包中的列表数据   与UI中的格子（相同序号）匹配
        var item = itemUI.Bag.items[itemUI.Index];
        //将数据传入后由ItemUI显示图片
        itemUI.SetupItemUI(item.itemData,item.amount);

    
    }
    private void OnDisable() 
    {
        //处理背包被关闭时物品介绍仍存在的问题
        InventoryManager.Instance.toolTip.gameObject.SetActive(false);
    }
}   
