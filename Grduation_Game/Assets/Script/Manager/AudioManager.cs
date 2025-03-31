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
        float dB = _amount > 0 ? Mathf.Log10(_amount) * 20 : -80f;
        audioMixer.SetFloat("MasterVolume", dB);
    }
    private void OnSetBGMVolume(float _amount)//�]�w�I�����֭��q
    {
        float dB = _amount > 0 ? Mathf.Log10(_amount) * 20 : -80f;
        audioMixer.SetFloat("BGMVolume", dB);
    }
    private void OnSetFXVolume(float _amount)//�]�w���ĭ��q
    {
        float dB = _amount > 0 ? Mathf.Log10(_amount) * 20 : -80f;
        audioMixer.SetFloat("FXVolume", dB);
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
