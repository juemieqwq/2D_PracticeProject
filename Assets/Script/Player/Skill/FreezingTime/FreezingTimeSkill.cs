using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FreezingTimeSkill : BaseSkill
{
    //生成的实例
    private FreezingTimeObject object_FreezingTime;
    private GameObject freezingTimePrefab;
    public GameObject cloneAttackPrefab { private set; get; }
    //是否可以进行克隆攻击
    public static bool isCanCloneAttack;

    public enum FreezingTimeState
    {
        Detonate,
        CloneAttack
    }

    public FreezingTimeState state;

    private void Awake()
    {
        isCanCloneAttack = false;
    }

    public FreezingTimeSkill(float coolDown, PlayerManager playerManager, Player player) : base(coolDown, playerManager, player)
    {
        freezingTimePrefab = playerManager.freezingTimePrefab;
        state = FreezingTimeState.CloneAttack;
        cloneAttackPrefab = playerManager.clonePrefab;
    }


    public override bool CanUseSkill()
    {
        ObjectPoolManager.instance.CreateNewPool(FreezingTimeObject.poolName, freezingTimePrefab);
        return base.CanUseSkill();
    }

    public override void SkillUpdate()
    {
        base.SkillUpdate();

    }

    public override void UseSkill()
    {
        object_FreezingTime = null;
        object_FreezingTime = ObjectPoolManager.instance.GetObject(FreezingTimeObject.poolName).GetComponent<FreezingTimeObject>();
        object_FreezingTime.transform.position = _player.transform.position + new Vector3(1 * _player.direction, 1);
        object_FreezingTime.SetState(state);
        object_FreezingTime.gameObject.SetActive(true);
        base.UseSkill();
    }

    public void SetisCloneAttack(bool _isCloneAttack)
    {
        isCanCloneAttack = false;
        object_FreezingTime.isCloneAttack = _isCloneAttack;

    }


}
