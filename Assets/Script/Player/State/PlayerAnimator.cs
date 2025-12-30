using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    [Header("攻击信息：")]
    //攻击检测的位置
    [SerializeField]
    protected Transform _attackPosition;
    [SerializeField]
    protected float _attackRaius;
    protected BaseEnemy[] _enemy;
    //动画是否结束
    public bool isFinish = false;

    private Player _player = null;
    private PlayerManager _playerManager;
    void Start()
    {
        _player = GetComponentInParent<Player>();
        _playerManager = PlayerManager.instance;
    }


    public void FinishAnimator(int estimate)
    {
        isFinish = true;
        if (_player != null)
            _player._CurrentState.FinishAnimator(estimate);
    }


    public void OnDrawGizmosSelected()
    {

        if (_attackPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_attackPosition.position, _attackRaius);
        }


    }

    public void SetRaius(float raius)
    {
        _attackRaius = raius;
    }

    public void CreateSword()
    {
        _playerManager.UseSkill<ThrowSwordSkill>((int)PlayerManager.SkillName.ThrowSwordSkill);
    }


    public void CheckAttack()
    {
        if (_attackPosition != null)
        {
            foreach (var character in Physics2D.OverlapCircleAll(_attackPosition.position, _attackRaius))
            {
                if (character == null)
                    return;
                BaseEnemy enemyClass = character.GetComponentInChildren<BaseEnemy>();
                if (enemyClass != null)
                {
                    PlayerInfo playerInfo = gameObject.GetComponentInParent<PlayerInfo>();
                    EnemyInfo enemyInfo = character.GetComponentInChildren<EnemyInfo>();
                    Skill_Info skill_Info = gameObject.GetComponentInParent<Skill_Info>();
                    if (playerInfo != null)
                        InfoManager.Instance.Damage(playerInfo, enemyInfo);
                    else if (skill_Info != null)
                        InfoManager.Instance.Damage(skill_Info, enemyInfo);

                }

            }
        }
    }


    public bool CheckBackAttack()
    {
        if (_attackPosition != null)
        {
            foreach (var character in Physics2D.OverlapCircleAll(_attackPosition.position, _attackRaius))
            {
                if (character == null)
                    return false;
                if (character.GetComponent<BaseEnemy>() != null && character.GetComponentInChildren<BaseEnemy>()._isCanBeBackAttack)
                {
                    character.GetComponent<BaseEnemy>().BeBackAttack();
                    return true;
                }

            }
        }
        return false;
    }


    //public void _StopCorutine(string name)
    //{
    //    StopCoroutine(name);
    //}

    //public void _StartCorutine(float second)
    //{
    //    StartCoroutine(WaitSecondFinshAnimator(second));
    //}

    private IEnumerator WaitSecondFinshAnimator(float second)
    {
        _player._CurrentState.FinishAnimator(0);
        yield return new WaitForSeconds(second);
        _player._CurrentState.FinishAnimator(1);

    }


}
