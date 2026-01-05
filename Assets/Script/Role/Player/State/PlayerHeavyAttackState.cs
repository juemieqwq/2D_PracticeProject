using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyAttackState : StateBase
{
    private float time;
    public override void Enter()
    {
        time = 0;
        Change_Info("HeavyAttack");
        //_player._CurrentState = this;
        _player.SetIsInput(false);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        time += Time.deltaTime;
        base.Update();
        if (time > .5f)
            _player.SetIsInput(true);
        if (IsFinish)
        {
            _statemachine.ChangeState<PlayerIdle>(0);
        }
    }
}
