using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//在Canvas上
public class InventoryManager : MonoBehaviour
{   
    //作为第三方保存拖拽之前的属性
    public class DragData
    {
        //保存原有Holoder以及Parent
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }
    public static InventoryManager Instance;
    //添加模板储存数据
    [Header("Inventory Data")]
    public InventoryData_SO inventoryTemplate;
    public InventoryData_SO inventoryData;
    public InventoryData_SO actionTemplate;
    public InventoryData_SO actionData;
    public InventoryData_SO equipmentTemplate;
    public InventoryData_SO equipmentData;

    [Header("Containers")]
    public ContainerUI inventoryUI;
    public ContainerUI actionUI;
    public ContainerUI equipmentUI;
    [Header("Drag Canvas")]
    public Canvas dragCanvas;
    //在拖拽开始时临时保存数值
    public DragData currentDrag;
    [Header("UI Panel")]
    public GameObject bagPanel;
    public GameObject statsPanel;

    bool isOpen = false;

     [Header("State Text")]
    public Text healthText;
    public Text attackText;
    public Text defenseText;
    public Text criticalText;
    [Header("Tool Tip")]
    public ItemToolTip toolTip;

    //TODO:改为UI单例 框架  自动生成UI
    private void Awake() 
    {
        Instance = this;  

        inventoryData=Resources.Load<InventoryData_SO>("Game Data/Inventory Data/Bag");
        //DontDestroyOnLoad(this);
        //开始新游戏时   每次生成背包
        if(inventoryTemplate!=null)
        {
            inventoryData = Instantiate(inventoryTemplate);
        }
        if(actionTemplate!=null)
        {
            actionData = Instantiate(actionTemplate);
        }
        if(equipmentTemplate!=null)
        {
            equipmentData = Instantiate(equipmentTemplate);
        }
        

    }
    private void Start() 
    {   
        //加载场景 刷新图片
        LoadData();
        //背包UI与背包数据库连接
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }       
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            //在打开 Dialogue UI 或启动 ItemTooltip 的时候强制刷新布局  避免对话被遮挡
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            //每次执行时bool值对调
            isOpen=!isOpen;
            statsPanel.SetActive(isOpen);
            bagPanel.SetActive(isOpen);
        }
        if(GameManager.Instance.playerStats!=null)
        {
            //实时更新数据          初始化时GameManager未实例化  在打开背包后更新    
            UpdateStatsText(GameManager.Instance.playerStats.CurrentHealth,
            GameManager.Instance.playerStats.attackData.minDamge,
            GameManager.Instance.playerStats.attackData.maxDamge,
            GameManager.Instance.playerStats.CurrentDefence,
            GameManager.Instance.playerStats.attackData.criticalChance);
        }
        

    }
    //保存与读取数据
    public void SaveData()
    {
        SaveManager.Instance.Save(inventoryData,inventoryData.name);
        SaveManager.Instance.Save(actionData,actionData.name);
        SaveManager.Instance.Save(equipmentData,equipmentData.name);
    }
    public void LoadData()
    {
        SaveManager.Instance.Load(inventoryData,inventoryData.name);
        SaveManager.Instance.Load(actionData,actionData.name);
        SaveManager.Instance.Load(equipmentData,equipmentData.name);
    }
    public void UpdateStatsText(int health, int max ,int min, int defence,float critical)
    {
        healthText.text = health.ToString();
        attackText.text = min + "-" + max ;
        defenseText.text = defence.ToString();
        criticalText.text = critical*100 +"%";

    }
    #region     检查拖拽物品是否在每一个Slot范围内      在DragItem中使用
    //传入鼠标坐标
    //是否在背包
    public bool  CheckInInventoryUI(Vector3 position)
    {
        for(int i = 0; i<inventoryUI.slotHolders.Length; i ++)
        {       
            //拿到第i个物品的       或者在后面添加 as RectTransform 进行强制转换
            RectTransform t = (RectTransform)inventoryUI.slotHolders[i].transform;
            //          判断鼠标位置是否在拿到的t位置之内 是则返回true
            if(RectTransformUtility.RectangleContainsScreenPoint(t,position))
            {
                return true;
            }
        }
        return false;
    }
    //是否在快捷栏
    public bool  CheckInActionUI(Vector3 position)
    {
        for(int i = 0; i<actionUI.slotHolders.Length; i ++)
        {       
            //拿到第i个物品的       或者在后面添加 as RectTransform 进行强制转换
            RectTransform t = (RectTransform)actionUI.slotHolders[i].transform;
            //          判断鼠标位置是否在拿到的t位置之内 是则返回true
            if(RectTransformUtility.RectangleContainsScreenPoint(t,position))
            {
                return true;
            }
        }
        return false;
    }
    //是否在状态栏
    public bool  CheckInEquipmentUI(Vector3 position)
    {
        for(int i = 0; i<equipmentUI.slotHolders.Length; i ++)
        {       
            //拿到第i个物品的       或者在后面添加 as RectTransform 进行强制转换
            RectTransform t = (RectTransform)equipmentUI.slotHolders[i].transform;
            //          判断鼠标位置是否在拿到的t位置之内 是则返回true
            if(RectTransformUtility.RectangleContainsScreenPoint(t,position))
            {
                return true;
            }
        }
        return false;
    }

    #endregion

}
