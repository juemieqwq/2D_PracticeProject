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
    protected bool isOnGoround;
    protected float inputX;
    protected float inputY;


    protected bool isInit = false;


    //动画播放完成
    public bool isFinish
    {
        get
        {
            if (hostAnimator != null)
                return hostAnimator.isFinshedPlay;
            else return false;
        }
        private set { hostAnimator.isFinshedPlay = value; }
    }
    protected bool isLoop = true;
    public string key;


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
        if (hostAnimator != null)
            isFinish = false;

    }

    public virtual void Update()
    {
        if (host.GetType() == typeof(Player))
        {
            var player = host as Player;
            this.isOnGoround = player.isOnGround;
            this.inputX = player.inputX;
            this.inputY = player.inputY;
        }
    }

    public virtual void FixedUpdate()
    {

    }
    public virtual void Exit()
    {
        if (hostAnimator != null)
            isFinish = true;
    }


}
