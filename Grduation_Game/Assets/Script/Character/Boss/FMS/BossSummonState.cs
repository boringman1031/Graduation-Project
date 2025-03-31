using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummonState : BossBaseState
{
    private float summonCooldown = 3f; // 召喚冷卻時間
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
            currentBoss.isSummonMinion = true; // **設定為已召喚**
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
