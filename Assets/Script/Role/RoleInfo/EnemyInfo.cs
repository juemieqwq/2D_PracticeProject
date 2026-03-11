using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour, IEnemyInfo, IBuff
{
    [SerializeField]
    private float speed;
    //原始值
    private float originalSpeed;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float health;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float defense;
    [Range(0f, 2f)]
    [SerializeField]
    private float scaleTime;

    [SerializeField]
    private DamageType damageType;
    [SerializeField]
    private DamageTimeType damageTimeType;
    [SerializeField]
    private DamageElementType damageElementType;

    [SerializeField]
    private CharacterType characterType;

    [SerializeField]
    private ArmorType armorType;
    [SerializeField]
    [Header("受伤后的无敌时间")]
    private float ineffectiveTime;
    private float time;
    //宿主的类
    private BaseEnemy enemyClass;

    #region 接口方法

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public float GetInfo(GetInfoType getInfoType)
    {
        switch (getInfoType)
        {
            case GetInfoType.Speed:
                return speed;
            case GetInfoType.Defense:
                return defense;
            case GetInfoType.Health:
                return health;
            case GetInfoType.Damage:
                return damage;
            case GetInfoType.MaxHealth:
                return maxHealth;
            case GetInfoType.ScaleTime:
                return scaleTime;
        }
        Debug.LogError(this.name + "获取信息错误");
        return 0;

    }

    public void SetSpeed(float Speed)
    {
        this.speed = Speed;
    }

    public void ResetSpeed()
    {
        speed = originalSpeed;
    }
    public void Hit(float damage, DamageTimeType damageTimeType)
    {
        bool isHit = false;
        if (damage > defense && (damageTimeType == DamageTimeType.ReduceHealth || damageTimeType == DamageTimeType.Hit || damageTimeType == DamageTimeType.Dizziness))
        {
            health = health - (damage - defense);
            Debug.LogError("受到伤害：" + (damage - defense));
            isHit = true;
        }
        else if (damage > defense && (damageTimeType == DamageTimeType.IntervalHit || damageTimeType == DamageTimeType.IntervalDizziness) && time < 0)
        {
            health = health - (damage - defense);
            time = ineffectiveTime;
            Debug.LogError("间隔受到伤害：" + (damage - defense));
            isHit = true;
        }
        if (health > 0 && isHit)
        {
            enemyClass.Hit(damageTimeType);
        }
        else if (health <= 0)

        {
            enemyClass.Dead();
        }
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

    public CharacterType GetCharacterType()
    {
        return characterType;
    }

    public ArmorType GetArmorType()
    {
        return armorType;
    }
    #endregion

    #region Unity方法
    void Start()
    {
        enemyClass = gameObject.GetComponent<BaseEnemy>();
        originalSpeed = speed;
    }

    void Update()
    {
        time -= Time.deltaTime;
    }
    private void OnEnable()
    {
        health = maxHealth;
    }

    private void OnDisable()
    {

    }
    #endregion

    #region 该类的方法
    public void SetHealth(float NewHealth) => health = NewHealth;
    #endregion
}
