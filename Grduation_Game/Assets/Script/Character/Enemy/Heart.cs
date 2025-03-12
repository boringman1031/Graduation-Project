using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : EnemyBase
{
    [Header("¼s¼½¨Æ¥ó")]
    public VoidEventSO AttackBossEvent;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnDead()
    {
        base.OnDead();
        AttackBossEvent.OnEventRaised();
    }
}
