using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/EnemyEventSO")]
public class EnemyEventSO : ScriptableObject
{
   public UnityAction<EnemyBase> OnEventRaised;

    public void Raise(EnemyBase _enemy)
    {
        OnEventRaised?.Invoke(_enemy);
    }
}
