using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

//使用接口调用实现物品拖拽
[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ItemUI currentItemUI;
    SlotHolder currentHolder;
    SlotHolder targetHolder;
    private void Awake() 
    {
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //记录初始数据                  设置为最上级，避免拖拽时被遮挡，同时保留其原有设置
        //临时变更，  在结束后返回原有层级
        //创建新的DragData 并设置值
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        InventoryManager.Instance.currentDrag.originalParent =(RectTransform) transform.parent;


        transform.SetParent(InventoryManager.Instance.dragCanvas.transform,true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //跟随鼠标移动
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //放下物品 交换数据
        //是否指向UI物品        EventSystem.current  等价于 xxx.instance
        if(EventSystem.current.IsPointerOverGameObject())
        {   
            //不论在三个区域中的哪个区域        需要找到SlotHolder进行更换图片与数量
            if(InventoryManager.Instance.CheckInActionUI(eventData.position)||
            InventoryManager.Instance.CheckInEquipmentUI(eventData.position)||
            InventoryManager.Instance.CheckInInventoryUI(eventData.position))
            {
                //判断鼠标进入位置是否包含 SlotHolder
                if(eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                {
                    targetHolder =eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                }
                else
                {
                    //如果不存在则去父级中查找
                    targetHolder =eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                }
                //Debug.Log(eventData.pointerEnter.gameObject);
                //判断目标是否原来的Holder  防止放置物体后显示层级问题
        if(targetHolder != InventoryManager.Instance.currentDrag.originalHolder)
                switch(targetHolder.slotType)
                {
                    //背包
                    case SlotType.BAG:
                        SwapItem();
                        break;
                    //快捷栏
                    case SlotType.ACTION:
                    if(currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType  == ItemType.Useable)
                        SwapItem();
                        break;
                    //子弹 （武器）
                    case SlotType.WEAPON:
                    if(currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType  == ItemType.Weapon)
                        SwapItem();
                        break;
                    //灯光
                    case SlotType.Light:
                    if(currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType  == ItemType.Tool)
                        SwapItem();
                        break;
                    
                }
                currentHolder.UpdateItem();
                targetHolder.UpdateItem();
            }
        }
        //更改父级          交换数据后将UI格子归位
        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);
        //当移动到边界时不能很好的重置到中心位置        获取格子本身的RectTransform
        RectTransform t =transform as RectTransform;
        //代码手册 offsetMax Min 为中心点   相对右上角 左下角的距离 即Right Top  Left Botton 
        t.offsetMax = - Vector2.one*5;
        t.offsetMin = Vector2.one*5;
    }
    public void SwapItem()
    {
        //创建变量获取目标物品和中间值来实现交换
        //              找到目标格子上、显示的UI中、对应背包、列表中、对应编号的物体
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];
        //              当前目标对应的序号物品
        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];
        //判断是否相同物品   临时变量返回值为InventoryItem
        bool isSameItem = tempItem.itemData == targetItem.itemData;
        //相同物品且该物品可以堆叠
        if(isSameItem && targetItem.itemData.stackable)
        {
            //叠加数字 清除原有物品
            targetItem.amount+=tempItem.amount;
            tempItem.itemData =null;
            tempItem.amount = 0;
        }
        else
        {
            //交换物品      这里不能使用临时变量  需要用到原有数据
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = targetItem;
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tempItem;
        }
    }
}
