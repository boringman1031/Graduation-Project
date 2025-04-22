using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionGameStarter : MonoBehaviour
{
    [Header("�s���ƥ�")]
    public VoidEventSO disablePlayerEvent;
    public VoidEventSO enablePlayerEvent;

    [Header("�ƥ��ť")]
    public VoidEventSO onDialogEndEvent;

    [Header("�����")]
    public GameObject questionGameRoot;

    private void Start()
    {
        if (disablePlayerEvent != null)
            disablePlayerEvent.RaiseEvent(); // ��ܶ}�l������ player
    }
    private void OnEnable()
    {
        if (onDialogEndEvent != null)
            onDialogEndEvent.OnEventRaised += OnDialogEnd;
    }

    private void OnDisable()
    {
        if (onDialogEndEvent != null)
            onDialogEndEvent.OnEventRaised -= OnDialogEnd;
    }

    private void OnDialogEnd()
    {
        if (questionGameRoot != null)
            questionGameRoot.SetActive(true);

        if (enablePlayerEvent != null)
            enablePlayerEvent.RaiseEvent(); // ��ܵ������٭� player
    }
}
