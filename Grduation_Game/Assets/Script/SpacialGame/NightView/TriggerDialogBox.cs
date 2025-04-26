using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogBox : MonoBehaviour
{
    public string dialogKey; // 這段對話 key
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player")) return;
        triggered = true;

        Debug.Log($"🎬 玩家觸發對話 {dialogKey}");
        DialogManager.Instance.StartDialog(dialogKey); // 只播放對話就好，不要管敵人
        FindAnyObjectByType<NightViewEnemyManager>().ReadyToSpawnEnemy();
    }
}
