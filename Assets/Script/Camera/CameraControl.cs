using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //相机边界的拓展组件
    private CinemachineConfiner2D confiner2D;
    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    private void Start()
    {
        GetCameraBound();
    }


    private void GetCameraBound()
    {
        var obj = GameObject.FindGameObjectWithTag("CameraBound");
        if (obj == null)
        {
            Debug.LogError("相机获取边界有误");
            return;
        }
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();

        confiner2D.InvalidateCache();

    }
}
