using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager :MonoBehaviour
{   
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new UIManager();
            return _instance;
        }
    }
    //存储UIPanel
    List<UIPanel> panelList = new List<UIPanel>();
    Dictionary<string, UIPanel> panelDic = new Dictionary<string, UIPanel>();
    public UIPanel ReturnPanel(string name, UIPanelType type)
    {
        if (!panelDic.ContainsKey(name))
        {
            //生成对象
            UIPanel panel = new UIPanel();
            panel.panelName = name;
            panel.panelType = type;
            panelDic.Add(name, panel);
            return panel;
        }
        else if (panelDic.ContainsKey(name))
        {
            return panelDic[name];
        }
        return null;
    }


//     //返回UI组件
//     public T ReturnUI<T>(string name) where T:UIBehaviour
//     {
//         if (uiDic.ContainsKey(name))
//         {
//             for (int i = 0; i < uiDic[name].Count; i++)
//             {
//                 if (uiDic[name][i] is T)
//                 {
//                     return uiDic[name][i] as T;
//                 }
//             }
//         }
//         return null;
//     }
    
//     //获取需要获取的UI组件  T:UI组件名字  挂载Canvas   子物体
//    public  void GetUI<T>() where T :UIBehaviour
//    {
//         T[] uis = this.GetComponentsInChildren<T>();
//         for (int i = 0; i < uis.Length; i++)
//         {
//             if (uiDic.ContainsKey(uis[i].name))
//             {
//                 uiDic[uis[i].name].Add(uis[i]);
//             }
//             else
//             {
//                 uiDic.Add(uis[i].name, new List<UIBehaviour>() { uis[i] });
//             }
//         }
//    }

}
