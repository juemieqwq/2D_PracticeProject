using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchWallJumpBehavior : PlayerBaseState
{

    public override void Enter()
    {

        isLoop = false;
        base.Enter();
        base.PlayerInit();
        Debug.LogError(player.direction);
        hostRigidbody2D.velocity = new Vector2(hostInfo.GetInfo(GetInfoType.Speed) * -.5f * player.direction, hostInfo.GetInfo(GetInfoType.ForceJump));
        player.Filp();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (inputX != 0)
            hostRigidbody2D.velocity = new Vector2(hostInfo.GetInfo(GetInfoType.Speed) * .5f * player.inputX, hostRigidbody2D.velocity.y);
    }

    public override void Update()
    {
        base.Update();
        if (player.isOnGround)
        {
            hostStateMachine.ChangeState<PlayerIdleBehavior>("Idle1");
        }
        else if (isFinish)
        {
            hostStateMachine.ChangeState<PlayerFallBehavior>("Fall1");
        }
    }
}
