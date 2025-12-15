using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineEnemy
{
    public BaseEnemy _host;
    public StateBaseEnemy _currentState;

    private Dictionary<int, StateBaseEnemy> DicState = new Dictionary<int, StateBaseEnemy>();

    public void Init(BaseEnemy host)
    {
        _host = host;
        ChangeState<SkeletonIdleState>(0);
    }

    public StateBaseEnemy GetState<T>(int type) where T : StateBaseEnemy, new()
    {
        StateBaseEnemy state;
        if (DicState.TryGetValue(type, out state))
        {
            return state;
        }
        state = new T();
        state.Init(_host, this);
        DicState.Add(type, state);
        return state;
    }

    public void ChangeState<T>(int newType) where T : StateBaseEnemy, new()
    {
        StateBaseEnemy newState = GetState<T>(newType);
        if (_currentState != null)
        {
            _currentState.Exit();
        }
        if (newState.IsEnter())
        {
            _currentState = newState;
            _currentState.Enter();
        }
    }

    public void ReturnBeforeState()
    {

    }




}
