using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IDragHandler,IPointerDownHandler
{
    RectTransform rectTransform;
    Canvas canvas;

    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = InventoryManager.Instance.GetComponent<Canvas>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        //delta 指针增量        scalerFactor 使适应屏幕
        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //调整排序
        rectTransform.SetSiblingIndex(2);
    }
}
