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
    public Transform summonEnemyPoint;//�l��ĤH�ͦ���m
    public GameObject AttackWarningEffect;//�����wĵ�S��
    public GameObject attackEffectPrefab1;//�����S��
    public GameObject attackEffectPrefab2;//����2�S��

    [Header("BGM")]
    public AudioDefination audioDefination;//���֩w�q

    protected override void Awake()
    {
        base.Awake();
        idleState = new BossIdelState();
        attackState = new BossAttackState();
        summonState = new BossSummonState();
        summonHeartState = new BossSummonHeartState();
    }   
    public void OnAttackEffect()//�b�ʵe�Y���q�ͦ������S��
    {

        int effectCount = 5; // ����S�Ī��ƶq
        float spacing = 20f; // ����S�Ĥ��������Z
        for (int i = -effectCount / 2; i <= effectCount / 2; i++)
        {
            Vector3 spawnPosition = attackEffectSpawnPoint.position + new Vector3(i * spacing, 0, 0);
            Instantiate(AttackWarningEffect, spawnPosition, Quaternion.identity);
            // �����ͦ������S��
            StartCoroutine(SpawnAttackEffectWithDelay(spawnPosition, 1.0f));
        }

        /*else
        {
            Debug.Log("�����S��2");
            int effectCount = 5; // ����S�Ī��ƶq
            float spacing = 5f; // ����S�Ĥ��������Z

            for (int i = 0; i < effectCount; i++)
            {
                Vector3 spawnPosition = attackEffectSpawnPoint.position + new Vector3(i * spacing, 0, 0);
                SpawnExplsionDelay(spawnPosition, 1.0f);
            }

            for (int i = 0; i < effectCount; i++)
            {
                Vector3 spawnPosition = attackEffectSpawnPoint.position + new Vector3(i * -spacing, 0, 0);
                SpawnExplsionDelay(spawnPosition, 1.0f);
            }

        }*/
    }
    private IEnumerator SpawnAttackEffectWithDelay(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay); // ���� delay ��
        Instantiate(attackEffectPrefab1, position, Quaternion.identity);     
    }

    /*private IEnumerator SpawnExplsionDelay(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay); // ���� delay ��
        Instantiate(attackEffectPrefab2, position, Quaternion.identity);
    }*/

    public override void OnSummon()
    {
        base.OnSummon();
        int minionCount = 5; // �]�w�n�ͦ����p�Ǽƶq
        float minX = -53f; // �]�w�ͦ��d�򪺳̤pX�y��
        float maxX = 53f; // �]�w�ͦ��d�򪺳̤jX�y��       

        for (int i = 0; i < minionCount; i++)
        {
            float randomX = Random.Range(minX, maxX);
            Vector3 spawnPosition = summonEnemyPoint.position + new Vector3(randomX, 0, 0);
            Addressables.InstantiateAsync(MinionPrefab, spawnPosition, Quaternion.Euler(0, 0, 0))
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

    public override void OnBossShow()
    {
       base.OnBossShow();
       DialogManager.Instance.StartDialog("Boss_Show");
       isTalk = true;
       audioDefination.PlayAudioClip();
    }
  
}
