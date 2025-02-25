/*------------BY 017-----------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BaseState
{
    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;//¤Á´«¬°°lÀ»³t«×     
    }
    public override void LogicUpdate()//ÅÞ¿è§PÂ_
    {
        if (currentEnemy.lostTime > 0)
            currentEnemy.SwitchState(NPCState.Patrol);//¤Á´«¬°¨µÅÞª¬ºA
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.transform.localScale =new Vector3(currentEnemy.faceDir.x, 1, 1);
        }
    }

    public override void PhysicsUpdate()//ª«²z§PÂ_ 
    {
       
    }
    public override void OnExit()
    {
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        currentEnemy.anim.SetBool("Run", false);
    }
}
