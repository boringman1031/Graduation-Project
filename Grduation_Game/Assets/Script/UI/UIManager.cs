using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;

    [Header("�ƥ��ť")]
    public CharacterEventSO healthEvenr;
    public SceneLoadEventSO unloadedSceneEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;
    public FloatEventSO syncMasterVolumeEvent;//�P�B�D���q�ƥ�
    public FloatEventSO syncBGMVolumeEvent;
    public FloatEventSO syncFXVolumeEvent;

    [Header("�s���ƥ�")]
    public VoidEventSO pasueEvent;
  
    [Header("�ե�")]
    public GameObject GameOverPanel;
    public GameObject restartButton;
    public GameObject GameInfoPanel;//�C����T���O
    public GameObject GameStatPanel;//�C���i�׭��O
    public GameObject GameSettingPanel;//�C���]�w���O
    public Button GameInfoButton;
    public Button GameStatButton;//�C���i�׫��s
    public Button GameSettingButton;//�C���]�w���s
    public Slider MasterSlider;//�D���q
    public Slider BGMSlider;//�I�����֭��q
    public Slider FXSlider;//���ĭ��q

    public void Awake()
    {
        GameInfoButton.onClick.AddListener(ToggleGameInfoPanel);
        GameStatButton.onClick.AddListener(ToggleGameStatPanel);
        GameSettingButton.onClick.AddListener(ToggleGameSettingPanel);
    }

    public void OnEnable()
    {
        healthEvenr.OnEventRaised += OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent += OnLoadSceneEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;//Ū���C���i�רƥ�
        gameOverEvent.OnEventRaised += OnGameOverEvent;//�C�������ƥ�
        backToMenuEvent.OnEventRaised +=OnLoadDataEvent;//��^�D���ƥ�
        syncMasterVolumeEvent.OnEventRaised += OnSyncMasterVolumeEvent;
        syncBGMVolumeEvent.OnEventRaised += OnSyncBGMVolumeEvent;
        syncFXVolumeEvent.OnEventRaised += OnSyncFXVolumeEvent;
    }

    public void OnDisable()
    {
        healthEvenr.OnEventRaised -= OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent -= OnLoadSceneEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
        syncMasterVolumeEvent.OnEventRaised -= OnSyncMasterVolumeEvent;
        syncBGMVolumeEvent.OnEventRaised -= OnSyncBGMVolumeEvent;
        syncFXVolumeEvent.OnEventRaised -= OnSyncFXVolumeEvent;
    }

    private void ToggleGameInfoPanel()//�}�ҹC����T���O
    {
        if(GameInfoPanel.activeInHierarchy)
        {
            GameInfoPanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pasueEvent.RaiseEvent();
            GameInfoPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void ToggleGameStatPanel()//�}�ҹC���i�׭��O    
    {
       GameStatPanel.SetActive(true);
    }
    private void ToggleGameSettingPanel()//�}�ҹC���]�w���O
    {         
            GameSettingPanel.SetActive(true);      
    }

    private void OnSyncMasterVolumeEvent(float _amount)//�P�B�D���q
    {
        MasterSlider.value = (_amount + 80) / 100;
    }
    private void OnSyncBGMVolumeEvent(float _amount)//�P�B�I�����֭��q
    {
        BGMSlider.value = (_amount + 80) / 100;
    }
    private void OnSyncFXVolumeEvent(float _amount)//�P�B���ĭ��q
    {
    FXSlider.value = (_amount + 80) / 100;
    }

   
    public void OnHealthEvent(CharactorBase _charactor)//��q�ܤƨƥ�
    {
        var persentage=_charactor.CurrentHealth/_charactor.MaxHealth;
        playerStatBar.OnHealthChange(persentage);
    }

    private void OnLoadSceneEvent(GameSceneSO _sceneToLoad, Vector3 arg1, bool arg2)//Ū�������ƥ�
    {
        var isMenu=_sceneToLoad.sceneType == SceneType.Menu;
        playerStatBar.gameObject.SetActive(!isMenu);
    }

    private void OnLoadDataEvent()//Ū���C���i�רƥ�
    {
       GameOverPanel.SetActive(false);
    }

    private void OnGameOverEvent()//�C�������ƥ�
    {
        GameOverPanel.SetActive(true);
       EventSystem.current.SetSelectedGameObject(restartButton);
    }

}
