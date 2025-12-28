using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorContainer : MonoBehaviour
{
    public enum RoleBehavior
    {
        Idle,
        Walk,
        Run,
        Attack,
        Jump,
        Fall,
        Hit,
        Dead,
        Aim,
        Fire,
        Dash,
        WallSlide,
        AttackCrash,
        BackAttack
    }

    [SerializeField]
    private RoleBehavior roleBehaviorName;
    [SerializeField]
    private int roleBehaviorSerialNumber = 1;
    [Header("一秒播放多少帧")]
    [SerializeField]
    private int playFrame = 9;
    private List<GameObject> PlayGameObjects;

    public List<GameObject> GetPlayGameObjects()
    {
        if (PlayGameObjects == null)
        {
            PlayGameObjects = new List<GameObject>();
            var Children = gameObject.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (var Child in Children)
            {
                Child.gameObject.SetActive(false);
                PlayGameObjects.Add(Child.gameObject);
            }
        }
        return PlayGameObjects;
    }

    public RoleBehavior GetRoleBehaviorName()
    {
        return roleBehaviorName;
    }

    public int GetRoleBehaviorSerialNumber()
    {
        return roleBehaviorSerialNumber;
    }

    public string GetContainerKey()
    {
        return string.Concat(roleBehaviorName, roleBehaviorSerialNumber);
    }

    public int GetPlayFrame()
    {
        return playFrame;
    }
}
