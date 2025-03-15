using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Chap1_Boss : BossBase
{
    [Header("�p�ǹw�s��")]
    public string minionPrefabAddress = "Goblin"; //�Ȯɨϥ�Goblin�N���p��
    public string HeartPrefabAddress = "Heart";

    [Header("�S��")]
    public GameObject attackEffectPrefab;//�����S��
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
            //TODO:�ͦ�����2�S��
        }
    }

    public override void OnSummon()
    {
        base.OnSummon();
        Addressables.InstantiateAsync(minionPrefabAddress, transform.position + Vector3.left * 2, Quaternion.identity)
            .Completed += OnMinionSpawned;
        Debug.Log("Boss �l�곽�����I");
    }

    private void OnMinionSpawned(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            obj.Result.tag = "Minion";
            Debug.Log("�l�곽�������\�I");
        }
        else
        {
            Debug.LogError("�L�k�[���p�ǹw�s��I");
        }
    }

    public override void SpawnHeartMinion()
    {
        base.SpawnHeartMinion();
        Addressables.InstantiateAsync(HeartPrefabAddress, transform.position + Vector3.left * 2, Quaternion.identity)
             .Completed += OnHeartSpawned;
        Debug.Log("�ڤ��n�A�R�A�F!!�I");
    }

    private void OnHeartSpawned(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            obj.Result.tag = "HeartMinion";
            Debug.Log("�R�ߥͦ����\�I");
        }
        else
        {
            Debug.LogError("�L�k�[���R�߹w�s��I");
        }
    }
}
