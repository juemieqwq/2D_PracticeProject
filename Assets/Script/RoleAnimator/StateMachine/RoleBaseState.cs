using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleBaseState
{
    protected RoleStateMachine hostStateMachine;
    protected RoleAnimator roleAnimator;

    protected bool isInit = false;

    //是否成功进入状态
    protected bool isEnter = true;

    //动画播放完成
    public bool isFinish { get; private set; } = false;
    protected bool isLoop = true;

    public string key;



    public virtual void Init(RoleStateMachine statemachine, IRoleInfo roleInfo, string key)
    {
        hostStateMachine = statemachine;
        roleAnimator = hostStateMachine.roleAnimator;
        isEnter = true;
        isInit = true;
        this.key = key;
    }




    public virtual void Enter()
    {
        if (!isInit)
        {
            Debug.LogError("状态：" + this + "未进行初始化");
            return;
        }
        hostStateMachine.currentRoleState = this;
        roleAnimator.PlayRoleBehavior(key, isLoop);
        roleAnimator.isFinshedPlay = false;
    }

    public virtual void Update()
    {

    }
    public virtual void Exit()
    {
        roleAnimator.isFinshedPlay = true;
    }
    public bool GetIsEnter()
    {
        return isEnter;
    }

    public void InitIsEnter()
    {
        isEnter = true;
    }


}
