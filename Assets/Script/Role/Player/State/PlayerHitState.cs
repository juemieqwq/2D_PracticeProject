using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : StateBase
{
    public override void Enter()
    {
        _rigidbody.velocity = Vector2.zero;
        Change_Info("Hit");
        //_player._CurrentState = this;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        FinishAnimator(0);
    }

    public override void Update()
    {
        base.Update();
        if (IsFinish)
        {
            _statemachine.ChangeState<PlayerIdle>(0);
        }

    }
}
