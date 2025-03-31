using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public VoidEventSO dialogEndEvent; // ��ܵ����ƥ�
    public GameObject QuestionGamePanel;
    public string Key;
    private bool isTalk = false;
    private void OnEnable()
    {
        dialogEndEvent.OnEventRaised += OnDialogEnd;
    }
    private void OnDisable()
    {
        dialogEndEvent.OnEventRaised -= OnDialogEnd;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogManager.Instance.StartDialog(Key);
            isTalk = true;         
        }
    }

    void OnDialogEnd()
    {
        if (isTalk)
        {
            QuestionGamePanel.SetActive(true);
        }
    }


}
