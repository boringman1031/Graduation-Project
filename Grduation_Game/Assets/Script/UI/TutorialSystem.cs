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

    [Header("UI����")]
    public GameObject tutorialPanel;
    public Text tutorialText;

    private bool hasShownMoveTutorial = false; // �аO�O�_�w�g��ܹL���ʱо�

    // ��ť�����J
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            HideTutorialPanel(); // ���U R ������ñо� UI
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
    }

    private void OnDisable()
    {
        tutorialMoveEvent.OnEventRaised -= ShowMoveTutorial;
        tutorialJumpEvent.OnEventRaised -= ShowJumpTutorial;
        tutorialAttackEvent.OnEventRaised -= ShowAttackTutorial;
        dialogEndEvent.OnEventRaised += OnDialogEnd;
    }
    void OnDialogEnd()
    {
        if (!hasShownMoveTutorial) // �p�G���ʱоǩ|����ܹL
        {
            ShowMoveTutorial(); // ��ܲ��ʱо�
            hasShownMoveTutorial = true; // �аO���w���
        }
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
}
