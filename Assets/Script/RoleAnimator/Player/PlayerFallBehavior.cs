using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallBehavior : PlayerBaseState
{
    public override void Enter()
    {
        base.PlayerInit();
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        if (inputX != 0)
            hostRigidbody2D.velocity = new Vector2(hostInfo.GetInfo(GetInfoType.Speed) * .5f * player.inputX, hostRigidbody2D.velocity.y);
        else if (hostRigidbody2D.velocity.x > hostInfo.GetInfo(GetInfoType.Speed) * .5f || hostRigidbody2D.velocity.x < -hostInfo.GetInfo(GetInfoType.Speed) * .5)
        {
            hostRigidbody2D.velocity = new Vector2(hostInfo.GetInfo(GetInfoType.Speed) * .5f * player.direction, hostRigidbody2D.velocity.y);
        }
        if (player.isOnGround)
        {
            hostStateMachine.ChangeState<PlayerIdleBehavior>("Idle1");
            return;
        }
        else if (player.isTouchWall)
        {
            hostStateMachine.ChangeState<PlayerWallSlideBehavior>("Fall2");
            return;
        }
    }
}
