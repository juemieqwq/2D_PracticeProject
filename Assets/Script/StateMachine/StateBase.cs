using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Player;

public class StateBase
{
    protected StateMachine _statemachine;
    //角色单例
    protected PlayerManager _playerManager;


    #region Character_Info
    protected BaseCharacter _character;
    protected string _animBoolName;
    protected Animator _anim;
    protected Rigidbody2D _rigidbody;
    protected float Speed { get { return _player._speed; } }
    protected float ForceJump { get { return _player._forceJump; } }
    #endregion

    //因为没分Player的基本状态所以写这了
    protected Player _player = null;

    //是否成功进入状态
    protected bool IsEnter = true;

    //动画播放完成
    public bool IsFinish { get; private set; } = false;




    public virtual void Init(StateMachine statemachine, BaseCharacter Character, PlayerManager playerManager)
    {
        _statemachine = statemachine;
        _character = Character;
        _anim = Character.anim;
        _rigidbody = Character.rigidbody;
        IsEnter = true;
        if (Character is Player)
            _player = Character as Player;
        _playerManager = playerManager;

    }
    protected virtual void Change_Info(string animBoolName)
    {
        this._animBoolName = animBoolName;
    }



    public virtual void Enter()
    {
        IsFinish = false;
        Debug.Log("角色" + _character + "进入" + _animBoolName + "状态");
        _anim.SetBool(_animBoolName, true);
    }

    public virtual void Update()
    {

    }
    public virtual void Exit()
    {
        Debug.Log("角色" + _character + "退出" + _animBoolName + "状态");
        _anim.SetBool(_animBoolName, false);
    }


    protected void SetVelocity(float x, float y)
    {
        _rigidbody.velocity = new Vector2(x, y);
    }



    public bool GetIsEnter()
    {
        return IsEnter;
    }

    public void InitIsEnter()
    {
        IsEnter = true;
    }

    public void FinishAnimator(int estimate)
    {
        if (estimate == 0)
            IsFinish = false;
        else if (estimate == 1)
            IsFinish = true;
        else
            Debug.Log("动画完成传入值出错");

    }
}
