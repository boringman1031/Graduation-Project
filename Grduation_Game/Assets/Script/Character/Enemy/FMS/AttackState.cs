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
        Debug.Log(currentEnemy.name + " 進入攻擊狀態");
    }

    public override void LogicUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(currentEnemy.transform.position, player.position);

        if (distance > currentEnemy.attackRange)
        {
            currentEnemy.SwitchState(EenemyState.Chase);
            return;
        }

        if (Time.time >= lastAttackTime + currentEnemy.attackCooldown)
        {
            lastAttackTime = Time.time;
            currentEnemy.anim.SetTrigger("Attack");
        }

        // 面向玩家方向
        if (player.position.x - currentEnemy.transform.position.x > 0)
            currentEnemy.transform.localScale = new Vector3(-1.6f, 1.6f, 1.6f);
        else
            currentEnemy.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
    }

    public override void PhysicsUpdate() { }

    public override void OnExit() { }
}
