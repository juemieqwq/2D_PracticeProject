using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SwordObject;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerAimBehaviour : RoleBaseState
{
    private Camera playerCamera;
    private Player player;
    private bool isThrowSword;
    public override void Enter()
    {
        if (player == null)
            player = (host as Player);
        base.Enter();
        player.SetIsInput(false);
        PlayerManager.instance.SetDotsActive(true);
        hostRigidbody2D.velocity = Vector3.zero;
        stopFrame = 2;
    }

    public override void Exit()
    {
        base.Exit();
        if (!isThrowSword)
            player.SetIsInput(true);
        PlayerManager.instance.SetDotsActive(false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        PlayerDiretionFollowMouse();
        //Å×ÎïÏßÏÔÊ¾
        if (PlayerManager.instance.swordState == SwordObject.SwordState.Normal)
            PlayerManager.instance.GetSkill<ThrowSwordSkill>((int)PlayerManager.SkillName.ThrowSwordSkill)?.SetDotsPosition();
        else if (PlayerManager.instance.swordState == SwordObject.SwordState.Penetrate || PlayerManager.instance.swordState == SwordObject.SwordState.Stop || PlayerManager.instance.swordState == SwordState.Catapult)
            PlayerManager.instance.GetSkill<ThrowSwordSkill>((int)PlayerManager.SkillName.ThrowSwordSkill)?.StraightLineAim();
        if (IsStopUpdate(1))
            return;
        if (Input.GetKeyDown("mouse 0"))
        {
            if (PlayerManager.instance.GetSkill<ThrowSwordSkill>((int)PlayerManager.SkillName.ThrowSwordSkill).CanUseSkill())
            {
                isThrowSword = true;
                hostStateMachine.ChangeState<PlayerThrowSwordBehavior>("Fire1");
            }
            else
            {
                isThrowSword = false;
                hostStateMachine.ChangeState<PlayerIdleBehavior>("Idle1");
            }
            return;
        }
        else if (Input.anyKeyDown || Input.GetKeyUp("mouse 1"))
        {
            if (Input.GetKeyDown("t"))
                return;
            isThrowSword = false;
            hostStateMachine.ChangeState<PlayerIdleBehavior>("Idle1");
            return;
        }
    }

    private void PlayerDiretionFollowMouse()
    {
        if (playerCamera == null)
        {
            playerCamera = player.transform.parent.GetComponentInChildren<Camera>();
        }
        Vector2 mousePosition = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x - player.transform.position.x > 0 && player.direction == -1)
        {
            player.Filp();
        }
        else if (mousePosition.x - player.transform.position.x < 0 && player.direction == 1)
        {
            player.Filp();
        }

    }

}
