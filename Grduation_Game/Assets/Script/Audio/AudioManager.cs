/*---------------BY017---------------*/
/*------���ı���---------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("���Ĳե�")]
    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource FXSource;

    [Header("�ƥ��ť")]
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
