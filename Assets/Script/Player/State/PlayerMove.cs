using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class PlayerMove : StateBase
{
    public override void Enter()
    {
        (_character as Player)._CurrentState = this;
        base.Change_Info("Move");
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        SetVelocity(Speed * _player._inputX, _rigidbody.velocity.y);
        if (_player._inputX == 0)
        {
            _statemachine.ChangeState<PlayerIdle>((int)PlayerState.Idle);
        }
        else if (!_player.isOnGround)
            _statemachine.ChangeState<PlayerAirState>(-1);

    }
    public override void Exit()
    {
        _player.SetInputX(0);
        base.Exit();
    }
}
