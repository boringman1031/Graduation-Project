using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottonMonster : EnemyBase
{
    public GameObject AttackEffectPrefab;//�����S��
    public Transform AttackEffectSpawnPoint;//�����S�ĥͦ��I

    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState();
        patrolState = new PatrolState();
        chaseState = new ChaseState();
        attackerState = new AttackState();
    }

    public void OnAttackEffectShow()
    {
        if (AttackEffectPrefab != null && AttackEffectSpawnPoint != null)
        {
            Instantiate(AttackEffectPrefab, AttackEffectSpawnPoint.position, Quaternion.identity);
            Debug.Log("�]�E");
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (AttackEffectSpawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(AttackEffectSpawnPoint.position, 0.15f);
        }
    }
}

