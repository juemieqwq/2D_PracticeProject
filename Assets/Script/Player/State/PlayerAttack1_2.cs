using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class PlayerAttack1_2 : StateBase
{
    private float _inputX;
    public override void Enter()
    {
        _player._CurrentState = this;
        Change_Info("Attack1_2");
        base.Enter();
    }

    public override void Exit()
    {
        _player.SetIsInput(true);
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        _inputX = Input.GetAxisRaw("Horizontal");

        if (_inputX != 0 && Input.GetKeyDown("left shift"))
        {
            _player.SetInputX(_inputX);
            _statemachine.ChangeState<PlayerDash>((int)PlayerState.Dash);
        }
        else if (IsFinish)
        {
            _statemachine.ChangeState<PlayerGroundState>(-2);
        }
    }
}
