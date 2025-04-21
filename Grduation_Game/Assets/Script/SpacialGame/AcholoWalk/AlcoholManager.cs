using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlcoholManager : MonoBehaviour
{
    [Header("事件廣播")]
    public VoidEventSO onAllEnemiesDefeated; // 敵人全部被擊敗（模擬廣播）

    [Header("便利商店相關")]
    public int maxStoreCount = 3;
    private int currentStoreCount = 0;

    private void OnEnable()
    {
        currentStoreCount = 0;
    }

    public void TriggerStore()
    {
        currentStoreCount++;
        Debug.Log($"🏪 已觸發便利商店：{currentStoreCount}/{maxStoreCount}");

        if (currentStoreCount >= maxStoreCount)
        {
            Debug.Log("✅ 達成條件，觸發 AllEnemyDefeated()");
            StartCoroutine(AllEnemyDefeated());
        }
    }

    private IEnumerator AllEnemyDefeated()
    {
        yield return new WaitForSeconds(1.5f); // 可以讓動畫/音效有點空間
        onAllEnemiesDefeated?.RaiseEvent();
    }
}
