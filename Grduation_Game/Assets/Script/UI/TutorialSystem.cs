using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSystem : MonoBehaviour
{
    [Header("��ť�ƥ�")]
    public VoidEventSO tutorialMoveEvent;
    public VoidEventSO tutorialJumpEvent;
    public VoidEventSO tutorialAttackEvent;
    public VoidEventSO tutorialBossSummonEvent;//Ĳ�oBoss�l��ƥ�о�
    public VoidEventSO tutorialBossAttackEvent;//Ĳ�oBoss�����ƥ�о�
    public VoidEventSO tutorialBossBrokenHeartEvent;//Ĳ�oBoss�߸H�ƥ�о�
    public VoidEventSO dialogEndEvent; // ��ܵ����ƥ�
    public SceneLoadedEventSO sceneLoadedEvent; // �s�W�G�����[�������ƥ�

    [Header("UI����")]
    public GameObject tutorialPanel;
    public Text tutorialText;

    private Dictionary<TutorialType, bool> tutorialShownDict; // �O���оǬO�_�w���
    private GameSceneSO currentScene; // �w�s��e����
    // ��ť�����J
    private void Awake()
    {
        // ��l�Ʀr��A�Ҧ��оǹw�]�����
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
            HideTutorialPanel(); // ���U F ������ñо� UI
        }
        if (Input.GetKeyDown(KeyCode.L) && currentScene.tutorialType == TutorialType.MusicGame)
        {
            HideTutorialPanel(); // ���ñо� UI
            ShowMusicGameTutorial2(); // �}�l���ֹC���о�
        }
    }
    // ���ñо� UI
    public void HideTutorialPanel()
    {
        tutorialPanel.SetActive(false);
    }
    private void OnEnable()
    {
        tutorialMoveEvent.OnEventRaised += ShowMoveTutorial;
        tutorialJumpEvent.OnEventRaised += ShowJumpTutorial;
        tutorialAttackEvent.OnEventRaised += ShowAttackTutorial;
        dialogEndEvent.OnEventRaised += OnDialogEnd; // �q�\��ܵ����ƥ�
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

        // �ˬd�оǬO�_�w��ܹL
        if (tutorialShownDict.TryGetValue(type, out bool shown) && !shown)
        {
            // �ھڼȦs�������Ѽ�Ĳ�o�о�
            switch (type)
            {
                case TutorialType.Move:
                    ShowMoveTutorial();
                    tutorialShownDict[type] = true; // �аO���w���
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
        currentScene = scene; // �Ȧs�����Ѽ�
    }
    // ���a��F�S�w�a�I����ܱо�
    private void ShowMoveTutorial()
    {
        tutorialText.text = "�ϥ� AD�� ���ʬݬݥ|�P";
        tutorialPanel.SetActive(true);
    }

    // ���ܵ�������ܱо�
    private void ShowJumpTutorial()
    {
        tutorialText.text = "�ϥ� �ť��� ���L�o�Ӭ]��";
        tutorialPanel.SetActive(true);
    }

    // ����o�s�ޯ����ܱо�
    private void ShowAttackTutorial()
    {
        tutorialText.text = "���U �ƹ����� ����";
        tutorialPanel.SetActive(true);
    }

    // ���ֹC���о�
    private void ShowMusicGameTutorial()
    {
        tutorialText.text = "���U L �}�l�C��";    
        tutorialPanel.SetActive(true);
    }
    private void ShowMusicGameTutorial2()
    {
        tutorialText.text = "�b�R�ߨ쥪����m�ɫ��U��LQWE";
        tutorialPanel.SetActive(true);
    }

    private void ShowTriviaGameTutorial()
    {
        tutorialText.text = "���L�h��o�����ܧa";
        tutorialPanel.SetActive(true);
    }

    private void ShowCleanEnemyTutorial()
    {
        tutorialText.text = "���ѩҦ��ĤH�a";
        tutorialPanel.SetActive(true);
    }

    private void ShowBossTutorial()
    {
        tutorialText.text = "���Irene";
        tutorialPanel.SetActive(true);
    }

    private void ShowBossSummonTutorial()
    {
        tutorialText.text = "Irene�l��F�������A���ѥL��";
        tutorialPanel.SetActive(true);
    }

    private void ShowBossAttackTutorial()
    {
        tutorialText.text = "�p�ߧ���!!!!!";
        tutorialPanel.SetActive(true);
    }

    private void ShowBosBrokenHeartTutorial()
    {
        tutorialText.text = "�A�}���F�A���o���R���H";
        tutorialPanel.SetActive(true);
    }
    private void ShowClassTutorial()
    {
        tutorialText.text = "���U���d���ö}��¾�~���";
        tutorialPanel.SetActive(true);
    }
}
