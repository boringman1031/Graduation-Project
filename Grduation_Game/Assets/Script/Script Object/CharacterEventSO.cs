/*--------------bY017-------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Event/CharacterEventSO")]
public class CharacterEventSO : ScriptableObject
{
    /// <summary>
    /// �ƥ�Ĳ�o�ɽեΪ��e�U�C
    /// </summary>
    public UnityAction<CharactorBase> OnEventRaised;
    /// <summary>
    /// Ĳ�o�ƥ�ýեΩҦ��w���U���e�U�C
    /// </summary>
    /// <param name="_charactor">�ƥ�Ĳ�o�ɶǻ��������H�C</param>
    public void Raise(CharactorBase _charactor)
    {
        OnEventRaised?.Invoke(_charactor);
    }
}
