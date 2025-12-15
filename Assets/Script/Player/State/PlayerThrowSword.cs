using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowSword : StateBase
{
    public override void Enter()
    {
        Change_Info("ThrowSword");
        _player._CurrentState = this;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        _anim.SetBool("Aim", false);
        _player.SetIsInput(true);
    }

    public override void Update()
    {
        base.Update();
        if (IsFinish)
        {
            _statemachine.ChangeState<PlayerIdle>((int)Player.PlayerState.Idle);
        }
    }
}
