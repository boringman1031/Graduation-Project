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
    public VoidEventSO tutorialBossSummonEvent;
    public VoidEventSO tutorialBoss2SummonEvent;
    public VoidEventSO tutorialBossAttackEvent;
    public VoidEventSO tutorialBossBrokenHeartEvent;
    public VoidEventSO dialogEndEvent;
    public SceneLoadedEventSO sceneLoadedEvent;
    public VoidEventSO unlockSkillEvent;

    [Header("UI元件")]
    public GameObject tutorialPanel;
    public Text tutorialText;

    private Dictionary<TutorialType, bool> tutorialShownDict;
    private GameSceneSO currentScene;

    private TutorialType currentTutorialType = TutorialType.None;
    private Coroutine autoHideCoroutine;

    private void Awake()
    {
        tutorialShownDict = new Dictionary<TutorialType, bool>();
        foreach (TutorialType type in System.Enum.GetValues(typeof(TutorialType)))
        {
            tutorialShownDict[type] = false;
        }
    }

    private void OnEnable()
    {
        tutorialMoveEvent.OnEventRaised += ShowMoveTutorial;
        tutorialJumpEvent.OnEventRaised += ShowJumpTutorial;
        tutorialAttackEvent.OnEventRaised += ShowAttackTutorial;
        dialogEndEvent.OnEventRaised += OnDialogEnd;
        sceneLoadedEvent.OnSceneLoaded += OnSceneLoaded;
        tutorialBossSummonEvent.OnEventRaised += ShowBossSummonTutorial;
        tutorialBoss2SummonEvent.OnEventRaised += ShowBoss2SummonTutorial;
        tutorialBossAttackEvent.OnEventRaised += ShowBossAttackTutorial;
        tutorialBossBrokenHeartEvent.OnEventRaised += ShowBosBrokenHeartTutorial;
        unlockSkillEvent.OnEventRaised += ShowUnlockSkillTutorial;
    }

    private void OnDisable()
    {
        tutorialMoveEvent.OnEventRaised -= ShowMoveTutorial;
        tutorialJumpEvent.OnEventRaised -= ShowJumpTutorial;
        tutorialAttackEvent.OnEventRaised -= ShowAttackTutorial;
        dialogEndEvent.OnEventRaised -= OnDialogEnd;
        sceneLoadedEvent.OnSceneLoaded -= OnSceneLoaded;
        tutorialBossSummonEvent.OnEventRaised -= ShowBossSummonTutorial;
        tutorialBoss2SummonEvent.OnEventRaised -= ShowBoss2SummonTutorial;
        tutorialBossAttackEvent.OnEventRaised -= ShowBossAttackTutorial;
        tutorialBossBrokenHeartEvent.OnEventRaised -= ShowBosBrokenHeartTutorial;
        unlockSkillEvent.OnEventRaised -= ShowUnlockSkillTutorial;
    }

    private void Update()
    {
        if (!tutorialPanel.activeSelf) return;

        switch (currentTutorialType)
        {
            case TutorialType.Move:
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                    HideTutorialPanel();
                break;
            case TutorialType.Jump:
                if (Input.GetKeyDown(KeyCode.Space))
                    HideTutorialPanel();
                break;
            case TutorialType.Attack:
                if (Input.GetKeyDown(KeyCode.Mouse1))
                    HideTutorialPanel();
                break;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            HideTutorialPanel();
            currentTutorialType = TutorialType.None;
        }

        if (Input.GetKeyDown(KeyCode.L) && currentScene.tutorialType == TutorialType.MusicGame)
        {
            HideTutorialPanel();
            ShowMusicGameTutorial2();
        }
    }

    public void HideTutorialPanel()
    {
        tutorialPanel.SetActive(false);
        currentTutorialType = TutorialType.None;

        if (autoHideCoroutine != null)
        {
            StopCoroutine(autoHideCoroutine);
            autoHideCoroutine = null;
        }
    }

    private void OnDialogEnd()
    {
        if (currentScene == null) return;

        TutorialType type = currentScene.tutorialType;

        if (tutorialShownDict.TryGetValue(type, out bool shown) && !shown)
        {
            switch (type)
            {
                case TutorialType.Move:
                    ShowMoveTutorial();
                    break;
                case TutorialType.Jump:
                    ShowJumpTutorial();
                    break;
                case TutorialType.Attack:
                    ShowAttackTutorial();
                    break;
                case TutorialType.MusicGame:
                    ShowMusicGameTutorial();
                    break;
                case TutorialType.TriviaGame:
                    ShowTriviaGameTutorial();
                    break;
                case TutorialType.CleanEnemy:
                    ShowCleanEnemyTutorial();
                    break;
                case TutorialType.Boss:
                    ShowBossTutorial();
                    break;
                case TutorialType.SkillAndClass:
                    ShowClassTutorial();
                    break;
                case TutorialType.UnlockSkill:
                    ShowUnlockSkillTutorial();
                    break;
            }

            tutorialShownDict[type] = true;
        }
    }

    private void OnSceneLoaded(GameSceneSO scene)
    {
        currentScene = scene;
    }

    private void ShowTutorial(string text, float autoHideTime = -1f)
    {
        tutorialText.text = text;
        tutorialPanel.SetActive(true);

        if (autoHideCoroutine != null)
            StopCoroutine(autoHideCoroutine);

        if (autoHideTime > 0)
            autoHideCoroutine = StartCoroutine(AutoHideTutorialAfterSeconds(autoHideTime));
    }

    private IEnumerator AutoHideTutorialAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideTutorialPanel();
    }

    private void ShowMoveTutorial()
    {
        currentTutorialType = TutorialType.Move;
        ShowTutorial("使用 AD鍵 移動看看四周");
    }

    private void ShowJumpTutorial()
    {
        currentTutorialType = TutorialType.Jump;
        ShowTutorial("使用 空白鍵 跳過這個柵欄");
    }

    private void ShowAttackTutorial()
    {
        currentTutorialType = TutorialType.Attack;
        ShowTutorial("按下 滑鼠右鍵 攻擊");
    }

    private void ShowMusicGameTutorial()
    {
        currentTutorialType = TutorialType.MusicGame;
        ShowTutorial("按下 L 開始遊戲");
    }

    private void ShowMusicGameTutorial2()
    {
        currentTutorialType = TutorialType.MusicGame;
        ShowTutorial("在愛心到左側位置時按下鍵盤ZXC");
    }

    private void ShowTriviaGameTutorial()
    {
        currentTutorialType = TutorialType.TriviaGame;
        ShowTutorial("走過去跟她說說話吧");
    }

    private void ShowCleanEnemyTutorial()
    {
        currentTutorialType = TutorialType.CleanEnemy;
        ShowTutorial("擊敗所有敵人吧");
    }

    private void ShowBossTutorial()
    {
        currentTutorialType = TutorialType.Boss;
        ShowTutorial("找到她");
    }

    private void ShowBossSummonTutorial()
    {
        currentTutorialType = TutorialType.Boss;
        ShowTutorial("Irene召喚了魚塘的魚，擊敗他們");
    }

    private void ShowBoss2SummonTutorial()
    {
        currentTutorialType = TutorialType.Boss;
        ShowTutorial("葳葳召喚了她的乾哥們，擊敗他們");
    }

    private void ShowBossAttackTutorial()
    {
        currentTutorialType = TutorialType.Boss;
        ShowTutorial("小心攻擊!!!!!");
    }

    private void ShowBosBrokenHeartTutorial()
    {
        currentTutorialType = TutorialType.Boss;
        ShowTutorial("你破防了，把對她的愛擊碎");
    }

    private void ShowClassTutorial()
    {
        currentTutorialType = TutorialType.SkillAndClass;
        ShowTutorial("按下衣櫃按鈕開啟職業選單");
    }

    private void ShowUnlockSkillTutorial()
    {
        if (currentScene == null) return;

        string skillName = currentScene.skillToUnlock;
        string className = currentScene.classToUnlock;

        currentTutorialType = TutorialType.UnlockSkill;

        // 若有解鎖職業
        if (!string.IsNullOrEmpty(className))
        {
            SkillManager.Instance.UnlockClass(className);

            // 嘗試找出這個職業
            ClassData unlockedClass = SkillManager.Instance.allClasses.Find(c => c.className == className);
            if (unlockedClass != null)
            {
                // 切換成新職業
                SkillManager.Instance.selectedClass = unlockedClass;

                // ✅ 自動裝備 ultimateSkill 到 R 鍵
                if (unlockedClass.ultimateSkill != null && unlockedClass.ultimateSkill.isUnlocked)
                {
                    SkillManager.Instance.EquipSkill(unlockedClass.ultimateSkill, 3);
                }
            }

            // ✅ 顯示提示
            ShowTutorial($"你解鎖了新職業：{className}\n大招已自動裝備！", 3f);
        }

        // 若有解鎖一般技能
        if (!string.IsNullOrEmpty(skillName))
        {
            SkillManager.Instance.UnlockSkill(skillName);

            // 嘗試找出這個技能
            SkillData unlockedSkill = SkillManager.Instance.allSkills.Find(s => s.skillName == skillName);
            if (unlockedSkill != null)
            {
                // ✅ 自動裝到 Q/W/E 任一個空位
                for (int i = 0; i < 3; i++)
                {
                    if (SkillManager.Instance.equippedSkills[i] == null)
                    {
                        SkillManager.Instance.EquipSkill(unlockedSkill, i);
                        break;
                    }
                }
            }

            ShowTutorial($"你解鎖了新技能：{skillName}\n已自動裝備在快捷鍵！", 3f);
        }

        // ✅ 更新技能 UI 和玩家大招資料
        FindObjectOfType<PlayerController>()?.UpdateUltimateSkill();
        FindObjectOfType<SkillUIController>()?.RefreshSkillIcons();
    }

}
