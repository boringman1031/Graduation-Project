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
    public float Knockback;//���h�Z��

    private void OnTriggerStay2D(Collider2D other)//�����P�w
    {
       other.GetComponent<CharactorBase>()?.TakeDamage(this);
    }
}

