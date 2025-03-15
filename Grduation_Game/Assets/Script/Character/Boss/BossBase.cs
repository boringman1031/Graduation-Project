using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase :MonoBehaviour
{
    [HideInInspector] public Animator anim;

    [Header("廣播事件")]
    public VoidEventSO CameraShakeEvent;
    public VoidEventSO BossDeadEvent;

    [Header("事件監聽")]
    public VoidEventSO AttackBossEvent;

    [Header("基礎數值")]
    public float maxHealth;
    [HideInInspector] public float currentHealth;

    private BossBaseState currentState;
    protected BossBaseState attackState;//攻擊狀態
    protected BossBaseState summonState;//召喚狀態  
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
    public void OnShow()//Boss出場
    {
        CameraShakeEvent.RaiseEvent();//相機震動
    }
    public void OnTakeDamage()//Boss受到傷害
    {
        anim.SetTrigger("Hit");
        currentHealth -= 100;           
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public virtual void OnAttack()//Boss攻擊
    {
        anim.SetTrigger("Attack");
        Debug.Log("Boss 攻擊！");
    }

    public virtual void OnSummon()//Boss召喚
    {
        anim.SetTrigger("Summon");   
    }

    public virtual void SpawnHeartMinion()//生成愛心小怪
    {
        anim.SetTrigger("Hit");
        Debug.Log("Boss 生成愛心小怪！");
    }
    public void Die()//Boss死亡
    {
        BossDeadEvent.RaiseEvent();
    }

    public void SwitchState(BossState _state)//切換狀態
    {
        var newState = _state switch//根據現有狀態切換敵人狀態(switch的語法糖寫法)
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
        //檢查場景內是否還有小怪
        GameObject[] minions = GameObject.FindGameObjectsWithTag("Minion");
        return minions.Length > 0; // 如果有小怪則返回 `true`，否則返回 `false`
    }
}
