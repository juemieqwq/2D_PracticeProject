using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveCamera : MonoBehaviour
{
    public Vector3 initPosition;
    public Vector3 goToPosition;
    public Camera sceneCamera;
    //public string crrentCameraName;
    public VoidEventSO loadSceneEvent;
    private float time;
    public float cameraMoveTime = 3f;
    // Update is called once per frame
    public QTE QTEClass;
    void Update()
    {
        if (PlayerManager.instance.player.playerCamera.gameObject.activeSelf)
            return;
        if ((float)time / (float)cameraMoveTime >= 1)
        {
            SceneLoadManager.instance.isFirstEnterCave = false;
            sceneCamera.gameObject.SetActive(false);
            PlayerManager.instance.player.playerCamera.gameObject.SetActive(true);
            PlayerManager.instance.player.playerController.SetPlayerController(false);
            QTEClass.gameObject.SetActive(true);
            QTEClass.StartQTE();
            loadSceneEvent.RemoveEventListener(ResetCamera);
        }
        else if (time > 0)
        {
            time += Time.deltaTime;
            sceneCamera.transform.position = new Vector3(Mathf.Lerp(initPosition.x, goToPosition.x, time), Mathf.Lerp(initPosition.y, goToPosition.y, (float)time / cameraMoveTime), -10);
        }
        else
            time += Time.deltaTime;
    }

    public void ResetCamera()
    {
        time = -2f;
        PlayerManager.instance.player.playerCamera.gameObject.SetActive(false);
        PlayerManager.instance.player.playerController.SetPlayerController(false);
        sceneCamera.gameObject.SetActive(true);
        sceneCamera.transform.localPosition = Vector3.zero;
    }

    private void OnEnable()
    {
        if (SceneLoadManager.instance.isFirstEnterCave)
        {
            loadSceneEvent.AddEventListener(ResetCamera);
        }
        else
        {
            QTEClass.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        if (SceneLoadManager.instance.isFirstEnterCave)
            loadSceneEvent.RemoveEventListener(ResetCamera);
    }
}
