using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BossBaseState
{
    private float attackCooldown = 10f; // 攻擊冷卻時間
    private float lastAttackTime;

    public override void OnEnter(BossBase boss)
    {
        currentBoss = boss;
        lastAttackTime = Time.time; // 記錄當前時間      
        currentBoss.OnAttack();
    }

    public override void LogicUpdate()
    {
        // **檢查普通小怪是否存在**
        if (!currentBoss.CheckMinionsExist() && !currentBoss.isSummonMinion)
        {            
            currentBoss.SwitchState(BossState.Summon);
            return;
        }

        // **檢查愛心小怪是否存在**
        if (!currentBoss.CheckHeartMinionsExist() && currentBoss.isSummonMinion)
        {          
            currentBoss.SwitchState(BossState.SummonHeart);
            return;
        }

        // **攻擊冷卻機制**
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
