using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
public class CameraController : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadedEvent;//場景加載完成事件
    public CameraShakeEventSO CameraShakeEvent; // 換成新版

    private CinemachineConfiner2D confiner2D;//相機邊界
    public CinemachineImpulseSource impulseSource;//相機震動


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

    private void OnCameraShakeEvent(float amplitude, float frequency, float decayTime)//相機震動事件處裡
    {
        var def = impulseSource.m_ImpulseDefinition;
        def.m_AmplitudeGain = amplitude;
        def.m_FrequencyGain = frequency;
        def.m_TimeEnvelope.m_AttackTime = 0.05f;
        def.m_TimeEnvelope.m_SustainTime = 0f;
        def.m_TimeEnvelope.m_DecayTime = decayTime;

        impulseSource.GenerateImpulse();
    }   
    private void OnAfterSceneLoadedEvent()//場景加載完成事件處裡
    {
        GetNewCameraBound(); //場景更換時重新取得邊界
    }
   
    public void GetNewCameraBound()
    {
        var obj = GameObject.FindGameObjectWithTag("Bound");
        if (obj == null)
            return;
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache();//重新計算邊界
    }
}
