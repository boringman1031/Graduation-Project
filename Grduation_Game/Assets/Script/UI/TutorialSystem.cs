using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSystem : MonoBehaviour
{
    [Header("廣播")]
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
}
