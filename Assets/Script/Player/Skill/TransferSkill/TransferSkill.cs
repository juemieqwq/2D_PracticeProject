using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TransferSkill : BaseSkill
{
    public bool _isCoolTime = true;
    public bool isCoolTime
    {
        get { return currentObject == null ? _isCoolTime : !objectClass.isActive; }
        set { _isCoolTime = value; }
    }

    public bool objectIsActive
    {
        get { return objectClass != null ? objectClass.isActive : false; }
        set { }
    }

    //public TransferObject.TransferState transferState
    //{
    //    get { return transferObject != null ? transferObject.state : TransferObject.TransferState.Normal; }
    //    set
    //    {
    //        if (transferObject != null)
    //            transferObject.state = value;
    //        else
    //        {
    //            Debug.Log("转移技能无对象设置转移状态失败");
    //        }
    //    }
    //}

    //public delegate void ReleaseObject();

    //public static event ReleaseObject releaseObject;


    ISkiilRealize realize;


    public TransferSkill(float coolDown, PlayerManager playerManager, Player player) : base(coolDown, playerManager, player)
    {

    }

    public override bool CanUseSkill()
    {

        ObjectPoolManager.instance.CreateNewPool(TransferObject.poolName, _playerManager.transferPrefab, 1);

        if (_coolDownTime < 0)
        {
            return true;
        }
        return false;
    }

    public override void SkillUpdate()
    {
        if (isCoolTime)
            _coolDownTime -= Time.deltaTime;


    }

    public override void UseSkill()
    {

        base.UseSkill();

        currentObject = ObjectPoolManager.instance.GetObject(TransferObject.poolName);
        objectClass = currentObject.GetComponent<TransferObject>();
        objectClass.Init(_player);
        currentObject.transform.position = _player.transform.position;
        currentObject.SetActive(true);
        isCoolTime = false;
    }

    public void SetObjectIsTransferPlayer(bool isTransfer)
    {
        if (objectClass.isTransferPlayer == true)
            return;

        if (currentObject != null)
        {

            objectClass.isTransferPlayer = isTransfer;
            ObjectPoolManager.instance.ReleaseObject(TransferObject.poolName, currentObject);
            RealizeManager.instance.realizeDic.TryGetValue(typeof(TransferSkill), out realize);
            (realize as TransferRealize).currentSkill = this;
            realize.BaseRealize();
            //realize.ExtraRealize();
        }
        else
        {
            Debug.Log("设置角色是否进行转移失败");
        }
    }



}
