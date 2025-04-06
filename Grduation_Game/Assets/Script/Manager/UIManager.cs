/*-------------------BY017-----------------*/
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
    public CharacterEventSO powerEvent;
    public SceneLoadEventSO unloadedSceneEvent;  
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;
    public VoidEventSO PortalEvent;//隨機場景加載事件
    public VoidEventSO openRandomCanvaEvent;//顯示隨機挑戰面板事件
    public VoidEventSO openGoHomeCanvaEvent;//顯示隨機挑戰面板事件
    public FloatEventSO syncMasterVolumeEvent;//同步主音量事件
    public FloatEventSO syncBGMVolumeEvent;
    public FloatEventSO syncFXVolumeEvent;
    public VoidEventSO goHomeEvent;

    [Header("廣播事件")]
    public VoidEventSO pasueEvent;
    public VoidEventSO loadRandomSceneEvent; //隨機場景加載事件
   
    [Header("面板組件")]
    public PlayerStatBar playerStatBar;//玩家狀態條(血條、能量條)
    public GameObject GameOverPanel;//遊戲結束面板
    public GameObject RandomChallengePanel;//隨機挑戰面板
    public GameObject GoHomePanel;//回家面板
    public GameObject GOToBossScenePanel;//進入Boss場景面板
    public GameObject GameInfoPanel;//遊戲資訊面板
    public GameObject GameSettingPanel;//遊戲設定面板
    public GameObject DialogPanel;//對話框

    [Header("按鈕組件")]
    public Button GoHomeButton;//回家按鈕
    public Button GoToLobbyButton;//回到大廳按鈕
    public Button GameInfoButton;//開啟遊戲資訊按鈕
    public Button closeGameInfoButton;//關閉遊戲資訊按鈕
    public Button ExitGameInfoButton;//退出遊戲資訊按鈕
    public Button RandomChallengeButton1;//隨機挑戰1按鈕
    public Button RandomChallengeButton2;
    public Button RandomChallengeButton3;
    

    [Header("音量控制組件")]
    public Slider MasterSlider;//主音量
    public Slider BGMSlider;//背景音樂音量
    public Slider FXSlider;//音效音量

    [Header("挑戰次數顯示")]
    public Image[] challengeLights; // 點亮用的燈（Image 陣列）
    private List<GameSceneSO> currentOptions;

    public void Awake()
    {
        GameInfoButton.onClick.AddListener(ToggleGameInfoPanel);
        closeGameInfoButton.onClick.AddListener(ToggleClsoeGameInfoPanel);
        ExitGameInfoButton.onClick.AddListener(ToggleExitGameEvent);
        RandomChallengeButton1.onClick.AddListener(() => ChooseScene(0));
        RandomChallengeButton2.onClick.AddListener(() => ChooseScene(1));
        RandomChallengeButton3.onClick.AddListener(() => ChooseScene(2));

    }

    public void OnEnable()
    {
        healthEvenr.OnEventRaised += OnHealthEvent;
        powerEvent.OnEventRaised += OnPowerEvent;
        unloadedSceneEvent.LoadRequestEvent += OnLoadSceneEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;//讀取遊戲進度事件
        gameOverEvent.OnEventRaised += OnGameOverEvent;//遊戲結束事件
        backToMenuEvent.OnEventRaised +=OnLoadDataEvent;//返回主選單事件
        openRandomCanvaEvent.OnEventRaised+=OnShowRandomPanelEvents;//顯示隨機挑戰面板事件
        openGoHomeCanvaEvent.OnEventRaised += OnOpenGoHomeCanvaEvents;//顯示回家的面板事件
        PortalEvent.OnEventRaised += OnShowGoToBossScenePanelEvent;//顯示進入Boss場景面板事件
        syncMasterVolumeEvent.OnEventRaised += OnSyncMasterVolumeEvent;
        syncBGMVolumeEvent.OnEventRaised += OnSyncBGMVolumeEvent;
        syncFXVolumeEvent.OnEventRaised += OnSyncFXVolumeEvent;
        goHomeEvent.OnEventRaised += CloseGoHomePanel;
    }

    public void OnDisable()
    {
        healthEvenr.OnEventRaised -= OnHealthEvent;
        powerEvent.OnEventRaised -= OnPowerEvent;
        unloadedSceneEvent.LoadRequestEvent -= OnLoadSceneEvent;      
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
        openRandomCanvaEvent.OnEventRaised -= OnShowRandomPanelEvents;
        openGoHomeCanvaEvent.OnEventRaised -= OnOpenGoHomeCanvaEvents;//顯示回家的面板事件
        PortalEvent.OnEventRaised -= OnShowGoToBossScenePanelEvent;
        syncMasterVolumeEvent.OnEventRaised -= OnSyncMasterVolumeEvent;
        syncBGMVolumeEvent.OnEventRaised -= OnSyncBGMVolumeEvent;
        syncFXVolumeEvent.OnEventRaised -= OnSyncFXVolumeEvent;
        goHomeEvent.OnEventRaised -= CloseGoHomePanel;
    }

    public void ShowRandomChallengeOptions(List<GameSceneSO> options)//顯示隨機挑戰選項
    {
        currentOptions = options;
        RandomChallengePanel.SetActive(true);

        // 設定每個按鈕的文字
        RandomChallengeButton1.GetComponentInChildren<Text>().text = options[0].displayName;
        RandomChallengeButton2.GetComponentInChildren<Text>().text = options[1].displayName;
        RandomChallengeButton3.GetComponentInChildren<Text>().text = options[2].displayName;

    }

    private void ChooseScene(int index)
    {
        if (index < currentOptions.Count)
        {
            FindObjectOfType<SceneLoader>().LoadChosenScene(currentOptions[index]);
            RandomChallengePanel.SetActive(false);
        }
    }
    public void UpdateChallengeCountUI(int count)//更新挑戰次數UI
    {
        for (int i = 0; i < challengeLights.Length; i++)
        {
            challengeLights[i].enabled = i < count;
        }
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

    private void ToggleClsoeGameInfoPanel()//關閉遊戲資訊面板
    {
        GameInfoPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void ToggleExitGameEvent()//退出遊戲
    {
        GameInfoPanel.SetActive(false);
        Time.timeScale = 1;
        backToMenuEvent.RaiseEvent();
    }
    private void OnSyncMasterVolumeEvent(float _amount)//同步主音量
    {
        MasterSlider.value = Mathf.Pow(10, _amount / 20);
    }
    private void OnSyncBGMVolumeEvent(float _amount)//同步背景音樂音量
    {
        BGMSlider.value = Mathf.Pow(10, _amount / 20);
    }
    private void OnSyncFXVolumeEvent(float _amount)//同步音效音量
    {
        FXSlider.value = Mathf.Pow(10, _amount / 20);
    }

   
    public void OnHealthEvent(CharactorBase _charactor)//血量變化事件
    {
        var persentage=_charactor.CurrentHealth/_charactor.MaxHealth;
        playerStatBar.OnHealthChange(persentage);
    }
    public void OnPowerEvent(CharactorBase _charactor)//能量變化事件
    {
        var persentage = _charactor.CurrentPower / _charactor.MaxPower;
        playerStatBar.OnPowerChange(persentage);
    }

    private void OnLoadSceneEvent(GameSceneSO _sceneToLoad, Vector3 arg1, bool arg2)//讀取場景事件判斷是否顯示玩家狀態條
    {
        var isMenu=_sceneToLoad.sceneName == SceneName.Menu;
        playerStatBar.gameObject.SetActive(!isMenu);
    }

    private void OnLoadDataEvent()//讀取遊戲進度事件判斷玩家死了沒
    {
       GameOverPanel.SetActive(false);
    }

    private void OnGameOverEvent()//遊戲結束事件
    {
        GameOverPanel.SetActive(true);     
    }

    private void OnShowRandomPanelEvents()//顯示隨機挑戰面板事件
    {
        // 如果 SceneLoader 有選項正在準備，從它那邊拿來用
        var loader = FindObjectOfType<SceneLoader>();

        if (loader != null && loader.selectedSceneChoices != null && loader.selectedSceneChoices.Count == 3)
        {
            ShowRandomChallengeOptions(loader.selectedSceneChoices);
        }
        else
        {
            Debug.LogWarning("SceneLoader 沒有提供三選一選項，無法顯示隨機挑戰面板！");
        }
    }
    private void OnOpenGoHomeCanvaEvents()
    {
        GoHomePanel.SetActive(true);
    }
    private void CloseGoHomePanel()
    {
        FindObjectOfType<PlayerController>().isDead = false;
        GameOverPanel.SetActive(false); // 關掉死亡面板
        GoHomePanel.SetActive(false); // 👈 關掉面板
    }
    private void OnShowGoToBossScenePanelEvent()
    {
        GOToBossScenePanel.SetActive(true);
    }   
}
