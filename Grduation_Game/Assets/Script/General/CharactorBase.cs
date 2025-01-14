/*--------by017--------*/
/*----------�����¦�ƭ�----------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharactorBase : MonoBehaviour
{
    [Header("�ƥ��ť")]
    public VoidEventSO newGameEvent;

    [Header("��¦�ƭ�")]
    public float Health;
    public float Defence;

    [Header("��e�ƭ�")]
    public float MaxHealth;
    public float MaxPower;
    public float CurrentHealth;
    public float CurrentPower;

    [Header("���A")]
    public float SuperArmourTime;//�Q��ɶ�
    private float SuperArmourTimeCounter;//�Q��ɶ��p�ƾ�
    public bool SuperArmour;//�O�_�b�Q�骬�A

    public UnityEvent<CharactorBase> OnHealthChange;//��q���ܨƥ�(�Ω��sUI�s��)
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDead;

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
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
    private void NewGame()
    {
        MaxHealth = Health + Defence;
        CurrentHealth = MaxHealth;
        CurrentPower = MaxPower;
        OnHealthChange?.Invoke(this);
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
            OnDead?.Invoke();
            //���`
        }
      OnHealthChange?.Invoke(this);//Ĳ�o��q���ܨƥ�
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
