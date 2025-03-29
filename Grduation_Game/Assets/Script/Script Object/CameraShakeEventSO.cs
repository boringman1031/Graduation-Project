using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/CameraShakeEventSO")]
public class CameraShakeEventSO : ScriptableObject
{
    public System.Action<float, float, float> OnEventRaised; // Amplitude, Frequency, Decay

    public void RaiseEvent(float amplitude, float frequency, float decay)
    {
        OnEventRaised?.Invoke(amplitude, frequency, decay);
    }
}
