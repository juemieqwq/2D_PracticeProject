using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffEffectTable", menuName = "Game/Buff Effect Table")]
public class BuffEffectTable : ScriptableObject
{

    public enum BuffEffectType
    {
        //燃烧
        Burn,
        //冰冻
        Freeze
    }


    [System.Serializable]
    public class BuffShowEffect
    {
        [Header("当前效果类型")]
        public BuffEffectType Type;
        [Header("效果的图片")]
        public Sprite[] sprites;
        [Header("触发函数的图片")]
        public Sprite triggerFrame = null;
        [Header("触发Buff时角色的材质")]
        public Material material;
        [Header("该动画是否进行循环播放")]
        public bool IsLoopPlay;

    }

    private bool isInit = false;
    public void Init()
    {
        buffShowEffectDic = new Dictionary<BuffEffectType, BuffShowEffect>();
        foreach (var buffShowEffect in buffShowEffectList)
        {
            if (!buffShowEffectDic.ContainsKey(buffShowEffect.Type))
            {
                buffShowEffectDic.Add(buffShowEffect.Type, buffShowEffect);
            }
        }
        isInit = true;
    }

    public void Reset()
    {
        buffShowEffectDic?.Clear();
        buffShowEffectDic = null;
        isInit = false;

    }

    [SerializeField]
    private List<BuffShowEffect> buffShowEffectList = new List<BuffShowEffect>();
    public Dictionary<BuffEffectType, BuffShowEffect> buffShowEffectDic;


}
