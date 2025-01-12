/*--------by017--------*/
/*----------�����¦�ƭ�----------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharactorBase : MonoBehaviour
{
    [Header("��¦�ƭ�")]
    public float Health;
    public float Defence;

    [Header("��e�ƭ�")]
    public float MaxHealth;
    public float CurrentHealth;
    public float CurrentPower;

    [Header("���A")]
    public float SuperArmourTime;//�Q��ɶ�
    private float SuperArmourTimeCounter;//�Q��ɶ��p�ƾ�
    public bool SuperArmour;//�O�_�b�Q�骬�A

    public UnityEvent<Transform> OnTakeDamage;

    private void Start()
    {
        MaxHealth = Health+Defence;
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        if (SuperArmour)
        {
            SuperArmourTimeCounter -= Time.deltaTime;
            if (SuperArmourTimeCounter <= 0)
            {
                SuperArmour = false;
            }
        }
    }
    public void TakeDamage(Attack _attacker)//����ˮ`�P�w
    {
        if (SuperArmour)
        {
            return;
        }
        if(CurrentHealth-_attacker.Damage > 0)
        {
            CurrentHealth -= _attacker.Damage;
            TriggerSuperArmour();
            //���ˮɭn�F���g�b�o
            OnTakeDamage?.Invoke(_attacker.transform);//Ĳ�o���˨ƥ�

        }
        else
        {
            CurrentHealth = 0;
            //���`
        }
      
    }

    private void TriggerSuperArmour()//Ĳ�o�Q��
    {
        if(!SuperArmour) 
        {
            SuperArmour = true;
            SuperArmourTimeCounter = SuperArmourTime;
        }
    }
}
