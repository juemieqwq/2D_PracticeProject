using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Info : MonoBehaviour, ISkillInfo
{
    [SerializeField]
    private float Damage;

    [SerializeField]
    private DamageType damageType;
    [SerializeField]
    private DamageTimeType damageTimeType;
    [SerializeField]
    private DamageElementType damageElementType;
    public float GetDamage()
    {
        return Damage;
    }
    public DamageTimeType GetDamageTimeType()
    {
        return damageTimeType;
    }

    public DamageType GetDamageType()
    {
        return damageType;
    }

    public DamageElementType GetDamageElementType()
    {
        return damageElementType;
    }


    public void SetDamageTimeType(DamageTimeType damageTimeType)
    {
        this.damageTimeType = damageTimeType;
    }

}
