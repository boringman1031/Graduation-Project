using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow :EnemyBase
{
    protected override void Awake()
    {
        base.Awake();
        idleState = new IdleState();
        patrolState = new PatrolState();
        chaseState = new ChaseState();
        attackerState = new AttackState();
    }
}
