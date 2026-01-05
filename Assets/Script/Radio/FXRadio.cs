using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXRadio : MonoBehaviour
{

    //[Header("是否唤醒时播放")]
    //private bool isEnablePlay;
    [Header("在启用时是否进行循环")]
    public bool isLoop;
    [Header("播放的音频")]
    public AudioClip clip;

    public void PlayAudio()
    {
        RadioManager.instance.PlayFXAudio(this);
    }

    public void OnEnable()
    {
        PlayAudio();
    }
}
