using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : StateBaseEnemy
{
    public override void Enter()
    {
        if (_host._rigidbody.velocity != Vector2.zero)
        {
            _host._rigidbody.velocity = Vector2.zero;
        }
        SetAnimBoolName("Idle");

        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (_time >= 1)
        {
            if (_host._frontIsCrashWall || !_host._frontIsHaveGround)
                _host.Filp();
            _stateMachine.ChangeState<SkeletonMoveState>((int)EnemyState.Move);
        }

    }
}
