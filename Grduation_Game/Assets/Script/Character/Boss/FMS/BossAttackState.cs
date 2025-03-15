using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BossBaseState
{
    private float attackCooldown = 2.5f; // 攻擊冷卻時間
    private float lastAttackTime;
    private bool hasSummoned = false; // 是否已經召喚過小怪

    public override void OnEnter(BossBase boss)
    {
        currentBoss = boss;
        lastAttackTime = -attackCooldown;
        Debug.Log("Boss 進入攻擊狀態！");

        if (!currentBoss.CheckMinionsExist() && !hasSummoned)
        {
            Debug.Log("場景內沒有小怪，切換到召喚狀態！");
            hasSummoned = true;
            currentBoss.SwitchState(BossState.Summon);
            return;
        }

        currentBoss.OnAttack();
    }

    public override void LogicUpdate()
    {
        if (currentBoss.CheckMinionsExist()) return; // 如果場景內還有小怪，繼續攻擊

        // 如果小怪已經全部被擊敗，則生成愛心小怪
        if (!hasSummoned)
        {
            hasSummoned = true;
            Debug.Log("所有小怪已被擊敗，Boss 生成愛心小怪！");
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
