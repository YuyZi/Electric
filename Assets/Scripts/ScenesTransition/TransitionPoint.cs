using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    //场景类型  同场景 不同场景传送
    public enum TransitionType
    {
        SameScene,DifferentScene
    }

    [Header("Transition Info")]
    //场景名称
    public string sceneName;
    //场景类型  同场景 不同场景
    public TransitionType transitionType;
     //生成终点变量
    public  TransitionDestination.DestinationTag destinationTag;
     //是否可以传送
    private bool canTrans;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canTrans)
        {
            //SceneController 传送
            SceneController.Instance.TransitionToDestination(this);
        }
    }

    //传送门触发器
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            canTrans = true;
            

    } 
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            canTrans = false;
    }
}
