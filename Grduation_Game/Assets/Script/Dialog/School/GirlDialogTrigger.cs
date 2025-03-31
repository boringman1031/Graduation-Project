using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlDialogTrigger : MonoBehaviour
{
    bool isTalk = false;

    public GameObject Enemy;
    public VoidEventSO dialogEndEvent; // ��ܵ����ƥ�
    public VoidEventSO OnEnemiesActivateEvent; // �j�wĲ�o�ĤH��ʪ��ƥ�
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
            OnEnemiesActivateEvent.RaiseEvent(); // Ĳ�o�ĤH��ʨƥ�
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
