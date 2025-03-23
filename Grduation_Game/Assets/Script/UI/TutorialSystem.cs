using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSystem : MonoBehaviour
{
    [Header("�s��")]
    public VoidEventSO tutorialMoveEvent;
    public VoidEventSO tutorialJumpEvent;
    public VoidEventSO tutorialAttackEvent;

    [Header("��ť�ƥ�")]
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            HideTutorialPanel(); // ���U R ������ñо� UI
        }
        if (Input.GetKeyDown(KeyCode.S) && currentScene.tutorialType == TutorialType.MusicGame)
        {
            HideTutorialPanel(); // ���U R ������ñо� UI
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
    }

    private void OnDisable()
    {
        tutorialMoveEvent.OnEventRaised -= ShowMoveTutorial;
        tutorialJumpEvent.OnEventRaised -= ShowJumpTutorial;
        tutorialAttackEvent.OnEventRaised -= ShowAttackTutorial;
        dialogEndEvent.OnEventRaised -= OnDialogEnd;
        sceneLoadedEvent.OnSceneLoaded -= OnSceneLoaded;
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
}
