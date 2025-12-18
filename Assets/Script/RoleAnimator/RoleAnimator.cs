using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAnimator : MonoBehaviour
{
    [Header("一秒播放多少帧，应用与本角色的所有动画播放")]
    public int PlayFrame;
    public List<GameObject> Containers;
    public Dictionary<string, List<GameObject>> DicPlayImagesGameObjects;
    public List<GameObject> CurrentPlayBehavior;
    private bool IsFinshedPlay = false;
    public void Init()
    {
        foreach (var Contanier in Containers)
        {
            var children = Contanier.GetComponentsInChildren<Transform>(true);
            List<GameObject> PlayImagesGameObjects = new List<GameObject>();
            DicPlayImagesGameObjects = new Dictionary<string, List<GameObject>>();
            foreach (var child in children)
            {
                if (child == children[0])
                    continue;
                child.gameObject.SetActive(false);
                PlayImagesGameObjects.Add(child.gameObject);
                //Debug.LogError("对象" + child.gameObject + "加入队列");
            }
            Contanier.SetActive(true);
            if (!DicPlayImagesGameObjects.ContainsKey(Contanier.name))
            {
                DicPlayImagesGameObjects.Add(Contanier.name, PlayImagesGameObjects);
            }
        }
    }

    public void Start()
    {
        if (Containers != null)
        {
            Init();
            CurrentPlayBehavior = DicPlayImagesGameObjects.GetValueOrDefault("Idle");
        }
        else
            Debug.Log("获取的容器为空");
    }

    public void PlayRoleBehavior(bool isLoop = true)
    {
        StopCoroutine("Play");
        StartCoroutine(Play(isLoop));
    }

    private IEnumerator Play(bool isLoop = true)
    {
        float time = 0;
        float EveryFrame = 1f / (float)this.PlayFrame;
        int Index = 0;
        Debug.Log("开始播放");
        while (!IsFinshedPlay)
        {
            time += Time.deltaTime;
            if (time >= EveryFrame)
            {
                Index++;
                if (Index < CurrentPlayBehavior.Count)
                {

                    var FrameGameObject = CurrentPlayBehavior[Index];
                    SetCurrentBehaviorAtive();
                    FrameGameObject.SetActive(true);
                    time = 0;
                }
                else if (isLoop)
                {
                    SetCurrentBehaviorAtive();
                    CurrentPlayBehavior[0].SetActive(true);
                    Index = 0;
                    time = 0;
                }
                else if (!isLoop)
                {
                    IsFinshedPlay = true;
                }

            }
            yield return null;
        }
    }

    public void SetCurrentBehaviorAtive(bool isAtive = false)
    {
        foreach (var frame in CurrentPlayBehavior)
        {
            frame.SetActive(isAtive);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            IsFinshedPlay = false;
            PlayRoleBehavior(true);
        }
        if (Input.GetKeyDown("o"))
        {
            IsFinshedPlay = true;
            StopCoroutine("Play");
        }
    }
}
