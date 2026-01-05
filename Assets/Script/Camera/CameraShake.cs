using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineCollisionImpulseSource))]
public class CameraShake : MonoBehaviour
{
    private CinemachineCollisionImpulseSource impulseSource;
    private void Start()
    {
        impulseSource = GetComponent<CinemachineCollisionImpulseSource>();
    }

    public void ShakeCamera()
    {
        impulseSource.GenerateImpulse();
    }
}
