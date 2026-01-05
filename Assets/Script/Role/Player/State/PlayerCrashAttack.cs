using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrashAttack : StateBase
{

    PlayerAnimator animator = null;
    private bool isReturnIdle = true;
    //ÊÇ·ñ¿ªÊ¼Æ´µ¶
    private bool _isCrashAttack = false;


    public override void Enter()
    {
        isReturnIdle = true;
        SetVelocity(0, 0);
        if (animator == null)
            animator = _player._playerAnimator;
        _isCrashAttack = false;
        Change_Info("CrashAttack");
        //_player._CurrentState = this;
        _player.SetIsInput(false);
        if (animator != null)
            animator.StartCoroutine("WaitSecondFinshAnimator", .2f);
        base.Enter();
    }

    public override void Exit()
    {
        if (isReturnIdle)
        {
            base.Exit();
            _player.SetIsInput(true);
        }

    }

    public override void Update()
    {
        base.Update();

        if (_isCrashAttack && IsFinish)
        {
            _statemachine.ChangeState<PlayerBackAttack>((int)Player.PlayerState.BackAttack);
        }

        if (!_isCrashAttack && animator.CheckBackAttack())
        {
            isReturnIdle = false;
            animator.StopCoroutine("WaitSecondFinshAnimator");
            animator.StartCoroutine("WaitSecondFinshAnimator", .5f);
            _playerManager?.ApplyCameraViewZoomAndOffset(.5f, 2, new Vector3(1, 0, 0));
            _playerManager?.ApplyCameraShake(.5f, 2);
            _isCrashAttack = true;
            return;
        }
        else if (IsFinish && !_isCrashAttack)
        {
            _statemachine.ChangeState<PlayerIdle>(0);
            _player.SetIsInput(true);
            return;
        }
    }


}
