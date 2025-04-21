using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDemon : EnemyBase
{
    [Header("攻擊特效設定")]
    public GameObject AttackEffectPrefab;          // 火球 prefab
    public Transform AttackEffectSpawnPoint;       // 發射位置
    private GameObject chargingEffectInstance;     // 火球暫存（還沒射出去）

    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState();
        patrolState = new PatrolState();
        chaseState = new ChaseState();
        attackerState = new AttackState();
    }

    // 由動畫事件呼叫：蓄力完成時呼叫，產生但不發射
    public void OnAttackChargeComplete()
    {
        if (AttackEffectPrefab != null && AttackEffectSpawnPoint != null)
        {
            chargingEffectInstance = Instantiate(AttackEffectPrefab, AttackEffectSpawnPoint.position, Quaternion.identity);
            chargingEffectInstance.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
        }
    }

    // 由動畫事件呼叫：真正發射火球
    public void OnAttackEffectLaunch()
    {
        if (chargingEffectInstance == null) return;

        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
        {
            Vector2 dir = (player.position - AttackEffectSpawnPoint.position).normalized;
            chargingEffectInstance.GetComponent<EnemyProjectile>()?.Initialize(dir);
        }

        chargingEffectInstance = null; // 清空暫存
    }
}
