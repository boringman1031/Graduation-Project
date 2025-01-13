using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
public class CameraController : MonoBehaviour
{
    [Header("�ƥ��ť")]
    public VoidEventSO afterSceneLoadedEvent;//�����[�������ƥ�
    public VoidEventSO CameraShakeEvent;//�۾��_�ʨƥ�

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

    private void OnCameraShakeEvent()//�۾��_�ʨƥ�B��
    {
        impulseSource.GenerateImpulse();
    }

    private void OnAfterSceneLoadedEvent()//�����[�������ƥ�B��
    {
        GetNewCameraBound(); //�����󴫮ɭ��s���o���
    }
   
    public void GetNewCameraBound()
    {
        var obj = GameObject.FindGameObjectWithTag("Bound");
        if (obj != null)
            return;
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache();//���s�p�����
    }
}
