using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private Transform player;//玩家
    private float lastAttackTime;//上次攻擊時間

    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = 0; // 進入攻擊狀態時停止移動          
    }

    public override void LogicUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (player != null)
        {
            float distance = Vector2.Distance(currentEnemy.transform.position, player.position);
            if (distance > currentEnemy.attackRange)
            {
                currentEnemy.SwitchState(NPCState.Patrol);//切換為巡邏狀態
                return;
            }

            if (Time.time >= lastAttackTime + currentEnemy.attackCooldown)
            {
                lastAttackTime = Time.time;
                currentEnemy.anim.SetTrigger("Attack");
            }
        }
    }

    public override void PhysicsUpdate()
    {
       
    }

    public override void OnExit()
    {

    }
}
