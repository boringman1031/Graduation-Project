using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
public class CameraController : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadedEvent;//場景加載完成事件
    public VoidEventSO CameraShakeEvent;//相機震動事件

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

    private void OnCameraShakeEvent()//相機震動事件處裡
    {
        impulseSource.GenerateImpulse();
    }

    private void OnAfterSceneLoadedEvent()//場景加載完成事件處裡
    {
        GetNewCameraBound(); //場景更換時重新取得邊界
    }
   
    public void GetNewCameraBound()
    {
        var obj = GameObject.FindGameObjectWithTag("Bound");
        if (obj != null)
            return;
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache();//重新計算邊界
    }
}
