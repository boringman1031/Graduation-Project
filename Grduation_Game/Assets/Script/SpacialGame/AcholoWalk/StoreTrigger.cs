using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTrigger : MonoBehaviour
{
    public int enemyCount = 5;
    public GameObject[] enemyPrefabs; // ✅ 改成可放多種敵人
    public Transform[] spawnPoints;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || other.tag != "Player") return;
        triggered = true;

        for (int i = 0; i < enemyCount; i++)
        {
            // ✅ 從陣列中隨機選擇一種敵人
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // 如果 spawnPoints 不夠，隨機挑一個點
            Transform spawnPoint = spawnPoints.Length > i ? spawnPoints[i] : spawnPoints[Random.Range(0, spawnPoints.Length)];

            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    
        FindObjectOfType<AlcoholManager>()?.TriggerStore();
    }
}
