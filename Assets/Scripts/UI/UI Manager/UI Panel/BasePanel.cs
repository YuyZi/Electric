using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel 
{
    


}

public enum UIPanelType
{
    //Panel种类 面板的prefab名称需要与枚举类的类型名称相同
        MainMenuPanel,
        LoadingPanel,
        PausePanel,
        HealthBarPanel
}
    //用于存储信息 类别和名称
public class UIPanel
{   
        public UIPanelType panelType;
        public string panelName;
}

