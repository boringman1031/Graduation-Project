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
    public Transform attackEffectSpawnPoint;//�����S�ĥͦ���m
    public GameObject AttackWarningEffect;//�����wĵ�S��
    public GameObject attackEffectPrefab;//�����S��
    protected override void Awake()
    {
        base.Awake();
        attackState = new BossAttackState();
        summonState = new BossSummonState();
    }
   
    public void OnAttackEffect()//�b�ʵe�Y���q�ͦ������S��
    {
        int effectCount = 5; // ����S�Ī��ƶq
        float spacing = 20f; // ����S�Ĥ��������Z

        if (currentHealth > maxHealth / 2)
        {
            for (int i = -effectCount / 2; i <= effectCount / 2; i++)
            {
                Vector3 spawnPosition = attackEffectSpawnPoint.position + new Vector3(i * spacing, 0, 0);
                Instantiate(AttackWarningEffect, spawnPosition, Quaternion.identity);
                // �����ͦ������S��
                StartCoroutine(SpawnAttackEffectWithDelay(spawnPosition, 1.0f));
            }
            Debug.Log("�ͦ�����2�S��");
        }
        else
        {
            //TODO:�ͦ�����2�S��
            Debug.Log("�ͦ�����2�S��");
        }
    }
    private IEnumerator SpawnAttackEffectWithDelay(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay); // ���� delay ��
        Instantiate(attackEffectPrefab, position, Quaternion.identity);     
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
            obj.Result.tag = "Enemy";
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
            obj.Result.tag = "Heart";
            Debug.Log("�R�ߥͦ����\�I");
        }
        else
        {
            Debug.LogError("�L�k�[���R�߹w�s��I");
        }
    }
}
