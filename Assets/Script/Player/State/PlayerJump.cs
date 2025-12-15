using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class PlayerJump : PlayerAirState
{
    float JumpTime;
    public override void Enter()
    {
        JumpTime = 0.2f;
        _player._CurrentState = this;
        // Change_Info("Jump");
        SetVelocity(this._rigidbody.velocity.x, ForceJump);
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
        if (_rigidbody.velocity.y < 0)
        {
            _statemachine.ChangeState<PlayerFall>((int)PlayerState.Fall);
        }
        else if (_character.isOnGround && JumpTime < 0)
        {
            _statemachine.ChangeState<PlayerGroundState>(-1);
        }

    }


}
