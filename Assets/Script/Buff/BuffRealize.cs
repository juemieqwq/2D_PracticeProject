using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static BuffEffectTable;
using static BuffRealize;

public interface IBuff
{
    public GameObject GetGameObject();
}

public class BuffRealize : MonoBehaviour
{

    //是否进行了初始化
    private bool isInit = false;
    //需要存在的时间
    private float needExistTime;
    //存在时间
    private float existTime;
    //当前运行的Buff类型
    public BuffType currentBuffType;
    //当前运行的属主
    private IBuff host;
    //当前的特效类
    private BuffShowEffect buffShowEffect;
    //当前运行模式的委托
    private delegate void RunningBuff();
    private RunningBuff runningBuffAction;

    #region 动画播放
    //数组动画
    private Animator hostAnimator;
    //图片播放类
    private SpriteRenderer spriteRenderer;
    //动画帧序列
    private Sprite[] frames;
    //触发帧
    private Sprite triggerFrame = null;
    //当前第几帧
    private int currentFrameNum;
    //播放帧率
    private float playFrame;
    //播放时间
    private float playTime;
    #endregion

    //是否需要还原数据
    private bool isResetHost = false;
    //对象的原本数据
    private float originalSpeed;
    private float originalForceJump;
    private Color originalMaterialColor = new Color(1, 1, 1, 1);

    //该Buff的数值
    [Range(0.2f, 4f)]
    private float scaleTime = 0;
    //伤害相关信息
    private float damage = 0;
    private DamageElementType damageElementType;
    private DamageType damageType;
    //Buff当前的层数
    private float stackNum = 1;
    //Buff的触发频率率
    private float buffTriggerTime = 1;
    //是否进行材质颜色的渐变
    private bool isGradualChangeColor = false;
    private float GradualChangerColorTime;
    public enum BuffType
    {
        //对血量进行修改
        DamageOverTime,
        HealOverTime,
        //对角色的动画和属性进行修改
        Haste,
        Slow

    }
    #region Unity方法

    private void Start()
    {
        gameObject.transform.position = Vector3.zero;
    }


    private void OnEnable()
    {

        if (isInit)
        {
            existTime = 0;
            playTime = 0;
            currentFrameNum = 0;
            GradualChangerColorTime = 0;
            buffTriggerTime = 1;
            spriteRenderer.sprite = frames[0];
            host = null;
            isResetHost = false;
            runningBuffAction = null;
            buffShowEffect = null;
            stackNum = 1;
            damageElementType = DamageElementType.Null;
            damageType = DamageType.TrueDamage;
            originalMaterialColor = new Color(1, 1, 1, 1);
        }

    }
    private void OnDisable()
    {
        if (SceneStateManager.IsQuitting)
            return;
        isInit = true;
        host.GetGameObject().GetComponentInChildren<SpriteRenderer>().material.color = originalMaterialColor;
        if (isResetHost)
            ResetHost();

    }

    private void Update()
    {
        if (isGradualChangeColor)
            GradualChangerColorTime += Time.deltaTime;
        if (isGradualChangeColor)
            GradualChangeColor();
        existTime += Time.deltaTime;
        buffTriggerTime += Time.deltaTime;
        if (transform.localPosition != Vector3.zero)
            transform.transform.localPosition = Vector3.zero;
        if (buffShowEffect.IsLoopPlay)
            PlayAnimation(buffShowEffect.IsLoopPlay);
        else if (buffTriggerTime >= 1 && !buffShowEffect.IsLoopPlay)
            PlayAnimation(buffShowEffect.IsLoopPlay);


    }

    private void OnDestroy()
    {
        if (SceneStateManager.IsQuitting)
            return;
    }

    #endregion

    public void Init(IBuff Host, float DurationTime, BuffShowEffect buffEffectTable)
    {
        this.buffShowEffect = buffEffectTable;
        triggerFrame = buffEffectTable.triggerFrame;
        needExistTime = DurationTime;
        frames = buffEffectTable.sprites;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        this.host = Host;
        playFrame = frames.Length * 2;
        spriteRenderer.sortingLayerName = "Buff";
        existTime = 0;
        playTime = 0;
        currentFrameNum = 0;
        spriteRenderer.sprite = frames[0];
        SetHostAnimator();
    }


    private void AnimationMethod()
    {
        if (currentFrameNum >= 0)
            if (frames[currentFrameNum] == triggerFrame)
            {
                runningBuffAction?.Invoke();
            }
    }

    private void PlayAnimation(bool IsLoopPlay)
    {
        playTime += Time.deltaTime;
        if (playTime > 1 / playFrame)
        {
            currentFrameNum += 1;
            if (currentFrameNum < frames.Length)
                spriteRenderer.sprite = frames[currentFrameNum];
            else if (IsLoopPlay)
            {
                if (existTime > needExistTime)
                {
                    ObjectPoolManager.instance.ReleaseObject(currentBuffType.ToString(), this.gameObject);
                    return;
                }

                spriteRenderer.sprite = frames[0];
                currentFrameNum = 0;
            }
            else if (!IsLoopPlay)
            {
                if (existTime > needExistTime)
                {
                    ObjectPoolManager.instance.ReleaseObject(currentBuffType.ToString(), this.gameObject);
                    return;
                }
                spriteRenderer.sprite = null;
                currentFrameNum = -1;
                buffTriggerTime = 0;
            }
            if (spriteRenderer.transform.localScale != new Vector3(2, 2, 2))
                spriteRenderer.transform.localScale = new Vector3(2, 2, 2);
            playTime = 0;
            AnimationMethod();
        }

    }

