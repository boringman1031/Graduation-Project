using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportDialogTrigger : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO dialogEndEvent; // 對話結束事件
    [Header("遊戲物件")]
    public GameObject VollyBallGameCanva;
    bool isTalk = false;
   
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
            VollyBallGameCanva.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTalk)
        {
            DialogManager.Instance.StartDialog("Sport_meetSportman");
            isTalk = true;
        }
    }
}
