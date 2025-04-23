using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpawnEnemyManager : MonoBehaviour
{
    [Header("事件廣播")]
    public VoidEventSO onAllEnemiesDefeated;

    [Header("事件監聽")]
    public VoidEventSO dialogEndEvent; // ✅ 加入對話結束事件

    [Header("生成設定")]
    public List<AssetReference> enemyReferences; // ✅ 改為可以存放多種敵人的 List
    public Transform[] spawnPoints;
    public float spawnDelay = 1f;

    [Header("目標擊殺數量")]
    public int targetKillCount = 10;

    [Header("UI顯示")]
    public Text killCountText;


    private int currentKillCount = 0;
    private List<GameObject> aliveEnemies = new();
    private bool spawning = false;

    private void OnEnable()
    {
        if (dialogEndEvent != null)
            dialogEndEvent.OnEventRaised += OnDialogEnd;
        UpdateKillCountUI(); // ✅ 更新UI
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
        if (enemyReferences == null || enemyReferences.Count == 0)
        {
            Debug.LogError("未指定任何敵人 AssetReference！");
            yield break;
        }

        // ✅ 隨機挑選一個敵人 prefab
        int randomIndex = Random.Range(0, enemyReferences.Count);
        AssetReference selectedEnemy = enemyReferences[randomIndex];

        AsyncOperationHandle<GameObject> handle = selectedEnemy.InstantiateAsync(position, Quaternion.identity);
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

    private void UpdateKillCountUI()
    {
        if (killCountText != null)
        {
            targetKillCount = Mathf.Max(0, targetKillCount - currentKillCount);
            killCountText.text = $"還需擊敗:{targetKillCount}名敵人";
        }
    }

    private void ClearAllRemainingEnemies()
    {
        foreach (var enemy in aliveEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        aliveEnemies.Clear();
    }
    private void HandleEnemyDeath(GameObject enemy)
    {
        if (aliveEnemies.Contains(enemy))
        {
            aliveEnemies.Remove(enemy);
            currentKillCount++;
            Debug.Log($"擊殺 {currentKillCount}/{targetKillCount}");

            UpdateKillCountUI();

            if (currentKillCount >= targetKillCount)
            {
                Debug.Log("擊殺目標達成！");
                StopAllCoroutines(); // ✅ 停止生成
                ClearAllRemainingEnemies(); // ✅ 清除敵人
                onAllEnemiesDefeated.RaiseEvent();
            }
        }
    }
}
