using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{   
    //物品信息
    public ItemData_SO itemData;
    public GameObject pickTipsPanel;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            //将物品添加到背包                  传入物品数据       
            InventoryManager.Instance.inventoryData.AddItem(itemData,itemData.itemAmount);
            //加入背包数据库之后更新背包UI
            InventoryManager.Instance.inventoryUI.RefreshUI();
            //装备
            // GameManager.Instance.playerStats.EquipLight(itemData);
            // pickTipsPanel.SetActive(true);
            // Destroy(pickTipsPanel,1.5f);
            AudioController.Instance.AudioPlay("拾取道具");
            Destroy(gameObject);
        }
    }
}
