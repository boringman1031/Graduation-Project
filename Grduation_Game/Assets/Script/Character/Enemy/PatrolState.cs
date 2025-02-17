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
    public override void LogicUpdate()//邏輯判斷
    {
        //發現player進入追擊狀態
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.anim.SetBool("Run", false);
        }
        else
        {
            currentEnemy.anim.SetBool("Run", true);
        }
    }
    public override void PhysicsUpdate()//物理判斷
    {
        throw new System.NotImplementedException();
    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Run", false);
    }
}
