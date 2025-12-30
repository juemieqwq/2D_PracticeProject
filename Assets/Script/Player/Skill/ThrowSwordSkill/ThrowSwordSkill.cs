using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ThrowSwordSkill : BaseSkill
{

    public static int swordNum = 1;
    private Camera playerCamera;
    public SwordObject.SwordState swordState { private set; get; }

    public ThrowSwordSkill(float coolDown, PlayerManager playerManager, Player player) : base(coolDown, playerManager, player)
    {

    }

    public override bool CanUseSkill()
    {

        if (ObjectPoolManager.instance.GetPool(SwordObject.poolName) == null)
        {
            ObjectPoolManager.instance.CreateNewPool(SwordObject.poolName, _playerManager.swordPrefab);
        }

        if (swordNum > 0)
            return true;
        return false;
    }

    public override void SkillUpdate()
    {

    }

    public override void UseSkill()
    {
        SwordObject sword;
        swordNum -= 1;
        sword = ObjectPoolManager.instance.GetObject(SwordObject.poolName)?.GetComponent<SwordObject>();
        if (sword != null)
        {
            sword.gameObject.SetActive(true);
            sword.transform.position = _player.transform.position + new Vector3(1f * _player.direction, 0.5f);
            sword.transform.rotation = _player.transform.rotation;
            sword.InitSword(GetThrowSwordDirection(), _playerManager._throwForce, swordState);
        }
        // sword = Instantiate(_playerManager.swordPrefab, _player.transform.position + new Vector3(1f * _player.direction, 0.5f), _player.transform.rotation).GetComponent<Sword>();


    }

    public void SetSwordState(SwordObject.SwordState state)
    {
        swordState = state;
    }

    public Vector2 GetThrowSwordDirection()
    {
        if (playerCamera == null)
            playerCamera = _player.transform.parent.GetComponentInChildren<Camera>();
        Vector2 throwDirection = (Vector2)playerCamera.ScreenToWorldPoint(Input.mousePosition) - (Vector2)_player.transform.position;
        return throwDirection.normalized;
    }

    public static void AddSwordNum(int num)
    {
        swordNum += num;
        Debug.Log("飞剑数量增加");
    }


    //直线点的坐标
    public Vector2 GetDotsPosition(int num, float time)
    {
        Vector2 position;
        position = (Vector2)_player.transform.position + GetThrowSwordDirection() * _playerManager._throwForce * (num + 1) * time;

        return position;
    }
    /// <summary>
    /// 设置抛物线为一条直线
    /// </summary>
    public void StraightLineAim()
    {
        for (int i = 0; i < _playerManager.dots.Length; i++)
        {
            Vector2 position;
            position = GetDotsPosition(i, .2f);
            _playerManager.dots[i].transform.position = position;
        }
    }
    //抛物线点的坐标
    public Vector2 DotsPosition(float num, float time)
    {
        num += 1;

        Vector2 position = Vector2.zero;
        position = (Vector2)_player.transform.position + new Vector2
        (this.GetThrowSwordDirection().x * _playerManager._throwForce
        , this.GetThrowSwordDirection().y * _playerManager._throwForce * 2) * time * num
        + .5f * Physics2D.gravity * (time * time * num * num);
        return position;
    }

    /// <summary>
    /// 设置一条抛物线
    /// </summary>
    public void SetDotsPosition()
    {
        for (int i = 0; i < _playerManager.dotsCount; i++)
        {
            Vector2 position = DotsPosition(i, .3f);
            _playerManager.dots[i].transform.position = position;
        }
    }
}
