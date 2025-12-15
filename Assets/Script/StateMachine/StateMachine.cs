using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public interface StateMachineHost { };
public class StateMachine
{
    //当前状态对应的枚举Id
    private int _CurrentStateId;
    //当前状态
    public StateBase _CurrentState { get; private set; }
    //宿主
    private BaseCharacter _Host;
    //所有的状态字典集合（Dictionary<TKey, TValue>，跟哈希表类型）
    //作用类似对象池，缓存所有已经用过的对象，减少每次切换时进行新的new操作
    private Dictionary<int, StateBase> _stateDic = new Dictionary<int, StateBase>();

    //存储场景中的角色
    private Player _player;
    //存储场景中的角色单例
    private PlayerManager _playerManager;


    //其他类创建状态机时进行初始化
    public StateMachine(BaseCharacter host, StateBase state, int currentStateId, PlayerManager playerManager)
    {
        _Host = host;
        _CurrentState = state;
        _CurrentStateId = currentStateId;
        if (host is Player)
            _player = (Player)host;
        _playerManager = playerManager;
    }

    public bool ChangeState<T>(int newStateType, bool reCurrentState = false) where T : StateBase, new()
    {
        //状态一致且不需要刷新状态
        if (_CurrentStateId == newStateType && reCurrentState) return false;
        //退出当前状态
        if (_CurrentState != null)
        {
            _CurrentState.Exit();
        }
        //进入新状态
        if (GetState<T>(newStateType).GetIsEnter())
        {
            //切换状态时将动画完成变为false
            if (_CurrentState != null && _CurrentState.IsFinish)
                _CurrentState.FinishAnimator(0);
            _CurrentStateId = newStateType;
            _CurrentState = GetState<T>(newStateType);
            _CurrentState.Enter();
        }
        else
        {
            //切换状态时将动画完成变为false
            if (_CurrentState != null && _CurrentState.IsFinish)
                _CurrentState.FinishAnimator(0);

            Debug.Log("状态" + _CurrentState + "失败，进行强行转换");
            if (_player.isOnGround)
            {
                _CurrentState = GetState<PlayerIdle>(0);
                _CurrentStateId = 0;
            }
            else if (!_player.isOnGround)
            {
                _CurrentState = GetState<PlayerAir>(4);
                _CurrentStateId = 4;
            }

            //(_CurrentState as PlayerIdle).ForceEnter();
            GetState<T>(newStateType).InitIsEnter();
            return false;
        }
        return true;
    }

    private StateBase GetState<T>(int stateType) where T : StateBase, new()
    {
        //先找缓存字典中是否存在
        if (_stateDic.TryGetValue(stateType, out StateBase stateObj))
        {
            return stateObj;
        }
        StateBase state = new T();
        state.Init(this, _Host, _playerManager);
        _stateDic.Add(stateType, state);
        return state;
    }

    /// <summary>
    /// 停止工作
    /// </summary>
    public void Stop()
    {
        if (_CurrentState != null)
        {
            _CurrentState.Exit();
            _CurrentState = null;
        }
        this._CurrentStateId = -1;
    }

    public void Update()
    {
        if (this._CurrentState != null)
            _CurrentState.Update();
    }

}
