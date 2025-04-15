using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpawnEnemyManager : MonoBehaviour
{
    [Header("事件廣播")]
    public VoidEventSO onAllEnemiesDefeated;

    [Header("事件監聽")]
    public VoidEventSO dialogEndEvent; // ✅ 加入對話結束事件

    [Header("生成設定")]
    public AssetReference enemyReference;
    public Transform[] spawnPoints;
    public float spawnDelay = 1f;

    [Header("目標擊殺數量")]
    public int targetKillCount = 10;

    

    private int currentKillCount = 0;
    private List<GameObject> aliveEnemies = new();
    private bool spawning = false;

    private void OnEnable()
    {
        if (dialogEndEvent != null)
            dialogEndEvent.OnEventRaised += OnDialogEnd;
    }

    private void OnDisable()
    {
        if (dialogEndEvent != null)
            dialogEndEvent.OnEventRaised -= OnDialogEnd;
    }

    private void OnDialogEnd() // 對話結束時執行生成敵人
    {
        StartCoroutine(SpawnEnemiesLoop());
    }

    private IEnumerator SpawnEnemiesLoop()
    {
        spawning = true;

        while (currentKillCount < targetKillCount)
        {
            foreach (var point in spawnPoints)
            {
                if (currentKillCount >= targetKillCount) break;
                yield return StartCoroutine(SpawnEnemyAt(point.position));
                yield return new WaitForSeconds(spawnDelay);
            }

            yield return new WaitForSeconds(2f);
        }

        spawning = false;
    }

    private IEnumerator SpawnEnemyAt(Vector3 position)
    {
        AsyncOperationHandle<GameObject> handle = enemyReference.InstantiateAsync(position, Quaternion.identity);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject enemy = handle.Result;
            aliveEnemies.Add(enemy);

            EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
            if (enemyScript != null)
            {
                enemyScript.OnDeath += () => HandleEnemyDeath(enemy);
            }
        }
        else
        {
            Debug.LogError("敵人載入失敗！");
        }
    }

    private void HandleEnemyDeath(GameObject enemy)
    {
        if (aliveEnemies.Contains(enemy))
        {
            aliveEnemies.Remove(enemy);
            currentKillCount++;
            Debug.Log($"擊殺 {currentKillCount}/{targetKillCount}");

            if (currentKillCount >= targetKillCount)
            {
                Debug.Log("擊殺目標達成！");
                onAllEnemiesDefeated.RaiseEvent();
            }
        }
    }
}
