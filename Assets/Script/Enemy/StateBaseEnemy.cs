using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBaseEnemy
{
    //宿主
    protected BaseEnemy _host;
    //状态机
    protected StateMachineEnemy _stateMachine;
    //宿主的动画
    protected Animator _animator;
    //属主的刚体
    protected Rigidbody2D _rigidbody;
    //动画播放是否结束
    public bool _isFinish { get; private set; } = false;
    //更改的动画bool的名称
    protected string _animBoolName;
    //前一个动画
    protected StateBaseEnemy _beforeAnim = null;
    //计算当前状态的时间
    protected float _time = 0;

    public enum EnemyState
    {
        Idle = 0,
        Move = 1,
        Alert = 2,
        Attack = 3,
        Hit = 4,
        CrashAttack = 5,
        BeBackAttack = 6,
        Dead = 7,

    }



    public void Init(BaseEnemy enemy, StateMachineEnemy stateMachine)
    {
        _host = enemy;
        _stateMachine = stateMachine;
        _animator = _host._animator;
        _rigidbody = _host._rigidbody;
    }


    public virtual void Enter()
    {
        _time = 0;
        SetIsFinish(false);
        _animator.SetBool(_animBoolName, true);
        Debug.Log("动画从：" + _beforeAnim + "转变到：" + this);
        _host._CurrentState = this;
    }

    public virtual void Update()
    {
        _time += Time.deltaTime;
    }

    public virtual void Exit()
    {
        _beforeAnim = this;
        _animator.SetBool(_animBoolName, false);
        Debug.Log("动画从：" + this + "退出");
    }

    public virtual bool IsEnter()
    {
        return true;
    }

    public void SetIsFinish(bool Is)
    {
        _isFinish = Is;
    }

    public void SetAnimBoolName(string name)
    {
        _animBoolName = name;
    }
}
