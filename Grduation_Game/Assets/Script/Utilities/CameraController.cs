using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
public class CameraController : MonoBehaviour
{
    [Header("�ƥ��ť")]
    public VoidEventSO afterSceneLoadedEvent;//�����[�������ƥ�
    public CameraShakeEventSO CameraShakeEvent; // �����s��

    private CinemachineConfiner2D confiner2D;//�۾����
    public CinemachineImpulseSource impulseSource;//�۾��_��


    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }
    private void OnEnable()
    {
            CameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
            afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
    }
   
    private void OnDisable()
    {
            CameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
            afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
    }

    private void OnCameraShakeEvent(float amplitude, float frequency, float decayTime)//�۾��_�ʨƥ�B��
    {
        var def = impulseSource.m_ImpulseDefinition;
        def.m_AmplitudeGain = amplitude;
        def.m_FrequencyGain = frequency;
        def.m_TimeEnvelope.m_AttackTime = 0.05f;
        def.m_TimeEnvelope.m_SustainTime = 0f;
        def.m_TimeEnvelope.m_DecayTime = decayTime;

        impulseSource.GenerateImpulse();
    }   
    private void OnAfterSceneLoadedEvent()//�����[�������ƥ�B��
    {
        GetNewCameraBound(); //�����󴫮ɭ��s���o���
    }
   
    public void GetNewCameraBound()
    {
        var obj = GameObject.FindGameObjectWithTag("Bound");
        if (obj == null)
            return;
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache();//���s�p�����
    }
}
