using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionCanvas : MonoBehaviour
{
    [Header("�ƥ��ť")]
    public TransitionEventSO transitionEvent;

    private void OnEnable()
    {
        transitionEvent.OnEventRaised += OnTransitionEvent;
    }

    private void OnDisable()
    {
        transitionEvent.OnEventRaised -= OnTransitionEvent;
    }

    private void OnTransitionEvent(bool transitionIn)
    {
        //����ĪGĲ�o�ɡA���檺�ʧ@
    }

}
