using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : StateBaseEnemy
{
    public override void Enter()
    {
        (_host as SkeletonEnemy).SetIsCheck(false);
        base.SetAnimBoolName("Dead");
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
