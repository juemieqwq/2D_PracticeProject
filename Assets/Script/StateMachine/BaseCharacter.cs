using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class BaseCharacter : MonoBehaviour, StateMachineHost, IRole
{
    //角色动画
    public Animator anim { get; protected set; }
    //角色的刚体
    public Rigidbody2D rigidbody { get; protected set; }
    //角色是否在地面
    public bool isOnGround { get; protected set; }
    //角色是否碰到墙体
    public bool isInWall { get; protected set; }
    //角色的朝向
    public float direction { get; protected set; }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
}
