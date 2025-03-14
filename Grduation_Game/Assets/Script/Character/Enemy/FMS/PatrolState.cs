using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public override void OnEnter(EnemyBase _enemy)
    {
        currentEnemy = _enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;//���������޳t��
    }
    public override void LogicUpdate()//�޿�P�_
    {
        //�o�{player�i�J�l�����A
        if(currentEnemy.FindPlayer())
        {         
            currentEnemy.SwitchState(NPCState.Chase);//�������l�����A
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
    public override void PhysicsUpdate()//���z�P�_
    {
        
    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Run", false);
    }
}
