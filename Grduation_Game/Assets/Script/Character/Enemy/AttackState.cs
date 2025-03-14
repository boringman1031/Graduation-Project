using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private Transform player;//ª±®a
    private float lastAttackTime;//¤W¦¸§ðÀ»®É¶¡

    public override void OnEnter(EnemyBase enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = 0; // ¶i¤J§ðÀ»ª¬ºA®É°±¤î²¾°Ê       
        Debug.Log("¶i¤J§ðÀ»ª¬ºA¡I");
        currentEnemy.anim.SetTrigger("Attack");
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
                Debug.Log(" ª±®aÂ÷¶}§ðÀ»½d³ò¡A¦^¨ì°lÀ»ª¬ºA");
                currentEnemy.SwitchState(NPCState.Patrol);//¤Á´«¬°¨µÅÞª¬ºA
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
        Debug.Log(" Â÷¶}§ðÀ»ª¬ºA");
    }
}
