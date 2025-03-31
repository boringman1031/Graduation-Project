using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlDialogTrigger : MonoBehaviour
{
    bool isTalk = false;

    public GameObject Enemy;
    public VoidEventSO dialogEndEvent; // 對話結束事件
    public VoidEventSO OnEnemiesActivateEvent; // 綁定觸發敵人行動的事件
    public VoidEventSO tutorialAttackEvent;

    private void OnEnable()
    {
        dialogEndEvent.OnEventRaised += OnDialogEnd;
    }
    private void OnDisable()
    {
        dialogEndEvent.OnEventRaised -= OnDialogEnd;
    }
    void OnDialogEnd()
    {
        if (isTalk)
        {
            OnEnemiesActivateEvent.RaiseEvent(); // 觸發敵人行動事件
            tutorialAttackEvent.RaiseEvent();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTalk)
        {
            DialogManager.Instance.StartDialog("FirstScene_meetGirl");
            isTalk = true;
        }    
    }
}
