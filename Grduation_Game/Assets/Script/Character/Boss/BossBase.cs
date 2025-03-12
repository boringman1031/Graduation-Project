using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase :MonoBehaviour
{
    [Header("��¦�ƭ�")]
    public float maxHealth = 1000f;
    private float currentHealth;
    [HideInInspector] public Animator anim;

    [Header("�s���ƥ�")]
    public VoidEventSO CameraShakeEvent;
    public VoidEventSO BossDeadEvent;

    [Header("�ƥ��ť")]
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
    public void OnShow()//Boss�X��
    {
        CameraShakeEvent.RaiseEvent();//�۾��_��
    }
    public void OnTakeDamage()//Boss����ˮ`
    {
        anim.SetTrigger("Hit");
        currentHealth -= 10;      

        if (currentHealth <= maxHealth / 2)
        {
            EnterPhaseTwo(); // �����ܲĤG���q
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void OnUseSkill1()
    {
        //TODO: �ͦ��@���z��
        Debug.Log("Use Skill 1");
    }

    public void OnUseSkill2()
    {
        //TODO: �ͦ��@���z��
        Debug.Log("Use Skill 2");
    }

    public void OnSummon()//�l��p��
    {
        //TODO: �ͦ��@��p��
        Debug.Log("Summon");
    }

    public void EnterPhaseTwo()//�i�J�ĤG���q
    {

    }

    public void Die()
    {
        BossDeadEvent.RaiseEvent();
    }
}
