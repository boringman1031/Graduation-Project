/*--------by017--------*/
/*----------攻擊判定--------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float Damage;
    public float attackRange;//攻擊範圍
    public float attackRate;//攻擊速度  

    private void OnTriggerStay2D(Collider2D other)//攻擊判定
    {
        //other.GetComponent<CharactorBase>()?.TakeDamage(this);
        CharactorBase target = other.GetComponent<CharactorBase>();

        if (target == null) // 確保 target 不為 null
        {            
            return;
        }

        target.TakeDamage(this);
    } 
}

