using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BossBaseState
{
    private float attackCooldown = 2.5f; // 攻擊冷卻時間
    private float lastAttackTime;

    public override void OnEnter(BossBase boss)
    {
        currentBoss = boss;
        Debug.Log("Boss 進入攻擊狀態！");
        lastAttackTime = -attackCooldown; // 讓 Boss 進入時立即攻擊
        currentBoss.OnAttack();
    }

    public override void LogicUpdate()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            currentBoss.OnAttack();
        }

        // **當 Boss 血量 ? 50%，切換到召喚狀態**
        if (currentBoss.currentHealth <= currentBoss.maxHealth / 2)
        {
            Debug.Log(" Boss 切換到召喚狀態！");
            currentBoss.SwitchState(BossState.Summon);
        }
    }

    public override void PhysicsUpdate()
    {
        // Boss 在攻擊狀態通常不會移動
    }

    public override void OnExit()
    {

    }

}
