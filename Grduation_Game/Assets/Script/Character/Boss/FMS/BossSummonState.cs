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
        // Boss �b�l�ꪬ�A���|����
    }

    public override void OnExit()
    {
       
    }

    private void Summon()
    {
        Debug.Log(" Boss �l��p�ǡI");
        // **TODO�G�b�o�̥ͦ��l�ꪫ��**
    }
}
