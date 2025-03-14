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
        Debug.Log(" Boss 進入召喚狀態！");
        lastSummonTime = -summonCooldown; // 立即召喚
        Summon();
    }

    public override void LogicUpdate()
    {
        if (Time.time >= lastSummonTime + summonCooldown)
        {
            lastSummonTime = Time.time;
            Summon();
        }
    }

    public override void PhysicsUpdate()
    {
        // Boss 在召喚狀態不會移動
    }

    public override void OnExit()
    {
       
    }

    private void Summon()
    {
        Debug.Log(" Boss 召喚小怪！");
        // **TODO：在這裡生成召喚物件**
    }
}
