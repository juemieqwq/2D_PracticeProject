using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ISkiilRealize
{
    public void BaseRealize();

    public void ExtraRealize();

    public void ContinueRealize(GameObject gameObject = null);
}


public class SkillRealizeBase : MonoBehaviour
{

    public delegate void ExtraRelize(GameObject gameObject = null);
    public ExtraRelize eRelize;
    public delegate void ContinueRealize(GameObject gameObject = null);
    public ContinueRealize cRelize;

    public enum RealizeName
    {
        TransferRealize,
    }


    //玩家单例
    public PlayerManager playerManager;

    public Player player;

    public BaseSkill currentSkill;
    public SkillRealizeBase(PlayerManager playerManger, BaseSkill skill)
    {
        this.playerManager = playerManger;
        player = playerManager.player;
        currentSkill = skill;
    }




    public virtual void AddExtraRealize(ExtraRelize handler)
    {
        if (handler == null)
        {
            Debug.Log("字典中没有" + handler + "添加失败");
            return;
        }

        if (eRelize != null)
        {
            var delegates = eRelize.GetInvocationList();
            foreach (var Handler in delegates)
            {
                if (handler as Delegate == Handler)
                {
                    Debug.Log("添加失败额外拓展已经拥有");
                    return;
                }
            }
        }
        eRelize += handler;
    }

    /// <summary>
    /// 通过物体不同时间添加或移除效果来实现一些特殊效果
    /// </summary>
    /// <param name="handler">添加效果的方法</param>
    /// <param name="isRemove">是否进行移除</param>
    public virtual void AddOrRemoveContinueRealize(ContinueRealize handler, bool isRemove = false)
    {
        if (handler == null)
        {
            Debug.Log("字典中没有" + handler + "添加失败");
            return;
        }

        if (!isRemove)
        {
            if (cRelize != null)
            {
                var delegates = cRelize.GetInvocationList();
                foreach (var Handler in delegates)
                {
                    if (handler as Delegate == Handler)
                    {
                        Debug.Log("添加失败额外拓展已经拥有");
                        return;
                    }
                }
            }
            cRelize += handler;
        }
        else
        {
            if (cRelize != null)
            {
                var delegates = cRelize.GetInvocationList();
                foreach (var Handler in delegates)
                {
                    if (handler as Delegate == Handler)
                    {
                        cRelize -= handler;
                        return;
                    }
                }
            }
        }
    }
}
