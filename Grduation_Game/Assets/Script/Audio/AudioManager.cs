/*---------------BY017---------------*/
/*------音效控制---------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("音效組件")]
    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource FXSource;

    [Header("事件監聽")]
    public PlayAudioEventSO FxEvent;
    public PlayAudioEventSO BGMEvent;

   private void OnEnable()
    {
        FxEvent.OnEventRised += OnFXEvent;
        BGMEvent.OnEventRised += OnBGMEvent;
    }

    
    private void OnDisable()
    {
        FxEvent.OnEventRised -= OnFXEvent;
        BGMEvent.OnEventRised -= OnBGMEvent;
    }

    private void OnFXEvent(AudioClip _clip)
    {
        FXSource.clip = _clip;
        FXSource.Play();
    }
    private void OnBGMEvent(AudioClip _clip)
    {
        BGMSource.clip = _clip;
        BGMSource.Play();
    }

}
