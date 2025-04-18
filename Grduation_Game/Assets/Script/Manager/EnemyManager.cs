/*------------BY017------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("事件廣播")]
    public VoidEventSO onAllEnemiesDefeated; // 當所有敵人被擊敗時的事件

    private List<GameObject> enemies = new List<GameObject>(); // 儲存所有敵人

    private void OnEnable()
    {
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy")); // 找到所有敵人
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

        if (enemies.Count == 0)
        {
            Debug.Log("所有敵人已被擊敗，廣播事件！");
            StartCoroutine(AllEnemyDefeated());
        }
    }

    private IEnumerator  AllEnemyDefeated()
    {
        yield return new WaitForSeconds(2);
        onAllEnemiesDefeated.RaiseEvent();
    }
}
