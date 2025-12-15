using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Player;

public class PlayerAirState : StateBase
{

    public virtual void SetIsEnter()
    {
        if (!_player.isOnGround)
            IsEnter = true;
        else
            IsEnter = false;
    }

    //protected void EnterGroundState()
    //{
    //    if (_player._inputX != 0)
    //        _statemachine.ChangeState<PlayerMove>((int)PlayerState.Move);
    //    else if (_player._inputX == 0)
    //        _statemachine.ChangeState<PlayerIdle>((int)PlayerState.Idle);

    //}

    public override void Enter()
    {

        Debug.Log("进入空中中转");
        if (!_player.isOnGround)
        {
            _statemachine.ChangeState<PlayerAir>((int)PlayerState.Air);
        }


    }

    public override void Exit()
    {
        Debug.Log("退出空中中转");
    }
}
