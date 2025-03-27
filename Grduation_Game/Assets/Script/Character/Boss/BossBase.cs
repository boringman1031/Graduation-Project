using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase :MonoBehaviour
{
    [HideInInspector] public Animator anim;
    [Header("UI�ե�")]
    public BossHealthUI bossHealthUI;

    [Header("�s���ƥ�")]
    public VoidEventSO CameraShakeEvent;
    public VoidEventSO BossDeadEvent;
    public VoidEventSO tutorialBossSummonEvent;
    public VoidEventSO tutorialBossAttackEvent;

    [Header("�ƥ��ť")]
    public VoidEventSO AttackBossEvent;
    public VoidEventSO dialogEndEvent; // ��ܵ����ƥ�


    [Header("��¦�ƭ�")]
    public float maxHealth;
    public float currentHealth;

    [Header("���A")]
    public bool isSummonMinion;   
    public float SuperArmourTime;//�Q��ɶ�
    private float SuperArmourTimeCounter;//�Q��ɶ��p�ƾ�
    public bool SuperArmour;//�O�_�b�Q�骬�A
    public bool isTalk = false;

    private BossBaseState currentState;
    protected BossBaseState idleState;//���m���A
    protected BossBaseState attackState;//�������A
    protected BossBaseState summonState;//�l�ꪬ�A  
    protected BossBaseState summonHeartState;
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        isSummonMinion = false;
        // ��l�Ʀ��
        if (bossHealthUI != null)
            bossHealthUI.UpdateHealth(currentHealth, maxHealth);
    }

    private void OnEnable()
    {
        currentState = idleState;
        currentState.OnEnter(this);
        AttackBossEvent.OnEventRaised += OnTakeDamage;
        dialogEndEvent.OnEventRaised += OnDialogEnd;
    }

    private void OnDisable()
    {
        currentState.OnExit();
        AttackBossEvent.OnEventRaised -= OnTakeDamage;
        dialogEndEvent.OnEventRaised -= OnDialogEnd;
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

    //-----------Boss�欰----------------
    public virtual void OnBossShow()//�۾��_��
    {          
        CameraShakeEvent.RaiseEvent();
        bossHealthUI.gameObject.SetActive(true); // ������
    }

    public void OnDialogEnd()//��ܵ���
    {
        if (isTalk)
        {
            SwitchState(BossState.Attack);
        }
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
            // ��s���
            if (bossHealthUI != null)
                bossHealthUI.UpdateHealth(currentHealth, maxHealth);
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
        tutorialBossAttackEvent.RaiseEvent();//�s���}�ҧ����ƥ�о�
    }

    public virtual void OnSummon()//Boss�l��
    {
        anim.SetTrigger("Summon");
        tutorialBossSummonEvent.RaiseEvent();//�s���}�ҥl��ƥ�о�
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
            BossState.Idle => idleState,
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
