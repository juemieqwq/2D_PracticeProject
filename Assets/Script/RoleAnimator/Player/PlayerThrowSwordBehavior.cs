using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowSwordBehavior : RoleBaseState
{
    public override void Enter()
    {
        isLoop = false;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        (host as Player).SetIsInput(true);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        if (isFinish)
        {
            //PlayerManager.instance.UseSkill<ThrowSwordSkill>((int)PlayerManager.SkillName.ThrowSwordSkill);
            hostStateMachine.ChangeState<PlayerThrowSwordBehavior>("Idle1");
        }
    }
}
