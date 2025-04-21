using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionGameStarter : MonoBehaviour
{
    [Header("�ƥ��ť")]
    public VoidEventSO onDialogEndEvent;

    [Header("�����")]
    public GameObject questionGameRoot;

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
    }
}
