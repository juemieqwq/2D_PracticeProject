using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadBehavior : RoleBaseState
{
    Player player;
    public override void Enter()
    {
        isLoop = false;
        player = (host as Player);
        base.Enter();
        hostRigidbody2D.velocity = new Vector2(0, hostRigidbody2D.velocity.y);
        player.SetIsInput(false);
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
    }
}
