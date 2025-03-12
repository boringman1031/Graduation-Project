using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase :MonoBehaviour
{
    [Header("基礎數值")]
    public float maxHealth = 1000f;
    private float currentHealth;
    [HideInInspector] public Animator anim;

    [Header("廣播事件")]
    public VoidEventSO CameraShakeEvent;
    public VoidEventSO BossDeadEvent;

    [Header("事件監聽")]
    public VoidEventSO AttackBossEvent;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        AttackBossEvent.OnEventRaised += OnTakeDamage;
    }

    private void OnDisable()
    {
        AttackBossEvent.OnEventRaised -= OnTakeDamage;
    }
    public void OnShow()//Boss出場
    {
        CameraShakeEvent.RaiseEvent();//相機震動
    }
    public void OnTakeDamage()//Boss受到傷害
    {
        anim.SetTrigger("Hit");
        currentHealth -= 10;      

        if (currentHealth <= maxHealth / 2)
        {
            EnterPhaseTwo(); // 切換至第二階段
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void OnUseSkill1()
    {
        //TODO: 生成一堆爆炸
        Debug.Log("Use Skill 1");
    }

    public void OnUseSkill2()
    {
        //TODO: 生成一堆爆炸
        Debug.Log("Use Skill 2");
    }

    public void OnSummon()//召喚小怪
    {
        //TODO: 生成一堆小怪
        Debug.Log("Summon");
    }

    public void EnterPhaseTwo()//進入第二階段
    {

    }

    public void Die()
    {
        BossDeadEvent.RaiseEvent();
    }
}
