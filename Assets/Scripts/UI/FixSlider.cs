using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
//使用DOTween       制作文本逐渐生成效果
using DG.Tweening;

public class FixSlider : MonoBehaviour
{
    [TextArea]
    public string finalText;
    public GameObject loadPanel;
    public GameObject errorPanel;
    public GameObject OverPanel;
    public Text overText;
    public Slider slider;
    public Text loadText;
    public Button closeBtn;
    bool canFix;
    public InventoryData_SO bagData;
    public static string[] requiredNames = new string[4]{"冲击钻","扳手","钳子","螺丝钉"};
    private void Awake() 
    {
        //数组集合转换为List集合
        
    }
    private void OnTriggerStay(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            canFix = true;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
            canFix = false;
    }
    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.E)&&canFix)
        {   
            bagData = InventoryManager.Instance.inventoryData;
            bool allNamesFound = true;
            //TODO:检测背包是否含有足够的道具
            foreach (string name in requiredNames)
            {
                //any()的方法主要功能是：判断是否为空、是否存在元素满足指定的条件。
                if (!bagData.items.Any(item => item.itemData.itemName == name))
                {
                    allNamesFound = false;
                    break;
                }
            }
            if(canFix && allNamesFound)
            {
                loadPanel.SetActive(true);
                StartCoroutine(LoadSlider());
            }
            else
            {
                errorPanel.SetActive(true);
            }
        }
    }
    IEnumerator LoadSlider()
    {
        float time = 0f;
        AudioController.Instance.AudioPlayLoop("修理");
        while (time < 5f)
        {
            time += Time.deltaTime;
            slider.value = time / 5f;
            loadText.text = slider.value*100 +"%";
            yield return null;
        }
        if(slider.value ==1)
        {
            AudioController.Instance.AudioStop("修理");
        }
        closeBtn.gameObject.SetActive(true);
    }
    public void GameOver()
    {
        Transform lightTransform = GameObject.Find("Directional Light").transform;
                lightTransform.rotation = Quaternion.Euler(90, 
                lightTransform.rotation.eulerAngles.y, 
                lightTransform.rotation.eulerAngles.z);
        lightTransform.GetChild(0).GetComponent<Light>().color = Color.green;
        loadPanel.SetActive(false);
        OverPanel.SetActive(true);
        overText.DOText(finalText,5f).OnComplete(() => StartCoroutine(WaitQuitGame()));
    }
    IEnumerator WaitQuitGame()
    {
        yield return new WaitForSeconds(2f);
        Application.Quit();
    }
}
