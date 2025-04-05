using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = 0;
        currentEnemy.anim.SetBool("Run", false);       
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.FindPlayer())
        {
            currentEnemy.SwitchState(EenemyState.Chase);
        }
    }

    public override void PhysicsUpdate() { }

    public override void OnExit() { }
}
