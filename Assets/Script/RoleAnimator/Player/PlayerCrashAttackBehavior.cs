using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrashAttackBehavior : PlayerBaseState
{

    float time;
    bool isCrashAttack;
    public override void Enter()
    {
        base.Enter();
        base.PlayerInit();
        hostRigidbody2D.velocity = Vector2.zero;
        time = 0;
        isCrashAttack = false;
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
        time += Time.deltaTime;
        if (player._playerAnimator.CheckBackAttack() && !isCrashAttack)
        {
            PlayerManager.instance.ApplyCameraViewZoomAndOffset(.5f, 2, new Vector3(1, 0, 0));
            PlayerManager.instance.ApplyCameraShake(.5f, 2);
            isCrashAttack = true;
            time = 0;
        }
        else if (time > .25f && !isCrashAttack)
        {
            hostStateMachine.ChangeState<PlayerIdleBehavior>("Idle1");
        }
        else if (time > .5f && isCrashAttack)
            hostStateMachine.ChangeState<PlayerBackAttackBehavior>("BackAttack1");
    }

}
