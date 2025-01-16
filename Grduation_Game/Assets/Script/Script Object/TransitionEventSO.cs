using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu( menuName = "Event/TransitionEventSO")]
public class TransitionEventSO : ScriptableObject
{
    public UnityAction<bool> OnEventRaised;
    public void TransitionIn( )//入場
    {
       RaiseEvent(true);
    }

    public void TransitionOut( )//出場
    {
        RaiseEvent(false);
    }

    public void RaiseEvent( bool transitionIn)
    { 
        OnEventRaised?.Invoke(transitionIn);
    }
}
