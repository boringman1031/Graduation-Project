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
    [HideInInspector] public float currentHealth;

    private BossBaseState currentState;
    protected BossBaseState attackState;//�������A
    protected BossBaseState summonState;//�l�ꪬ�A
    protected BossBaseState HeartState;//�l��i�����R�ߪ��A
    protected virtual void Awake()
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
    public virtual void OnAttack()//Boss����
    {
        anim.SetTrigger("Attack");
    }
    public void Die()//Boss���`
    {
        BossDeadEvent.RaiseEvent();
    }

    public void SwitchState(BossState _state)//�������A
    {
        var newState = _state switch//�ھڲ{�����A�����ĤH���A(switch���y�k�}�g�k)
        {
            BossState.Attack => attackState,
            BossState.Summon => summonState,
            BossState.Heart => HeartState,
            _ => null,
        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }
}
