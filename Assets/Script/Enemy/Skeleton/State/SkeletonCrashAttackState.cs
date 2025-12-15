using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCrashAttackState : StateBaseEnemy
{
    private bool isReTime = true;
    public override void Enter()
    {
        (_host as SkeletonEnemy).SetIsCheck(false);
        _host.SetIsCanBeBackAttack(false);
        _host.SetShowCanBeBackAttackTime(false);
        SetAnimBoolName("CrashAttack");
        base.Enter();
    }

    public override void Exit()
    {
        isReTime = true;
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (isReTime)
        {
            _time = -0.6f;
            isReTime = false;
        }
        if (_time > 0)
        {
            _stateMachine.ChangeState<SkeletonBeBackAttackState>((int)EnemyState.BeBackAttack);
        }
    }
}
