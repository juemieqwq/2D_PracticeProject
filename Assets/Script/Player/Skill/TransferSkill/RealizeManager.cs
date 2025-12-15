using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealizeManager : MonoBehaviour
{
    public Dictionary<Type, ISkiilRealize> realizeDic = new Dictionary<Type, ISkiilRealize>();
    //技能实现类字典
    public Dictionary<string, SkillRealizeBase> realizeClassDic = new Dictionary<string, SkillRealizeBase>();


    public Player player;

    public PlayerManager playerManager;

    //实现的不同类型

    public TransferRealize transferRealize;

    private RealizeManager()
    {

    }

    public static RealizeManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            instance.playerManager = FindObjectOfType<PlayerManager>();
            instance.IntiDic();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void IntiDic()
    {
        //创建实现类
        transferRealize = new TransferRealize(playerManager, playerManager.GetSkill<TransferSkill>((int)PlayerManager.SkillName.TransferSkill));
        //通过类的类型存储类
        realizeClassDic.Add(TransferRealize.realizeName, transferRealize);
        //通过类的类型储存不同接口
        realizeDic.Add(typeof(TransferSkill), transferRealize);
    }

    public T GetRealizeClass<T>(string RealizeName) where T : SkillRealizeBase
    {
        SkillRealizeBase realizeBaseClass;
        if (instance.realizeClassDic.TryGetValue(RealizeName, out realizeBaseClass))
        {
            Debug.Log("获取类成功");
            return realizeBaseClass as T;
        }
        Debug.Log("获取实现类失败");
        return null;
    }

}
