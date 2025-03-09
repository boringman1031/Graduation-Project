/*------------BY017------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("事件廣播")]
    public VoidEventSO onAllEnemiesDefeated; // 當所有敵人被擊敗時的事件
    [Header("事件監聽")]
    public EnemyEventSO OnEnemyDied; // 當敵人死亡時的事件

    private List<EnemyBase> enemies = new List<EnemyBase>(); // 儲存所有敵人

    private void Start()
    {
        // 獲取所有場景中的敵人並加入列表
        enemies.AddRange(FindObjectsOfType<EnemyBase>());    
    }

    private void OnEnable()
    {
        // 訂閱敵人死亡事件
        OnEnemyDied.OnEventRaised += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        // 取消訂閱敵人死亡事件
        OnEnemyDied.OnEventRaised -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath(EnemyBase enemy)
    {
        // 從列表中移除死亡的敵人
        enemies.Remove(enemy);
        Debug.Log("敵人死亡，目前敵人數量：" + enemies.Count);

        // 如果敵人數量為0且場景不是主頁面，廣播事件
        if (enemies.Count == 0)
        {
            Debug.Log("所有敵人已被擊敗，廣播事件通知 UIManager");
            onAllEnemiesDefeated.RaiseEvent();
        }
    }
}
