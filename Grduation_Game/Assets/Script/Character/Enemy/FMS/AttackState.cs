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
        currentEnemy.currentSpeed = 0; // �����
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        currentEnemy.anim.SetBool("Run", false);
        Debug.Log(currentEnemy.name + " �i�J�������A");
    }

    public override void LogicUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(currentEnemy.transform.position, player.position);

        // ? �Y���b�����d��A�B���O�������A���^�l��
        if (!currentEnemy.isAttacking && distance > currentEnemy.attackRange)
        {
            currentEnemy.SwitchState(EenemyState.Chase);
            return;
        }

        // ? �Y�����N�o�����B���O���b����
        if (Time.time >= lastAttackTime + currentEnemy.attackCooldown && !currentEnemy.isAttacking)
        {
            lastAttackTime = Time.time;
            currentEnemy.isAttacking = true;
            currentEnemy.anim.SetTrigger("Attack");
        }

        // ? ���V���a
        if (player.position.x - currentEnemy.transform.position.x > 0)
            currentEnemy.transform.localScale = new Vector3(-1.6f, 1.6f, 1.6f);
        else
            currentEnemy.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
    }

    public override void PhysicsUpdate()
    {
        // ������
        currentEnemy.rb.velocity = new Vector2(0, currentEnemy.rb.velocity.y);
    }

    public override void OnExit()
    {
        currentEnemy.isAttacking = false;
    }
}
