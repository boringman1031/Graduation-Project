using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyman : EnemyBase
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

    public override void OnDead()
    {
        base.OnDead();
        rb.velocity = new Vector2(0, 5f); // Set upward velocity     
    }
}
