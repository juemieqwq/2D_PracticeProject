using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using static CharacterMultiplierTable;


public interface IPlayerInfo : IRoleInfo
{


    public void SetSpeedAndForceJump(float Speed, float ForceJump);
    public void ResetSpeedAndForceJump();

}

public interface IEnemyInfo : IRoleInfo
{
    public void SetSpeed(float Speed);
    public void ResetSpeed();
}

public interface ISkillInfo
{
    public float GetDamage();
    public DamageTimeType GetDamageTimeType();
    public void SetDamageTimeType(DamageTimeType damageTimeType);
    public DamageType GetDamageType();
    public DamageElementType GetDamageElementType();

}

/// <summary>
/// 造成伤害的时机
/// </summary>
public enum DamageTimeType
{
    ReduceHealth,
    Hit,
    Dizziness,
    IntervalHit,
    IntervalDizziness
}
/// <summary>
/// 造成伤害的元素类型
/// </summary>
public enum DamageElementType
{
    Null,
    Fire,
    Ice,
    //闪电
    Lightning,
    //毒
    Toxic,
    //神圣
    Holy,
    //腐化，对人特攻
    Corruption

}

/// <summary>
/// 三大攻击类型和真实伤害
/// </summary>
public enum DamageType
{
    /// <summary>
    /// 斩击
    /// </summary>
    Slashing,
    /// <summary>
    /// 钝击
    /// </summary>
    Bludgeoning,
    /// <summary>
    /// 穿刺
    /// </summary>
    Piercing,
    /// <summary>
    /// 元素伤害
    /// </summary>
    Elementaldamage,
    TrueDamage

}

/// <summary>
/// 角色护甲类型
/// </summary>
public enum ArmorType
{
    //脆弱
    Fragile,
    //无护甲
    UnArmored,
    //轻甲
    LightArmor,
    //重甲
    HeavyArmor
}

/// <summary>
/// 角色种类
/// </summary>
public enum CharacterType
{
    Human,
    //亡灵
    Ghost

}

public enum GetInfoType
{
    ForceJump,
    Speed,
    MaxHealth,
    Health,
    Defense,
    Damage,
    ScaleTime

}

public class InfoManager : MonoBehaviour
{

    public static InfoManager Instance = null;

    private InfoManager()
    {

    }

    //存放玩家角色信息
    private IPlayerInfo playerInfo;

    //角色类型抗性表
    [SerializeField]
    private CharacterMultiplierTable characterMultiplierTable;


    #region Unity方法
    void Awake()
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
        Debug.Assert(characterMultiplierTable != null, "角色受伤倍率表没有进行赋值", this);

        characterMultiplierTable.Init();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDisable()
    {
        //取消技能列表的初始化
        characterMultiplierTable?.CancelInit();
    }
    #endregion

    #region 该类的公开方法


    #region Damage

    /// <summary>
    /// 计算前一个对后一个造成的伤害，先通过护甲对伤害进行减少，后在乘以元素伤害
    /// </summary>
    /// <param name="playerInfo"></param>
    /// <param name="enemyInfo"></param>
    /// <returns></returns>
    public float ComputeDamage(IPlayerInfo playerInfo, IEnemyInfo enemyInfo)
    {
        float damage = playerInfo.GetInfo(GetInfoType.Damage);
        float defense = enemyInfo.GetInfo(GetInfoType.Defense);
        CharacterType characterType = enemyInfo.GetCharacterType();
        DamageType damageType = playerInfo.GetDamageType();
        DamageElementType damageElementType = playerInfo.GetDamageElementType();
        ArmorType armorType = enemyInfo.GetArmorType();

        //如果伤害为真实伤害将直接返回伤害不就行减免
        if (damageType == DamageType.TrueDamage)
            return damage;
        float DamageResistanceMultiplier = characterMultiplierTable.GetCharacterDamageResistanceMultiplier(armorType, damageType);
        float ElementResistanceMultiplier = characterMultiplierTable.GetCharacterElementResistanceMultiplier(characterType, damageElementType);
        //先以攻击类型对伤害进行减免或增加
        if (defense != 0)
            damage = damage - defense / DamageResistanceMultiplier;
        //如果护甲的减免高于伤害将免疫伤害
        if (damage < 0)
            return 0;
        //在通过元素抗性对伤害进行减免和增加
        damage = damage * ElementResistanceMultiplier;

        return damage;
    }

    public float ComputeDamage(IEnemyInfo enemyInfo, IPlayerInfo playerInfo)
    {
        float damage = enemyInfo.GetInfo(GetInfoType.Damage);
        float defense = playerInfo.GetInfo(GetInfoType.Defense);
        CharacterType characterType = playerInfo.GetCharacterType();
        DamageType damageType = enemyInfo.GetDamageType();
        DamageElementType damageElementType = enemyInfo.GetDamageElementType();
        ArmorType armorType = playerInfo.GetArmorType();


        //如果伤害为真实伤害将直接返回伤害不就行减免
        if (damageType == DamageType.TrueDamage)
            return damage;
        float DamageResistanceMultiplier = characterMultiplierTable.GetCharacterDamageResistanceMultiplier(armorType, damageType);
        float ElementResistanceMultiplier = characterMultiplierTable.GetCharacterElementResistanceMultiplier(characterType, damageElementType);
        //先以攻击类型对伤害进行减免或增加
        if (defense != 0)
            damage = damage - (defense / DamageResistanceMultiplier);
        else
            damage = damage + damage * .2f;
        //如果护甲的减免高于伤害将免疫伤害
        if (damage < 0)
            return 0;
        //在通过元素抗性对伤害进行减免和增加
        damage = damage * ElementResistanceMultiplier;
        return damage;
    }

