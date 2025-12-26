using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunBehavior : RoleBaseState
{
    private float speed;
    private float inputX;
    public override void Enter()
    {
        base.Enter();
        speed = hostInfo.GetInfo(GetInfoType.Speed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        inputX = (host as Player).inputX;

        if (inputX == 0 && isOnGoround)
        {
            string key = RoleAnimator.BehaviorNameAndNumToString(BehaviorContainer.RoleBehavior.Idle, 1);
            hostStateMachine.ChangeState<PlayerIdleBehavior>(key);
        }
        else if (!isOnGoround)
        {
            hostStateMachine.ChangeState<PlayerFallBehavior>("Fall1");
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (inputX != 0)
            hostRigidbody2D.velocity = new Vector2(inputX * speed, hostRigidbody2D.velocity.y);
    }
}
