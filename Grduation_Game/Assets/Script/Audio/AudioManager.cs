/*---------------BY017---------------*/
/*------音效控制---------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{    
    [Header("事件監聽")]
    public PlayAudioEventSO FxEvent;
    public PlayAudioEventSO BGMEvent;
    public FloatEventSO setMaterVolumeEvent;
    public FloatEventSO setBGMVolumeEvent;
    public FloatEventSO setFXVolumeEvent;
    public VoidEventSO pauseEvent;

    [Header("事件廣播")]
    public FloatEventSO syncVolumeEvent;

    [Header("音效組件")]
    public AudioSource BGMSource;
    public AudioSource FXSource;
    public AudioMixer audioMixer;


    private void OnEnable()
    {
        FxEvent.OnEventRised += OnFXEvent;
        BGMEvent.OnEventRised += OnBGMEvent;
        setMaterVolumeEvent.OnEventRaised += OnSetMasterVolume;
        setBGMVolumeEvent.OnEventRaised += OnSetBGMVolume;
        setFXVolumeEvent.OnEventRaised += OnSetFXVolume;
        pauseEvent.OnEventRaised += OnPauseEvent;
    }

    
    private void OnDisable()
    {
        FxEvent.OnEventRised -= OnFXEvent;
        BGMEvent.OnEventRised -= OnBGMEvent;
        setMaterVolumeEvent.OnEventRaised -= OnSetMasterVolume;
        setBGMVolumeEvent.OnEventRaised -= OnSetBGMVolume;
        setFXVolumeEvent.OnEventRaised -= OnSetFXVolume;
        pauseEvent.OnEventRaised -= OnPauseEvent;
    }

    private void OnPauseEvent()//遊戲暫停時傳遞主音量數據
    {
        float amount;
        audioMixer.GetFloat("MasterVolume", out amount);      
        syncVolumeEvent.RaiseEvent(amount);
    }
    //TODO:暫停時傳遞BGM音量、FX音量數據
    private void OnSetMasterVolume(float _amount)//設定主音量
    {
        audioMixer.SetFloat("MasterVolume", _amount * 100 - 80);
    }
    private void OnSetBGMVolume(float _amount)//設定背景音樂音量
    {
        audioMixer.SetFloat("BGMVolume", _amount * 100 - 80);
    }
    private void OnSetFXVolume(float _amount)//設定音效音量
    {
        audioMixer.SetFloat("FXVolume", _amount * 100 - 80);
    }

    private void OnFXEvent(AudioClip _clip)//播放音效
    {
        FXSource.clip = _clip;
        FXSource.Play();
    }
    private void OnBGMEvent(AudioClip _clip)//播放背景音樂
    {
        BGMSource.clip = _clip;
        BGMSource.Play();
    }

}
