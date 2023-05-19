using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.UI;
using System;

public class SceneController : SingletonMono<SceneController>
{
    //淡入淡出         需要设置为预制体    18.13
    //public FadeToScene fadeToScenePrefab;
    //玩家信息
    GameObject playerPrefab;
    //玩家角色
    GameObject player;
    NavMeshAgent playerAgent;
    //进度条
    GameObject loadPanel;
    Slider loadSlider;
    Text loadText;
    public override void OnInit() 
    {
        base.OnInit();
        playerPrefab = (GameObject)Resources.Load("Prefabs/Character/Player/Player");
        //对象被初始化时无法判断当前场景 返回值为null
        if(SceneManager.GetActiveScene().name == "Menu")
        {   
            var canvas = GameObject.Find("Canvas");
            Transform panelTransform = canvas.transform.Find("Load");
            loadPanel = panelTransform.gameObject;
            loadSlider = loadPanel.GetComponentInChildren<Slider>();
            loadText = loadPanel.GetComponentInChildren<Text>();
        }    
    }
    //传送到目标终点
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        //判断是否同场景
        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                //携程 传送     拿到标签
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }
    //游戏内场景转换
    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {

        //保存数据
        SaveManager.Instance.SavePlayerData();
        InventoryManager.Instance.SaveData();

        //判断是否同场景传送        当前场景名与传入变量名是否相同
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            //先加载场景
            yield return SceneManager.LoadSceneAsync(sceneName);
            //生成角色
            yield return Instantiate(playerPrefab,
                GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            //同场景可以不读取数据，不同场景需要加载数据
            SaveManager.Instance.LoadPlayerData();
            //跳出携程
            yield break;
        }
        else
        {
            player = GameManager.Instance.playerStats.gameObject;
            playerAgent = player.GetComponent<NavMeshAgent>();
            playerAgent.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position,
                GetDestination(destinationTag).transform.rotation);
            playerAgent.enabled = true;
            yield return null;
        }

    }
    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        //找到所有目标点  返回一个数组  循环数组找到想要的点   找到后返回
        var entrances = FindObjectsOfType<TransitionDestination>();
        for (int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i].destinationTag == destinationTag)
                return entrances[i];
        }

        return null;
    }
    //加载主菜单
    public void TransitionToLoadMain()
    {
        StartCoroutine(LoadMain());
    }
    //加载第一个场景  
    public void TransitionToFirstScene()
    {
         StartCoroutine(LoadScene("City"));

        //var scene = SceneManager.LoadSceneAsync("SampleScene");
        //scene.completed += LoadSceneCallback;
    }
    //读取场景
    public void TransitionToLoadGame()
    {
        StartCoroutine(LoadScene(SaveManager.Instance.SceneName));
    }

    //开始游戏时加载场景  传入Enter点   生成角色   
    IEnumerator LoadScene(string name)
    {
        //名称不为空
        if (name != "")
        {
            if(SceneManager.GetActiveScene().name == "Menu")
            {   
                loadPanel.SetActive(true);
                //异步加载进度条
                AsyncOperation operation = SceneManager.LoadSceneAsync(name);
                operation.allowSceneActivation = false;
                while(!operation.isDone)
                {
                    loadSlider.value = operation.progress;
                    loadText.text = operation.progress*100 +"%";
                    if(operation.progress==0f)
                        yield return new WaitForSeconds(1f);
                    if(operation.progress>=0.9f)
                    {
                        loadSlider.value = 1;
                        loadText.text = "按任意键继续";
                        if(Input.anyKeyDown)
                        {
                            operation.allowSceneActivation = true;
                    
                        }
                    yield return null;
                    }
                }
                yield return player = Instantiate(
                playerPrefab,
                GameManager.Instance.GetEnterance().position,
                GameManager.Instance.GetEnterance().rotation
                );
                //保存游戏   
                SaveManager.Instance.SavePlayerData();
                InventoryManager.Instance.SaveData();
                //结束协程
                yield break;
            }
            else
            {
                    //需要DontDestoryOnLoad()方法的物体在界面最外层即不能有父级物体
                    yield return SceneManager.LoadSceneAsync(name);
                    yield return player = Instantiate(
                    playerPrefab,
                    GameManager.Instance.GetEnterance().position,
                    GameManager.Instance.GetEnterance().rotation
                    );
                    //保存游戏   
                    SaveManager.Instance.SavePlayerData();
                    //结束协程
                    yield break;
            }
        }

    }
    //返回主界面
    IEnumerator LoadMain()
    {
        yield return SceneManager.LoadSceneAsync("Menu");
        yield break;
    }

    //void LoadSceneCallback(AsyncOperation obj)
    //{
        
    //    var player = Instantiate(
    //        playerPrefab,
    //        GameManager.Instance.GetEnterance().position,
    //        GameManager.Instance.GetEnterance().rotation
    //       );
        
    //    //保存游戏    使用该方法有bug
    //    SaveManager.Instance.SavePlayerData();
    //}
}
