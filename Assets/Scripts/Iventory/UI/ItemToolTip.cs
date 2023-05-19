using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    public Text itemNameText;
    public Text itemInfoText;
    RectTransform rectTransform;
    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetupTooltip(ItemData_SO item)
    {
        itemNameText.text = item.itemName;
        itemInfoText.text = item.descpription;
    }
    private void OnEnable() 
    {
        UpdatePosition();
    }
    void Update()
    {
        UpdatePosition();
    }
    public void UpdatePosition()
    {
        //获取鼠标坐标
        Vector3 mousePos = Input.mousePosition;
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        //获取高度与宽度
        float width = corners[3].x-corners[0].x;
        float height =corners[1].y-corners[0].y;
        //如果鼠标与屏幕边缘距离小于UI界面的宽度，则生成在左边 否则生成在右边 上下同理
        if(mousePos.y<height)
            rectTransform.position = mousePos + Vector3.up * height * 0.7f;
        else if(Screen.width - mousePos.x > width)
            rectTransform.position = mousePos + Vector3.right * width * 0.7f;
        else
            rectTransform.position = mousePos + Vector3.left * width * 0.7f;
    }
}
