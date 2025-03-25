using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = 0;  // 停止移動
        currentEnemy.anim.SetBool("Run", false); // 停止跑步動畫      
    }

    public override void LogicUpdate()
    {
        // 空閒狀態下不執行任何邏輯
    }

    public override void PhysicsUpdate()
    {
        // 空閒狀態下不執行任何物理更新
    }

    public override void OnExit()
    {
        // 退出時不需要特別處理
    }
}
