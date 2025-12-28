using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideBehavior : PlayerBaseState
{
    Vector2 normalFallVelocity;
    Vector2 reduceFallVelocity;

    protected override void PlayerInit()
    {
        base.PlayerInit();
        normalFallVelocity = new Vector2(0, -2);
        reduceFallVelocity = new Vector2(0, -1);
    }

    public override void Enter()
    {
        base.Enter();
        PlayerInit();

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Input.GetAxisRaw("Horizontal") == player.direction)
        {
            hostRigidbody2D.velocity = reduceFallVelocity;
        }
        else
            hostRigidbody2D.velocity = normalFallVelocity;
        //Debug.LogError(string.Concat("当前滑墙速度:", hostRigidbody2D.velocity.y));
    }

    public override void Update()
    {
        base.Update();
        if (player.isOnGround)
        {
            hostStateMachine.ChangeState<PlayerIdleBehavior>("Idle1");
            return;
        }
        else if (!player.isTouchWall)
        {
            hostStateMachine.ChangeState<PlayerFallBehavior>("Fall1");
            return;
        }
        else if (Input.GetKeyDown("space"))
        {
            hostStateMachine.ChangeState<PlayerTouchWallJumpBehavior>("Jump2");
            return;
        }
    }
}