    private void GradualChangeColor(float DurationTime = 0.5f)
    {

        host.GetGameObject().GetComponentInChildren<SpriteRenderer>().material.color = Color.Lerp(buffShowEffect.material.color, originalMaterialColor, GradualChangerColorTime / DurationTime);
        if (host.GetGameObject().GetComponentInChildren<SpriteRenderer>().material.color == originalMaterialColor)
        {
            host.GetGameObject().GetComponentInChildren<SpriteRenderer>().material.color = originalMaterialColor;
            isGradualChangeColor = false;
            GradualChangerColorTime = 0f;
        }
    }


    #region buff种类的实现方法
    public void DamageOrHealOverTime(float damage, DamageType damageType = DamageType.Elementaldamage, DamageElementType damageElementType = DamageElementType.Null)
    {
        this.damage = damage;
        this.damageType = damageType;
        this.damageElementType = damageElementType;
        runningBuffAction += DamageOrHealOverTime;
    }

    private void DamageOrHealOverTime()
    {
        if (host.GetType() == typeof(PlayerInfo))
        {
            var Ihost = (host as IPlayerInfo);
            float LastDamage = InfoManager.Instance.ComputeDamage(Ihost, damage, damageType, damageElementType);
            Ihost.Hit(LastDamage, DamageTimeType.ReduceHealth);
        }
        else if (host.GetType() == typeof(EnemyInfo))
        {
            var Ihost = (host as IEnemyInfo);
            float LastDamage = InfoManager.Instance.ComputeDamage(Ihost, damage, damageType, damageElementType);
            Ihost.Hit(LastDamage, DamageTimeType.ReduceHealth);
        }
        isGradualChangeColor = true;
    }
    public void HasteOrSlowMethod(float scaleTime)
    {
        isResetHost = true;
        this.scaleTime = scaleTime;
        originalMaterialColor = host.GetGameObject().GetComponentInChildren<SpriteRenderer>().material.color;
        if (host.GetType() == typeof(PlayerInfo))
        {
            var Ihost = (host as IPlayerInfo);
            originalSpeed = Ihost.GetInfo(GetInfoType.Speed);
            originalForceJump = Ihost.GetInfo(GetInfoType.ForceJump);

            if (scaleTime < 1)
                Ihost.SetSpeedAndForceJump(originalSpeed * scaleTime, originalForceJump - originalForceJump / 2 * scaleTime);
            else
                Ihost.SetSpeedAndForceJump(originalSpeed * scaleTime, originalForceJump);
        }
        else if (host.GetType() == typeof(EnemyInfo))
        {
            var Ihost = (host as IEnemyInfo);
            originalSpeed = Ihost.GetInfo(GetInfoType.Speed);
            Ihost.SetSpeed(originalSpeed * scaleTime);
        }
        if (hostAnimator != null)
        {
            hostAnimator.speed = scaleTime;
        }
        else
        {
            Debug.LogError("Buff动画加减速的属主动画为空");
        }
        host.GetGameObject().GetComponentInChildren<SpriteRenderer>().material = buffShowEffect.material;
    }

    private void HasteOrSlowMethod()
    {
        if (!(currentBuffType == BuffType.Haste || currentBuffType == BuffType.Slow))
            return;
        if (host.GetType() == typeof(PlayerInfo))
        {
            var Ihost = (host as IPlayerInfo);
            if (scaleTime < 1)
                Ihost.SetSpeedAndForceJump(originalSpeed * scaleTime, originalForceJump - originalForceJump / 2 * (1 - scaleTime));
            else
                Ihost.SetSpeedAndForceJump(originalSpeed * scaleTime, originalForceJump);
        }
        else if (host.GetType() == typeof(EnemyInfo))
        {
            var Ihost = (host as IEnemyInfo);
            Ihost.SetSpeed(originalSpeed * scaleTime);
        }
        if (hostAnimator != null)
        {
            hostAnimator.speed = scaleTime;
        }
        else
        {
            Debug.LogError("Buff动画加减速的属主动画为空");
        }
    }


    private void ResetHost()
    {
        if (host.GetType() == typeof(PlayerInfo))
        {
            var Ihost = (host as IPlayerInfo);
            Ihost.SetSpeedAndForceJump(originalSpeed, originalForceJump);
            hostAnimator.speed = 1;
        }
        else if (host.GetType() == typeof(EnemyInfo))
        {
            var Ihost = (host as IEnemyInfo);
            Ihost.SetSpeed(originalSpeed);
            hostAnimator.speed = 1;
        }

    }

    /// <summary>
    /// 可以直接传一个1/？的帧率，或者传原本的帧率进行乘法
    /// </summary>
    /// <param name="PlayFrame">直接设置的帧率或者原本帧率</param>
    /// <param name="Multiple">原本帧率的倍数</param>
    public void SetPlayFrame(float PlayFrame, float Multiple = 1)
    {
        this.playFrame = PlayFrame * Multiple;
    }

    public void SetHostAnimator()
    {
        if (host.GetType() == typeof(PlayerInfo))
        {
            hostAnimator = (host as PlayerInfo).GetComponentInChildren<Animator>();
        }
        else if (host.GetType() == typeof(EnemyInfo))
        {
            hostAnimator = (host as EnemyInfo).GetComponentInChildren<Animator>();
        }
    }

    /// <summary>
    /// 对Buff进行叠层操作
    /// </summary>
    /// <param name="IsIncrease">设置Buff数值的增加，true为加法计算，false为减法计算</param>
    public void StackBuff(bool IsIncrease = true)
    {
        existTime = 0;
        if (stackNum >= 5)
            return;
        stackNum += 1;
        if (IsIncrease)
        {
            this.scaleTime += this.scaleTime * .2f;
            this.damage += this.damage * .2f;
        }
        else
        {
            this.scaleTime -= this.scaleTime * .2f;
            this.damage -= this.damage * .2f;
        }
        HasteOrSlowMethod();

    }
    #endregion
}
