using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BossBaseState
{
    private float attackCooldown = 20f; // 攻擊冷卻時間
    private float lastAttackTime;
    private bool hasSummoned = false; // 是否已經召喚過小怪

    public override void OnEnter(BossBase boss)
    {
        currentBoss = boss;
        lastAttackTime = Time.time; // 進入狀態時記錄當前時間
        Debug.Log("Boss 進入攻擊狀態！");

        if (!currentBoss.CheckMinionsExist() && !hasSummoned)
        {
            hasSummoned = true;
            currentBoss.SwitchState(BossState.Summon);
            return;
        }
    }

    public override void LogicUpdate()
    {
        if (currentBoss.CheckMinionsExist()) return; // 如果場景內還有小怪，繼續攻擊

        // 攻擊冷卻時間到了才攻擊
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            currentBoss.OnAttack();
        }
        // 如果小怪已經全部被擊敗，則生成愛心小怪
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
