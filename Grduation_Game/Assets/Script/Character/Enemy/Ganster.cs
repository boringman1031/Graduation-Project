using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ganster :EnemyBase
{
    public GameObject DeadEffect;
    public GameObject HitEffect;
    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState();
        patrolState = new PatrolState();
        chaseState = new ChaseState();
        attackerState = new AttackState();
    }

    public void OnDeadEffect()
    {
        Instantiate(DeadEffect, transform.position, Quaternion.identity);
    }

    public void OnHitEffect()
    {
        Instantiate(HitEffect, transform.position, Quaternion.identity);
    }
}
