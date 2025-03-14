using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BossBaseState
{
    private float attackCooldown = 2.5f; // �����N�o�ɶ�
    private float lastAttackTime;

    public override void OnEnter(BossBase boss)
    {
        currentBoss = boss;
        Debug.Log("Boss �i�J�������A�I");
        lastAttackTime = -attackCooldown; // �� Boss �i�J�ɥߧY����
        currentBoss.OnAttack();
    }

    public override void LogicUpdate()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            currentBoss.OnAttack();
        }

        // **�� Boss ��q ? 50%�A������l�ꪬ�A**
        if (currentBoss.currentHealth <= currentBoss.maxHealth / 2)
        {
            Debug.Log(" Boss ������l�ꪬ�A�I");
            currentBoss.SwitchState(BossState.Summon);
        }
    }

    public override void PhysicsUpdate()
    {
        // Boss �b�������A�q�`���|����
    }

    public override void OnExit()
    {

    }

}
