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
    }
    public override void Move()
    {
        base.Move();
        anim.SetBool("Run", true);
    }
}
