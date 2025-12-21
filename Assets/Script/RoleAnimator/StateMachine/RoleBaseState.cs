using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleBaseState
{
    protected RoleStateMachine hostStateMachine;
    protected RoleAnimator hostAnimator;
    protected IRole host;
    protected IRoleInfo hostInfo;
    protected Rigidbody2D hostRigidbody2D;

    protected bool isInit = false;


    //动画播放完成
    public bool isFinish { get; private set; } = false;
    protected bool isLoop = true;
    public string key;


    public void BindStatemachine(RoleStateMachine statemachine, IRole role, IRoleInfo roleInfo)
    {
        hostStateMachine = statemachine;
        hostInfo = roleInfo;
        host = role;
    }


    public virtual void Init(RoleStateMachine statemachine, IRole role, IRoleInfo roleInfo, string key)
    {
        hostStateMachine = statemachine;
        hostAnimator = hostStateMachine.roleAnimator;
        host = role;
        hostInfo = roleInfo;
        hostRigidbody2D = role.GetGameObject().GetComponent<Rigidbody2D>();
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
        hostAnimator.PlayRoleBehavior(key, isLoop);
        hostAnimator.isFinshedPlay = false;

    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }
    public virtual void Exit()
    {
        if (hostAnimator != null)
            hostAnimator.isFinshedPlay = true;
    }


}
