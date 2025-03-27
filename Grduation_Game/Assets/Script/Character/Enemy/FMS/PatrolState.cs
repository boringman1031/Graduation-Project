using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public override void OnEnter(EnemyBase _enemy)
    {
        currentEnemy = _enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.anim.SetBool("Run", true);
        Debug.Log(currentEnemy.name + " ¶i¤J¨µÅÞª¬ºA");
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.PlayerInAttackRange())
        {
            currentEnemy.SwitchState(EenemyState.Attack);
            return;
        }

        if (currentEnemy.FindPlayer())
        {
            currentEnemy.SwitchState(EenemyState.Chase);
            return;
        }

        if (!currentEnemy.physicsCheck.isGround ||
            (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) ||
            (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.anim.SetBool("Run", false);
        }
        else
        {
            currentEnemy.anim.SetBool("Run", true);
        }
    }

    public override void PhysicsUpdate() { }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Run", false);
    }
}
