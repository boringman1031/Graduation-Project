/*-------------------BY017-----------------*/
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
    public VoidEventSO PortalEvent;//�H�������[���ƥ�
    public VoidEventSO openRandomCanvaEvent;//����H���D�ԭ��O�ƥ�
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
    public GameObject GOToBossScenePanel;//�i�JBoss�������O
    public GameObject GameInfoPanel;//�C����T���O
    public GameObject GameSettingPanel;//�C���]�w���O
    public GameObject DialogPanel;//��ܮ�

    [Header("���s�ե�")]
    public GameObject restartButton;
    public Button GameInfoButton;//�}�ҹC����T���s
    public Button closeGameInfoButton;//�����C����T���s
    public Button ExitGameInfoButton;//�h�X�C����T���s
    public Button RandomChallengeButton1;//�H���D��1���s

    [Header("���q����ե�")]
    public Slider MasterSlider;//�D���q
    public Slider BGMSlider;//�I�����֭��q
    public Slider FXSlider;//���ĭ��q

    public void Awake()
    {
        GameInfoButton.onClick.AddListener(ToggleGameInfoPanel);
        closeGameInfoButton.onClick.AddListener(ToggleClsoeGameInfoPanel);
        ExitGameInfoButton.onClick.AddListener(ToggleExitGameEvent);
        RandomChallengeButton1.onClick.AddListener(ToggleRandomChallengeButton);    
    }

    public void OnEnable()
    {
        healthEvenr.OnEventRaised += OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent += OnLoadSceneEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;//Ū���C���i�רƥ�
        gameOverEvent.OnEventRaised += OnGameOverEvent;//�C�������ƥ�
        backToMenuEvent.OnEventRaised +=OnLoadDataEvent;//��^�D���ƥ�
        openRandomCanvaEvent.OnEventRaised+=OnShowRandomPanelEvents;//����H���D�ԭ��O�ƥ�
        PortalEvent.OnEventRaised += OnShowGoToBossScenePanelEvent;//��ܶi�JBoss�������O�ƥ�
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
        openRandomCanvaEvent.OnEventRaised -= OnShowRandomPanelEvents;
        PortalEvent.OnEventRaised -= OnShowGoToBossScenePanelEvent;
        syncMasterVolumeEvent.OnEventRaised -= OnSyncMasterVolumeEvent;
        syncBGMVolumeEvent.OnEventRaised -= OnSyncBGMVolumeEvent;
        syncFXVolumeEvent.OnEventRaised -= OnSyncFXVolumeEvent;
    }

    private void ToggleRandomChallengeButton()//���U�H���D�ԫ��sĲ�o
    {
        loadRandomSceneEvent.OnEventRaised();
        RandomChallengePanel.SetActive(false);
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

    private void ToggleClsoeGameInfoPanel()//�����C����T���O
    {
        GameInfoPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void ToggleExitGameEvent()//�h�X�C��
    {
        GameInfoPanel.SetActive(false);
        Time.timeScale = 1;
        backToMenuEvent.RaiseEvent();
    }
    private void OnSyncMasterVolumeEvent(float _amount)//�P�B�D���q
    {
        MasterSlider.value = Mathf.Pow(10, _amount / 20);
    }
    private void OnSyncBGMVolumeEvent(float _amount)//�P�B�I�����֭��q
    {
        BGMSlider.value = Mathf.Pow(10, _amount / 20);
    }
    private void OnSyncFXVolumeEvent(float _amount)//�P�B���ĭ��q
    {
        FXSlider.value = Mathf.Pow(10, _amount / 20);
    }

   
    public void OnHealthEvent(CharactorBase _charactor)//��q�ܤƨƥ�
    {
        var persentage=_charactor.CurrentHealth/_charactor.MaxHealth;
        playerStatBar.OnHealthChange(persentage);
    }

    private void OnLoadSceneEvent(GameSceneSO _sceneToLoad, Vector3 arg1, bool arg2)//Ū�������ƥ�P�_�O�_��ܪ��a���A��
    {
        var isMenu=_sceneToLoad.sceneName == SceneName.Menu;
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

    private void OnShowRandomPanelEvents()//����H���D�ԭ��O�ƥ�
    {
        RandomChallengePanel.SetActive(true);
    }

    private void OnShowGoToBossScenePanelEvent()
    {
        GOToBossScenePanel.SetActive(true);
    }
}
