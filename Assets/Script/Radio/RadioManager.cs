using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RadioManager : MonoBehaviour
{

    private RadioManager()
    {

    }
    [SerializeField]
    private AudioMixer audioMixer;

    public static RadioManager instance;

    private Dictionary<string, FXRadio> DicAudioClip = new Dictionary<string, FXRadio>();

    private Dictionary<AudioSource, float> DicPlayingAudioSourceExistTime = new Dictionary<AudioSource, float>();

    private AudioSource[] FXAudioSources;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            FXAudioSources = new AudioSource[5];
            for (int i = 0; i < FXAudioSources.Length; i++)
            {
                FXAudioSources[i] = gameObject.AddComponent<AudioSource>();
                var audioSource = FXAudioSources[i];
                audioSource.playOnAwake = false;
                audioSource.loop = false;
            }
        }
        else
            Destroy(gameObject);
    }


    private void Update()
    {
        foreach (var FXAudioSource in FXAudioSources)
        {
            if (DicPlayingAudioSourceExistTime.ContainsKey(FXAudioSource))
            {
                if (!FXAudioSource.isPlaying)
                {
                    DicPlayingAudioSourceExistTime.Remove(FXAudioSource);
                    Debug.Log("移除:" + FXAudioSource);
                }
                else
                {
                    DicPlayingAudioSourceExistTime[FXAudioSource] += Time.deltaTime;
                }
            }

        }
    }

    private AudioSource GetFreeAudioSource(AudioSource[] audioSources)
    {
        foreach (var audioSource in audioSources)
        {
            if (!audioSource.isPlaying || audioSource.clip == null)
                return audioSource;
        }
        return null;
    }

    private AudioSource GetAudioSourceFromPlayingAudio(AudioSource[] audioSources)
    {
        float maxTime = 0;
        int index = 0;
        for (int i = 0; i < audioSources.Length; i++)
        {
            var currentTime = DicPlayingAudioSourceExistTime[audioSources[i]];
            if (maxTime < currentTime)
            {
                maxTime = currentTime;
                index = i;
            }
        }
        audioSources[index].Stop();
        audioSources[index].clip = null;
        DicPlayingAudioSourceExistTime.Remove(audioSources[index]);
        //Debug.Log("播放队列中的：" + audioSources[index] + "已被释放");
        return audioSources[index];
    }

    public void PlayFXAudio(FXRadio fXRadioInfo)
    {
        AudioSource freeAudioSource = GetFreeAudioSource(FXAudioSources);
        if (freeAudioSource != null)
        {
            if (DicAudioClip.TryGetValue(fXRadioInfo.clip.name, out var radioInfo))
            {
                freeAudioSource.clip = radioInfo.clip;
                freeAudioSource.loop = radioInfo.isLoop;
            }
            else
            {
                freeAudioSource.clip = fXRadioInfo.clip;
                freeAudioSource.loop = fXRadioInfo.isLoop;
                DicAudioClip.Add(fXRadioInfo.clip.name, fXRadioInfo);
            }
            freeAudioSource.Play();
            DicPlayingAudioSourceExistTime.Add(freeAudioSource, 0);
        }
        else
        {
            freeAudioSource = GetAudioSourceFromPlayingAudio(FXAudioSources);
            if (DicAudioClip.TryGetValue(fXRadioInfo.clip.name, out var radioInfo))
            {
                freeAudioSource.clip = radioInfo.clip;
                freeAudioSource.loop = radioInfo.isLoop;
            }
            else
            {
                freeAudioSource.clip = fXRadioInfo.clip;
                freeAudioSource.loop = fXRadioInfo.isLoop;
                DicAudioClip.Add(fXRadioInfo.clip.name, fXRadioInfo);
            }
            freeAudioSource.Play();
            DicPlayingAudioSourceExistTime.Add(freeAudioSource, 0);
        }


    }
}
