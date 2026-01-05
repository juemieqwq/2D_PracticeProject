using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SwordObject;

public class PlayerAimState : StateBase
{

    //是否转到投出剑状态
    private bool isThrowSword;
    //持续时间
    private float continueTime = 3;
    //使用时间
    private float time;
    //现在的时间比例
    private float nowTimeScale;
    //角色相机
    private Camera playerCamera;


    public override void Enter()
    {
        _playerManager.SetDotsActive(true);
        _rigidbody.velocity = Vector3.zero;
        _player.SetIsInput(false);
        isThrowSword = false;
        Change_Info("Aim");
        //_player._CurrentState = this;
        time = 0;
        nowTimeScale = 0.25f;
        base.Enter();

    }

    public override void Exit()
    {
        if (!isThrowSword)
        {
            base.Exit();
            _player.SetIsInput(true);
        }
        _playerManager.SetDotsActive(false);
        Time.timeScale = 1;
    }

    public override void Update()
    {


        time += Time.deltaTime;
        base.Update();
        //角色跟随鼠标改变朝向
        PlayerDiretionFollowMouse();

        //抛物线显示
        if (_playerManager.swordState == SwordObject.SwordState.Normal)
            _playerManager.GetSkill<ThrowSwordSkill>((int)PlayerManager.SkillName.ThrowSwordSkill)?.SetDotsPosition();
        else if (_playerManager.swordState == SwordObject.SwordState.Penetrate || _playerManager.swordState == SwordObject.SwordState.Stop || _playerManager.swordState == SwordState.Catapult)
            _playerManager.GetSkill<ThrowSwordSkill>((int)PlayerManager.SkillName.ThrowSwordSkill)?.StraightLineAim();

        //角色进入瞄准状态给予几秒的时间延缓
        if (time <= continueTime && nowTimeScale < 1)
        {
            nowTimeScale += Time.deltaTime / 4;
            Time.timeScale = nowTimeScale;


        }
        else
        {
            Time.timeScale = 1;
        }


        if (Input.GetKeyDown("mouse 0"))
        {
            if (_playerManager.GetSkill<ThrowSwordSkill>((int)PlayerManager.SkillName.ThrowSwordSkill).CanUseSkill())
            {
                isThrowSword = true;
                _statemachine.ChangeState<PlayerThrowSword>((int)Player.PlayerState.ThrowSword);
            }
            else
            {
                isThrowSword = false;
                _statemachine.ChangeState<PlayerIdle>((int)Player.PlayerState.Idle);
            }
            return;
        }
        else if (Input.GetKeyUp("mouse 1"))
        {
            isThrowSword = false;
            _statemachine.ChangeState<PlayerIdle>((int)Player.PlayerState.Idle);
            return;
        }

    }

    private void PlayerDiretionFollowMouse()
    {
        if (playerCamera == null)
        {
            playerCamera = _player.transform.parent.GetComponentInChildren<Camera>();
        }
        Vector2 mousePosition = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x - _player.transform.position.x > 0 && _player.direction == -1)
        {
            _player.Filp();
        }
        else if (mousePosition.x - _player.transform.position.x < 0 && _player.direction == 1)
        {
            _player.Filp();
        }

    }


    //public Vector2 DotsPosition(float num, float time)
    //{
    //    num += 1;

    //    Vector2 position = Vector2.zero;
    //    position = (Vector2)_player.transform.position + new Vector2
    //    (_playerManager.throwSwordSkill.GetThrowSwordDirection().x * _playerManager._throwForce
    //    , _playerManager.throwSwordSkill.GetThrowSwordDirection().y * _playerManager._throwForce * 2) * time * num
    //    + .5f * Physics2D.gravity * (time * time * num * num);
    //    return position;
    //}

    ////设置瞄准白点的位置
    //public void SetDotsPosition()
    //{
    //    for (int i = 0; i < _playerManager.dotsCount; i++)
    //    {
    //        Vector2 position = DotsPosition(i, .3f);
    //        //Vector2 position = _playerManager.throwSwordSkill.GetDotsPosition(i, .2f);
    //        _playerManager.dots[i].transform.position = position;
    //    }
    //}

}
