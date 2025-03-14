using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase :MonoBehaviour
{
    [HideInInspector] public Animator anim;

    [Header("�s���ƥ�")]
    public VoidEventSO CameraShakeEvent;
    public VoidEventSO BossDeadEvent;

    [Header("�ƥ��ť")]
    public VoidEventSO AttackBossEvent;

    [Header("��¦�ƭ�")]
    public float maxHealth;
    private float currentHealth;

    private BossBaseState currentState;
    private BossBaseState patrolState;
    private BossBaseState chaseState;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        currentState.OnEnter(this);
        AttackBossEvent.OnEventRaised += OnTakeDamage;
    }

    private void OnDisable()
    {
        currentState.OnExit();
        AttackBossEvent.OnEventRaised -= OnTakeDamage;
    }

    private void Update()
    {
        currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();
    }
    public void OnShow()//Boss�X��
    {
        CameraShakeEvent.RaiseEvent();//�۾��_��
    }
    public void OnTakeDamage()//Boss����ˮ`
    {
        anim.SetTrigger("Hit");
        currentHealth -= 100;           
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Die()//Boss���`
    {
        BossDeadEvent.RaiseEvent();
    }

    public void SwitchState(BossState _state)//�������A
    {
        var newState = _state switch//�ھڲ{�����A�����ĤH���A(switch���y�k�}�g�k)
        {
           BossState.Idle => patrolState,      
            _ => null,
        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }
}
