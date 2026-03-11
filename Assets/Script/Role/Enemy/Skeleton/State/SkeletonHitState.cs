using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHitState : StateBaseEnemy
{

    public override void Enter()
    {
        (_host as SkeletonEnemy).SetIsCheck(false);
        _host.SetIsCanBeBackAttack(false);
        _host.SetShowCanBeBackAttackTime(false);
        (_host as SkeletonEnemy).StartRepelCoroutine(.2f);
        SetAnimBoolName("Hit");
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        {
            base.Update();
            if (_isFinish)
            {
                _stateMachine.ChangeState<SkeletonAlertState>((int)StateBaseEnemy.EnemyState.Alert);
            }


        }



    }
}
