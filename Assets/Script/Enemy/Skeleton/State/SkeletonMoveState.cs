using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : StateBaseEnemy
{
    public override void Enter()
    {

        SetAnimBoolName("Move");

        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (_host._frontIsCrashWall || !_host._frontIsHaveGround)
        {
            _stateMachine.ChangeState<SkeletonIdleState>((int)EnemyState.Idle);
            return;
        }
        else if (_time >= 2)
        {
            _stateMachine.ChangeState<SkeletonIdleState>((int)EnemyState.Idle);
            return;
        }

        _rigidbody.velocity = new Vector2(_host.GetSpeed() * _host._direction, 0);



    }
}
