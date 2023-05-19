using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    //Action   攻击时更新血量  想要订阅该方法必须有两个int类型参数  当前生命值和最大生命值
    public event Action<int, int> UpdateHealthBarOnAttack;
    //临时模板数据
    public CharacterData_SO templateData;
    //属性数值
    public CharacterData_SO characterData;
    //攻击数值
    public AttackData_SO attackData;
    //基础攻击力
    private AttackData_SO baseattackData;
    //保存原有动画控制器，在卸下武器时切换      runtimeAnimator才是Animator中放置的控制器
    //private RuntimeAnimatorController baseAnimator; 
    [Header("Light")]
    public Transform lightSlot;
    [Header("Weapon")]
    public Transform weaponSlot;
    public bool attackUp;
    //暴击          在属性页面隐藏
    [HideInInspector]
    public bool isCritical;
    private void Awake()
    {   
        //如果模板数据不为空  生成副本模板数据给到角色       删除属性中的characterData   赋值给tempData
        if (templateData != null)
        {
            characterData = Instantiate(templateData);
        }
        //生成模板攻击力作为基础攻击力
        baseattackData = Instantiate(attackData);
        //baseAnimator = GetComponent<Animator>().runtimeAnimatorController;
    }
    //快速折叠 region
    #region Read From Data_SO
    // get 可读， set 可写
    public int readyBullets
    {
        get
        {   //读取数值，如果没有则返回0
            if (characterData != null)
                return characterData.readyBullets;
            else return 0;
        }
        set
        {   //写入数值      value会直接更改CharacterData中的数值
            characterData.readyBullets = value;
        }
    } 
    public int MaxHealth
    {
        get
        {   //读取数值，如果没有则返回0
            if (characterData != null)
                return characterData.maxHealth;
            else return 0;
        }
        set
        {   //写入数值      value会直接更改CharacterData中的数值
            characterData.maxHealth = value;

        }
    } 
    public int CurrentHealth
    {
        get
        {   //读取数值，如果没有则返回0
            if (characterData != null)
                return characterData.currentHealth;
            else return 0;
        }
        set
        {   //写入数值
            characterData.currentHealth = value;

        }
    } 
    
    public int BaseDefence
    {
        get
        {   //读取数值，如果没有则返回0
            if (characterData != null)
                return characterData.baseDefence;
            else return 0;
        }
        set
        {   //写入数值
            characterData.baseDefence = value;

        }
    }

    public int CurrentDefence
    {
        get
        {   //读取数值，如果没有则返回0
            if (characterData != null)
                return characterData.currentDefence;
            else return 0;
        }
        set
        {   //写入数值
            characterData.currentDefence = value;

        }
    }
    #endregion
    #region Damage
    //伤害计算
    public void TakeDamge(CharacterStats attacker ,CharacterStats defener )
    {
        //攻击者攻击力-防御者防御力，且要先获得两个玩家的状态
        //取最大值，若为负数则为0保证数据不出错
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.CurrentDefence,0);
        CurrentHealth = Mathf.Max(CurrentHealth-damage,0);
        //暴击调用敌方动画      命名相同        攻击者暴击，受伤者调用
        //FIXME:实现在子弹身上
        if (attacker.isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }
        //是否为空   更新血条
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        //如果人物死亡 则增加攻击者经验值
        if (CurrentHealth <= 0)
        {
            //attacker.characterData.UpdateExp(characterData.killPoint);
            GameManager.Instance.playerStats.characterData.UpdateExp(characterData.killPoint);
        }

    }
    //方法重载
    // public void TakeDamage(int damage,CharacterStats defener)
    // {
    //     int currentDamage = Mathf.Max(damage - defener.CurrentDefence, 0);
    //     CurrentHealth = Mathf.Max(CurrentHealth - currentDamage, 0);
    //     //是否为空   更新血条
    //     UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
    //     //石头砸死也能获得经验值
    //     GameManager.Instance.playerStats.characterData.UpdateExp(characterData.killPoint);
    // }

    private int CurrentDamage()
    {
        //获取随机伤害值
        float coreDamage = UnityEngine.Random.Range(attackData.minDamge, attackData.maxDamge);
        //大前提判断是否暴击
        if(isCritical)
        {
            coreDamage *= attackData.criticalMultilier;
            //Debug.Log("暴击"+coreDamage);
        }
        //强制转换返回值
        return (int)coreDamage; 
    }
    #endregion
    #region 装备 武器 等
    public void SwitchLightEquip(ItemData_SO light)
    {
        UnEquipLight();
        EquipLight(light);
    }
    public void EquipLight(ItemData_SO light) 
    {   
        //生成
        if(light.toolPrefab!=null)
        {   //                            Transform Parent
            Instantiate(light.toolPrefab,lightSlot);
        }
    }
    public void UnEquipLight()
    {
        if(lightSlot.GetComponentInChildren<Light>()!=null)
        {
            print(lightSlot.GetComponentInChildren<Light>());
            Destroy(lightSlot.GetComponentInChildren<Light>().gameObject);
        }
        else
        {
            return;
        }
       
    }
    public void SwitchBulletType(ItemData_SO weapon)
    {   
        //检测原有子弹类型，以及对象池中是否含有需要的对象
            UnEquipBullet();
            EquipBullet(weapon);
    }

    public  void UnEquipBullet()
    {
        attackData.ApplyWeaponData(baseattackData);
        attackUp =false;
    }
    public  void EquipBullet(ItemData_SO weapon)
    {
        //子弹的攻击数据不为空  则替换玩家攻击数据
        if(weapon.weaponAttackData!=null)
        {
            //FIXME：添加子弹池        并重新实例化？
            // PoolManager.Instance.poolList.Add(new Pool ( Resources.Load<GameObject>("Prefabs/Character/SpecialBullet"), 10));
            // PoolManager.Instance.Initialize(PoolManager.Instance.poolList);
            attackData.ApplyWeaponData(weapon.weaponAttackData);
            attackUp = true;
            
        }
    }

    //?л?????   
    // public void ChangeWeapon(ItemData_SO weapon)
    // {
    //     UnEquipWeapon();
    //     EquipWeapon(weapon);
    //     //?л????????????????????            ???????InventoryManager?е???
    //     //InventoryManager.Instance.UpdateStatsText(MaxHealth,attackData.minDamge,attackData.maxDamge);
    // }
    // public void EquipWeapon(ItemData_SO weapon) 
    // {
    //     // if(weapon.weaponPrefab!=null)
    //     // {   
    //     //     //?????????
    //     //     Instantiate(weapon.weaponPrefab,weaponSlot);
    //     // }
    //     //TODO:???????????????????
    //     //????????  ????????????????????????
    //     //?л?????     ????????????????
    //     //GetComponent<Animator>().runtimeAnimatorController =  weapon.weaponAnimator;
    //     attackData.ApplyWeaponData(weapon.weaponAttackData);
    // }
    // public void UnEquipWeapon()
    // {
    //     //??????λ?????????????     ??????????   ??????????к??????????????

    //     if(weaponSlot.transform.childCount!=0)
    //     {
    //         for(int i = 0 ; i<weaponSlot.transform.childCount; i++)
    //         {
    //             Destroy(weaponSlot.transform.GetChild(i).gameObject);
    //         }
    //     }
    //     //???????
    //     //?л?????
    //     //GetComponent<Animator>().runtimeAnimatorController = baseAnimator;
    //     attackData.ApplyWeaponData(baseattackData);

    // }
    #endregion
    #region  使用道具等数据更改
    public void ApplyHealth(int amount)
    {

        if(CurrentHealth+amount <=MaxHealth)
        {
            CurrentHealth+=amount;
        }
        else
        {
            CurrentHealth = MaxHealth;
        }
        
    }
    public void ApplyBullet(int amount)
    {
        if(readyBullets+amount <=60)
        {
            readyBullets+=amount;
        }
        else
        {
            readyBullets = 60;
        }
        
    }
    #endregion
}
