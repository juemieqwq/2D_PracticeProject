using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [HideInInspector]
    public CinemachineVirtualCamera playerVirtualCamera;
    [HideInInspector]
    public Camera playerRealCamera;

    private CameraManager()
    {

    }
    [HideInInspector]
    public CameraManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        playerVirtualCamera = player.GetComponentInChildren<CinemachineVirtualCamera>();
        playerRealCamera = player.GetComponentInChildren<Camera>();
    }

}
