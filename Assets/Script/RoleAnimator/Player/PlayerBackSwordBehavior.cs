using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBackSwordBehavior : PlayerBaseState
{

    SwordObject sword;
    RaycastHit2D hit;
    public override void Enter()
    {
        base.PlayerInit();
        base.Enter();
        player.SetIsInput(false);
        PlayerManager.instance.SetDotsActive(true);
        hostRigidbody2D.velocity = Vector3.zero;
    }

    public override void Exit()
    {
        base.Exit();
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
        PlayerManager.instance.GetSkill<ThrowSwordSkill>((int)PlayerManager.SkillName.ThrowSwordSkill)?.StraightLineAim();
        if (IsStopUpdate(2))
            return;


        if (controller.mouse0.isPressed)
        {

            hit = Physics2D.Raycast(player.transform.position, PlayerManager.instance.GetSkill<ThrowSwordSkill>((int)PlayerManager.SkillName.ThrowSwordSkill).GetThrowSwordDirection(), 15f, LayerMask.GetMask("Skill"));
            if (hit.collider != null)
            {
                sword = hit.rigidbody.GetComponent<SwordObject>();
                Debug.Log("尝试获取飞剑类");
            }

            if (sword == null)
                Debug.Log("未检测到");
            sword?.BackPlayer(player);
            hostStateMachine.ChangeState<PlayerIdleBehavior>("Idle1");
            return;
        }
        else if (Keyboard.current.anyKey.wasPressedThisFrame || controller.mouse1.isReleased)
        {
            hostStateMachine.ChangeState<PlayerIdleBehavior>("Idle1");
            return;
        }
    }

    /// <summary>
    /// 根据鼠标转变角色朝向
    /// </summary>
    private void PlayerDiretionFollowMouse()
    {

        Vector2 mousePosition = (Vector2)player.playerCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
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
