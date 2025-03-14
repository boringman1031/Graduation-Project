using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chap1_Boss : BossBase
{
    protected override void Awake()
    {
        base.Awake();
        attackState = new BossAttackState();
        summonState = new BossSummonState();
    }
    public override void OnAttack()
    {
        base.OnAttack();
        if (currentHealth > maxHealth / 2)
        {
            //TODO:生成攻擊1特效
        }
        else
        {
            //TODO:生成攻擊2特效
        }
    }
}
