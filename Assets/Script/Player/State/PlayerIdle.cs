using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;
public class PlayerIdle : StateBase
{
    public override void Enter()
    {

        (_character as Player)._CurrentState = this;
        base.Change_Info("Idle");
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (_player._inputX != 0)
            _statemachine.ChangeState<PlayerMove>((int)PlayerState.Move);
        else if (!_player.isOnGround)
            _statemachine.ChangeState<PlayerAirState>(-1);
    }

    public override void Exit()
    {

        base.Exit();
    }


}
