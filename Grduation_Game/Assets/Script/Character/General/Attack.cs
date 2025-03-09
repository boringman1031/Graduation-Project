/*--------by017--------*/
/*----------§ðÀ»§P©w--------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float Damage;
    public float attackRange;//§ðÀ»½d³ò
    public float attackRate;//§ðÀ»³t«×
    public float Knockback;//À»°h¶ZÂ÷

    private void OnTriggerStay2D(Collider2D other)//§ðÀ»§P©w
    {
        //other.GetComponent<CharactorBase>()?.TakeDamage(this);
        CharactorBase target = other.GetComponent<CharactorBase>();

        if (target == null) // ½T«O target ¤£¬° null
        {            
            return;
        }

        target.TakeDamage(this);
    } 
}

