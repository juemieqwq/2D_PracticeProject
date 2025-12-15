using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static BuffEffectTable;
using static BuffRealize;

public class BuffManager : MonoBehaviour
{
    private BuffManager()
    {

    }

    public static BuffManager Instance = null;

    [SerializeField]
    public BuffEffectTable BuffEffectTable = null;

    [SerializeField]
    private SkeletonEnemy test;

    private void Awake()
    {

        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
        Debug.Assert(BuffEffectTable != null, "Buff效果表为空", this);

        BuffEffectTable.Init();

    }

    private void OnDisable()
    {
        BuffEffectTable.Reset();
    }

    private void Update()
    {
        if (Input.GetKeyDown("o"))
        {
            RuningBuff(PlayerManager.instance.player.GetComponent<IBuff>(), 3, .5f, BuffType.Slow, BuffEffectType.Freeze);
        }
        else if (Input.GetKeyDown("p"))
        {
            RuningBuff(test.GetComponent<IBuff>(), 3, 10, BuffType.DamageOverTime, BuffEffectType.Burn, DamageType.Elementaldamage, DamageElementType.Fire);
        }
    }

    #region 公开函数
    public BuffRealize AddContainerObjectAddClass(IBuff host, float durationTime, BuffType buffType, BuffEffectType buffEffectType)
    {
        GameObject containerObject = ObjectPoolManager.instance.GetObject(buffType.ToString());
        BuffRealize realizeClass = null;
        if (containerObject != null)
        {
            var HostObject = host.GetGameObject();
            containerObject.transform.parent = HostObject.transform;
            realizeClass = containerObject.GetComponent<BuffRealize>();
            containerObject.SetActive(true);
            //if (host.GetType() == typeof(PlayerInfo))
            //{
            //    var playerInfo = (PlayerInfo)host;
            //    containerObject.transform.parent = playerInfo.transform;
            //    realizeClass = containerObject.GetComponent<BuffRealize>();
            //    containerObject.SetActive(true);
            //}
            //else if (host.GetType() == typeof(EnemyInfo))
            //{
            //    var enemyInfo = (EnemyInfo)host;
            //    containerObject.transform.parent = enemyInfo.transform;
            //    realizeClass = containerObject.GetComponent<BuffRealize>();
            //    containerObject.SetActive(true);
            //}
        }
        else if (containerObject == null)
        {
            var HostObject = host.GetGameObject();
            containerObject = new GameObject(buffType.ToString());
            containerObject.transform.SetParent(HostObject.transform);
            //if (host.GetType() == typeof(PlayerInfo))
            //{
            //    var playerInfo = (PlayerInfo)host;
            //    containerObject = new GameObject(buffType.ToString());
            //    containerObject.transform.SetParent(playerInfo.transform);
            //}
            //else if (host.GetType() == typeof(EnemyInfo))
            //{
            //    var enemyInfo = (EnemyInfo)host;
            //    containerObject = new GameObject(buffType.ToString());
            //    containerObject.transform.SetParent(enemyInfo.transform);
            //}

            if (containerObject != null)
            {
                realizeClass = containerObject.AddComponent<BuffRealize>();
                realizeClass.currentBuffType = buffType;
                ObjectPoolManager.instance.CreateNewPool(buffType.ToString(), containerObject, 0);
            }
        }

        if (realizeClass != null)
        {
            var buffTable = BuffEffectTable.buffShowEffectDic.GetValueOrDefault(buffEffectType);
            realizeClass.Init(host, durationTime, buffTable);

        }
        return realizeClass;
    }

    //通过实例化一个空对象在使得角色身上多一个用于存储效果和逻辑的临时对象（容器）
    /// <summary>
    /// 可以传入单个种类，可以使角色上该类型的Buff
    /// </summary>
    /// <param name="host">角色信息类</param>
    /// <param name="durationTime">持续时间</param>
    /// <param name="damage">持续伤害的每次伤害量</param>
    /// <param name="scaleTime">缓慢或急速的强度（0-2）之间</param>
    /// <param name="buffType">buff种类</param>
    public void RuningBuff(IBuff host, float durationTime, float scaleTime, BuffType buffType, BuffEffectType buffEffectType)
    {
        if (IsHaveBuff(host, buffType, 0, scaleTime))
            return;

        BuffRealize realizeClass = AddContainerObjectAddClass(host, durationTime, buffType, buffEffectType);
        switch (buffType)
        {
            case BuffType.Slow:
                realizeClass.HasteOrSlowMethod(scaleTime);
                break;
            case BuffType.Haste:
                realizeClass.HasteOrSlowMethod(scaleTime);
                break;
        }
    }
    public void RuningBuff(IBuff host, float durationTime, float damage, BuffType buffType, BuffEffectType buffEffectType, DamageType damageType, DamageElementType damageElement)
    {
        if (IsHaveBuff(host, buffType, damage, 0))
            return;
        BuffRealize realizeClass = AddContainerObjectAddClass(host, durationTime, buffType, buffEffectType);
        switch (buffType)
        {
            case BuffType.DamageOverTime:
                realizeClass.DamageOrHealOverTime(damage, damageType, damageElement);
                break;
            case BuffType.HealOverTime:
                realizeClass.DamageOrHealOverTime(damage, damageType, damageElement);
                break;
        }

    }
    public bool IsHaveBuff(IBuff host, BuffType buffType, float damage, float scaleTime)
    {
        BuffRealize[] buffs = null;
        var HostObject = host.GetGameObject();
        buffs = HostObject.GetComponentsInChildren<BuffRealize>();
        if (buffs != null)
        {
            foreach (BuffRealize buff in buffs)
            {
                if (buff.currentBuffType == buffType)
                {
                    if (buff.currentBuffType == BuffType.Slow)
                        buff.StackBuff(false);
                    else
                        buff.StackBuff(true);
                    return true;
                }
            }
        }
        return false;
    }
    #endregion
}
