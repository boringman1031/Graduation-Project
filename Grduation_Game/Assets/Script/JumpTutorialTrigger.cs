using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTutorialTrigger : MonoBehaviour
{
    [Header("廣播")]
    public VoidEventSO tutorialJumpEvent; // 跳躍教學事件

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 檢查是否是玩家
        {
            tutorialJumpEvent.RaiseEvent(); // 觸發跳躍教學事件
        }
    }
}