    public float ComputeDamage(ISkillInfo skillInfo, IEnemyInfo enemyInfo)
    {
        float damage = skillInfo.GetDamage();
        float defense = enemyInfo.GetInfo(GetInfoType.Defense);
        CharacterType characterType = enemyInfo.GetCharacterType();
        DamageType damageType = skillInfo.GetDamageType();
        DamageElementType damageElementType = skillInfo.GetDamageElementType();
        ArmorType armorType = enemyInfo.GetArmorType();
        //如果伤害为真实伤害将直接返回伤害不就行减免
        if (damageType == DamageType.TrueDamage)
            return damage;
        float DamageResistanceMultiplier = characterMultiplierTable.GetCharacterDamageResistanceMultiplier(armorType, damageType);
        float ElementResistanceMultiplier = characterMultiplierTable.GetCharacterElementResistanceMultiplier(characterType, damageElementType);
        //先以攻击类型对伤害进行减免或增加
        if (defense != 0)
            damage = damage - defense / DamageResistanceMultiplier;
        else
            damage = damage + damage * .2f;
        //如果护甲的减免高于伤害将免疫伤害
        if (damage < 0)
            return 0;
        //在通过元素抗性对伤害进行减免和增加
        damage = damage * ElementResistanceMultiplier;
        return damage;
    }

    public float ComputeDamage(IPlayerInfo playerInfo, float damage, DamageType damageType, DamageElementType damageElementType = DamageElementType.Null)
    {
        if (damageType == DamageType.TrueDamage)
            return damage;
        float LastDamage = damage;
        float DamageResistanceMultiplier = characterMultiplierTable.GetCharacterDamageResistanceMultiplier(playerInfo.GetArmorType(), damageType);
        float ElementResistanceMultiplier = characterMultiplierTable.GetCharacterElementResistanceMultiplier(playerInfo.GetCharacterType(), damageElementType);
        float defense = playerInfo.GetInfo(GetInfoType.Defense);
        //先以攻击类型对伤害进行减免或增加
        if (defense != 0)
            LastDamage = damage - defense / DamageResistanceMultiplier;
        else
            LastDamage = damage + damage * .2f;
        //如果护甲的减免高于伤害将免疫伤害
        if (LastDamage < 0)
            return 0;
        //在通过元素抗性对伤害进行减免和增加
        LastDamage = LastDamage * ElementResistanceMultiplier;
        return LastDamage;
    }

    public float ComputeDamage(IEnemyInfo enemyInfo, float damage, DamageType damageType, DamageElementType damageElementType = DamageElementType.Null)
    {

        if (damageType == DamageType.TrueDamage)
            return damage;
        float LastDamage = damage;
        float DamageResistanceMultiplier = characterMultiplierTable.GetCharacterDamageResistanceMultiplier(enemyInfo.GetArmorType(), damageType);
        float ElementResistanceMultiplier = characterMultiplierTable.GetCharacterElementResistanceMultiplier(enemyInfo.GetCharacterType(), damageElementType);
        float defense = enemyInfo.GetInfo(GetInfoType.Defense);
        //先以攻击类型对伤害进行减免或增加
        if (defense != 0)
            LastDamage = damage - defense / DamageResistanceMultiplier;
        else
            LastDamage = damage + damage * .2f;
        //如果护甲的减免高于伤害将免疫伤害
        if (LastDamage < 0)
            return 0;
        //在通过元素抗性对伤害进行减免和增加
        LastDamage = LastDamage * ElementResistanceMultiplier;
        return LastDamage;
    }


    /// <summary>
    /// 对角色造成伤害，前面的参数对后面的参数造成伤害
    /// </summary>
    /// <param name="playerInfo"></param>
    /// <param name="enemyInfo"></param>
    /// <param name="action">当血量归零时调用的方法</param>
    public void Damage(IPlayerInfo playerInfo, IEnemyInfo enemyInfo)
    {

        float health = enemyInfo.GetInfo(GetInfoType.Health);
        if (health <= 0) return;
        float damage = ComputeDamage(playerInfo, enemyInfo);
        enemyInfo.Hit(damage, playerInfo.GetDamageTimeType());

    }

    public void Damage(IEnemyInfo enemyInfo, IPlayerInfo playerInfo)
    {
        float health = enemyInfo.GetInfo(GetInfoType.Health);
        if (health <= 0) return;
        float damage = ComputeDamage(enemyInfo, playerInfo);
        playerInfo.Hit(damage, enemyInfo.GetDamageTimeType());

    }

    public void Damage(ISkillInfo skillInfo, IEnemyInfo enemyInfo)
    {
        float health = enemyInfo.GetInfo(GetInfoType.Health);
        if (health <= 0) return;
        float damage = ComputeDamage(skillInfo, enemyInfo);
        enemyInfo.Hit(damage, skillInfo.GetDamageTimeType());

    }
    #endregion


    #endregion


}



