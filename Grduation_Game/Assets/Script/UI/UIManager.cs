using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;

    [Header("事件監聽")]
    public CharacterEventSO healthEvenr;
    public SceneLoadEventSO unloadedSceneEvent;
    public VoidEventSO loadDataEvent;

    [Header("組件")]
    public GameObject GameOverPanel;
    public GameObject restartButton;

    public void OnEnable()
    {
        healthEvenr.OnEventRaised += OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent += OnLoadSceneEvent;
    }

    public void OnDisable()
    {
        healthEvenr.OnEventRaised -= OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent -= OnLoadSceneEvent;
    }

    public void OnHealthEvent(CharactorBase _charactor)
    {
        var persentage=_charactor.CurrentHealth/_charactor.MaxHealth;
        playerStatBar.OnHealthChange(persentage);
    }

    private void OnLoadSceneEvent(GameSceneSO _sceneToLoad, Vector3 arg1, bool arg2)
    {
        var isMenu=_sceneToLoad.sceneType == SceneType.Menu;
        playerStatBar.gameObject.SetActive(!isMenu);
    }
}
