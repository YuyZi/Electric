using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//枚举状态
public enum EnemyStates { GUARD, PATROL, CHASE, DEAD }
//自动判断  并添加组件
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
//                                               接口使用   用逗号隔开，使用修补实现接口方法 一般在代码末端
public class EnemyController : MonoBehaviour, IEndGameObserver
{
    //敌人状态
    private EnemyStates enemyStates;
    private NavMeshAgent agent;
    //创建属性变量    并在Awake中赋值
    protected CharacterStats characterStats;
    //攻击目标
    protected GameObject attackTarget;
    //设置速度变量 控制速度变化
    private float speed;
    //动画组件
    private Animator anim;
    //碰撞体组件       ---防止怪物死亡仍能点击怪物攻击     各类Colldier都继承于Collider   可以直接使用Collider
    private Collider coll;
    //巡逻留观时间
    public float lookAtTime;
    //计时器 
    private float remainLookAtTime;
    //攻击CD计时器     Update中计时
    private float lastAttackTime;
    //初始守卫朝向
    private Quaternion guardRotation;
    public GameObject hitEffect;

    [Header("Basic Settings")]
    //可视范围
    public float sightRadius;
    //是否为守卫敌人
    public bool isGuard;

    //bool值判断动画切换
    bool isWalk;
    bool isChase;
    bool isFollow;
    bool isDead;
    bool playerDead;

