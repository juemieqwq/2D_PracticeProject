using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public VoidEventSO unloadSceneEvent;
    public GameObject dialog;


    void HideDialog()
    {
        dialog.SetActive(false);
    }

    private void OnDisable()
    {
        unloadSceneEvent.AddEventListener(HideDialog);
    }

    private void OnEnable()
    {
        unloadSceneEvent.RemoveEventListener(HideDialog);
    }
}
