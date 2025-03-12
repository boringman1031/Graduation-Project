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

    public GameObject tutorialPanel;
    public Text tutorialText;

    private void OnEnable()
    {
        tutorialMoveEvent.OnEventRaised += ShowMoveTutorial;
        tutorialJumpEvent.OnEventRaised += ShowJumpTutorial;
        tutorialAttackEvent.OnEventRaised += ShowAttackTutorial;
    }

    private void OnDisable()
    {
        tutorialMoveEvent.OnEventRaised -= ShowMoveTutorial;
        tutorialJumpEvent.OnEventRaised -= ShowJumpTutorial;
        tutorialAttackEvent.OnEventRaised -= ShowAttackTutorial;
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
