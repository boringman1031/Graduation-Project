using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottonMonster : EnemyBase
{
    public GameObject AttackEffectPrefab;//攻擊特效
    public Transform AttackEffectSpawnPoint;//攻擊特效生成點

    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState();
        patrolState = new PatrolState();
        chaseState = new ChaseState();
        attackerState = new AttackState();
    }

    public void  OnAttackEffectShow()
    {
        if (AttackEffectPrefab != null && AttackEffectSpawnPoint != null)
        {
            Instantiate(AttackEffectPrefab, AttackEffectSpawnPoint.position, Quaternion.identity);
        }
    }
   
}

