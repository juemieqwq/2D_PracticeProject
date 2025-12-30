using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.AnimatedValues;
using UnityEditor.Build.Content;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Assertions.Must;
using static SwordObject;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField]
    public Player player;
    public enum SkillName
    {
        DashSkill = 0,
        ThrowSwordSkill = 1,
        FreezingTimeSkill = 2,
        TransferSkill = 3,
    }


    #region 角色技能信息
    [Header("Skill")]
    private BaseSkill _currentSkill;

    #region 冲刺相关信息
    private DashSkill dashSkill;
    //克隆预制体
    [SerializeField]
    public GameObject clonePrefab;
    #endregion

    #region 飞剑相关信息
    [Header("ThrowSwordSkill")]
    private ThrowSwordSkill throwSwordSkill;
    [SerializeField]
    public GameObject swordPrefab;
    [SerializeField]
    public GameObject dotPrefab;
    [SerializeField]
    public float _throwForce;
    //点所在的父类
    [SerializeField]
    private Transform dotsParent;
    //瞄准时所用到的点
    public GameObject[] dots { private set; get; }
    //需要几个点
    [SerializeField]
    public int dotsCount { private set; get; } = 6;
    //飞剑的状态
    public SwordObject.SwordState swordState
    {
        get { return this.throwSwordSkill.swordState; }
        set { this.throwSwordSkill.SetSwordState(value); }
    }
    #endregion

    #region 冻结时间相关信息
    private FreezingTimeSkill freezingTimeSkill;
    [SerializeField]
    public GameObject freezingTimePrefab;
    //FreezingTime状态
    public FreezingTimeSkill.FreezingTimeState freezingTimeState
    {
        get
        {
            return freezingTimeSkill.state;
        }
        set { freezingTimeSkill.state = value; }
    }

    ////是否进行克隆攻击
    //public bool isCanCloneAttack
    //{
    //    get { return freezingTimeSkill.isCanCloneAttack; }
    //    set { freezingTimeSkill.isCanCloneAttack = value; }
    //}
    #endregion

    #region 转移相关信息
    private TransferSkill transferSkill;
    [SerializeField]
    public GameObject transferPrefab;

    #endregion

    //角色技能字典
    Dictionary<int, BaseSkill> dicSkill = new Dictionary<int, BaseSkill>();

    //角色的技能队列
    private List<BaseSkill> listSkill = new List<BaseSkill>();
    #endregion

    #region 角色相机相关
    [SerializeField]
    public Cinemachine.CinemachineVirtualCamera _virtualCamera;
    [SerializeField]
    public Cinemachine.NoiseSettings _noiseSettings;
    #endregion

    private Coroutine shakeCoroutine;
    private Coroutine viewZoomCoroutine;
    private float originalCameraSize;
    private Vector3 originalCameraOffset;
    private void Awake()
    {
        if (instance == null)
        {
            if (player == null)
            {
                player = GameObject.FindObjectOfType<Player>();
            }
            originalCameraSize = _virtualCamera.m_Lens.OrthographicSize;
            originalCameraOffset = _virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineFramingTransposer>().m_TrackedObjectOffset;
            instance = this;
            DontDestroyOnLoad(gameObject);
            //当玩家数据为空时阻止游戏运行并且报错
            Assert.IsNotNull(player, "玩家单例中的玩家数据为空");
            Assert.IsNotNull(clonePrefab, "玩家单例中的克隆体预制体为空");
            InitSkill();
            CreateDots();

        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        SkillCoolDownTimeUpdate();

        ////切换飞剑模式
        //if (Input.GetKeyDown("t"))
        //{
        //    if (swordState == SwordState.Catapult)
        //        swordState = SwordState.Normal;
        //    else if (swordState == SwordState.Normal)
        //        swordState = SwordState.Penetrate;
        //    else if (swordState == SwordState.Penetrate)
        //        swordState = SwordState.Stop;
        //    else if (swordState == SwordState.Stop)
        //        swordState = SwordState.Catapult;
        //    Debug.Log("SwordState:" + swordState);
        //}
    }

    public T GetSkill<T>(int skillnum) where T : BaseSkill
    {
        if (dicSkill.TryGetValue(skillnum, out _currentSkill))
        {
            return _currentSkill as T;
        }
        return null;

    }


    public void UseSkill<T>(int skillnum) where T : BaseSkill
    {
        if (GetSkill<T>(skillnum).CanUseSkill())
            GetSkill<T>(skillnum).UseSkill();
    }

    public void SkillCoolDownTimeUpdate()
    {
        foreach (var skill in listSkill)
        {
            skill.SkillUpdate();
        }

    }

    //生成瞄准时的白点并预存
    private void CreateDots()
    {
        dots = new GameObject[dotsCount];
        for (int i = 0; i < dotsCount; i++)
        {
            dots[i] = Instantiate(dotPrefab, dotsParent);
        }
        SetDotsActive(false);
    }

    //设置瞄准白点是否活跃
    public void SetDotsActive(bool active)
    {
        for (int i = 0; i < dotsCount; i++)
        {
            dots[i].SetActive(active);
        }
    }




    //技能初始化
    public void InitSkill()
    {
        //技能冷却时间初始化
        dashSkill = new DashSkill(1f, this, player);
        throwSwordSkill = new ThrowSwordSkill(0f, this, player);
        freezingTimeSkill = new FreezingTimeSkill(20f, this, player);
        transferSkill = new TransferSkill(3f, this, player);
        //技能队列初始化
        listSkill.Add(dashSkill);
        listSkill.Add(throwSwordSkill);
        listSkill.Add(freezingTimeSkill);
        listSkill.Add(transferSkill);
        //技能字典初始化
        dicSkill.Add((int)SkillName.DashSkill, dashSkill);
        dicSkill.Add((int)SkillName.ThrowSwordSkill, throwSwordSkill);
        dicSkill.Add((int)SkillName.FreezingTimeSkill, freezingTimeSkill);
        dicSkill.Add((int)SkillName.TransferSkill, transferSkill);
    }



    // 创建简单的抖动效果
    /// <summary>
    /// 调用一次相机的抖动
    /// </summary>
    /// <param name="duration">相机抖动的持续时间</param>
    /// <param name="intensity">相机抖动的强度</param>
    /// <param name="frequency">相机抖动的频率</param>
    public void ApplyCameraShake(float duration, float intensity, float frequency = 1)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, intensity, frequency));
    }


    public void ApplyCameraViewZoomAndOffset(float wait, float size, Vector3 offset)
    {
        if (viewZoomCoroutine != null)
            StopCoroutine(viewZoomCoroutine);
        viewZoomCoroutine = StartCoroutine(ViewZoomCoroutine(wait, size, offset));
    }



    /// <summary>
    /// 调用一次相机的抖动
    /// </summary>
    /// <param name="duration">相机抖动的持续时间</param>
    /// <param name="intensity">相机抖动的强度</param>
    /// <param name="frequency">相机抖动的频率</param>
    /// <returns></returns>
    private IEnumerator ShakeCoroutine(float duration, float intensity, float frequency = 1)
    {
        var noise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise == null)
        {
            noise = _virtualCamera.AddComponent<CinemachineBasicMultiChannelPerlin>();
            //noise.m_NoiseProfile = _noiseSettings;
        }


        noise.m_AmplitudeGain = intensity;
        noise.m_FrequencyGain = frequency;


        yield return new WaitForSeconds(duration);
        // 恢复平静
        noise.m_FrequencyGain = 0f;
        noise.m_AmplitudeGain = 0f;
    }

    private IEnumerator ViewZoomCoroutine(float wait, float size, Vector3 offset)

    {
        var framingTransposer = _virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineFramingTransposer>();
        float everyWair = .1f;
        var num = wait / everyWair;

        ////存储原本的正交大小
        //var originalSize = _virtualCamera.m_Lens.OrthographicSize;
        ////储存原本的相机偏移量
        //var originalOffset = framingTransposer.m_TrackedObjectOffset;
        //for (int i = 1; i < num + 1; i++)
        //{
        //    //修改正交大小，拉近相机距离
        //    _virtualCamera.m_Lens.OrthographicSize = size / num * i;
        //    framingTransposer.m_TrackedObjectOffset = offset / num * i;

        //    yield return new WaitForSeconds(everyWair);
        //}
        _virtualCamera.m_Lens.OrthographicSize = size;
        framingTransposer.m_TrackedObjectOffset = offset;
        yield return new WaitForSeconds(wait);
        _virtualCamera.m_Lens.OrthographicSize = originalCameraSize;
        framingTransposer.m_TrackedObjectOffset = originalCameraOffset;


    }
}
