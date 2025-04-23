using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogBox : MonoBehaviour
{
    public string dialogKey; // 第二段對話的 key，例如 "BeforeFight"
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player")) return;
        triggered = true;

        Debug.Log($"🎬 玩家觸發對話 {dialogKey}");
        DialogManager.Instance.StartDialog(dialogKey); // 播放第二段對話
    }
}
