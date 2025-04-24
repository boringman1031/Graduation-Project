using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTrigger : MonoBehaviour
{
    public int enemyCount = 5;
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || other.tag != "Player") return;
        triggered = true;

        List<GameObject> spawnedEnemies = new List<GameObject>();

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Transform spawnPoint = spawnPoints.Length > i ? spawnPoints[i] : spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            spawnedEnemies.Add(enemy);
        }

        FindObjectOfType<AlcoholManager>()?.SpawnEnemies(spawnedEnemies);
    }
}
