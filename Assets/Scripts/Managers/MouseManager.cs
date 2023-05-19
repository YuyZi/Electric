using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//引用新的命名空间  使用自带事件action
using System;
using UnityEngine.EventSystems;
/* //引用事件命名空间
using UnityEngine.Events;  */
/* //序列化显示内容
[System.Serializable]               
//定义类 命名：继承<引用类型>
public class EventVector3:UnityEvent<Vector3>{ };    */
//Singleton<MouseManager> 更改继承
public class MouseManager : SingletonMono<MouseManager>
{   
    //创建图片变量
    [SerializeField]
    Texture2D point, doorway, attack, aim, move;

    //创建一个Raycast类 并命名  用于保存射线碰撞物体信息
    RaycastHit hitInfo;   

    //创建event事件,需要他人调用和注册  <所需参数>          点击地面时
    //函数当中调用时，所有注册这个事件的方法都会被调用
    public event Action<Vector3> OnMouseClicked;        
    //点击敌人时
    public event Action<GameObject> OnEnemyClicked;
    //转换场景不销毁
    // protected override void Awake()
    // {   //在原有基础上
    //     base.Awake();
    //     //可以添加额外内容
    //     DontDestroyOnLoad(this);
    // }
    public override void OnInit()
    {
       base.OnInit();
       //初始化 自动获取资源
        point = Resources.Load<Texture2D>("Cursor/05");
        doorway = Resources.Load<Texture2D>("Cursor/04");
        attack = Resources.Load<Texture2D>("Cursor/03");
        aim = Resources.Load<Texture2D>("Cursor/02");
        move = Resources.Load<Texture2D>("Cursor/01");
    }
    void Update()
    {   
        //实时监测返回值并切换贴图
        SetCursorTexture();
        //实时判断鼠标情况      
        //如果在与UI互动则不执行
        if(InteractWithUI())    return;
        MousControl();
    }
    //设置鼠标贴图   （不同物体时
    void SetCursorTexture()     
    {
        //创建射线 并使用鼠标点击位置做返回值   (Vector3 pos)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //获得碰撞信息         （与碰撞体相交返回true 否则false
        if(Physics.Raycast(ray, out hitInfo))
        {
            //转换鼠标贴图
            switch(hitInfo.collider.tag)
            {
                //鼠标点击地面      切换贴图（尺寸设置为32*32）并hotspot设置16偏移量为中心点 
                case "Ground":
                Cursor.SetCursor(move, new Vector2(16, 16),CursorMode.Auto);
                    break;
                    //点击敌人
                case "Enemy":
                Cursor.SetCursor(aim, new Vector2(16, 16),CursorMode.Auto);
                    break; 
                //TODO：分为瞄准和直接攻击   区别瞄准与攻击图标
                case "Attackable":
                Cursor.SetCursor(attack, new Vector2(16, 16),CursorMode.Auto);
                    break;
                case "Portal":
                Cursor.SetCursor(doorway, new Vector2(16, 16),CursorMode.Auto);
                    break;
                case "Human":
                Cursor.SetCursor(point, new Vector2(16, 16),CursorMode.Auto);
                    break;
                case "Item":
                Cursor.SetCursor(point, new Vector2(16, 16),CursorMode.Auto);
                    break;
            }
        }
    }
    //鼠标控制  
    void MousControl()
    {
        //判断点击返回值   点击右键且碰撞体不为空
        if(Input.GetMouseButtonDown(1) && hitInfo.collider!= null)
        {   
            //如果点击地面      
            if(hitInfo.collider.gameObject.CompareTag("Ground"))
            //不为空执行invoke  ?. 等价于 ！=null    传入hitinfo的值（Vector3
            OnMouseClicked?.Invoke(hitInfo.point);
            //传送门  移动到该点
            if (hitInfo.collider.gameObject.CompareTag("Portal"))
            OnMouseClicked?.Invoke(hitInfo.point);
            if(hitInfo.collider.gameObject.CompareTag("Enemy"))
            OnEnemyClicked?.Invoke(hitInfo.collider.gameObject); 
            if (hitInfo.collider.gameObject.CompareTag("Item"))
            OnMouseClicked?.Invoke(hitInfo.point);
        }
        // if(Input.GetMouseButtonDown(0) && hitInfo.collider!= null)
        // {  

        //     // //如果点击可被攻击的物体
        //     // if(hitInfo.collider.gameObject.CompareTag("Attackable"))
        //     // OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
        // }
    }

    bool InteractWithUI()
    {
        //判断是否在与UI界面互动
        if(EventSystem.current!=null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
