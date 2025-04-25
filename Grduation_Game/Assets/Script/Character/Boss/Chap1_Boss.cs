using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Chap1_Boss : BossBase
{

    [Header("小怪預製體")]
    public string MinionPrefab = "Goblin";
    public string HeartMinionPrefab = "Heart";

    [Header("特效")]
    public Transform attackEffectSpawnPoint;
    public Transform HeartEffectSpawnPoint;
    public Transform summonEnemyPoint;
    public GameObject AttackWarningEffect;
    public GameObject attackEffectPrefab1;
    public GameObject attackEffectPrefab2;

    [Header("BGM")]
    public AudioDefination audioDefination;

    [Header("UI 顯示")]
    public Text EnemyCount;

    // 小怪數量管理
    private int aliveEnemyCount = 0;
    private HashSet<GameObject> trackedEnemies = new HashSet<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        idleState = new BossIdelState();
        attackState = new BossAttackState();
        summonState = new BossSummonState();
        summonHeartState = new BossSummonHeartState();
    }

    public override void OnBossShow()
    {
        base.OnBossShow();
        DialogManager.Instance.StartDialog("Boss_Show");
        isTalk = true;
        audioDefination.PlayAudioClip();
    }

    public override void OnSummon()
    {
        base.OnSummon();
        int minionCount = 5;
        float minX = -53f;
        float maxX = 53f;

        for (int i = 0; i < minionCount; i++)
        {
            float randomX = Random.Range(minX, maxX);
            Vector3 spawnPosition = summonEnemyPoint.position + new Vector3(randomX, 0, 0);
            Addressables.InstantiateAsync(MinionPrefab, spawnPosition, Quaternion.identity)
                .Completed += OnMinionSpawned;
        }
    }

    private void OnMinionSpawned(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject enemy = obj.Result;
            enemy.tag = "Enemy";

            EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
            if (enemyBase != null && !trackedEnemies.Contains(enemy))
            {
                trackedEnemies.Add(enemy);
                aliveEnemyCount++;
                UpdateEnemyCountText();

                enemyBase.onEnemyDead += OnMinionDead;
            }
        }
        else
        {
            Debug.LogError("無法加載小怪預製體！");
        }
    }

    private void OnMinionDead(GameObject deadEnemy)
    {
        if (trackedEnemies.Contains(deadEnemy))
        {
            trackedEnemies.Remove(deadEnemy);
            aliveEnemyCount--;
            UpdateEnemyCountText();

            if (aliveEnemyCount <= 0)
            {
                Debug.Log("✅ 所有小怪已被擊敗！");
                // 👉 在這裡可以切換下一階段，例如：
                // SwitchState(BossState.SummonHeart);
            }
        }
    }

    private void UpdateEnemyCountText()
    {
        if (EnemyCount != null)
        {
            EnemyCount.text = $"還需擊敗：{aliveEnemyCount} 名敵人";
        }
    }

    public override void SpawnHeartMinion()
    {
        base.SpawnHeartMinion();
        Addressables.InstantiateAsync(HeartMinionPrefab, HeartEffectSpawnPoint.position + Vector3.left * 2, Quaternion.identity)
            .Completed += OnHeartSpawned;
    }

    private void OnHeartSpawned(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            obj.Result.tag = "Heart";
        }
        else
        {
            Debug.LogError("無法加載愛心預製體！");
        }
    }

    public void OnAttackEffect()
    {
        int effectCount = 8;
        float spacing = 20f;

        for (int i = -effectCount / 2; i <= effectCount / 2; i++)
        {
            Vector3 spawnPosition = attackEffectSpawnPoint.position + new Vector3(i * spacing, 0, 0);
            Instantiate(AttackWarningEffect, spawnPosition, Quaternion.identity);
            StartCoroutine(SpawnAttackEffectWithDelay(spawnPosition, 1.0f));
        }
    }

    private IEnumerator SpawnAttackEffectWithDelay(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(attackEffectPrefab1, position, Quaternion.identity);
    }
}
