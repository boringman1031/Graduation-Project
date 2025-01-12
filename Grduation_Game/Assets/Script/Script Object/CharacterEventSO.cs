/*--------------bY017-------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Event/CharacterEventSO")]
public class CharacterEventSO : ScriptableObject
{
    /// <summary>
    /// 事件觸發時調用的委託。
    /// </summary>
    public UnityAction<CharactorBase> OnEventRaised;
    /// <summary>
    /// 觸發事件並調用所有已註冊的委託。
    /// </summary>
    /// <param name="_charactor">事件觸發時傳遞的角色對象。</param>
    public void Raise(CharactorBase _charactor)
    {
        OnEventRaised?.Invoke(_charactor);
    }
}
