using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private Transform player;
    private float lastAttackTime;

    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = 0;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        currentEnemy.isAttacking = false; // 進入攻擊狀態時重設攻擊中狀態
        Debug.Log(currentEnemy.name + " 進入攻擊狀態");
    }

    public override void LogicUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(currentEnemy.transform.position, player.position);

        // 超出攻擊距離就切回追擊狀態
        if (distance > currentEnemy.attackRange && !currentEnemy.isAttacking)
        {
            currentEnemy.SwitchState(EenemyState.Chase);
            return;
        }

        // 如果冷卻時間到，且當前沒有在攻擊中，就進行攻擊
        if (Time.time >= lastAttackTime + currentEnemy.attackCooldown && !currentEnemy.isAttacking)
        {
            lastAttackTime = Time.time;
            currentEnemy.isAttacking = true;
            currentEnemy.anim.SetTrigger("Attack");
        }

        // 面向玩家方向
        if (player.position.x - currentEnemy.transform.position.x > 0)
            currentEnemy.transform.localScale = new Vector3(-1.6f, 1.6f, 1.6f);
        else
            currentEnemy.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
    }

    public override void PhysicsUpdate()
    {
       
    }

    public override void OnExit()
    {
        currentEnemy.isAttacking = false; // 離開攻擊狀態時保險清除
    }
}
