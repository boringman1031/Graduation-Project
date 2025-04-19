using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDemon : EnemyBase
{
    [Header("�����S�ĳ]�w")]
    public GameObject AttackEffectPrefab;          // ���y prefab
    public Transform AttackEffectSpawnPoint;       // �o�g��m
    private GameObject chargingEffectInstance;     // ���y�Ȧs�]�٨S�g�X�h�^

    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState();
        patrolState = new PatrolState();
        chaseState = new ChaseState();
        attackerState = new AttackState();
    }

    // �Ѱʵe�ƥ�I�s�G�W�O�����ɩI�s�A���ͦ����o�g
    public void OnAttackChargeComplete()
    {
        if (AttackEffectPrefab != null && AttackEffectSpawnPoint != null)
        {
            chargingEffectInstance = Instantiate(AttackEffectPrefab, AttackEffectSpawnPoint.position, Quaternion.identity);
            chargingEffectInstance.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
        }
    }

    // �Ѱʵe�ƥ�I�s�G�u���o�g���y
    public void OnAttackEffectLaunch()
    {
        if (chargingEffectInstance == null) return;

        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
        {
            Vector2 dir = (player.position - AttackEffectSpawnPoint.position).normalized;
            chargingEffectInstance.GetComponent<EnemyProjectile>()?.Initialize(dir);
        }

        chargingEffectInstance = null; // �M�żȦs
    }
}
