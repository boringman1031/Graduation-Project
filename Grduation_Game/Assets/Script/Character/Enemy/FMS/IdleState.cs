using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = 0;  // �����
        currentEnemy.anim.SetBool("Run", false); // ����]�B�ʵe      
    }

    public override void LogicUpdate()
    {
        // �Ŷ����A�U����������޿�
    }

    public override void PhysicsUpdate()
    {
        // �Ŷ����A�U��������󪫲z��s
    }

    public override void OnExit()
    {
        // �h�X�ɤ��ݭn�S�O�B�z
    }
}
