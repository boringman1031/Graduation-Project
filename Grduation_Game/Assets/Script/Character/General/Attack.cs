/*--------by017--------*/
/*----------�����P�w--------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float Damage;
    public float attackRange;//�����d��
    public float attackRate;//�����t��

    private void OnTriggerStay2D(Collider2D other)//�����P�w
    {       
        //other.GetComponent<CharactorBase>()?.TakeDamage(this);
        CharactorBase target = other.GetComponent<CharactorBase>();

        if (target == null) // �T�O target ���� null
        {            
            return;
        }

        target.TakeDamage(Damage, transform);
    } 
}

