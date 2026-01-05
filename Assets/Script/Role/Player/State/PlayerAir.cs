using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class PlayerAir : StateBase
{
    private float JumpTime;
    public override void Enter()
    {

        if (_player.isOnGround)
        {
            JumpTime = 0.1f;
            //_player._CurrentState = this;
            Change_Info("Air");
            SetVelocity(_rigidbody.velocity.x, ForceJump);
        }
        else
        {
            if (_rigidbody.velocity.x * _player.direction > _player._speed * _player.direction * 0.5f)
                _rigidbody.velocity = new Vector2(_player._speed * _player.direction * .5f, _rigidbody.velocity.y);
            JumpTime = 0.1f;
            //_player._CurrentState = this;
            Change_Info("Air");
        }


        base.Enter();
    }

    public override void Exit()
    {

        base.Exit();
    }

    public override void Update()
    {
        JumpTime -= Time.deltaTime;
        base.Update();
        if (_player.inputX != 0)
            SetVelocity(Speed * _player.inputX * 0.5f, _rigidbody.velocity.y);
        else
            SetVelocity(_rigidbody.velocity.x, _rigidbody.velocity.y);
        if (_player.isOnGround && JumpTime < 0)
        {
            _statemachine.ChangeState<PlayerGroundState>(-2);
        }
        else if (_player.isTouchWall && !_player.isOnGround && _rigidbody.velocity.y <= 0)
        {
            _statemachine.ChangeState<PlayerWallSlideState>((int)PlayerState.WallSlide);
        }


    }
}
