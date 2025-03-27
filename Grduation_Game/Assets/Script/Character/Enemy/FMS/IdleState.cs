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
        Debug.Log(currentEnemy.name + " ¶i¤J Idle ª¬ºA");
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
