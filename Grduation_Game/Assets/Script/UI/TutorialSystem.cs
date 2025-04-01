using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSystem : MonoBehaviour
{
    [Header("監聽事件")]
    public VoidEventSO tutorialMoveEvent;
    public VoidEventSO tutorialJumpEvent;
    public VoidEventSO tutorialAttackEvent;
    public VoidEventSO tutorialBossSummonEvent;//觸發Boss召喚事件教學
    public VoidEventSO tutorialBossAttackEvent;//觸發Boss攻擊事件教學
    public VoidEventSO tutorialBossBrokenHeartEvent;//觸發Boss心碎事件教學
    public VoidEventSO dialogEndEvent; // 對話結束事件
    public SceneLoadedEventSO sceneLoadedEvent; // 新增：場景加載完成事件

    [Header("UI元件")]
    public GameObject tutorialPanel;
    public Text tutorialText;

    private Dictionary<TutorialType, bool> tutorialShownDict; // 記錄教學是否已顯示
    private GameSceneSO currentScene; // 緩存當前場景
    // 監聽按鍵輸入
    private void Awake()
    {
        // 初始化字典，所有教學預設未顯示
        tutorialShownDict = new Dictionary<TutorialType, bool>();
        foreach (TutorialType type in System.Enum.GetValues(typeof(TutorialType)))
        {
            tutorialShownDict[type] = false;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            HideTutorialPanel(); // 按下 F 鍵時隱藏教學 UI
        }
        if (Input.GetKeyDown(KeyCode.L) && currentScene.tutorialType == TutorialType.MusicGame)
        {
            HideTutorialPanel(); // 隱藏教學 UI
            ShowMusicGameTutorial2(); // 開始音樂遊戲教學
        }
    }
    // 隱藏教學 UI
    public void HideTutorialPanel()
    {
        tutorialPanel.SetActive(false);
    }
    private void OnEnable()
    {
        tutorialMoveEvent.OnEventRaised += ShowMoveTutorial;
        tutorialJumpEvent.OnEventRaised += ShowJumpTutorial;
        tutorialAttackEvent.OnEventRaised += ShowAttackTutorial;
        dialogEndEvent.OnEventRaised += OnDialogEnd; // 訂閱對話結束事件
        sceneLoadedEvent.OnSceneLoaded += OnSceneLoaded;
        tutorialBossSummonEvent.OnEventRaised += ShowBossSummonTutorial;
        tutorialBossAttackEvent.OnEventRaised += ShowBossAttackTutorial;
        tutorialBossBrokenHeartEvent.OnEventRaised += ShowBosBrokenHeartTutorial;
    }

    private void OnDisable()
    {
        tutorialMoveEvent.OnEventRaised -= ShowMoveTutorial;
        tutorialJumpEvent.OnEventRaised -= ShowJumpTutorial;
        tutorialAttackEvent.OnEventRaised -= ShowAttackTutorial;
        dialogEndEvent.OnEventRaised -= OnDialogEnd;
        sceneLoadedEvent.OnSceneLoaded -= OnSceneLoaded;
        tutorialBossSummonEvent.OnEventRaised -= ShowBossSummonTutorial;
        tutorialBossAttackEvent.OnEventRaised -= ShowBossAttackTutorial;
        tutorialBossBrokenHeartEvent.OnEventRaised -= ShowBosBrokenHeartTutorial;
    }
    void OnDialogEnd()
    {
        
        if (currentScene == null) return;

        TutorialType type = currentScene.tutorialType;

        // 檢查教學是否已顯示過
        if (tutorialShownDict.TryGetValue(type, out bool shown) && !shown)
        {
            // 根據暫存的場景參數觸發教學
            switch (type)
            {
                case TutorialType.Move:
                    ShowMoveTutorial();
                    tutorialShownDict[type] = true; // 標記為已顯示
                    break;
                case TutorialType.Jump:
                    ShowJumpTutorial();
                    tutorialShownDict[type] = true;
                    break;
                case TutorialType.Attack:
                    ShowAttackTutorial();
                    tutorialShownDict[type] = true;
                    break;
                case TutorialType.MusicGame:                 
                    ShowMusicGameTutorial();
                    tutorialShownDict[type] = true;
                    break;
                case  TutorialType.TriviaGame:
                    ShowTriviaGameTutorial();
                    tutorialShownDict[type] = true;
                    break;
                case TutorialType.CleanEnemy:
                    ShowCleanEnemyTutorial();
                    tutorialShownDict[type] = true;
                    break;
                case TutorialType.Boss:
                    ShowBossTutorial();
                    tutorialShownDict[type] = true;
                    break;
                case TutorialType.SkillAndClass:
                    ShowClassTutorial();
                    tutorialShownDict[type] = true;
                    break;
                case TutorialType.None:
                default:
                    break;
            }
        }
    }
    private void OnSceneLoaded(GameSceneSO scene)
    {
        currentScene = scene; // 暫存場景參數
    }
    // 當玩家到達特定地點時顯示教學
    private void ShowMoveTutorial()
    {
        tutorialText.text = "使用 AD鍵 移動看看四周";
        tutorialPanel.SetActive(true);
    }

    // 當對話結束時顯示教學
    private void ShowJumpTutorial()
    {
        tutorialText.text = "使用 空白鍵 跳過這個柵欄";
        tutorialPanel.SetActive(true);
    }

    // 當獲得新技能時顯示教學
    private void ShowAttackTutorial()
    {
        tutorialText.text = "按下 滑鼠左鍵 攻擊";
        tutorialPanel.SetActive(true);
    }

    // 音樂遊戲教學
    private void ShowMusicGameTutorial()
    {
        tutorialText.text = "按下 L 開始遊戲";    
        tutorialPanel.SetActive(true);
    }
    private void ShowMusicGameTutorial2()
    {
        tutorialText.text = "在愛心到左側位置時按下鍵盤QWE";
        tutorialPanel.SetActive(true);
    }

    private void ShowTriviaGameTutorial()
    {
        tutorialText.text = "走過去跟她說說話吧";
        tutorialPanel.SetActive(true);
    }

    private void ShowCleanEnemyTutorial()
    {
        tutorialText.text = "擊敗所有敵人吧";
        tutorialPanel.SetActive(true);
    }

    private void ShowBossTutorial()
    {
        tutorialText.text = "找到Irene";
        tutorialPanel.SetActive(true);
    }

    private void ShowBossSummonTutorial()
    {
        tutorialText.text = "Irene召喚了魚塘的魚，擊敗他們";
        tutorialPanel.SetActive(true);
    }

    private void ShowBossAttackTutorial()
    {
        tutorialText.text = "小心攻擊!!!!!";
        tutorialPanel.SetActive(true);
    }

    private void ShowBosBrokenHeartTutorial()
    {
        tutorialText.text = "你破防了，把對她的愛擊碎";
        tutorialPanel.SetActive(true);
    }
    private void ShowClassTutorial()
    {
        tutorialText.text = "按下衣櫃按紐開啟職業選單";
        tutorialPanel.SetActive(true);
    }
}
