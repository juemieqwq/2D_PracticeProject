using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : StateBaseEnemy
{
    public override void Enter()
    {

        _host.SetIsChangeState(false);
        _host.SetCoolTime(10);
        if (_host._rigidbody.velocity != Vector2.zero)
            _host._rigidbody.velocity = Vector2.zero;
        SetAnimBoolName("Attack1");
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();


        if (_isFinish)
        {
            _host.SetCoolTime(1);
            _stateMachine.ChangeState<SkeletonIdleState>(0);
            return;
        }

    }
}
