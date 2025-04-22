using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionGameStarter : MonoBehaviour
{
    [Header("廣播事件")]
    public VoidEventSO disablePlayerEvent;
    public VoidEventSO enablePlayerEvent;

    [Header("事件監聽")]
    public VoidEventSO onDialogEndEvent;

    [Header("控制物件")]
    public GameObject questionGameRoot;

    private void Start()
    {
        if (disablePlayerEvent != null)
            disablePlayerEvent.RaiseEvent(); // 對話開始時關掉 player
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
            enablePlayerEvent.RaiseEvent(); // 對話結束後還原 player
    }
}
