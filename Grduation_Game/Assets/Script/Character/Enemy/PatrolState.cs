using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public override void OnEnter(EnemyBase _enemy)
    {
        currentEnemy = _enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;//¤Á´«¬°¨µÅÞ³t«×
    }
    public override void LogicUpdate()//ÅÞ¿è§PÂ_
    {
        //µo²{player¶i¤J°lÀ»ª¬ºA
        if(currentEnemy.FindPlayer())
        {         
            currentEnemy.SwitchState(NPCState.Chase);//¤Á´«¬°°lÀ»ª¬ºA
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
    public override void PhysicsUpdate()//ª«²z§PÂ_
    {
        
    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Run", false);
    }
}
