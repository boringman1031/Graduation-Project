using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : EnemyBase
{
    public GameObject DeadEffect;

    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState();
        patrolState = new PatrolState();
        chaseState = new ChaseState();
        attackerState = new AttackState();
    }

    public  void OnDeadEffect()
    {
        Instantiate(DeadEffect, transform.position, Quaternion.identity);
    }
    
}
