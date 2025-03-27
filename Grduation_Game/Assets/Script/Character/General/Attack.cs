/*--------by017--------*/
/*----------�����P�w--------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float Damage;   

    private void OnTriggerStay2D(Collider2D other)//�����P�w
    {
        Debug.Log($"����{other.name}");
        //other.GetComponent<CharactorBase>()?.TakeDamage(this);
        CharactorBase target = other.GetComponent<CharactorBase>();

        if (target == null) // �T�O target ���� null
        {            
            return;
        }

        target.TakeDamage(this);
    } 
}

