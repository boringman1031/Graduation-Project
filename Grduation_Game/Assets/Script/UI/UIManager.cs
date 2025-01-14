using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;

    [Header("®∆•Û∫ ≈•")]
    public CharacterEventSO healthEvenr;
    public SceneLoadEventSO loadEvent;

    public void OnEnable()
    {
        healthEvenr.OnEventRaised += OnHealthEvent;
        loadEvent.LoadRequestEvent += OnLoadEvent;
    }

    public void OnDisable()
    {
        healthEvenr.OnEventRaised -= OnHealthEvent;
        loadEvent.LoadRequestEvent -= OnLoadEvent;
    }

    public void OnHealthEvent(CharactorBase _charactor)
    {
        var persentage=_charactor.CurrentHealth/_charactor.MaxHealth;
        playerStatBar.OnHealthChange(persentage);
    }

    private void OnLoadEvent(GameSceneSO _sceneToLoad, Vector3 arg1, bool arg2)
    {
        var isMenu=_sceneToLoad.sceneType == SceneType.Menu;
        playerStatBar.gameObject.SetActive(!isMenu);
    }
}
