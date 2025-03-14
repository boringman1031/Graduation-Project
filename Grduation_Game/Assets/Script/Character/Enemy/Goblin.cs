using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : EnemyBase
{
    protected override void Awake()
    {
        base.Awake();
        patrolState = new PatrolState();
        chaseState = new ChaseState();
        attackerState = new AttackState();
    }
    public override void OnMove()
    {
        base.OnMove();
        anim.SetBool("Run", true);
    }
}
