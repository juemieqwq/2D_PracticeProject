using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyAttackBehavior : PlayerBaseState
{
    public override void Enter()
    {
        isLoop = false;
        base.PlayerInit();
        base.Enter();
        hostRigidbody2D.velocity = Vector3.zero;
        player.SetIsInput(false);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetIsInput(true);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        if (isFinish)
        {
            hostStateMachine.ChangeState<PlayerIdleBehavior>("Idle1");
        }
    }
}
