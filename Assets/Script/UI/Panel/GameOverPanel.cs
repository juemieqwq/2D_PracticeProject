using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [Header("监听事件")]
    [SerializeField]
    private VoidEventSO gameOverEventSO;
    [Header("广播事件")]
    [SerializeField]
    private VoidEventSO newGameEventSO;
    [SerializeField]
    private VoidEventSO loadGameEventSO;
    [Header("所有显示的UI")]
    [SerializeField]
    private GameObject showUI;
    private void OnEnable()
    {

        gameOverEventSO.AddEventListener(ShowPanpel);
    }
    private void OnDisable()
    {
        gameOverEventSO.RemoveEventListener(ShowPanpel);
    }

    void Start()
    {
        if (showUI == null)
            showUI = gameObject.GetComponentInChildren<Image>().gameObject;
        showUI.SetActive(false);
    }

    public void ShowPanpel()
    {
        Time.timeScale = 0;
        showUI.SetActive(true);
    }

    public void HidePanpel()
    {
        Time.timeScale = 1;
        showUI.SetActive(false);
    }

    public void LoadGame() => loadGameEventSO?.Raise();
    public void NewGame() => newGameEventSO?.Raise();
    public void BackMenuScene()
    {
        //这边应该搞个场景加载事件SO搞的，偷懒这样写
        PlayerManager.instance.player.playerCamera.gameObject.SetActive(false);
        PlayerManager.instance.player.ResetPlayer();
        SceneLoadManager.instance.LoadNewScene("Menu", SceneLoadManager.instance.menuPosition);
    }

    public void QuitGame() => Application.Quit();
}
