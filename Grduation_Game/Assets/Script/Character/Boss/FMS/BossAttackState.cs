using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BossBaseState
{
    private float attackCooldown = 2.5f; // �����N�o�ɶ�
    private float lastAttackTime;
    private bool hasSummoned = false; // �O�_�w�g�l��L�p��

    public override void OnEnter(BossBase boss)
    {
        currentBoss = boss;
        lastAttackTime = -attackCooldown;
        Debug.Log("Boss �i�J�������A�I");

        if (!currentBoss.CheckMinionsExist() && !hasSummoned)
        {
            Debug.Log("�������S���p�ǡA������l�ꪬ�A�I");
            hasSummoned = true;
            currentBoss.SwitchState(BossState.Summon);
            return;
        }

        currentBoss.OnAttack();
    }

    public override void LogicUpdate()
    {
        if (currentBoss.CheckMinionsExist()) return; // �p�G�������٦��p�ǡA�~�����

        // �p�G�p�Ǥw�g�����Q���ѡA�h�ͦ��R�ߤp��
        if (!hasSummoned)
        {
            hasSummoned = true;
            Debug.Log("�Ҧ��p�Ǥw�Q���ѡABoss �ͦ��R�ߤp�ǡI");
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
