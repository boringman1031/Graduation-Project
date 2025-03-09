/*---------------BY017---------------*/
/*------���ı���---------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{    
    [Header("�ƥ��ť")]
    public PlayAudioEventSO FxEvent;
    public PlayAudioEventSO BGMEvent;
    public FloatEventSO setMaterVolumeEvent;
    public FloatEventSO setBGMVolumeEvent;
    public FloatEventSO setFXVolumeEvent;
    public VoidEventSO pauseEvent;

    [Header("�ƥ�s��")]
    public FloatEventSO syncVolumeEvent;

    [Header("���Ĳե�")]
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

    private void OnPauseEvent()//�C���Ȱ��ɶǻ��D���q�ƾ�
    {
        float amount;
        audioMixer.GetFloat("MasterVolume", out amount);      
        syncVolumeEvent.RaiseEvent(amount);
    }
    //TODO:�Ȱ��ɶǻ�BGM���q�BFX���q�ƾ�
    private void OnSetMasterVolume(float _amount)//�]�w�D���q
    {
        audioMixer.SetFloat("MasterVolume", _amount * 100 - 80);
    }
    private void OnSetBGMVolume(float _amount)//�]�w�I�����֭��q
    {
        audioMixer.SetFloat("BGMVolume", _amount * 100 - 80);
    }
    private void OnSetFXVolume(float _amount)//�]�w���ĭ��q
    {
        audioMixer.SetFloat("FXVolume", _amount * 100 - 80);
    }

    private void OnFXEvent(AudioClip _clip)//���񭵮�
    {
        FXSource.clip = _clip;
        FXSource.Play();
    }
    private void OnBGMEvent(AudioClip _clip)//����I������
    {
        BGMSource.clip = _clip;
        BGMSource.Play();
    }

}
