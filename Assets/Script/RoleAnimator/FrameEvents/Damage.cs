using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Damage : MonoBehaviour, IFrameEvent
{

    private BehaviorContainer container;
    private RoleAnimator roleAnimator;


    [Header("控制伤害的调用")]
    [SerializeField]
    private DamageTimeType damageTimeType;
    [Header("攻击中心点")]
    [SerializeField]
    private Transform attackCenter;
    [Header("攻击半径")]
    [SerializeField]
    private float attackRadius;
    [Header("攻击目标")]
    [SerializeField]
    private LayerMask targetLayerMask;
    [Header("开始帧")]
    [SerializeField]
    private GameObject startFrame;
    [Header("结束帧")]
    [SerializeField]
    private GameObject endFrame;
    private bool endFrameIsAtive;
    private bool isDetection;
    private float damage;
    private IRoleInfo host;
    private List<Collider2D> hitRoles;

    private void Awake()
    {
        host = GetComponentInParent<IRoleInfo>();
        if (hitRoles == null)
            hitRoles = new List<Collider2D>();

        damage = host.GetInfo(GetInfoType.Damage);
        container = GetComponentInParent<BehaviorContainer>();
        roleAnimator = container.GetComponentInParent<RoleAnimator>();
        if (startFrame == null || (startFrame == null && endFrame == null))
            GetStartAndEndFrame();
    }


    private void OnDrawGizmos()
    {
        if (attackCenter == null) return;

        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawWireSphere(attackCenter.position, attackRadius);
    }



    private void Update()
    {
        SetIsDetection();
        if (!isDetection)
            return;
        if (attackCenter != null)
        {
            var targets = Physics2D.OverlapCircleAll(attackCenter.position, attackRadius);
            foreach (var target in targets)
            {
                if (hitRoles.Contains(target))
                    continue;
                var roleInfo = target.GetComponent<IRoleInfo>();
                if (roleInfo == host || roleInfo == null)
                    continue;
                roleInfo.Hit(damage, damageTimeType);
                hitRoles.Add(target);
            }
        }
    }
    void OnEnable()
    {

        //// 或者使用延迟检测
        //StartCoroutine(DelayedTriggerDetection());
    }

    private void GetStartAndEndFrame()
    {
        var frames = GetComponentsInChildren<SpriteRenderer>(true);
        if (frames != null)
        {
            startFrame = frames.First().gameObject;
            endFrame = frames.Last().gameObject;
        }
    }

    private void SetIsDetection()
    {
        if (startFrame.activeSelf)
            isDetection = true;
        if (endFrame.activeSelf)
            endFrameIsAtive = true;
        if (!endFrame.activeSelf && endFrameIsAtive)
        {
            isDetection = false;
            hitRoles?.Clear();
            endFrameIsAtive = false;
        }

    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }



    //IEnumerator DelayedTriggerDetection()
    //{
    //    yield return new WaitForFixedUpdate();
    //    Physics2D.SyncTransforms();
    //}
}
