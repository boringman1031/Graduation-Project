/*------------BY 017-----------------*/
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChaseState : BaseState
{
    private Transform player;

    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        currentEnemy.anim.SetBool("Run", true);
    }

    public override void LogicUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(currentEnemy.transform.position, player.position);

        if (distance <= currentEnemy.attackRange)
        {
            currentEnemy.SwitchState(EenemyState.Attack);
            return;
        }

        // 玩家消失倒數時間計算
        if (!currentEnemy.FindPlayer())
        {
            currentEnemy.lostTimeCounter -= Time.deltaTime;
            if (currentEnemy.lostTimeCounter <= 0)
            {
                currentEnemy.SwitchState(EenemyState.Patrol);
            }
        }
        else
        {
            currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        }

        // 面向玩家方向
        if (player.position.x - currentEnemy.transform.position.x > 0)
            currentEnemy.transform.localScale = new Vector3(-1.6f, 1.6f, 1.6f);
        else
            currentEnemy.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
    }

    public override void PhysicsUpdate()
    {
        if (player == null || currentEnemy.isDead || currentEnemy.isHit) return;

        currentEnemy.rb.velocity = new Vector2(
            currentEnemy.currentSpeed * -currentEnemy.transform.localScale.x,
            currentEnemy.rb.velocity.y
        );
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Run", false);
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;

        //  確保離開追擊狀態時，方向回到面向前進的方向
        currentEnemy.faceDir = new Vector3(-currentEnemy.transform.localScale.x, 0, 0);
    }
}
