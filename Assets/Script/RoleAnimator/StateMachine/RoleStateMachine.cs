using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoleInfo
{
    public float GetInfo(GetInfoType getInfoType);
    public void Hit(float damage, DamageTimeType damageType);

    public DamageTimeType GetDamageTimeType();

    public DamageType GetDamageType();

    public DamageElementType GetDamageElementType();

    public CharacterType GetCharacterType();
    public ArmorType GetArmorType();
}

public class RoleStateMachine
{
    //当前状态对应Key
    private string currentStateKey;
    //当前状态
    public RoleBaseState currentRoleState;

    //初始状态（用于转换失败时强行转换）
    private RoleBaseState initRoleState;
    private string initRoleStatekey;

    //角色动画播放器
    public RoleAnimator roleAnimator { get; private set; }

    //宿主
    private IRoleInfo host;
    //所有的状态字典集合（Dictionary<TKey, TValue>，跟哈希表类型）
    //作用类似对象池，缓存所有已经用过的对象，减少每次切换时进行新的new操作
    private Dictionary<string, RoleBaseState> _stateDic = new Dictionary<string, RoleBaseState>();



    //其他类创建状态机时进行初始化
    public RoleStateMachine(IRoleInfo host, RoleAnimator roleAnimator, RoleBaseState state, string key)
    {
        this.host = host;
        this.currentRoleState = state;
        this.initRoleState = state;
        this.currentStateKey = key;
        this.initRoleStatekey = key;
        this.roleAnimator = roleAnimator;

    }

    public bool ChangeState<T>(string key, bool reCurrentState = false) where T : RoleBaseState, new()
    {
        //状态一致且不需要刷新状态
        if (currentStateKey == key && reCurrentState) return false;
        //退出当前状态
        if (currentRoleState != null)
        {
            currentRoleState.Exit();
        }
        //进入新状态
        if (GetState<T>(key).GetIsEnter())
        {
            ////切换状态时将动画完成变为false
            //if (currentRoleState != null && currentRoleState.IsFinish)
            //    currentRoleState.FinishAnimator(0);
            currentStateKey = key;
            currentRoleState = GetState<T>(key);
            currentRoleState.Enter();
        }
        else
        {
            //////切换状态时将动画完成变为false
            ////if (currentRoleState != null && currentRoleState.IsFinish)
            ////    currentRoleState.FinishAnimator(0);

            //Debug.Log("状态" + _CurrentState + "失败，进行强行转换");
            //if (_player.isOnGround)
            //{
            //    _CurrentState = GetState<PlayerIdle>(0);
            //    _CurrentStateId = 0;
            //}
            //else if (!_player.isOnGround)
            //{
            //    _CurrentState = GetState<PlayerAir>(4);
            //    _CurrentStateId = 4;
            //}

            ////(_CurrentState as PlayerIdle).ForceEnter();
            //GetState<T>(newStateType).InitIsEnter();
            return false;
        }
        return true;
    }

    private RoleBaseState GetState<T>(string key) where T : RoleBaseState, new()
    {
        //先找缓存字典中是否存在
        if (_stateDic.TryGetValue(key, out RoleBaseState stateObj))
        {
            return stateObj;
        }
        RoleBaseState state = new T();
        state.Init(this, host, key);
        _stateDic.Add(key, state);
        return state;
    }

    /// <summary>
    /// 停止工作
    /// </summary>
    public void Stop()
    {
        if (currentRoleState != null)
        {
            currentRoleState.Exit();
            currentRoleState = null;
        }
        this.currentStateKey = "";
    }

    public void Update()
    {
        if (this.currentRoleState != null)
            currentRoleState.Update();
    }
}
