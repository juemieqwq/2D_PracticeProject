using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class PlayerGroundState : StateBase
{
    public override void Enter()
    {

        Debug.Log("进入地面中转");
        if (_player._inputX != 0)
            _statemachine.ChangeState<PlayerMove>((int)PlayerState.Move);
        else if (_player._inputX == 0)
        {
            SetVelocity(0, 0);
            _statemachine.ChangeState<PlayerIdle>((int)PlayerState.Idle);

        }


    }

    public virtual void SetIsEnter()
    {
        if (_player.isOnGround)
            IsEnter = true;
        else
            IsEnter = false;
    }

    //protected void EnterAirState()
    //{
    //    if (!_player.isOnGround)
    //        _statemachine.ChangeState<PlayerAir>((int)PlayerState.Air);

    //}
    public override void Exit()
    {
        Debug.Log("退出地面中转");
    }

}