    [Header("Patrol State")]
    //巡逻范围 /移动范围
    public float patrolRange;
    //随机路径点
    private Vector3 wayPoint;
    //初始位置
    private Vector3 guradPos;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        coll = GetComponent<Collider>();
        speed = agent.speed;
        guradPos = transform.position;
        guardRotation = transform.rotation;
        remainLookAtTime = lookAtTime;
    }
    void Start()
    {   //判断类别更改状态
        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            //开始时设定一个点
            GetNewWayPoint();
        }
        //  FIXME:  先使用在Start
        GameManager.Instance.AddObserver(this);
    }
    //报错，加载场景再时使用
    //启用与禁用时；         
    // private void OnEnable()
    // {
    //     //GameManager.Instance.AddObserver(this); 
    // }
    //区别于OnDestroy  OnDisable在销毁完成后执行
    private void OnDisable()
    {
        //如果没有生成GameManager 则返回     避免编辑器报错
        // 暂时不需要 if (!GameManager.IsInitialized) return;
        GameManager.Instance.RemoveObserver(this);
        //死亡掉落物品
        if(GetComponent<LootSpawner>() && isDead)
        {
            GetComponent<LootSpawner>().SpawnLoot();
        }
    }

    void Update()
    {
        // //死亡判断
        if (characterStats.CurrentHealth == 0)
            isDead = true;
        if (!playerDead)
        {
            SwitchStates();
            SwitchAnimation();
            //在日常中减少计时
            lastAttackTime -= Time.deltaTime;
        }

    }
    //动画切换
    void SwitchAnimation()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Critical", characterStats.isCritical);
        anim.SetBool("Death", isDead);
    }
    //状态切换
    void SwitchStates()
    {   //判断死亡
        if (isDead)
            enemyStates = EnemyStates.DEAD;
        //如果找到玩家 则切换到Chase    
        else if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
        }
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                GUARD();
                break;
            case EnemyStates.PATROL:
                Patrol();
                break;
            case EnemyStates.CHASE:
                Chase();
                break;
            case EnemyStates.DEAD:
                Dead();
                break;
        }
    }
    #region  死亡；守卫；巡逻；追击；攻击；寻找目标等角色状态
    //死亡
    void Dead()
    {    //关闭组件防止死亡时仍在追击以及攻击移动等
        coll.enabled = false;
        //会出现共计一半死亡然后StopAgent脚本Update动画报错
        //agent.enabled = false;
        //设置为0也不会阻挡玩家移动
        agent.radius = 0;
        //TODO:对象池生成
        Destroy(this.gameObject, 2f);

    }
    //站桩
    void GUARD()
    {
        //停止追击
        isChase = false;
        //如果不在原位
        if (transform.position != guradPos)
        {   //切换移动
            isWalk = true;
            //取消停止
            agent.isStopped = false;
            //设定目标
            agent.destination = guradPos;
            //距离判断     SqrMagnitude(guradPos - transform.position)  计算两者差值 
            //同Distance(wayPoint, transform.position) ，但此方法开销更小
            if (Vector3.Distance(guradPos, transform.position) <= agent.stoppingDistance)
            {

                isWalk = false;
                //角度更改，  Lerp(初始值角度,目标角度,转速（0-1）);
                transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.2f);
            }
        }
    }
    //巡逻
    public void Patrol()
    {
        //非追击状态
        isChase = false;
        //速度减慢   *乘法比除法开销小
        agent.speed = speed * 0.5f;
        //判断是否到了随机点               如果自己和随机点的距离小于停止距离
        if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
        {
            isWalk = false;
            if (remainLookAtTime > 0)
            {   //等待观察
                remainLookAtTime -= Time.deltaTime;
            }
            else
            {
                //找到新的点    并在方法中设置新的值
                GetNewWayPoint();
            }

        }
        else
        {
            isWalk = true;
            agent.destination = wayPoint;
        }
    }
    //追击玩家
    public void Chase()
    {
        isWalk = false;
        isChase = true;
        agent.speed = speed;
        //如果没找到玩家  不在攻击范围内
        if (!FoundPlayer())
        {
            isFollow = false;

            if (remainLookAtTime > 0)
            {
                //脱离时停止
                agent.destination = transform.position;
                //等待观察
                remainLookAtTime -= Time.deltaTime;
            }
            else if (isGuard)
            {   //如果是守卫者则返回守卫者状态
                enemyStates = EnemyStates.GUARD;
            }
            else
            {
                enemyStates = EnemyStates.PATROL;
            }
        }
        else
        {
            isFollow = true;
            agent.isStopped = false;     //防止攻击后卡死   恢复移动
            agent.destination = attackTarget.transform.position;
        }

        //攻击范围检测   
        if (TargetInAttackRange() || TargetInSkillRange())
        {
            //在攻击范围内则停止追踪
            isFollow = false;
            //停止移动
            agent.isStopped = true;
            //使用计时器判断攻击间隔
            //小于0可以攻击       计时器开始为0并减少 则开始就可以攻击
            if (lastAttackTime < 0)
            {
                //计时器重置
                lastAttackTime = characterStats.attackData.collDown;
                //暴击判断                    随机值是否小于暴击率   小于为true
                characterStats.isCritical = Random.value <= characterStats.attackData.criticalChance;
                //执行攻击
                Attack();
            }
        }

    }
    //攻击方法
    void Attack()
    {
        //转向目标
        transform.LookAt(attackTarget.transform);
        if (TargetInAttackRange())
        {
            //近身攻击动画
            anim.SetTrigger("Attack");

        }
        if (TargetInSkillRange())
        {
            //远程攻击动画  
            anim.SetTrigger("Skill");
        }
    }
    //新建布尔值判断玩家是否在攻击范围内
    bool TargetInAttackRange()
    {

        if (attackTarget != null)
            //判断攻击目标和当前自身坐标的距离是否小于攻击范围     直接返回的即为true 与 false
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        else
            return false;
    }
    bool TargetInSkillRange()
    {
        if (attackTarget != null)
            //判断攻击目标和当前自身坐标的距离是否小于技能范围     直接返回的即为true 与 false
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        else
            return false;
    }

    //检测玩家
    bool FoundPlayer()
    {
        //返回值为碰撞体数组  以敌人位置为中心，视野范围为半径检测
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        //循环检测  如果找到的对象标签为玩家  则返回True 否则false；
        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                //如果找到玩家
                attackTarget = target.gameObject;
                //Debug.Log("找到玩家");
                return true;

            }
        }
        //玩家脱离
        attackTarget = null;
        return false;

    }
    //随机取点
    void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;
        //不改变Y轴，避免升天
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);
        //接受值                                  y值不变 因为地形有区别 防止坑洼bug
        Vector3 randomPoint = new Vector3(guradPos.x + randomX, transform.position.y, guradPos.z + randomZ);
        NavMeshHit hit;
        //  找到导航网格最近的点 找到为true 否则为false    
        //1代表Nav中Area的walkable   if语句 问号代替判断    成立赋值一个hit.position  否则赋值当前pos
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;

    }
    //显示范围  （OnDrawGizmos为一直显示，Selected则为选中目标时显示
    void OnDrawGizmosSelected()
    {   //设置颜色
        Gizmos.color = Color.red;
        //画一个圆圈来定义范围     需要使用Wire 否则为实心球体    (中心点坐标Vector3值，范围)  
        Gizmos.DrawWireSphere(transform.position, sightRadius);      //自身为中心，视野范围为半径
    }

    //Animation Event       受伤事件
    void Hit()
    {
        //判断攻击目标是否为空（玩家跑走的情况）   以及是否在范围内    
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        //if (attackTarget != null)
        {
            //获取敌人属性
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            //给定攻击目标和受击目标返回值
            targetStats.TakeDamge(characterStats, targetStats);
            Vector3 insTransform = new Vector3 (attackTarget.transform.position.x,
            attackTarget.transform.position.y+1.7f,
            attackTarget.transform.position.z);
            Instantiate(hitEffect,insTransform,Quaternion.identity);
        }
    }
    //修后实现接口方法
    #endregion
    public void EndNotify()
    {
        //获胜动画
        anim.SetBool("Win", true);
        //布尔值判断死亡  防止Update中执行切换动画
        playerDead = true;
        //print("Victory");
        //停止所有移动
        //停止Agent
        isChase = false;
        isWalk = false;
        attackTarget = null;
    }
}
