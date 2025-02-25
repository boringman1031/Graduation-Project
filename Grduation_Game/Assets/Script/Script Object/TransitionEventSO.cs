using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu( menuName = "Event/TransitionEventSO")]
public class TransitionEventSO : ScriptableObject
{
    public UnityAction<bool> OnEventRaised;
    public void TransitionIn( )//�J��
    {
       RaiseEvent(true);
    }

    public void TransitionOut( )//�X��
    {
        RaiseEvent(false);
    }

    public void RaiseEvent( bool transitionIn)
    { 
        OnEventRaised?.Invoke(transitionIn);
    }
}
