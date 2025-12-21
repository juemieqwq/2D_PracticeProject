using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StringExpands;

public class TestPlayer : MonoBehaviour, IRole
{
    public float inputX;
    private RoleStateMachine stateMachine;
    private RoleBaseState currentState;
    private RoleAnimator roleAnimator;
    private IRoleInfo playerInfo;



    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        if (inputX != 0)
        {
            stateMachine.ChangeState<PlayerRunBehavior>(RoleAnimator.BehaviorNameAndNumToString(BehaviorContainer.RoleBehavior.Run, 1));
        }

        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    void Awake()
    {

    }

    private void Start()
    {
        roleAnimator = transform.GetComponentInChildren<RoleAnimator>();
        playerInfo = GetComponent<IRoleInfo>();
        Debug.Log("roleAnimator:" + roleAnimator);
        currentState = new RoleBaseState();
        stateMachine = new RoleStateMachine(this, roleAnimator, currentState, RoleAnimator.BehaviorNameAndNumToString(BehaviorContainer.RoleBehavior.Idle));

    }
}
