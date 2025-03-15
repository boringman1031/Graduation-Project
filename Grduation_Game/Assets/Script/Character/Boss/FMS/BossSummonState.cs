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
        currentBoss.OnSummon();
    }

    public override void LogicUpdate()
    {
        if (currentBoss.CheckMinionsExist()) return; // 如果場景內還有小怪，繼續召喚

        currentBoss.SwitchState(BossState.Attack);

    }

    public override void PhysicsUpdate()
    {
        // Boss 在召喚狀態不會移動
    }

    public override void OnExit()
    {

    }

}
