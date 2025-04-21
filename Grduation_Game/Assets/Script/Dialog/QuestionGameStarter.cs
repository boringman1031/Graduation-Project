using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionGameStarter : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO onDialogEndEvent;

    [Header("控制物件")]
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
