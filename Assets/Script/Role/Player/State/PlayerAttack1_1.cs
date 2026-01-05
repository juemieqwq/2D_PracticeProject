using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class PlayerAttack1_1 : StateBase
{
    private float _inputX;
    private bool IsAttack;
    private bool IsFlip;
    //按鼠标的次数
    private float DownMouse0Number;
    public override void Enter()
    {
        _inputX = 0;
        IsFlip = false;
        IsAttack = false;
        DownMouse0Number = 0;
        SetVelocity(0, 0);
        _player.SetIsInput(false);
        //_player._CurrentState = this;
        Change_Info("Attack1_1");
        base.Enter();
    }

    public override void Exit()
    {
        if (!IsAttack)
            _player.SetIsInput(true);
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown("a") || Input.GetKeyDown("d"))
        {
            _inputX = Input.GetAxisRaw("Horizontal");
        }


        if (Input.GetKeyDown("mouse 0"))
            DownMouse0Number++;

        if (DownMouse0Number > 1 && IsFinish)
        {

            _player.SetInputX(_inputX);
            IsAttack = true;
            _statemachine.ChangeState<PlayerAttack1_2>((int)PlayerState.Attack1_2);
        }
        else if (_inputX != 0 && Input.GetKeyDown("left shift"))
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
