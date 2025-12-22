using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static BehaviorContainer;

public class RoleAnimator : MonoBehaviour
{
    [Header("一秒播放多少帧，应用与本角色的所有动画播放")]
    public int playFrame;
    private List<GameObject> containers;
    public Dictionary<string, List<GameObject>> DicPlayImagesGameObjects;
    [HideInInspector]
    public List<GameObject> currentPlayBehavior;
    private bool isInit = false;
    private Coroutine currentCoroutine;
    [HideInInspector]
    public bool isFinshedPlay = false;
    //是否停止播放
    private bool isPausePlay = false;
    public void Init()
    {
        DicPlayImagesGameObjects = new Dictionary<string, List<GameObject>>();
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
            if (!DicPlayImagesGameObjects.ContainsKey(KeyName))
            {
                DicPlayImagesGameObjects.Add(KeyName, PlayImagesGameObjects);
                Debug.Log(PlayImagesGameObjects + "已存入");
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
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        SetCurrentBehaviorAtive();

        if (DicPlayImagesGameObjects.TryGetValue(Key, out var Behavior))
        {
            currentPlayBehavior = Behavior;
            isFinshedPlay = false;
            currentCoroutine = StartCoroutine(Play(isLoop));
            Debug.Log("当前播放协程：" + currentCoroutine);
        }
        else
        {
            Debug.LogError("字典未查出key：" + Key + "的value值");
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

    private void PausePlay(bool isPause)
    {
        this.isPausePlay = isPause;
    }

    public void SetCurrentBehaviorAtive(bool isAtive = false)
    {
        foreach (var frame in currentPlayBehavior)
        {
            frame.SetActive(isAtive);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown("p"))
        {

            PlayRoleBehavior(BehaviorNameAndNumToString(RoleBehavior.Idle, 1), true);
        }
        else if (Input.GetKeyDown("o"))
        {

            PlayRoleBehavior(BehaviorNameAndNumToString(RoleBehavior.Run, 1), false);
        }
        if (Input.GetKeyDown("s"))
        {
            PausePlay(!isPausePlay);
        }
    }
}
