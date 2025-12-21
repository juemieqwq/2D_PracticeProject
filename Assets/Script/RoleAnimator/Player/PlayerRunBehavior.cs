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
        inputX = host.GetGameObject().GetComponent<TestPlayer>().inputX;

        if (inputX == 0)
        {
            string key = RoleAnimator.BehaviorNameAndNumToString(BehaviorContainer.RoleBehavior.Idle, 1);
            hostStateMachine.ChangeState<PlayerIdleBehavior>(key);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (inputX != 0)
            hostRigidbody2D.velocity = new Vector2(inputX * speed, 0);
    }
}
