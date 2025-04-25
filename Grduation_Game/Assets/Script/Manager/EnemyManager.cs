/*------------BY017------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("事件廣播")]
    public VoidEventSO onAllEnemiesDefeated;

    private List<GameObject> enemies = new List<GameObject>();
    private bool hasTriggeredDefeatedEvent = false; // ✅ 防止重複觸發

    private void OnEnable()
    {
        enemies.Clear();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        hasTriggeredDefeatedEvent = false; // ✅ 每次啟用時重設
        Debug.Log($"找到 {enemies.Count} 個敵人");
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void HandleEnemyDeath(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Debug.Log($"敵人死亡，剩餘敵人數量: {enemies.Count}");
        }

        if (enemies.Count == 0 && !hasTriggeredDefeatedEvent)
        {
            hasTriggeredDefeatedEvent = true; // ✅ 設定旗標，確保只觸發一次
            Debug.Log("所有敵人已被擊敗，廣播事件！");
            StartCoroutine(AllEnemyDefeated());
        }
    }

    private IEnumerator AllEnemyDefeated()
    {
        yield return new WaitForSeconds(2);
        onAllEnemiesDefeated.RaiseEvent();
    }
}
