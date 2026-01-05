using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;
public class PlayerDash : StateBase
{
    private float cTime;
    //冲刺的方向
    private float DashDir;
    //冲刺倍率
    private float MuSpeed = 2;
    //原来的y轴速度
    private float velocityY;

    public override void Enter()
    {
        velocityY = -2;
        DashDir = _player.inputX;
        cTime = 0.2f;
        //_player._CurrentState = this;
        Change_Info("Dash");
        _player.SetIsInput(false);
        base.Enter();
    }

    public override void Exit()
    {
        _player.SetIsInput(true);
        SetVelocity(_rigidbody.velocity.x, velocityY);
        base.Exit();
    }

    public override void Update()
    {
        if (cTime > 0 && DashDir != 0)
            SetVelocity(Speed * MuSpeed * DashDir, 0);

        else if (cTime > 0 && DashDir == 0)
        {
            SetVelocity(Speed * MuSpeed * _player.direction, 0);
        }
        else if (cTime <= 0 && _player.isOnGround)
        {
            _statemachine.ChangeState<PlayerGroundState>(-2);
        }
        else if (cTime <= 0 && !_player.isOnGround)
        {
            _statemachine.ChangeState<PlayerAirState>(-1);
        }
        base.Update();
        cTime -= Time.deltaTime;
    }
}
