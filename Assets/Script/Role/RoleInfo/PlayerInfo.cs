using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour, IPlayerInfo, IBuff
{
    [SerializeField]
    private float speed;
    private float originalSpeed;
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private float forceJump;
    private float originalForceJump;
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
    //宿主类
    private Player playerClass;

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
            case GetInfoType.ForceJump:
                return forceJump;
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
            case GetInfoType.DashSpeed:
                return dashSpeed;
        }
        Debug.LogError(this.name + "获取信息" + getInfoType + "错误");
        return 0;

    }
    public void Hit(float damage, DamageTimeType damageTimeType)
    {
        if (damage > defense)
        {
            health = health - (damage - defense);
        }

        if (health > 0 && damageTimeType == DamageTimeType.Dizziness)
        {
            playerClass.Hit();
        }
        else if (health > 0 && damageTimeType == DamageTimeType.ReduceHealth)
        {

        }
        else if (health <= 0)
        {
            if (!playerClass._isDead)
                playerClass.Dead();
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

    public void SetSpeedAndForceJump(float Speed, float ForceJump)
    {
        this.speed = Speed;
        this.forceJump = ForceJump;
    }

    public void ResetSpeedAndForceJump()
    {
        speed = originalSpeed;
        forceJump = originalForceJump;

    }

    public void SetHealth(float NewHealth)
    {
        health = NewHealth;
    }
    #endregion



    #region Unity方法
    void Start()
    {
        playerClass = gameObject.GetComponent<Player>();
        originalSpeed = speed;
        originalForceJump = forceJump;
    }

    void Update()
    {

    }

    private void OnEnable()
    {
        health = maxHealth;
    }

    private void OnDisable()
    {

    }
    #endregion



}
