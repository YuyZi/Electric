using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : SingletonMono<PlayerController>
{
    private NavMeshAgent agent;
    private Animator anim;
    //创建属性变量
    private CharacterStats characterStats;
    private GameObject attackTarget;
    private float lastAttackTime;
    private bool isDead;
    private float stopDistance;
    private bool runing;
    //背包物品相关
    public bool lightbool;
    public Image cooldownImage;
    [SerializeField] GameObject lightObject;
    //子弹相关
    [SerializeField] Transform armPosition;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject specialBullet;
    //自身变量获取放在Awake     非重写 用new 否则用overrider
    public  override void OnInit()
    {   
        base.OnInit();
        //获取组件
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        //赋值停止距离 在移动攻击事件中调整
        stopDistance = agent.stoppingDistance;
        //玩家生成时引用单例注册
        GameManager.Instance.RigisterPlayer(characterStats);
        var playerHealthCanvas = FindObjectOfType<PlayerHealthUI>();
        var coolDownFrame = playerHealthCanvas.transform.GetChild(5);
        cooldownImage = coolDownFrame.GetChild(0).GetComponent<Image>();
    }
    //任务启用时 注册
    private void OnEnable()
    {
        //                +=  事件订阅  函数方法参数需要一致（Vector3
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        //订阅攻击事件
        MouseManager.Instance.OnEnemyClicked += EventAttack;
        //玩家生成时引用单例注册
        // GameManager.Instance.RigisterPlayer(characterStats);
    }

    void Start()
    {    
        //使用或者更改属性数值    会应用到Data中         直接在Data中调整不需要管理代码
        //characterStats.MaxHealth = 2;
        //获取自身数据
        SaveManager.Instance.LoadPlayerData();
    }
    //当任务关闭时，取消订阅
    private void OnDisable()
    {
        //if (!MouseManager.IsInitialized) return;
        MouseManager.Instance.OnMouseClicked -= MoveToTarget;
        MouseManager.Instance.OnEnemyClicked -= EventAttack;
    }
    void Update()
    {   
        //控制灯光
        if(lightObject.GetComponentInChildren<Light>()!=null)
        {
            SetLight();
        }
        //直接判断血量 进行布尔值更改
        isDead = characterStats.CurrentHealth == 0;
        //print("PlayerDie");
        //死亡广播
        if (isDead)
            GameManager.Instance.NotifyObservers();
        SwitchAnimation();
        //时间衰减
        lastAttackTime -= Time.deltaTime;
        if(lastAttackTime>=0)
        {
            //CD= 2  自减       fillAmount = 2/2 =0
            cooldownImage.fillAmount = 1-  (lastAttackTime / characterStats.attackData.collDown);
        }
        else
        {
            cooldownImage.fillAmount=1;
        }
    }
    //开关灯
    void SetLight()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            AudioController.Instance.AudioPlay("开关手电");
            if(lightbool==true)
            {
                lightbool = false;
            }
            else if(lightbool ==false)
            {
                lightbool = true;
            }
        }
        if(lightbool==false)
        {
            lightObject.SetActive(false);
        }
        else if(lightbool ==true)
        {
            lightObject.SetActive(true);
        }

    }

    //切换动画  
    private void SwitchAnimation()
    {   //  sqrMagnitude    Vector3---Float值  
        anim.SetFloat("Speed",agent.velocity.sqrMagnitude);
        anim.SetBool("Death", isDead);
    }

    //注册到Mousemanager    EventAction中  事件启用会自动调用该函数        ——委托
    public void MoveToTarget(Vector3 target)
    {
        //AudioController.Instance.AudioPlayLoop("跑");
        //停止携程 取消攻击
        StopAllCoroutines();
        //死亡时不能移动
        if (isDead) return;
        //日常移动时更改回到最初停止距离
        agent.stoppingDistance = stopDistance;
        //攻击后点击恢复移动
        agent.isStopped = false ; 
        agent.destination = target ;
    }
    //攻击事件
    private void EventAttack(GameObject target)
    {
        //死亡时不能攻击
        if (isDead) return;
        //如果目标不为空则赋值
        if (target != null)
        {
            attackTarget = target;
            //是否暴击的判断  暴击率
            characterStats.isCritical = UnityEngine.Random.value <= characterStats.attackData.criticalChance;
            //携程执行
            StartCoroutine(MoveToAttackTarget());
        }
    }
    //携程判断距离以及移动
    IEnumerator MoveToAttackTarget()
    {
        //AudioController.Instance.AudioPlayLoop("跑");
        //开始时确认为可以移动
        agent.isStopped = false ;
        //调整停止距离为攻击距离，防止怪物体积过大无法接近攻击
        agent.stoppingDistance = characterStats.attackData.attackRange;
        //朝向敌人
        transform.LookAt(attackTarget.transform);
        //判断攻击距离  3D使用Vector3   characterStats.attackData.attackRange 为攻击范围
        //TODO: 手动攻击  攻击方向为正前方 到达距离后更换为举枪瞄准状态  
        while (Vector3.Distance(attackTarget.transform.position,transform.position)>characterStats.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            //下一帧再次执行上述命令     如果距离小于1则跳出循环
            yield return null   ;
        }
        //AudioController.Instance.AudioStop("跑");
        //停止  true为停 false为没有停止 
        agent.isStopped = true ; 
        //  攻击CD结束且子弹数不为0
        if(lastAttackTime<=0 && characterStats.characterData.currentBullets!=0)
        {   
            ShootBullet();
            //重置冷却时间      衰减在Update中
            lastAttackTime = characterStats.attackData.collDown;
        }
    }
    //子弹减少      Animaiton Event
    void ShootBullet()
    {   
        AudioController.Instance.AudioPlay("射击");
        //暴击
        if(characterStats.characterData.currentBullets>0)
        {
        anim.SetBool("Critical", characterStats.isCritical);
        anim.SetTrigger("Attack"); 
        //指定位置生成子弹     实例化 与对象池
        //Instantiate(bullet,armPosition.position,Quaternion.identity);
        
        //判断攻击数值是否被更改 如果更改更换实例化的子弹对象池
        if(characterStats.attackUp)
        {
            PoolManager.Instance.Release(specialBullet,armPosition.position,Quaternion.identity);
        }
        else
        {
            PoolManager.Instance.Release(bullet,armPosition.position,Quaternion.identity);
        }
        characterStats.characterData.currentBullets-=1;
        if(characterStats.characterData.currentBullets==0)
        {   
            //子弹不足时   取消攻击     更换弹药
            anim.SetTrigger("ReLoad");
            AudioController.Instance.AudioPlay("换弹");
            //备用弹药够一个弹夹
            if(characterStats.characterData.readyBullets!=0 && 
            characterStats.characterData.readyBullets>=characterStats.characterData.magazineClipSize)
            {
            //填充弹夹，减少备用弹药
                characterStats.characterData.currentBullets = characterStats.characterData.magazineClipSize;
                characterStats.characterData.readyBullets -= characterStats.characterData.magazineClipSize;
            }
            else
            {   
                //不够时
                 characterStats.characterData.currentBullets = characterStats.characterData.readyBullets;
                 characterStats.characterData.readyBullets = 0;
            }

        }
        }
        
    }

    // //Animation Event  赋予攻击动画关键帧
    // public void Hit()
    // {
    //     if(attackTarget.CompareTag("Attackable") )
    //     {
    //         if (attackTarget.getComponent<GolemRock_SO>())
    //         {    //在空中也可以打回石头
    //             attackTarget.getComponent<GolemRock_SO>().rockStates = GolemRock_SO.RockStates.HitEnemy;
    //             //给初速度 防止变为hitnothing
    //             attackTarget.getComponent<Rigidbody>().velocity = Vector3.one;
    //             attackTarget.getComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
    //         }
    //     }
    //     else
    //     {
    //         //获取敌人属性
    //         var targetStats = attackTarget.GetComponent<CharacterStats>();
    //         //给定攻击目标和受击目标返回值
    //         targetStats.TakeDamge(characterStats, targetStats);
    //     }
        
    // }
} 


