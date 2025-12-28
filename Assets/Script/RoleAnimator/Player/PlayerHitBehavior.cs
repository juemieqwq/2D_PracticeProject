using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBehavior : RoleBaseState
{
    float time;
    Player player;
    public override void Enter()
    {
        isLoop = false;
        if (player == null)
            player = (host as Player);
        base.Enter();
        hostRigidbody2D.velocity = new Vector2(0, hostRigidbody2D.velocity.y);
        time = 0;
    }

    public override void Exit()
    {
        base.Exit();
        player.SetIsInput(true);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        time += Time.deltaTime;
        if (time > 0.2f && Input.anyKeyDown)
        {
            hostStateMachine.ChangeState<PlayerIdleBehavior>("Idle1");
        }

        if (player.isOnGround && isFinish)
        {
            hostStateMachine.ChangeState<PlayerIdleBehavior>("Idle1");
        }
        else if (!player.isOnGround && isFinish)
        {
            hostStateMachine.ChangeState<PlayerFallBehavior>("Fall1");
        }
    }
}
