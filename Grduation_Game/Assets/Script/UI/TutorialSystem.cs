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

    [Header("監聽事件")]
    public VoidEventSO dialogEndEvent; // 對話結束事件

    [Header("UI元件")]
    public GameObject tutorialPanel;
    public Text tutorialText;

    private bool hasShownMoveTutorial = false; // 標記是否已經顯示過移動教學

    // 監聽按鍵輸入
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            HideTutorialPanel(); // 按下 R 鍵時隱藏教學 UI
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
        if (!hasShownMoveTutorial) // 如果移動教學尚未顯示過
        {
            ShowMoveTutorial(); // 顯示移動教學
            hasShownMoveTutorial = true; // 標記為已顯示
        }
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
