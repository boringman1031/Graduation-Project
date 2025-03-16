using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BossBaseState
{
    private float attackCooldown = 20f; // �����N�o�ɶ�
    private float lastAttackTime;
    private bool hasSummoned = false; // �O�_�w�g�l��L�p��

    public override void OnEnter(BossBase boss)
    {
        currentBoss = boss;
        lastAttackTime = Time.time; // �i�J���A�ɰO����e�ɶ�
        Debug.Log("Boss �i�J�������A�I");

        if (!currentBoss.CheckMinionsExist() && !hasSummoned)
        {
            hasSummoned = true;
            currentBoss.SwitchState(BossState.Summon);
            return;
        }
    }

    public override void LogicUpdate()
    {
        if (currentBoss.CheckMinionsExist()) return; // �p�G�������٦��p�ǡA�~�����

        // �����N�o�ɶ���F�~����
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            currentBoss.OnAttack();
        }
        // �p�G�p�Ǥw�g�����Q���ѡA�h�ͦ��R�ߤp��
        if (!hasSummoned)
        {
            hasSummoned = true;
            currentBoss.SpawnHeartMinion();
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }

}
