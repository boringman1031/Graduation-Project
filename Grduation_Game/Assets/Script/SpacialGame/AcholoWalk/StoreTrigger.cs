using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StoreTrigger : MonoBehaviour
{
    [Header("事件廣播")]
    public VoidEventSO onAcoholEnemyShowEvent;

    [Header("生成設定")]
    public int enemyCount = 5;
    public List<AssetReference> enemyPrefabs; // ✅ 改成 Addressable 引用
    public Transform[] spawnPoints;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || other.tag != "Player") return;

        triggered = true;
        onAcoholEnemyShowEvent.OnEventRaised();

        StartCoroutine(SpawnEnemiesAsync());
    }

    private IEnumerator SpawnEnemiesAsync()
    {
        List<GameObject> spawnedEnemies = new List<GameObject>();

        for (int i = 0; i < enemyCount; i++)
        {
            var prefabRef = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            var spawnPoint = (spawnPoints.Length > i) ? spawnPoints[i] : spawnPoints[Random.Range(0, spawnPoints.Length)];

            var handle = prefabRef.InstantiateAsync(spawnPoint.position, Quaternion.identity);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                spawnedEnemies.Add(handle.Result);
            }
            else
            {
                Debug.LogError($"❌ 生成敵人失敗：{prefabRef.RuntimeKey}");
            }
        }

        AlcoholManager alcoholManager = FindObjectOfType<AlcoholManager>();
        if (alcoholManager != null)
        {
            alcoholManager.SpawnEnemies(spawnedEnemies);
        }
        else
        {
            Debug.LogWarning("⚠️ 找不到 AlcoholManager，無法傳送敵人清單。");
        }
    }
}
