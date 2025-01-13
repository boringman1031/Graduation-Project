using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineImpulseSource impulseSource;
    public VoidEventSO CameraShakeEvent;
    private CinemachineConfiner2D confiner2D; 

    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }
      private void OnEnable()
        {
            CameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
        }
       private void OnDisable()
        {
            CameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
        }

    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

    private void Start() 
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
