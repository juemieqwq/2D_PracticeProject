using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleBehavior : RoleBaseState
{
    public override void Enter()
    {
        base.Enter();
        hostRigidbody2D.velocity = Vector3.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }
}
