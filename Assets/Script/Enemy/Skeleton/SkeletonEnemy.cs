using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : BaseEnemy
{
    //僵住冷却
    private float _rigidityCoolTime = 1f;
    //伤害冷却
    private float _hitCoolTime = .4f;
    void Start()
    {
        Init_Base_Info();
        _fxHit = GetComponentInChildren<FXHit>();
    }


    void Update()
    {
        if (_isDead)
            return;
        CheckCollsion();
        //Debug.Log("_isOnGround：" + _isOnGround);
        //Debug.Log("_frontIsCrashWall：" + _frontIsCrashWall);
        //Debug.Log("_frontIsHaveGround：" + _frontIsHaveGround);
        //Debug.Log("_isCanAttack:" + _isCanAttack);
        //Debug.Log("_frontIsHavePlayer：" + _frontIsHavePlayer);
        if (!_isFreezingTime)
            _CurrentState.Update();
        TimeController();
        ChangeState();
        if (!_isOnGround)
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -2);
        else if (_isOnGround && _rigidbody.velocity.y != 0)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        }
        _rigidityCoolTime += Time.deltaTime;
        _hitCoolTime += Time.deltaTime;
    }

    public override string GetPoolName()
    {
        return "Skeleton";
    }


    //根据当前检测的状态来强行切换状态
    protected void ChangeState()
    {
        if (_frontIsHavePlayer && _isChangeState && !(_CurrentState is SkeletonAlertState))
        {
            _stateMachineEnemy.ChangeState<SkeletonAlertState>((int)StateBaseEnemy.EnemyState.Alert);
        }

        if (_isCanBeBackAttack && _isBeBackAttack)
        {
            _isBeBackAttack = false;
            _stateMachineEnemy.ChangeState<SkeletonCrashAttackState>((int)StateBaseEnemy.EnemyState.CrashAttack);
        }


    }

    public override void Hit(DamageTimeType damageTimeType)
    {
        if (_isDead)
            return;

        switch (damageTimeType)
        {
            case DamageTimeType.IntervalDizziness:
                if (_rigidityCoolTime >= 1.2f && _hitCoolTime > .4f)
                {
                    _stateMachineEnemy.ChangeState<SkeletonHitState>((int)StateBaseEnemy.EnemyState.Hit);
                    StartRepelCoroutine();
                    _rigidityCoolTime = 0;
                    _hitCoolTime = 0f;
                }
                else if (_hitCoolTime > .4f)
                {
                    _hitCoolTime = 0f;
                }
                base.Hit(damageTimeType);
                break;
            case DamageTimeType.IntervalHit:
                if (_hitCoolTime > .4f)
                {
                    _hitCoolTime = 0f;
                }
                base.Hit(damageTimeType);
                break;
            case DamageTimeType.Hit:
                _hitCoolTime = 0f;
                base.Hit(damageTimeType);
                break;
            case DamageTimeType.Dizziness:
                _stateMachineEnemy.ChangeState<SkeletonHitState>((int)StateBaseEnemy.EnemyState.Hit);
                StartRepelCoroutine();
                _rigidityCoolTime = 0;
                _hitCoolTime = 0f;
                base.Hit(damageTimeType);
                break;
            default:
                Debug.Log("怪物：" + gameObject + "的受伤方法出现问题");
                break;
        }

    }



    public override void BeBackAttack()
    {
        base.BeBackAttack();
    }





    public void StartRepelCoroutine(float ContinueTime = .2f)
    {
        StartCoroutine(Repel(ContinueTime));
    }


    //角色攻击时被击退
    public IEnumerator Repel(float ContinueTime = 2f)
    {
        float time = 0;

        Vector2 targetPosition = new Vector2(_rigidbody.position.x + .5f * _player.direction, _rigidbody.position.y + .25f);
        Vector2 originPosition = _rigidbody.position;
        Vector2 distance;
        bool isHaveWall;
        while (time < ContinueTime)
        {
            distance = Vector2.Lerp(originPosition, targetPosition, time / ContinueTime);
            //还得是射线检测艹了都- -
            isHaveWall = Physics2D.Raycast(_rigidbody.position, distance.normalized, distance.magnitude, LayerMask.GetMask("Ground"));
            time += Time.deltaTime;
            if (!isHaveWall)
                _rigidbody.MovePosition(distance);
            yield return null;
        }

    }



    public override void SetIsCheck(bool _is)
    {
        base.SetIsCheck(_is);
    }

    public override void SetIsFreezingTime(bool Is)
    {
        base.SetIsFreezingTime(Is);
    }


    public override void Dead()
    {
        SetShowCanBeBackAttackTime(false);
        _rigidbody.velocity = Vector2.zero;
        _stateMachineEnemy.ChangeState<SkeletonDeadState>((int)StateBaseEnemy.EnemyState.Dead);
        _isDead = true;
    }
}
