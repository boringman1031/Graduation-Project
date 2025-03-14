using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private Transform player;//���a
    private float lastAttackTime;//�W�������ɶ�

    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = 0; // �i�J�������A�ɰ����          
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
                currentEnemy.SwitchState(NPCState.Patrol);//���������ު��A
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
