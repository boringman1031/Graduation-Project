using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Chap1_Boss : BossBase
{

    [Header("小怪預製體")]
    public string MinionPrefab="Goblin"; //暫時使用Goblin代替小怪
    public string HeartMinionPrefab = "Heart"; //暫時使用Heart代替愛心小怪

    [Header("特效")]
    public Transform attackEffectSpawnPoint;//攻擊特效生成位置
    public Transform HeartEffectSpawnPoint;//愛心特效生成位置
    public Transform summonEnemyPoint;//召喚敵人生成位置
    public GameObject AttackWarningEffect;//攻擊預警特效
    public GameObject attackEffectPrefab1;//攻擊特效
    public GameObject attackEffectPrefab2;//攻擊2特效

    [Header("BGM")]
    public AudioDefination audioDefination;//音樂定義

    protected override void Awake()
    {
        base.Awake();
        idleState = new BossIdelState();
        attackState = new BossAttackState();
        summonState = new BossSummonState();
        summonHeartState = new BossSummonHeartState();
    }   
    public void OnAttackEffect()//在動畫某階段生成攻擊特效
    {

        int effectCount = 5; // 控制特效的數量
        float spacing = 20f; // 控制特效之間的間距
        for (int i = -effectCount / 2; i <= effectCount / 2; i++)
        {
            Vector3 spawnPosition = attackEffectSpawnPoint.position + new Vector3(i * spacing, 0, 0);
            Instantiate(AttackWarningEffect, spawnPosition, Quaternion.identity);
            // 延遲後生成攻擊特效
            StartCoroutine(SpawnAttackEffectWithDelay(spawnPosition, 1.0f));
        }

        /*else
        {
            Debug.Log("攻擊特效2");
            int effectCount = 5; // 控制特效的數量
            float spacing = 5f; // 控制特效之間的間距

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
        yield return new WaitForSeconds(delay); // 等待 delay 秒
        Instantiate(attackEffectPrefab1, position, Quaternion.identity);     
    }

    /*private IEnumerator SpawnExplsionDelay(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay); // 等待 delay 秒
        Instantiate(attackEffectPrefab2, position, Quaternion.identity);
    }*/

    public override void OnSummon()
    {
        base.OnSummon();
        int minionCount = 5; // 設定要生成的小怪數量
        float minX = -53f; // 設定生成範圍的最小X座標
        float maxX = 53f; // 設定生成範圍的最大X座標       

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
            Debug.LogError("無法加載小怪預製體！");
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

    public override void OnBossShow()
    {
       base.OnBossShow();
       DialogManager.Instance.StartDialog("Boss_Show");
       isTalk = true;
       audioDefination.PlayAudioClip();
    }
  
}
