/*-------BY017-------*/
/* 這個腳本是用來初始化遊戲的，主要是用來生成玩家角色。 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;

    [Inject] private DiContainer container;

    [Inject]
    private void Construct(DiContainer injectedContainer)
    {
        container = injectedContainer;

        if (container == null)
        {
            Debug.LogError("DiContainer injection failed!");
        }
        else
        {
            Debug.Log("DiContainer injected successfully.");
        }
    }

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("PlayerPrefab is not set in the Inspector.");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPoint is not set in the Inspector.");
            return;
        }

        if (container == null)
        {
            Debug.LogError("DiContainer is not injected.");
            return;
        }

        container.InstantiatePrefab(playerPrefab, spawnPoint.position, Quaternion.identity, null);
    }
}
