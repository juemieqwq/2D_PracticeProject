using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class PlayerWallJump : StateBase
{
    //private float JumpTime;
    public override void Enter()
    {
        //JumpTime = 0.2f;
        Change_Info("WallJump");
        //_player._CurrentState = this;
        // Debug.Log("½ÇÉ«³ËµÄ×´Ì¬" + _player.direction);
        SetVelocity(Speed * _player.direction * -0.5f, ForceJump);
        // Debug.Log(_rigidbody.velocity);
        _player._isFlip = true;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        // if (JumpTime <= 0)
        //{
        if (!_player.isOnGround)
        {
            _statemachine.ChangeState<PlayerAirState>(-1);
            return;
        }
        // }
        else if (_player.isTouchWall)
        {
            _statemachine.ChangeState<PlayerWallSlideState>((int)PlayerState.WallSlide);
            return;
        }
        else if (_player.isOnGround)
        {
            _statemachine.ChangeState<PlayerGroundState>(-2);
            return;
        }




        //if (_player._inputX == 0)
        //{
        //    SetVelocity(_rigidbody.velocity.x, _rigidbody.velocity.y);
        //}
        //else if (_player._inputX != 0)
        //{
        //    SetVelocity(Speed * _player._inputX * 0.5f, _rigidbody.velocity.y);
        //}


        //JumpTime -= Time.deltaTime;
    }
}
