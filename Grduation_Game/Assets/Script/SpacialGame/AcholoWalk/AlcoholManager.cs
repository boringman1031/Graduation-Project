using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlcoholManager : MonoBehaviour
{
    [Header("事件廣播")]
    public VoidEventSO onAllEnemiesDefeated;

    [Header("便利商店相關")]
    public int maxWaveCount = 3;
    private int currentWave = 0;

    private List<GameObject> aliveEnemies = new();

    private void OnEnable()
    {
        currentWave = 0;
        aliveEnemies.Clear();
    }

    /// <summary>
    /// 每次 StoreTrigger 呼叫這個來註冊敵人
    /// </summary>
    public void SpawnEnemies(List<GameObject> enemies)
    {
        currentWave++;
        bool isFinalWave = currentWave == maxWaveCount;

        Debug.Log($"🧟‍♂️ 生成第 {currentWave} 波敵人，是否為最後一波：{isFinalWave}");

        aliveEnemies.Clear();

        foreach (var enemy in enemies)
        {
            aliveEnemies.Add(enemy);

            EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
            if (enemyBase != null)
            {
                enemyBase.onEnemyDead -= OnEnemyDead; // 先移除（防止重複註冊）
                enemyBase.onEnemyDead += OnEnemyDead;
            }
        }
    }

    private void OnEnemyDead(GameObject enemy)
    {
        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
        if (enemyBase != null)
        {
            enemyBase.onEnemyDead -= OnEnemyDead; // ✅ 先解除事件註冊
        }

        aliveEnemies.Remove(enemy);      
        if (aliveEnemies.Count == 0)
        {
            OnWaveCleared();
        }
    }

    private void OnWaveCleared()
    {
        Debug.Log($"✅ 第 {currentWave} 波敵人已清除！");

        if (currentWave >= maxWaveCount)
        {
            Debug.Log("🎉 最後一波敵人被擊敗，觸發結束事件！");
            StartCoroutine(AllEnemiesDefeated());
        }
    }

    private IEnumerator AllEnemiesDefeated()
    {
        yield return new WaitForSeconds(1.5f);
        onAllEnemiesDefeated?.RaiseEvent();
    }
}
