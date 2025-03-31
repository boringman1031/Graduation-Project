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
        Debug.Log("進入巡邏模式");

        // 確保方向與 faceDir 同步
        currentEnemy.faceDir = new Vector3(-currentEnemy.transform.localScale.x, 0, 0);
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

    public override void PhysicsUpdate() 
    {
        if (currentEnemy.isDead || currentEnemy.isHit) return;

        currentEnemy.rb.velocity = new Vector2(
            currentEnemy.currentSpeed * -currentEnemy.transform.localScale.x,
            currentEnemy.rb.velocity.y
        );
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Run", false);
    }
}
