using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public override void OnEnter(EnemyBase _enemy)
    {
        currentEnemy = _enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;//切換為巡邏速度
        Debug.Log("進入巡邏狀態");
    }
    public override void LogicUpdate()//邏輯判斷
    {
        //發現player進入追擊狀態
        if(currentEnemy.FindPlayer())
        {         
            currentEnemy.SwitchState(EenemyState.Chase);//切換為追擊狀態
            Debug.Log("發現player進入追擊狀態");
        }
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
        
    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Run", false);
    }
}
