using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBackSwordState : StateBase

{
    SwordObject sword;
    RaycastHit2D hit;
    public override void Enter()
    {
        sword = null;
        _playerManager.SetDotsActive(true);
        _rigidbody.velocity = Vector3.zero;
        _player.SetIsInput(false);
        //_player._CurrentState = this;
        Change_Info("BackSword");
        base.Enter();
    }

    public override void Exit()
    {
        _playerManager.SetDotsActive(false);
        _player.SetIsInput(true);
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        PlayerDiretionFollowMouse();
        _playerManager.GetSkill<ThrowSwordSkill>((int)PlayerManager.SkillName.ThrowSwordSkill).StraightLineAim();
        if (Input.GetKeyUp("mouse 1"))
        {
            _statemachine.ChangeState<PlayerIdle>(0);
        }
        else if (Input.GetKeyDown("mouse 0"))
        {
            hit = Physics2D.Raycast(_player.transform.position, _playerManager.GetSkill<ThrowSwordSkill>((int)PlayerManager.SkillName.ThrowSwordSkill).GetThrowSwordDirection(), 15f, LayerMask.GetMask("Skill"));
            if (hit.collider != null)
                sword = hit.rigidbody.GetComponent<SwordObject>();
            if (sword == null)
                Debug.Log("未检测到");
            sword?.BackPlayer(_player);
            _statemachine.ChangeState<PlayerIdle>(0);
        }
    }

    /// <summary>
    /// 根据鼠标转变角色朝向
    /// </summary>
    private void PlayerDiretionFollowMouse()
    {

        Vector2 mousePosition = (Vector2)_player._playerCamera.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x - _player.transform.position.x > 0 && _player.direction == -1)
        {
            _player.Filp();
        }
        else if (mousePosition.x - _player.transform.position.x < 0 && _player.direction == 1)
        {
            _player.Filp();
        }

    }


}
