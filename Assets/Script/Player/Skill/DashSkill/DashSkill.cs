using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class
    DashSkill : BaseSkill
{

    public bool isCanClonePlayer = true;


    public DashSkill(float coolDown, PlayerManager playerManager, Player player) : base(coolDown, playerManager, player)
    {

    }

    public override void SkillUpdate()
    {
        base.SkillUpdate();
    }

    public override bool CanUseSkill()
    {
        if (isCanClonePlayer)
        {
            ObjectPoolManager.instance.CreateNewPool(CloneObject.poolName, _playerManager.clonePrefab);
        }
        return base.CanUseSkill();
    }
    public override void UseSkill()
    {
        base.UseSkill();
        if (isCanClonePlayer && ObjectPoolManager.instance.GetPool(CloneObject.poolName) != null)
        {
            GameObject clone = ObjectPoolManager.instance.GetObject(CloneObject.poolName);
            clone.SetActive(true);
            clone.transform.position = _player.transform.position;
            clone.transform.rotation = _player.transform.rotation;
        }
    }



}
