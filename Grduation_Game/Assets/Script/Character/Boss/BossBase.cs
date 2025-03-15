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
        Debug.Log("Boss �����I");
    }

    public virtual void OnSummon()//Boss�l��
    {
        anim.SetTrigger("Summon");   
    }

    public virtual void SpawnHeartMinion()//�ͦ��R�ߤp��
    {
        anim.SetTrigger("Hit");
        Debug.Log("Boss �ͦ��R�ߤp�ǡI");
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
            _ => null,
        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }

    public bool CheckMinionsExist()
    {
        //�ˬd�������O�_�٦��p��
        GameObject[] minions = GameObject.FindGameObjectsWithTag("Minion");
        return minions.Length > 0; // �p�G���p�ǫh��^ `true`�A�_�h��^ `false`
    }
}
