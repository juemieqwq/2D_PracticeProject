using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QTE : MonoBehaviour
{
    [Header("输入设置")]
    public List<QTESC> QTEGameObjectList = new List<QTESC>();
    public Dictionary<string, QTESC> QTEGameObjectDict;
    public Canvas canvas;
    private int cout;
    private bool isInit = false;

    private void Start()
    {
        if (QTEGameObjectList.Count > 0)
        {
            QTEGameObjectDict = new Dictionary<string, QTESC>();
            foreach (var UIGameObject in QTEGameObjectList)
            {
                var key = UIGameObject.QTEName;
                if (!QTEGameObjectDict.ContainsKey(key))
                {
                    QTEGameObjectDict.Add(key, UIGameObject);
                }
            }
        }
    }

    private void Update()
    {
        if (!isInit)
            return;
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            QTEGameObjectDict["A"].QTEUIGameObject.SetActive(false);
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            QTEGameObjectDict["S"].QTEUIGameObject.SetActive(false);
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            QTEGameObjectDict["D"].QTEUIGameObject.SetActive(false);
        }
        else if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            QTEGameObjectDict["Space"].QTEUIGameObject.SetActive(false);
        }
        else if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            QTEGameObjectDict["MouseLeft"].QTEUIGameObject.SetActive(false);
        }
        else if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            QTEGameObjectDict["MouseRight"].QTEUIGameObject.SetActive(false);
        }
        else if (Keyboard.current.shiftKey.wasPressedThisFrame)
        {
            QTEGameObjectDict["Shift"].QTEUIGameObject.SetActive(false);
        }
        else if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            QTEGameObjectDict["R"].QTEUIGameObject.SetActive(false);
        }
        else if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            QTEGameObjectDict["F"].QTEUIGameObject.SetActive(false);
        }
        if (Keyboard.current.anyKey.wasPressedThisFrame)
            Debug.LogError("检测到按键");

        Debug.Log(GetComponentsInChildren<Image>().Length);
        if (GetComponentsInChildren<Image>().Length <= 0)
        {
            PlayerManager.instance.player.playerController.SetPlayerController(true);
            isInit = true;
            gameObject.SetActive(false);
        }
    }


    public void StartQTE()
    {
        gameObject.SetActive(true);
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = PlayerManager.instance.player.playerCamera;
        canvas.sortingLayerName = "UI";
        isInit = true;
    }
}

[System.Serializable]
public class QTESC
{
    public string QTEName;
    public GameObject QTEUIGameObject;
}
