using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBackAttackBehavior : PlayerBaseState
{
    public override void Enter()
    {
        isLoop = false;
        base.Enter();
        base.PlayerInit();
        PlayerManager.instance.ApplyCameraViewZoomAndOffset(1f / 9f * 8f, 2, new Vector3(3, 0, 0));
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
