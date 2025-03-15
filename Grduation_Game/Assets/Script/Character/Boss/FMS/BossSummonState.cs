using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummonState : BossBaseState
{
    private float summonCooldown = 5.0f;
    private float lastSummonTime;

    public override void OnEnter(BossBase boss)
    {
        currentBoss = boss;
        Debug.Log(" Boss �i�J�l�ꪬ�A�I");
        lastSummonTime = -summonCooldown; // �ߧY�l��
        currentBoss.OnSummon();
    }

    public override void LogicUpdate()
    {       
        // **��p�ǥl���A���^�������A**
        if (currentBoss.CheckMinionsExist()) return; // �p�G�������٦��p�ǡA�~��l��
        
        Debug.Log("�p�Ǥw�l��A���^�������A�I");
        currentBoss.SwitchState(BossState.Attack);

    }

    public override void PhysicsUpdate()
    {
        // Boss �b�l�ꪬ�A���|����
    }

    public override void OnExit()
    {
       
    }
  
}
