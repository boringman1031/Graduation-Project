using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionCanvas : MonoBehaviour
{
    [Header("事件監聽")]
    public TransitionEventSO transitionEvent;

    public Animator transitionAnimator;   

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
        //轉場效果觸發時，執行的動作
        if (transitionIn)
        {         
            transitionAnimator.SetTrigger("Start");           
        }
        else
        {
            transitionAnimator.SetTrigger("End");           
        }

    }

}
