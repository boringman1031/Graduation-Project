using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
  
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
    public VoidEventSO loadRandomSceneEvent; //�H�������[���ƥ�

    [Header("���O�ե�")]
    public PlayerStatBar playerStatBar;//���a���A��(����B��q��)
    public GameObject GameOverPanel;//�C���������O
    public GameObject RandomChallengePanel;//�H���D�ԭ��O
    public GameObject GameInfoPanel;//�C����T���O
    public GameObject GameStatPanel;//�C���i�׭��O
    public GameObject GameSettingPanel;//�C���]�w���O

    [Header("���s�ե�")]
    public GameObject restartButton;
    public Button GameInfoButton;
    public Button GameStatButton;//�C���i�׫��s
    public Button GameSettingButton;//�C���]�w���s
    public Button RandomChallengeButton1;//�H���D��1���s
    public Button RandomChallengeButton2;//�H���D��2���s
    public Button RandomChallengeButton3;//�H���D��3���s

    [Header("���q����ե�")]
    public Slider MasterSlider;//�D���q
    public Slider BGMSlider;//�I�����֭��q
    public Slider FXSlider;//���ĭ��q

    public void Awake()
    {
        GameInfoButton.onClick.AddListener(ToggleGameInfoPanel);
        GameStatButton.onClick.AddListener(ToggleGameStatPanel);
        GameSettingButton.onClick.AddListener(ToggleGameSettingPanel);
        RandomChallengeButton1.onClick.AddListener(ToggleRandomChallengeButton);
        RandomChallengeButton2.onClick.AddListener(ToggleRandomChallengeButton);
        RandomChallengeButton3.onClick.AddListener(ToggleRandomChallengeButton);
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
    private void ToggleRandomChallengeButton()
    {
        loadRandomSceneEvent.OnEventRaised();
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

    private void OnLoadSceneEvent(GameSceneSO _sceneToLoad, Vector3 arg1, bool arg2)//Ū�������ƥ�P�_�O�_��ܪ��a���A��
    {
        var isMenu=_sceneToLoad.sceneType == SceneType.Menu;
        playerStatBar.gameObject.SetActive(!isMenu);
    }

    private void OnLoadDataEvent()//Ū���C���i�רƥ�P�_���a���F�S
    {
       GameOverPanel.SetActive(false);
    }

    private void OnGameOverEvent()//�C�������ƥ�
    {
        GameOverPanel.SetActive(true);
       EventSystem.current.SetSelectedGameObject(restartButton);
    }

}
