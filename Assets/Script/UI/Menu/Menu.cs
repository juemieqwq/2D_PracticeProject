using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject firstGameObject;

    [Header("监听事件")]
    [SerializeField]
    private VoidEventSO UnLoadSceneEventSO;
    [Header("广播事件")]
    [SerializeField]
    private VoidEventSO loadGameEventSO;
    [SerializeField]
    private VoidEventSO newGameEventSO;
    private void Start()
    {
        EventSystem.current.firstSelectedGameObject = firstGameObject;
    }

    private void OnEnable()
    {
        UnLoadSceneEventSO.AddEventListener(OpenPlayerCamera);
    }
    private void OnDisable()
    {
        UnLoadSceneEventSO.RemoveEventListener(OpenPlayerCamera);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("退出游戏");
    }

    public void OpenPlayerCamera()
    {
        PlayerManager.instance.player.playerCamera.gameObject.SetActive(true);
    }

    public void BroadcastEvent() => loadGameEventSO?.Raise();

    public void NewGame()
    {
        PlayerManager.instance.player.deathNum = 0;
        newGameEventSO?.Raise();
    }
}
