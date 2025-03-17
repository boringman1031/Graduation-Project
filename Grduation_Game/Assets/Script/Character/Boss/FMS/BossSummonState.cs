using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummonState : BossBaseState
{
    private float summonCooldown = 3f; // �l��N�o�ɶ�
    private float summonStartTime;
    private bool hasSummoned;

    public override void OnEnter(BossBase boss)
    {
        currentBoss = boss;        
        summonStartTime = Time.time;
        hasSummoned = false;
    }

    public override void LogicUpdate()
    {
        if (Time.time < summonStartTime + summonCooldown)
        {
            return;
        }

        if (!hasSummoned)
        {          
            currentBoss.OnSummon();
            hasSummoned = true;
            currentBoss.isSummonMinion = true; // **�]�w���w�l��**
        }

        if (currentBoss.CheckMinionsExist())
        {          
            currentBoss.SwitchState(BossState.Attack);
        }
    }

    public override void PhysicsUpdate()
    {
    }

    public override void OnExit()
    {
      
    }
}
