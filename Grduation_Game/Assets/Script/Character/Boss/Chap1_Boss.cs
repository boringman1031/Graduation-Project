using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Chap1_Boss : BossBase
{
    [Header("�p�ǹw�s��")]
    public string MinionPrefab="Goblin"; //�Ȯɨϥ�Goblin�N���p��
    public string HeartMinionPrefab = "Heart"; //�Ȯɨϥ�Heart�N���R�ߤp��

    [Header("�S��")]
    public Transform attackEffectSpawnPoint;//�����S�ĥͦ���m
    public Transform HeartEffectSpawnPoint;//�R�߯S�ĥͦ���m
    public GameObject AttackWarningEffect;//�����wĵ�S��
    public GameObject attackEffectPrefab;//�����S��
    protected override void Awake()
    {
        base.Awake();
        attackState = new BossAttackState();
        summonState = new BossSummonState();
        summonHeartState = new BossSummonHeartState();
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
        int minionCount = 5; // �]�w�n�ͦ����p�Ǽƶq
        float spacing = 2f; // �]�w�p�Ǥ��������Z
        for (int i = 0; i < minionCount; i++)
        {
            Vector3 spawnPosition = transform.position +new Vector3(i * spacing, 0, 0);
            Addressables.InstantiateAsync(MinionPrefab, spawnPosition, Quaternion.identity)
                .Completed += OnMinionSpawned;
        }      
    }

    private void OnMinionSpawned(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            obj.Result.tag = "Enemy";       
        }
        else
        {
            Debug.LogError("�L�k�[���p�ǹw�s��I");
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
            Debug.LogError("�L�k�[���R�߹w�s��I");
        }
    }
}
