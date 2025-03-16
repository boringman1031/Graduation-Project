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
    public float currentHealth;

    [Header("���A")]
    public bool isSummonMinion;   
    public float SuperArmourTime;//�Q��ɶ�
    private float SuperArmourTimeCounter;//�Q��ɶ��p�ƾ�
    public bool SuperArmour;//�O�_�b�Q�骬�A

    private BossBaseState currentState;
    protected BossBaseState attackState;//�������A
    protected BossBaseState summonState;//�l�ꪬ�A  
    protected BossBaseState summonHeartState;
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        isSummonMinion = false;
    }

    private void OnEnable()
    {
        currentState = attackState;
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
        if (SuperArmour)
        {
            SuperArmourTimeCounter -= Time.deltaTime;
            if (SuperArmourTimeCounter <= 0)
            {
                SuperArmour = false;
            }
        }
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();
    }
    public void OnShakeCamera()//�۾��_��
    {
        CameraShakeEvent.RaiseEvent();
    }
    public void OnTakeDamage()//Boss����ˮ`
    {
        if(SuperArmour)
        {
            return;
        }  
        if(currentHealth > 0)
        {
            anim.SetTrigger("Hit");
            currentHealth -= 100;
            TriggerSuperArmour();
            Debug.Log("Boss����ˮ`�A��e��q�G" + currentHealth);
        }     
        else
        {
            Die();
        }
    }
    private void TriggerSuperArmour()//Ĳ�o�Q��
    {
        if (!SuperArmour)
        {
            SuperArmour = true;
            SuperArmourTimeCounter = SuperArmourTime;
        }
    }
    public virtual void OnAttack()//Boss����
    {
        anim.SetTrigger("Attack");      
    }

    public virtual void OnSummon()//Boss�l��
    {
        anim.SetTrigger("Summon");     
    }

    public virtual void SpawnHeartMinion()//�ͦ��R�ߤp��
    {
        anim.SetTrigger("Hit");          
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
            BossState.SummonHeart => summonHeartState,
            _ => null,
        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }

    public bool CheckMinionsExist()
    {
        // �ˬd�������O�_�٦��p��
        GameObject[] minions = GameObject.FindGameObjectsWithTag("Enemy");
        bool hasMinions = minions.Length > 0;     
        return hasMinions; // �p�G���p�ǫh��^ `true`�A�_�h��^ `false`
    }

    public bool CheckHeartMinionsExist()
    {
        // �ˬd�������O�_�٦��R�ߤp��
        GameObject[] heartMinions = GameObject.FindGameObjectsWithTag("Heart");
        bool hasHeartMinions = heartMinions.Length > 0;    
        return hasHeartMinions; // �p�G���R�ߤp�ǫh��^ `true`�A�_�h��^ `false`
    }
}
