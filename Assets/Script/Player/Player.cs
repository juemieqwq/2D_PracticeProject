using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.Experimental.GraphView.GraphView;
using static PlayerManager;
using UnityEngine.Assertions;

public class Player : BaseCharacter
{

    private StateMachine _statemachine;
    public StateBase _CurrentState;
    //玩家动画事件类
    public PlayerAnimator _playerAnimator { get; private set; }


    #region 新的动画播放器
    private RoleStateMachine stateMachine;
    private RoleBaseState currentState;
    private RoleAnimator roleAnimator;
    #endregion

    #region 射线检测
    [Header("角色射线检测")]
    //检测地板所在的图层
    private LayerMask _WhatIsGround;
    //检测墙体所在的图层
    private LayerMask _WhatIsWall;
    //检测地板的距离
    [SerializeField]
    private float _groundCheckDistance = 0.4f;
    [SerializeField]
    private float _wallCheckDistance = 0.16f;
    #endregion

    #region 输入的相关变量
    //X轴输入信息
    public float inputX { get; private set; }
    //Y轴输入信息
    public float inputY { get; private set; }


    //是否检测输入
    private bool isInput = true;
    #endregion

    //角色状态对角色进行一次反转
    public bool _isFlip = false;
    private Vector2 velocity;
    public enum PlayerState
    {
        EnterGroundState = -2,
        EnterAirState = -1,
        Idle = 0,
        Move = 1,
        Jump = 2,
        Fall = 3,
        Air = 4,
        Dash = 5,
        WallSlide = 6,
        WallJump = 7,
        Attack1_1 = 8,
        Attack1_2 = 9,
        Hit = 10,
        CrashAttack = 11,
        BackAttack = 12,
        Aim = 13,
        ThrowSword = 14,
        BackSword = 15,
        HeavyAttack = 16,
        Dead = 17,
    }

    public float _forceJump
    {
        get { return _playerInfo.GetInfo(GetInfoType.ForceJump) - (10 * (1 - _playerInfo.GetInfo(GetInfoType.ScaleTime))); }
    }
    public float _speed
    {
        get { return _playerInfo.GetInfo(GetInfoType.Speed) * _playerInfo.GetInfo(GetInfoType.ScaleTime); }
    }

    public LayerMask _selfLayerMask;
    //角色是否死亡
    private bool _isDead = false;

    //玩家单例
    [SerializeField]
    private PlayerManager _playerManager;

    //玩家信息类
    public PlayerInfo _playerInfo { get; private set; }
    //玩家相机
    public Camera _playerCamera;


    void Start()
    {   //角色组件的初始化
        anim = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        _playerInfo = GetComponent<PlayerInfo>();
        _playerCamera = transform.parent.GetComponentInChildren<Camera>();
        Assert.IsNotNull(_playerCamera, (this + "玩家相机为空"));
        Assert.IsNotNull(_playerInfo, (this + "玩家信息类为空"));
        //获取角色场景单例
        if (_playerManager == null)
            _playerManager = PlayerManager.instance;
        //状态机的初始化
        //_statemachine = new StateMachine(this, _CurrentState, -1, _playerManager);
        //_statemachine.ChangeState<PlayerAir>((int)PlayerState.Air);
        //_CurrentState.Init(_statemachine, this, _playerManager);
        roleAnimator = GetComponentInChildren<RoleAnimator>();
        //要自己new成初始状态，不然StateMachine内的当前为：RoleBaseState类
        currentState = new PlayerIdleBehavior();
        stateMachine = new RoleStateMachine(this, roleAnimator, currentState, RoleAnimator.BehaviorNameAndNumToString(BehaviorContainer.RoleBehavior.Idle));
        _WhatIsGround = LayerMask.GetMask("Ground");
        _WhatIsWall = LayerMask.GetMask("Wall");
        //gameObject是实例（当前对象）
        //GameObject是类（当前场景）
        _selfLayerMask = gameObject.layer;
        //方向默认为右，1
        direction = 1;
        _playerAnimator = GetComponentInChildren<PlayerAnimator>();

    }


