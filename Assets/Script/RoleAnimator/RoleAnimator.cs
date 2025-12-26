using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static BehaviorContainer;

public interface IFrameEvent
{
    public GameObject GetGameObject();
}

public class RoleAnimator : MonoBehaviour
{
    [Header("一秒播放多少帧，应用与本角色的所有动画播放")]
    public int playFrame;
    private List<GameObject> containers;
    //播放图片对象字典
    public Dictionary<string, List<GameObject>> DicPlayImagesGameObjects;
    //播放容器中的帧方法
    public Dictionary<string, IFrameEvent[]> DicFrameEvents;
    [HideInInspector]
    public List<GameObject> currentPlayBehavior;
    private string currentKey;
    private bool isInit = false;
    private Coroutine currentCoroutine;
    [HideInInspector]
    public bool isFinshedPlay = false;
    //是否停止播放
    private bool isPausePlay = false;


    public void Init()
    {
        DicPlayImagesGameObjects = new Dictionary<string, List<GameObject>>();
        DicFrameEvents = new Dictionary<string, IFrameEvent[]>();
        if (containers == null)
        {
            containers = new List<GameObject>();
            foreach (var container in GetComponentsInChildren<BehaviorContainer>())
            {
                if (!containers.Contains(container.gameObject))
                {
                    containers.Add(container.gameObject);
                    Debug.Log("容器列表添加：" + container.gameObject);
                }
            }
        }
        if (containers.Count == 0)
        {
            isInit = false;
            Debug.LogError("容器存储列表为赋值,初始化失败");
            return;
        }
        foreach (var Contanier in containers)
        {
            var ContanierClass = Contanier.GetComponent<BehaviorContainer>();
            List<GameObject> PlayImagesGameObjects = new List<GameObject>();
            PlayImagesGameObjects = ContanierClass.GetPlayGameObjects();
            Contanier.gameObject.SetActive(true);
            var KeyName = String.Concat(ContanierClass.GetRoleBehaviorName(), ContanierClass.GetRoleBehaviorSerialNumber());
            //获取每一个行为容器中的帧事件
            var frameEvents = Contanier.gameObject.GetComponentsInChildren<IFrameEvent>(true);
            //如果存在帧事件，通过所在行为名字存入事件字典当中
            if (frameEvents.Length > 0 && !DicFrameEvents.ContainsKey(KeyName))
            {
                DicFrameEvents.Add(KeyName, frameEvents);
                //Debug.LogError("已存入行为：" + KeyName + "的帧方法");
                foreach (var frameEvent in frameEvents)
                {
                    frameEvent.GetGameObject().SetActive(false);
                    //Debug.LogError(frameEvent + "方法设为非活跃");
                }
            }
            //将图片对象存入字典
            if (!DicPlayImagesGameObjects.ContainsKey(KeyName))
            {
                DicPlayImagesGameObjects.Add(KeyName, PlayImagesGameObjects);
                //Debug.Log(PlayImagesGameObjects + "已存入");
            }
            else
                Debug.Log("字典已存在Key：" + KeyName);
        }
        isInit = true;
    }

    public void Awake()
    {
        Init();
    }

    public void PlayRoleBehavior(string Key, bool isLoop = true)
    {
        //如果未成功初始化，在播放时再次尝试初始化
        if (!isInit)
            Init();
        StopPlayCoroutine();
        SetCurrentBehaviorAllObjectGameAtive();

        if (DicPlayImagesGameObjects.TryGetValue(Key, out var Behavior))
        {
            currentKey = Key;
            currentPlayBehavior = Behavior;
            isFinshedPlay = false;
            if (DicFrameEvents.TryGetValue(Key, out var frameMethods))
            {
                foreach (var frameMethod in frameMethods)
                {
                    frameMethod.GetGameObject().SetActive(true);
                }
            }
            currentCoroutine = StartCoroutine(Play(isLoop));
            Debug.Log("当前播放协程：" + currentCoroutine);
        }
        else
        {
            Debug.LogError("字典未查出key：" + Key + "的value值");
        }
    }

    private void StopPlayCoroutine()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            if (DicFrameEvents.TryGetValue(currentKey, out var frameMethods))
            {
                foreach (var frameMethod in frameMethods)
                {
                    frameMethod.GetGameObject().SetActive(false);
                }
            }
        }
    }

    public static string BehaviorNameAndNumToString(RoleBehavior BehaviorName, int BehaviorSerialNumber = 1)
    {
        return String.Concat(BehaviorName, BehaviorSerialNumber);
    }

    private IEnumerator Play(bool isLoop = true)
    {
        float time = 0;
        float everyFrame = 0;
        everyFrame = 1f / (float)this.playFrame;
        int Index = 0;
        // 先激活第一帧
        if (currentPlayBehavior != null && currentPlayBehavior.Count > 0)
        {
            currentPlayBehavior[0].SetActive(true);
        }
        else
        {
            Debug.LogError("CurrentPlayBehavior is null or empty!");
            yield break;
        }
        while (!isFinshedPlay)
        {
            //是否暂停播放
            if (isPausePlay)
            {
                yield return new WaitForFixedUpdate();
                continue;
            }
            time += Time.fixedDeltaTime;
            while (time >= everyFrame && !isFinshedPlay)
            {
                time -= everyFrame;
                Index++;
                // 检查是否到达最后一帧
                if (Index >= currentPlayBehavior.Count)
                {
                    if (isLoop)
                    {
                        Index = 0; // 循环回到第一帧
                    }
                    else
                    {
                        // 非循环播放，停留在最后一帧
                        Index = currentPlayBehavior.Count - 1;
                        isFinshedPlay = true;
                        yield break;
                    }
                }
                if (!isFinshedPlay)
                {
                    for (int i = 0; i < currentPlayBehavior.Count; i++)
                    {
                        currentPlayBehavior[i].SetActive(i == Index);
                    }
                }

            }
            yield return new WaitForFixedUpdate();
        }
    }

    //停止图片播放
    public void PausePlay(bool isPause)
    {
        this.isPausePlay = isPause;
    }

    /// <summary>
    /// 设置当前行为全部图片的活跃状态为：传入值，默认为false
    /// </summary>
    /// <param name="isAtive"></param>
    public void SetCurrentBehaviorAllObjectGameAtive(bool isAtive = false)
    {
        foreach (var frame in currentPlayBehavior)
        {
            frame.SetActive(isAtive);
        }
    }

}
