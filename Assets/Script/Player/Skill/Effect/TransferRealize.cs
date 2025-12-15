using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class TransferRealize : SkillRealizeBase, ISkiilRealize
{
    //一个大类的技能效果字典，通过字典中的效果为其添加额外效果
    public static Dictionary<string, ExtraRelize> extreRealizeDic = new Dictionary<string, ExtraRelize>();
    //
    public static Dictionary<string, ContinueRealize> continueRealizeDic = new Dictionary<string, ContinueRealize>();
    //追踪技能的目标
    private GameObject target;

    public TransferRealize(PlayerManager playerManger, BaseSkill skill) : base(playerManger, skill)
    {

    }

    public static string realizeName = "TransferRealize";

    void ISkiilRealize.BaseRealize()
    {
        player.transform.position = currentSkill.currentObject.transform.position;
        ObjectPoolManager.instance.ReleaseObject(TransferObject.poolName, currentSkill.currentObject);
    }

    void ISkiilRealize.ContinueRealize(GameObject gameObject = null)
    {
        if (gameObject == null)
            cRelize?.Invoke();
        else if (gameObject != null)
            cRelize?.Invoke(gameObject);
    }

    void ISkiilRealize.ExtraRealize()
    {
        eRelize?.Invoke();
    }


    /// <summary>
    /// 使对象去追踪离身边最近的敌人
    /// </summary>
    /// <param name="gameObject">去追踪的对象</param>
    public void TrackingEnemy(GameObject gameObject)
    {
        var enemies = Physics2D.OverlapCircleAll(gameObject.transform.position, 5);
        float distance = Mathf.Infinity;
        if (enemies != null)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.GetComponent<BaseEnemy>() == null)
                    continue;

                if (Vector2.Distance(gameObject.transform.position, enemy.transform.position) < distance)

                {
                    target = enemy.gameObject;
                    distance = Vector2.Distance(gameObject.transform.position, enemy.transform.position);
                }

            }
        }
        else
        {
            target = null;
        }

        if (target != null)
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, target.transform.position, 10 * Time.deltaTime);
        }
        else
            return;
    }


    /// <summary>
    /// 当检测范围内有怪物时，获取对象的Animator组件将其中的Explosion变为true使其爆炸，并回归对象池
    /// </summary>
    /// <param name="gameObject">对象</param>
    /// <param name="Radius">检测范围</param>
    public void Explosion(GameObject gameObject, float Radius)
    {
        var enemies = Physics2D.OverlapCircleAll(gameObject.transform.position, Radius);
        foreach (var enemy in enemies)
        {
            var script = enemy.GetComponent<BaseEnemy>();
            if (script != null)
            {
                ObjectPoolManager.instance.ReleaseObject(script.GetPoolName(), gameObject);
                enemy.GetComponentInChildren<Animator>().SetBool("Explosion", true);
            }

        }
    }

    public override void AddExtraRealize(ExtraRelize handler)
    {
        base.AddExtraRealize(handler);
    }

    public override void AddOrRemoveContinueRealize(ContinueRealize handler, bool isRemove = false)
    {
        base.AddOrRemoveContinueRealize(handler, isRemove);
    }
}
