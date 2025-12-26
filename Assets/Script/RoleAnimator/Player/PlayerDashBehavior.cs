using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashBehavior : RoleBaseState
{
    float duration = 0.2f;
    float time;
    float dashSpeed;
    float inputX;
    public override void Enter()
    {
        base.Enter();
        time = duration;
        inputX = (host as Player).inputX;
        dashSpeed = hostInfo.GetInfo(GetInfoType.DashSpeed);
        (host as Player).SetIsInput(false);
    }

    public override void Exit()
    {
        base.Exit();
        (host as Player).SetIsInput(true);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (inputX != 0 && time > 0)
            hostRigidbody2D.velocity = new Vector2(dashSpeed * inputX, 0);
        else if (inputX == 0 && time > 0)
            hostRigidbody2D.velocity = new Vector2(dashSpeed * (host as Player).direction, 0);
        if (isOnGoround && time <= 0)
        {
            hostStateMachine.ChangeState<PlayerIdleBehavior>("Idle1");
        }
        else if (!isOnGoround && time <= 0)
        {
            hostStateMachine.ChangeState<PlayerFallBehavior>("Fall1");
        }
    }

    public override void Update()
    {
        base.Update();
        time -= Time.deltaTime;
    }
}
