using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummonHeartState : BossBaseState
{
    private float summonCooldown = 3f;
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
            currentBoss.SpawnHeartMinion();
            hasSummoned = true;
        }

        if (currentBoss.CheckHeartMinionsExist())
        {          
            currentBoss.SwitchState(BossState.Attack);
        }
    }

    public override void PhysicsUpdate()
    {
    }

    public override void OnExit()
    {       
        currentBoss.isSummonMinion = false; // **重置 `isSummonMinion`，確保可以再召喚普通小怪**
    }
}
