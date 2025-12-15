using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ResistanceMultiplierTable", menuName = "Game/Resistance Multiplier Table")]
public class CharacterMultiplierTable : ScriptableObject
{
    [System.Serializable]
    public class ElementResistanceMultiplier
    {
        [Header("角色类型")]
        public CharacterType characterType;


        [Header("火性受伤倍率"), Range(-2, 5)]
        public float multiplierFire;

        [Header("冰系受伤倍率"), Range(-2, 5)]
        public float multiplierIce;

        [Header("电系受伤倍率"), Range(-2, 5)]
        public float multiplierLightning;

        [Header("毒系受伤倍率"), Range(-2, 5)]
        public float multiplierToxic;

        [Header("神圣受伤倍率"), Range(-2, 5)]
        public float multiplierHoly;

        [Header("腐化受伤倍率"), Range(-2, 5)]
        public float multiplierCorruption;

        [Header("真实伤害")]
        public const float TrueHit = 1;

        public float GetResistanceMultiplier(DamageElementType damageElementType)
        {

            switch (damageElementType)
            {
                case DamageElementType.Null:
                    return 1;
                case DamageElementType.Fire:
                    return multiplierFire;
                case DamageElementType.Ice:
                    return multiplierIce;
                case DamageElementType.Lightning:
                    return multiplierLightning;
                case DamageElementType.Corruption:
                    return multiplierCorruption;
                case DamageElementType.Toxic:
                    return multiplierToxic;
                case DamageElementType.Holy:
                    return multiplierHoly;

            }
            Debug.Log("抗性获取有问题");
            return 1;
        }
    }

    [System.Serializable]
    public class DamageTypeResistanceMultiplier
    {
        //装备类型
        [Header("角色装备类型")]
        public ArmorType armorType;
        //斩击
        [Header("斩击伤害造成的倍率")]
        public float multiplierSlashing;
        //钝击
        [Header("钝击伤害造成的倍率")]
        public float multiplierBludgeoning;
        //突刺
        [Header("突刺伤害造成的倍率")]
        public float multiplierPiercing;


        public float GetDamageTypeResistanceMultiplier(DamageType damageType)
        {
            float multiplier = 0;

            switch (damageType)
            {
                case DamageType.Slashing:
                    multiplier = multiplierSlashing;
                    break;
                case DamageType.Bludgeoning:
                    multiplier = multiplierBludgeoning;
                    break;
                case DamageType.Piercing:
                    multiplier = multiplierPiercing;
                    break;
                case DamageType.TrueDamage:
                    return multiplier;

            }
            return multiplier;
        }

    }

    //元素抗性表
    public List<ElementResistanceMultiplier> CharacterElementResistanceMultiplierTable = new List<ElementResistanceMultiplier>();
    //元素抗性字典
    public Dictionary<CharacterType, ElementResistanceMultiplier> CharacterElementResistanceMultiplierDic;

    //攻击类型抗性表
    public List<DamageTypeResistanceMultiplier> CharacterDamageTypeResistanceMultiplierTable = new List<DamageTypeResistanceMultiplier>();
    //攻击类型抗性字典
    public Dictionary<ArmorType, DamageTypeResistanceMultiplier> CharacterDamageTypeResistanceMultiplierDic;

    private bool isInit = false;

    //游戏结束时它不会还原，isInit为true，导致如果修改数据它的字典不会进行更新

    public void Init()
    {
        if (isInit)
            return;
        CharacterElementResistanceMultiplierDic = new Dictionary<CharacterType, ElementResistanceMultiplier>();
        CharacterDamageTypeResistanceMultiplierDic = new Dictionary<ArmorType, DamageTypeResistanceMultiplier>();

        foreach (var data in CharacterElementResistanceMultiplierTable)
        {
            if (!CharacterElementResistanceMultiplierDic.ContainsKey(data.characterType))
                CharacterElementResistanceMultiplierDic.Add(data.characterType, data);
        }

        foreach (var data in CharacterDamageTypeResistanceMultiplierTable)
        {
            if (!CharacterDamageTypeResistanceMultiplierDic.ContainsKey(data.armorType))
                CharacterDamageTypeResistanceMultiplierDic.Add(data.armorType, data);
        }

        isInit = true;
    }

    /// <summary>
    /// 通过给予角色类型和受到伤害的类型来获取抗性
    /// </summary>
    /// <param name="characterType">角色类型</param>
    /// <param name="damageElementType">伤害类型</param>
    /// <returns></returns>
    public float GetCharacterElementResistanceMultiplier(CharacterType characterType, DamageElementType damageElementType)
    {
        if (!isInit)
            Init();
        if (CharacterElementResistanceMultiplierDic.TryGetValue(characterType, out var date))
        {
            return date.GetResistanceMultiplier(damageElementType);
        }
        else
        {
            Debug.Log("字典中未有改类型数据");
        }
        return 1;
    }

    /// <summary>
    /// 通过角色属于的防具类型获取对不同攻击的抗性
    /// </summary>
    /// <param name="armorType">防具类型</param>
    /// <param name="damageType">要获取的攻击类型</param>
    /// <returns></returns>
    public float GetCharacterDamageResistanceMultiplier(ArmorType armorType, DamageType damageType)
    {
        if (!isInit)
            Init();
        if (CharacterDamageTypeResistanceMultiplierDic.TryGetValue(armorType, out var date))
        {
            return date.GetDamageTypeResistanceMultiplier(damageType);
        }
        else
        {
            Debug.LogError("获取攻击类型倍率失败");
        }
        return 1;
    }

    [ContextMenu("CancelInit")]
    public void CancelInit()
    {
        isInit = false;
        CharacterElementResistanceMultiplierDic?.Clear();
        CharacterDamageTypeResistanceMultiplierDic?.Clear();
    }

    public void OnDestroy()
    {
        isInit = false;
        CharacterElementResistanceMultiplierDic?.Clear();
        CharacterDamageTypeResistanceMultiplierDic?.Clear();
    }


}
