using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class PlayerWallSlideState : StateBase
{
    private float _downSpeed;
    public override void Enter()
    {
        _downSpeed = -2;
        _player._CurrentState = this;
        Change_Info("WallSlide");
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (_player.isOnGround)
        {
            _statemachine.ChangeState<PlayerGroundState>(-2);
            return;
        }
        else if (!_player.isTouchWall)
        {
            _statemachine.ChangeState<PlayerAirState>(-1);
            return;
        }
        else if (_player.isTouchWall && Input.GetKeyDown("space"))
        {
            _statemachine.ChangeState<PlayerWallJump>((int)PlayerState.WallJump);
            return;
        }


        if (_player.inputX == _player.direction)
        {
            Debug.Log("减速滑墙");
            SetVelocity(_rigidbody.velocity.x, _downSpeed * 0.7f);
        }
        else if (_player.inputY <= 0)
        {
            Debug.Log("正常下落");
            SetVelocity(_rigidbody.velocity.x, _downSpeed);
        }

    }
}
