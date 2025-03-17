using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BossBaseState
{
    private float attackCooldown = 10f; // �����N�o�ɶ�
    private float lastAttackTime;

    public override void OnEnter(BossBase boss)
    {
        currentBoss = boss;
        lastAttackTime = Time.time; // �O����e�ɶ�      
        currentBoss.OnAttack();
    }

    public override void LogicUpdate()
    {
        // **�ˬd���q�p�ǬO�_�s�b**
        if (!currentBoss.CheckMinionsExist() && !currentBoss.isSummonMinion)
        {            
            currentBoss.SwitchState(BossState.Summon);
            return;
        }

        // **�ˬd�R�ߤp�ǬO�_�s�b**
        if (!currentBoss.CheckHeartMinionsExist() && currentBoss.isSummonMinion)
        {          
            currentBoss.SwitchState(BossState.SummonHeart);
            return;
        }

        // **�����N�o����**
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            currentBoss.OnAttack();
        }
    }

    public override void PhysicsUpdate()
    {
    }

    public override void OnExit()
    {
       
    }
}
