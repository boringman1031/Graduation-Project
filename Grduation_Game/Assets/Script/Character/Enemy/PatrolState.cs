using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PatrolState : BaseState
{
    public override void OnEnter(EnemyBase _enemy)
    {
        currentEnemy = _enemy;
        
    }
    public override void LogicUpdate()//�޿�P�_
    {
        //�o�{player�i�J�l�����A
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.anim.SetBool("Run", false);
        }
        else
        {
            currentEnemy.anim.SetBool("Run", true);
        }
    }
    public override void PhysicsUpdate()//���z�P�_
    {
        throw new System.NotImplementedException();
    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Run", false);
    }
}
