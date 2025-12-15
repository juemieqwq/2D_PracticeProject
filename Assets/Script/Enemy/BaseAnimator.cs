using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseAnimator : MonoBehaviour
{

    protected BaseEnemy _host;



    [Header("攻击信息：")]
    //攻击检测的位置
    [SerializeField]
    protected Transform _attackPosition;
    [SerializeField]
    protected float _attackRaius;
    protected Player _player;


    void Start()
    {
        _player = FindObjectOfType<Player>();
        _host = GetComponentInParent<BaseEnemy>();
        Debug.Log("动画父组件的类为：" + _host);

    }

    public void IsAnimatorFinish(int IsFinish)
    {
        if (IsFinish == 0)
        {
            _host._CurrentState.SetIsFinish(false);
        }
        else if (IsFinish > 0)
        {
            _host._CurrentState.SetIsFinish(true);
        }
    }

    public void CheckAttack()
    {
        if (_attackPosition != null)
        {
            foreach (var player in Physics2D.OverlapCircleAll(_attackPosition.position, _attackRaius))
            {
                if (player == null)
                    return;
                if (player.GetComponentInChildren<Player>() == _player)
                {
                    InfoManager.Instance.Damage(gameObject.GetComponentInParent<EnemyInfo>(), player.GetComponentInChildren<PlayerInfo>());
                    //if((player.GetComponent<PlayerInfo>() as IPlayerInfo).GetHealth()>0)

                }
            }
        }
    }



    public void OnDrawGizmos()
    {

        if (_attackPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_attackPosition.position, _attackRaius);
        }
    }

    public void CanBeBackAttack(int num)
    {
        if (num > 0)
        {
            _host.SetIsCanBeBackAttack(true);
            _host.SetShowCanBeBackAttackTime(true);
        }
        else
        {
            _host.SetIsCanBeBackAttack(false);
            _host.SetShowCanBeBackAttackTime(false);
        }


    }
}