    void Update()
    {
        if (_isDead) return;
        CheckCollsion();
        if (isInput)
            InputCheck();
        //_CurrentState.Update();

        stateMachine.Update();
        ControlFilp();
        //Debug.Log("isOnGround:" + isOnGround);
        //Debug.Log("CoolTime:" + _coolTime);
        this.velocity = rigidbody.velocity;
        //anim.SetFloat("VelocityY", velocity.y);
        //anim.SetFloat("VelocityX", velocity.x);
        // Debug.Log("Velocity:" + this.velocity);


    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void CheckCollsion()
    {
        //ContactFilter2D 是一个过滤器和配置工具
        //可以通过filter（变量）.layerMask = LayerMask.GetMask("图层名称");设置检测图层
        //useLayerMask (bool): 必须设置为 true，layerMask 才会生效。
        //可以通过
        //Physics2D.OverlapCircle(transform.position, 5f, filter, overlapResults)；
        //获取过滤的List<Collider2D>列表
        //在通过foreach(Collider2D col in overlapResults)和col.GetComponent<标签类>()过滤标签；
        isOnGround = Physics2D.Raycast(rigidbody.position, Vector2.down, _groundCheckDistance, _WhatIsGround);
        isTouchWall = Physics2D.Raycast(rigidbody.position, Vector2.right * direction, _wallCheckDistance, _WhatIsGround);

        //anim.SetBool("IsOnGround", isOnGround);
    }

    //绘制射线检测的线
    private void OnDrawGizmosSelected()
    {
        if (rigidbody != null)
        {   //检测地面的线
            Gizmos.color = Color.green;
            Gizmos.DrawLine(rigidbody.transform.position, new Vector3(rigidbody.position.x, rigidbody.position.y - _groundCheckDistance, rigidbody.transform.position.z));
            //检测墙体的线
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(rigidbody.transform.position, new Vector3(rigidbody.position.x + _wallCheckDistance * direction, rigidbody.position.y, rigidbody.transform.position.z));

        }
    }

    public void Filp()
    {
        direction = direction * -1;
        this.transform.Rotate(0, 180, 0);
        Debug.Log("角色方向: " + direction);
    }

    private void ControlFilp()
    {
        if (!isInput)
            return;
        if (_isFlip)
        {
            Debug.Log("状态机进行了一次反转");
            Filp();
            _isFlip = false;
        }

        if (inputX == 1 && direction == -1)
            Filp();
        else if (inputX == -1 && direction == 1)
            Filp();

    }


    private void InputCheck()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        //跳跃
        if (Input.GetKeyDown("space") && isOnGround)
            stateMachine.ChangeState<PlayerJumpBehavior>(RoleAnimator.BehaviorNameAndNumToString(BehaviorContainer.RoleBehavior.Jump));

        ////释放时间冻结
        if (Input.GetKeyDown("r") && _playerManager.GetSkill<FreezingTimeSkill>((int)PlayerManager.SkillName.FreezingTimeSkill).CanUseSkill())
        {
            _playerManager.GetSkill<FreezingTimeSkill>((int)PlayerManager.SkillName.FreezingTimeSkill).UseSkill();
        }
        //进行时间冻结后的衔接攻击
        else if (isOnGround && Input.GetKeyDown("r") && FreezingTimeSkill.isCanCloneAttack)
        {
            _playerManager.GetSkill<FreezingTimeSkill>((int)PlayerManager.SkillName.FreezingTimeSkill).SetisCloneAttack(true);
            stateMachine.ChangeState<PlayerHeavyAttackBehavior>(("Attack3"));
            return;
        }
        ////转移技能
        if (Input.GetKeyDown("f") && _playerManager.GetSkill<TransferSkill>((int)PlayerManager.SkillName.TransferSkill).CanUseSkill())
        {
            _playerManager.GetSkill<TransferSkill>((int)PlayerManager.SkillName.TransferSkill).UseSkill();
        }
        else if (Input.GetKeyDown("f") && _playerManager.GetSkill<TransferSkill>((int)PlayerManager.SkillName.TransferSkill).objectIsActive)
        {
            _playerManager.GetSkill<TransferSkill>((int)PlayerManager.SkillName.TransferSkill).SetObjectIsTransferPlayer(true);
        }



        ////冲刺
        if (Input.GetKeyDown("left shift") && _playerManager.GetSkill<DashSkill>((int)SkillName.DashSkill).CanUseSkill())
        {

            _playerManager.UseSkill<DashSkill>((int)SkillName.DashSkill);
            stateMachine.ChangeState<PlayerDashBehavior>("Dash1");
            return;
        }

        //普通攻击
        if (isOnGround && Input.GetKeyDown("mouse 0"))
        {

            stateMachine.ChangeState<PlayerAttackBehavior>("Attack1");
            return;
        }


        //进入瞄准状态
        if (isOnGround && Input.GetKeyDown("mouse 1") && _playerManager.GetSkill<ThrowSwordSkill>((int)PlayerManager.SkillName.ThrowSwordSkill).CanUseSkill())
        {
            stateMachine.ChangeState<PlayerAimBehaviour>("Aim1");
            return;
        }
        else if (isOnGround && Input.GetKeyDown("mouse 1") && !_playerManager.GetSkill<ThrowSwordSkill>((int)PlayerManager.SkillName.ThrowSwordSkill).CanUseSkill())
        {
            stateMachine.ChangeState<PlayerBackSwordBehavior>("Aim2");
            return;
        }

        ////进入尝试反击状态
        if (isOnGround && Input.GetKeyDown("left ctrl"))
        {
            stateMachine.ChangeState<PlayerCrashAttackBehavior>("CrashAttack1");
            return;
        }



    }

    public void SetIsInput(bool Is)
    {
        isInput = Is;
    }

    //IEnumerator函数可以通过yield return new 类型（）
    //让其运行中止一段时间后继续运行
    public IEnumerator DisableInputSecond(float Second = 0)
    {
        isInput = false;
        yield return new WaitForSeconds(Second);
        isInput = true;

    }

    public void SetInputX(float x)
    {
        inputX = x;
    }

    public void Hit()
    {
        Debug.Log("玩家被击中");
        stateMachine.ChangeState<PlayerHitBehavior>("Hit1");
        //_statemachine.ChangeState<PlayerHitState>((int)PlayerState.Hit);
        StartCoroutine(DisableInputSecond(.2f));
    }

    public void Dead()
    {
        isInput = false;
        _isDead = true;
        stateMachine.ChangeState<PlayerDeadBehavior>("Dead1");
        //_statemachine.ChangeState<PlayerDead>((int)PlayerState.Dead);
    }


}
