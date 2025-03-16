using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Chap1_Boss : BossBase
{
    [Header("小怪預製體")]
    public string minionPrefabAddress = "Goblin"; //暫時使用Goblin代替小怪
    public string HeartPrefabAddress = "Heart";

    [Header("特效")]
    public Transform attackEffectSpawnPoint;//攻擊特效生成位置
    public GameObject AttackWarningEffect;//攻擊預警特效
    public GameObject attackEffectPrefab;//攻擊特效
    protected override void Awake()
    {
        base.Awake();
        attackState = new BossAttackState();
        summonState = new BossSummonState();
    }
   
    public void OnAttackEffect()//在動畫某階段生成攻擊特效
    {
        int effectCount = 5; // 控制特效的數量
        float spacing = 20f; // 控制特效之間的間距

        if (currentHealth > maxHealth / 2)
        {
            for (int i = -effectCount / 2; i <= effectCount / 2; i++)
            {
                Vector3 spawnPosition = attackEffectSpawnPoint.position + new Vector3(i * spacing, 0, 0);
                Instantiate(AttackWarningEffect, spawnPosition, Quaternion.identity);
                // 延遲後生成攻擊特效
                StartCoroutine(SpawnAttackEffectWithDelay(spawnPosition, 1.0f));
            }
            Debug.Log("生成攻擊2特效");
        }
        else
        {
            //TODO:生成攻擊2特效
            Debug.Log("生成攻擊2特效");
        }
    }
    private IEnumerator SpawnAttackEffectWithDelay(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay); // 等待 delay 秒
        Instantiate(attackEffectPrefab, position, Quaternion.identity);     
    }
    public override void OnSummon()
    {
        base.OnSummon();
        Addressables.InstantiateAsync(minionPrefabAddress, transform.position + Vector3.left * 2, Quaternion.identity)
            .Completed += OnMinionSpawned;
        Debug.Log("Boss 召喚魚塘的魚！");
    }

    private void OnMinionSpawned(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            obj.Result.tag = "Enemy";
            Debug.Log("召喚魚塘的魚成功！");
        }
        else
        {
            Debug.LogError("無法加載小怪預製體！");
        }
    }

    public override void SpawnHeartMinion()
    {
        base.SpawnHeartMinion();
        Addressables.InstantiateAsync(HeartPrefabAddress, transform.position + Vector3.left * 2, Quaternion.identity)
             .Completed += OnHeartSpawned;
        Debug.Log("我不要再愛你了!!！");
    }

    private void OnHeartSpawned(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            obj.Result.tag = "Heart";
            Debug.Log("愛心生成成功！");
        }
        else
        {
            Debug.LogError("無法加載愛心預製體！");
        }
    }
}
