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
        currentEnemy.currentSpeed = 0; // 停止移動
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        currentEnemy.anim.SetBool("Run", false);
        Debug.Log(currentEnemy.name + " 進入攻擊狀態");
    }

    public override void LogicUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(currentEnemy.transform.position, player.position);

        // ? 若不在攻擊範圍，且不是攻擊中，切回追擊
        if (!currentEnemy.isAttacking && distance > currentEnemy.attackRange)
        {
            currentEnemy.SwitchState(EenemyState.Chase);
            return;
        }

        // ? 若攻擊冷卻完畢且不是正在攻擊
        if (Time.time >= lastAttackTime + currentEnemy.attackCooldown && !currentEnemy.isAttacking)
        {
            lastAttackTime = Time.time;
            currentEnemy.isAttacking = true;
            currentEnemy.anim.SetTrigger("Attack");
        }

        // ? 面向玩家
        if (player.position.x - currentEnemy.transform.position.x > 0)
            currentEnemy.transform.localScale = new Vector3(-1.6f, 1.6f, 1.6f);
        else
            currentEnemy.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
    }

    public override void PhysicsUpdate()
    {
        // 不移動
        currentEnemy.rb.velocity = new Vector2(0, currentEnemy.rb.velocity.y);
    }

    public override void OnExit()
    {
        currentEnemy.isAttacking = false;
    }
}
