using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class PlayerFall : PlayerAirState
{
    public override void Enter()
    {
        //_player._CurrentState = this;
        //  Change_Info("Fall");
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (_character.isOnGround)
        {
            _statemachine.ChangeState<PlayerGroundState>(-1);
        }
        else if (_rigidbody.velocity.y > 0)
        {
            _statemachine.ChangeState<PlayerJump>((int)PlayerState.Jump);
        }
    }
}
