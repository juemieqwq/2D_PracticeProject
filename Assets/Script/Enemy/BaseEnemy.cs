using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Windows;

//public interface Enemy
//{
//    public void Hit();

//}

public class BaseEnemy : MonoBehaviour, IRole
{


    #region 角色基础信息
    public Animator _animator { protected set; get; }
    public Rigidbody2D _rigidbody { protected set; get; }

    public EnemyInfo _enemyInfo { protected set; get; }

    [SerializeField]
    protected float _speed { get { return _enemyInfo.GetInfo(GetInfoType.Speed); } }
    public int _direction { protected set; get; }
    //外部控制反转
    public bool _isFlip = false;
    //状态机
    protected StateMachineEnemy _stateMachineEnemy;
    //角色当前状态
    public StateBaseEnemy _CurrentState;
    //状态转换冷却
    public float _time { protected set; get; }
    //是否可以进行任意状态转换
    protected bool _isChangeState = true;
    //存储玩家
    public Player _player { protected set; get; }
    //玩家是否被击中
    public bool _isHit { protected set; get; } = false;
    //角色是否被反击
    public bool _isBeBackAttack { protected set; get; } = false;

    #endregion

    //用于调用角色的受伤材质
    protected FXHit _fxHit = null;
    #region 反击相关信息
    //是否可以反击
    public bool _isCanBeBackAttack { get; private set; } = false;
    //可以反击时显示
    [SerializeField]
    private GameObject _backTimeShow = null;
    #endregion


    #region 射线检测

    //是否开启检测
    protected bool _isCheck = true;

    //地板所在的图层
    protected LayerMask _WhatIsGround;
    //墙体所在的图层
    protected LayerMask _WhatIsWall;
    //玩家所在的图层
    protected LayerMask _WhatIsPlayer;

    [Header("角色射线检测的距离")]
    //检测地板的距离
    [SerializeField]
    protected float _groundCheckDistance = 0.4f;
    //检测墙体的距离
    [SerializeField]
    protected float _wallCheckDistance = 0.16f;
    //检测的初始位置
    [SerializeField]
    protected Transform _checkPosition;
    //检测玩家的距离
    [SerializeField]
    protected float _playerCheckDistance;
    //检测攻击的距离
    [SerializeField]
    protected float _attackCheckDistance;

    //检测结果
    public bool _isOnGround { protected set; get; }

    public bool _frontIsCrashWall { protected set; get; }

    public bool _frontIsHaveGround { protected set; get; }

    public bool _frontIsHavePlayer { protected set; get; }

    public bool _isCanAttack { protected set; get; }
    #endregion

    //是否被冻结
    protected bool _isFreezingTime = false;
    //角色是否死亡
    public bool _isDead { get; protected set; } = false;

    public virtual string GetPoolName()
    {
        return "";
    }
    protected void Init_Base_Info()
    {
        _WhatIsGround = LayerMask.GetMask("Ground");
        _WhatIsWall = LayerMask.GetMask("Ground");
        _WhatIsPlayer = LayerMask.GetMask("Player");
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _enemyInfo = GetComponent<EnemyInfo>();
        Assert.IsNotNull(_enemyInfo, (this.name + "怪物信息类为空"));
        _stateMachineEnemy = new StateMachineEnemy();
        _CurrentState = new StateBaseEnemy();
        _CurrentState.Init(this, _stateMachineEnemy);
        _stateMachineEnemy.Init(this);
        _direction = -1;
        _time = 0;
        _player = FindObjectOfType<Player>();

    }

