using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashBehavior : PlayerBaseState
{
    float duration = 0.2f;
    float time;
    float dashSpeed;
    float inputX;
    bool isHeavyAttack;
    public override void Enter()
    {
        base.Enter();
        base.PlayerInit();
        time = duration;
        inputX = player.inputX;
        dashSpeed = hostInfo.GetInfo(GetInfoType.DashSpeed);
        player.SetIsInput(false);
    }

    public override void Exit()
    {
        base.Exit();
        if (!isHeavyAttack)
            player.SetIsInput(true);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (inputX != 0 && time > 0)
            hostRigidbody2D.velocity = new Vector2(dashSpeed * inputX, 0);
        else if (inputX == 0 && time > 0)
            hostRigidbody2D.velocity = new Vector2(dashSpeed * player.direction, 0);


        if (Input.GetKeyDown("mouse 0"))
        {
            isHeavyAttack = true;
            hostStateMachine.ChangeState<PlayerHeavyAttackBehavior>("Attack3");
        }

        if (isOnGoround && time <= 0)
        {
            isHeavyAttack = false;
            hostStateMachine.ChangeState<PlayerIdleBehavior>("Idle1");
            return;
        }
        else if (!isOnGoround && time <= 0)
        {
            isHeavyAttack = false;
            hostStateMachine.ChangeState<PlayerFallBehavior>("Fall1");
            return;
        }
    }

    public override void Update()
    {
        base.Update();
        time -= Time.deltaTime;
    }
}
