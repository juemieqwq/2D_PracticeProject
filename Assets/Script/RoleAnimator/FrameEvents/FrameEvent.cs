using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FrameEvent : MonoBehaviour
{

    [Header("该帧启用时运行")]
    public UnityEvent onEnableEvents;
    [Header("该帧禁用时运行")]
    public UnityEvent onDisableEvents;
    [Header("该帧存在时运行(Update)")]
    public UnityEvent onUpdateEvents;
    [Header("该帧存在时运行(FixedUpdate)")]
    public UnityEvent onFixedUpdateEvents;

    private bool isUpdate = false;
    private bool isFixedUpdate = false;

    void Start()
    {

    }

    private void OnDisable()
    {
        StopAllCoroutines();
        isFixedUpdate = false;
        isUpdate = false;
        if (SceneStateManager.IsQuitting || SceneStateManager.IsApplicationQuitting)
            return;
        onDisableEvents.Invoke();

    }

    private void OnEnable()
    {


        onEnableEvents.Invoke();
        if (onUpdateEvents.GetPersistentEventCount() > 0)
        {
            isUpdate = true;
            StartCoroutine(update());
        }

        if (onFixedUpdateEvents.GetPersistentEventCount() > 0)
        {
            isFixedUpdate = true;
            StartCoroutine(fixedUpdate());
        }


    }

    /// <summary>
    /// 通过协程代替MonoBehavior中的Update
    /// </summary>
    /// <returns></returns>
    private IEnumerator update()
    {
        while (isUpdate)
        {
            onUpdateEvents.Invoke();
            yield return null;
        }
    }
    /// <summary>
    /// 通过协程代替MonoBehavior的FixedUpdate
    /// </summary>
    /// <returns></returns>
    private IEnumerator fixedUpdate()
    {
        var wait = new WaitForFixedUpdate();
        while (isFixedUpdate)
        {
            onFixedUpdateEvents.Invoke();
            yield return wait;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        onUpdateEvents.RemoveAllListeners();
        onFixedUpdateEvents.RemoveAllListeners();
        onDisableEvents.RemoveAllListeners();
        onEnableEvents.RemoveAllListeners();
    }
}
