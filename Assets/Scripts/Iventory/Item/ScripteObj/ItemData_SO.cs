using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//物品类型
public enum ItemType
{
    Useable,Weapon,Tool,Light
}

[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/Item Data")]
public class ItemData_SO :ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public int itemAmount;

    [TextArea]
    public string  descpription = "";
    //是否可堆叠
    public bool stackable;
    [Header("Tool")]
    public GameObject toolPrefab;
    [Header("Weapon")]
    public GameObject weaponPrefab;
    //public AnimatorOverrideController weaponAnimator;
    public AttackData_SO weaponAttackData;
    [Header("Useable")]
    public UseableItemData_SO   usableItemData;

}
