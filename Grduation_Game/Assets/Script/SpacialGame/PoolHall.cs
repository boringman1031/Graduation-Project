using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class PoolHall : MonoBehaviour
{
    [Header("事件廣播")]
    public VoidEventSO onAllEnemiesDefeated;
    public VoidEventSO gotoBoss2Event;

    [Header("事件監聽")]
    public VoidEventSO dialogEndEvent;

    [Header("生成設定")]
    public AssetReference enemyReference;
    public Transform[] spawnPoints;
    public float spawnDelay = 1f;

    [Header("目標擊殺數量")]
    public int targetKillCount = 10;

    [Header("UI顯示")]
    public Text killCountText;

    [Header("對話設定")]
    public string storyDialogKey; // Boss 前劇情對話 key

    private int currentKillCount = 0;
    private List<GameObject> aliveEnemies = new();
    private bool spawning = false;
    private bool isWaitingForBossStory = false; // ✅ 用來判斷對話結束是否進入 Boss

    private void OnEnable()
    {
        dialogEndEvent.OnEventRaised += OnDialogEnd;
        UpdateKillCountUI();
    }

    private void OnDisable()
    {
        dialogEndEvent.OnEventRaised -= OnDialogEnd;
    }

    private void OnDialogEnd()
    {
        if (!isWaitingForBossStory)
        {
            // ✅ 播完開場對話 → 開始戰鬥
            StartCoroutine(SpawnEnemiesLoop());
        }
        else
        {
            // ✅ 播完 Boss 前對話 → 進入 Boss
            Debug.Log("✅ Boss 對話結束，切換 Boss 場景！");
            gotoBoss2Event.RaiseEvent();
        }
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

            UpdateKillCountUI();

            if (currentKillCount >= targetKillCount)
            {
                Debug.Log("🎯 擊殺目標達成，播放 Boss 劇情");
                StopAllCoroutines();
                ClearAllRemainingEnemies();
                StartCoroutine(PlayStoryDialogueBeforeBoss());
            }
        }
    }

    private void UpdateKillCountUI()
    {
        if (killCountText != null)
        {
            int remaining = Mathf.Max(0, targetKillCount - currentKillCount);
            killCountText.text = $"還需擊敗:{remaining}名敵人";
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

    private IEnumerator PlayStoryDialogueBeforeBoss()
    {
        yield return new WaitForSeconds(1f);
        isWaitingForBossStory = true;

        if (DialogManager.Instance != null)
        {
            DialogManager.Instance.StartDialog(storyDialogKey);
        }
        else
        {
            Debug.LogError("❌ 找不到 DialogManager！");
        }
    }
}

