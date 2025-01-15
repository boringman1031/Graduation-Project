using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
  
    [Header("事件監聽")]
    public CharacterEventSO healthEvenr;
    public SceneLoadEventSO unloadedSceneEvent;  
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;
    public FloatEventSO syncMasterVolumeEvent;//同步主音量事件
    public FloatEventSO syncBGMVolumeEvent;
    public FloatEventSO syncFXVolumeEvent;
  
    [Header("廣播事件")]
    public VoidEventSO pasueEvent;
    public VoidEventSO loadRandomSceneEvent; //隨機場景加載事件

    [Header("面板組件")]
    public PlayerStatBar playerStatBar;//玩家狀態條(血條、能量條)
    public GameObject GameOverPanel;//遊戲結束面板
    public GameObject RandomChallengePanel;//隨機挑戰面板
    public GameObject GameInfoPanel;//遊戲資訊面板
    public GameObject GameStatPanel;//遊戲進度面板
    public GameObject GameSettingPanel;//遊戲設定面板

    [Header("按鈕組件")]
    public GameObject restartButton;
    public Button GameInfoButton;
    public Button GameStatButton;//遊戲進度按鈕
    public Button GameSettingButton;//遊戲設定按鈕
    public Button RandomChallengeButton1;//隨機挑戰1按鈕
    public Button RandomChallengeButton2;//隨機挑戰2按鈕
    public Button RandomChallengeButton3;//隨機挑戰3按鈕

    [Header("音量控制組件")]
    public Slider MasterSlider;//主音量
    public Slider BGMSlider;//背景音樂音量
    public Slider FXSlider;//音效音量

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
        loadDataEvent.OnEventRaised += OnLoadDataEvent;//讀取遊戲進度事件
        gameOverEvent.OnEventRaised += OnGameOverEvent;//遊戲結束事件
        backToMenuEvent.OnEventRaised +=OnLoadDataEvent;//返回主選單事件
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

    private void ToggleGameInfoPanel()//開啟遊戲資訊面板
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

    private void ToggleGameStatPanel()//開啟遊戲進度面板    
    {
       GameStatPanel.SetActive(true);
    }
    private void ToggleGameSettingPanel()//開啟遊戲設定面板
    {         
            GameSettingPanel.SetActive(true);      
    }

    private void OnSyncMasterVolumeEvent(float _amount)//同步主音量
    {
        MasterSlider.value = (_amount + 80) / 100;
    }
    private void OnSyncBGMVolumeEvent(float _amount)//同步背景音樂音量
    {
        BGMSlider.value = (_amount + 80) / 100;
    }
    private void OnSyncFXVolumeEvent(float _amount)//同步音效音量
    {
    FXSlider.value = (_amount + 80) / 100;
    }

   
    public void OnHealthEvent(CharactorBase _charactor)//血量變化事件
    {
        var persentage=_charactor.CurrentHealth/_charactor.MaxHealth;
        playerStatBar.OnHealthChange(persentage);
    }

    private void OnLoadSceneEvent(GameSceneSO _sceneToLoad, Vector3 arg1, bool arg2)//讀取場景事件判斷是否顯示玩家狀態條
    {
        var isMenu=_sceneToLoad.sceneType == SceneType.Menu;
        playerStatBar.gameObject.SetActive(!isMenu);
    }

    private void OnLoadDataEvent()//讀取遊戲進度事件判斷玩家死了沒
    {
       GameOverPanel.SetActive(false);
    }

    private void OnGameOverEvent()//遊戲結束事件
    {
        GameOverPanel.SetActive(true);
       EventSystem.current.SetSelectedGameObject(restartButton);
    }

}
