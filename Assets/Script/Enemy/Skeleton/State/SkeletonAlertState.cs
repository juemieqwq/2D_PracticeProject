using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAlertState : StateBaseEnemy
{
    //当玩家离开后将依然停留此状态的时间
    private float _stopTime;
    //玩家以离开的时间
    private float _time;

    public override void Enter()
    {
        (_host as SkeletonEnemy).SetIsCheck(true);
        _rigidbody.velocity = Vector3.zero;
        _stopTime = 3;
        _time = _stopTime;
        SetAnimBoolName("Move");
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        //用与攻击后有一定的间隔
        if (_host._isCanAttack)
        {
            _stateMachine.ChangeState<SkeletonAttackState>((int)EnemyState.Attack);
            return;
        }

        //当玩家离开检测持续一段时间的警戒状态
        if (!_host._frontIsHavePlayer)
        {
            _time -= Time.deltaTime;
        }
        else if (_time < 3 && _time > 0)
        {
            _time = _stopTime;
        }


        //警戒结束
        if (_time <= 0)
        {
            _stateMachine.ChangeState<SkeletonIdleState>(0);
            return;
        }

        _rigidbody.velocity = new Vector2(_host.GetSpeed() * _host._direction * 2, 0);

        //前方有没路的时候继续反转
        if (_host._frontIsCrashWall || !_host._frontIsHaveGround)
        {
            _rigidbody.velocity = Vector2.zero;
            _animator.SetBool("Idle", true);
            _animator.SetBool("Move", false);
        }
        else
        {
            _animator.SetBool("Move", true);
            _animator.SetBool("Idle", false);
        }


        if ((_host._player.rigidbody.position.x - _rigidbody.position.x) > 0 && _host._direction == -1)
        {
            _host.Filp();
        }
        else if ((_host._player.rigidbody.position.x - _rigidbody.position.x) < 0 && _host._direction == 1)
        {
            _host.Filp();
        }



    }

}