    protected void OnDrawGizmos()
    {
        if (_rigidbody != null)
        {   //检测地面
            Gizmos.color = Color.green;
            //检测自身地面
            Gizmos.DrawLine(_rigidbody.transform.position - new Vector3(0, 0.9f), new Vector3(_rigidbody.position.x, _rigidbody.position.y - _groundCheckDistance - 0.9f));
            //检测前方地面
            Gizmos.DrawLine(_checkPosition.position, new Vector2(_checkPosition.position.x, _checkPosition.position.y - _groundCheckDistance));
            //检测墙体
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_checkPosition.position, new Vector2(_checkPosition.position.x, _checkPosition.position.y + _wallCheckDistance));
            //检测面前是否有玩家
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_rigidbody.position, new Vector2(_rigidbody.position.x + _playerCheckDistance * _direction, _rigidbody.position.y));
            //检测攻击范围内是否有玩家
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_rigidbody.position + new Vector2(0, 0.7f), new Vector2(_rigidbody.position.x + _attackCheckDistance * _direction, _rigidbody.position.y + 0.7f));

        }
    }

    protected void CheckCollsion()
    {
        if (_isCheck)
        {
            //自身脚下是否有地面
            _isOnGround = Physics2D.Raycast(_rigidbody.position - new Vector2(0, 0.9f), Vector2.down, _groundCheckDistance, _WhatIsGround);
            //前方是否有墙体
            _frontIsCrashWall = Physics2D.Raycast(_checkPosition.position, Vector2.up, _wallCheckDistance, _WhatIsWall);
            //前方是否有地面
            _frontIsHaveGround = Physics2D.Raycast(_checkPosition.position, Vector2.down, _groundCheckDistance, _WhatIsGround);
            //检测前方的玩家
            _frontIsHavePlayer = Physics2D.Raycast(_rigidbody.position, Vector2.right * _direction, _playerCheckDistance, _WhatIsPlayer);
            //检测攻击距离内的玩家
            _isCanAttack = Physics2D.Raycast(_rigidbody.position + new Vector2(0, 0.7f), Vector2.right * _direction, _attackCheckDistance, _WhatIsPlayer);
        }


    }

    public void Filp()

    {
        _direction = _direction * -1;
        this.transform.Rotate(0, 180, 0);

    }

    /// <summary>
    /// 怪物受伤时调用,且伤害和僵直不能同时设置
    /// </summary>
    /// <param name="isHit">如果为true将无视无敌时间直接扣血</param>
    /// <param name="isRepel">如果为true将无视僵直冷却直接进入僵直</param>
    /// <param name="isOpenRepel">伤害是否能造成僵直</param>

    public virtual void Hit(DamageTimeType damageType)
    {
        _fxHit.StartCoroutine(_fxHit.ChangeFxHit());
    }


    private void ControlFilp()
    {
        if (_isFlip)
        {
            Debug.Log("状态机进行了一次反转");
            Filp();
            _isFlip = false;
        }


    }

    public float GetSpeed()
    {
        return _speed;
    }

    //设置转换冷却时间
    public void SetCoolTime(float time)
    {
        _time = -time;
    }

    protected void TimeController()
    {
        if (_time < 0)
            _time += Time.deltaTime;

        if (_time < 0 && _isChangeState)
            _isChangeState = false;
        else if (_time >= 0)
            _isChangeState = true;
    }

    public void SetIsChangeState(bool Is)
    {
        _isChangeState = Is;
    }




    public virtual void BeBackAttack()
    {
        _isBeBackAttack = true;
    }


    //设置角色是否可以被反击
    public void SetIsCanBeBackAttack(bool iscan)
    {
        _isCanBeBackAttack = iscan;
    }
    //设置显示反击的时机
    public void SetShowCanBeBackAttackTime(bool iscan)
    {
        if (_backTimeShow != null)
        {
            _backTimeShow.SetActive(iscan);
        }
        else
            Debug.Log("反击时间提示图片为空");
    }

    //设置是否开启检测
    public virtual void SetIsCheck(bool _is)
    {
        _isCheck = _is;
    }

    //设置角色是否停止
    public virtual void SetIsFreezingTime(bool Is)
    {
        if (Is)
        {
            _isFreezingTime = true;
            SetIsCheck(false);
            _rigidbody.velocity = Vector2.zero;
            _animator.speed = 0;
        }
        else
        {
            _isFreezingTime = false;
            SetIsCheck(true);
            _animator.speed = 1;
        }
    }



    ///// <summary>
    ///// 调用时使怪物直接进入眩晕状态
    ///// </summary>
    //public virtual void BeDizzy()
    //{

    //}

    public virtual void Dead()
    {

    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
}
