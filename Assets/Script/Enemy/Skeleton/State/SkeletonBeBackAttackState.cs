using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBeBackAttackState : StateBaseEnemy
{
    private bool _reSetTime = true;
    public override void Enter()
    {
        //(_host as SkeletonEnemy).SetIsCheck(false);
        //_host.SetIsCanBeBackAttack(false);
        //_host.SetShowCanBeBackAttackTime(false);
        (_host as SkeletonEnemy).StartRepelCoroutine(.2f);

        SetAnimBoolName("BeBackAttack");
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        _reSetTime = true;
        (_host as SkeletonEnemy).SetIsCheck(true);
    }

    public override void Update()
    {
        base.Update();
        if (_reSetTime)
        {
            _time = -2;
            _reSetTime = false;
        }

        if (_time >= 0)
        {
            _stateMachine.ChangeState<SkeletonAlertState>((int)EnemyState.Alert);
            return;
        }
    }
}
