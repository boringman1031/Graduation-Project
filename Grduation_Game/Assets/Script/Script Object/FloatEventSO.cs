using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu( menuName = "Event/FloatEventSO")]
public class FloatEventSO : ScriptableObject
{
    public UnityAction<float> OnEventRaised;
    public void RaiseEvent(float _amount)
    {
        OnEventRaised?.Invoke(_amount);
    }
}
