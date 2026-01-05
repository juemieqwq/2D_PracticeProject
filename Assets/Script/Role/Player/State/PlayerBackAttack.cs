using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBackAttack : StateBase
{
    public override void Enter()
    {

        _player.SetIsInput(false);

        _playerManager.ApplyCameraViewZoomAndOffset(.3f, 2, new Vector3(3, 0, 0));
        Change_Info("BackAttack");
        //_player._CurrentState = this;
        base.Enter();
    }

    public override void Exit()
    {
        _anim.SetBool("CrashAttack", false);
        _player.SetIsInput(true);
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (IsFinish)
        {
            _statemachine.ChangeState<PlayerIdle>(0);
        }
    }
}
