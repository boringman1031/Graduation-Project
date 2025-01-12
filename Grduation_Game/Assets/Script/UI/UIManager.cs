using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;

    [Header("®∆•Û∫ ≈•")]
    public CharacterEventSO healthEvenr;

    public void OnEnable()
    {
        healthEvenr.OnEventRaised += OnHealthEvent;
    }

    public void OnDisable()
    {
        healthEvenr.OnEventRaised -= OnHealthEvent;
    }

    public void OnHealthEvent(CharactorBase _charactor)
    {
        var persentage=_charactor.CurrentHealth/_charactor.MaxHealth;
        playerStatBar.OnHealthChange(persentage);
    }
}
