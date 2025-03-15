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
    public GameObject attackEffectPrefab;//攻擊特效
    protected override void Awake()
    {
        base.Awake();
        attackState = new BossAttackState();
        summonState = new BossSummonState();
    }
    public override void OnAttack()
    {
        base.OnAttack();
        if (currentHealth > maxHealth / 2)
        {
            Instantiate(attackEffectPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            //TODO:生成攻擊2特效
        }
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
            obj.Result.tag = "Minion";
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
            obj.Result.tag = "HeartMinion";
            Debug.Log("愛心生成成功！");
        }
        else
        {
            Debug.LogError("無法加載愛心預製體！");
        }
    }
}
