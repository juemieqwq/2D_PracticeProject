using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : StateBase
{
    public override void Enter()
    {
        Change_Info("Dead");
        _player._CurrentState = this;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
