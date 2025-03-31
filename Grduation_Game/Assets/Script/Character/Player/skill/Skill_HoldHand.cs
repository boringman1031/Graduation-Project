using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHoldHand : MonoBehaviour, ISkillEffect
{
    [Header("�ޯ�ƭ�")]
    public float damage = 50f;
    public float defenseReduction = 10f;
    public float energyCost = 10f;
    public float attackRadius = 1f;

    [Header("�ĪG�]�w")]
    public AudioClip skillSound;
    public ParticleSystem skillEffect;
    public Animator animator; // �w�m���ۨ��� Animator�A�i��

    [Header("��L�]�w")]
    public LayerMask targetLayer;
    public Vector3 spawnOffset; // �Ҧp (1, 0, 0)

    [Header("�ʺA�Ѧ�")]
    public Transform origin; // ���a�Υͦ��I�Ѧ�
    public Animator playerAnimator; // ���a���⪺ Animator

    private bool hasAttacked = false;

    // ��@ ISkillEffect ����
    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        // �ھڪ��a��m�P�¦V�p�⥿�T�ͦ���m
        int faceDir = origin.localScale.x >= 0 ? 1 : -1;
        transform.position = origin.position + new Vector3(spawnOffset.x * faceDir, spawnOffset.y, spawnOffset.z);
    }

    public void SetPlayerAnimator(Animator animator)
    {
        playerAnimator = animator;
    }

    private void Start()
    {
        // ����ޯ୵��
        if (skillSound != null)
        {
            AudioSource.PlayClipAtPoint(skillSound, transform.position);
        }
        // ����S��
        if (skillEffect != null)
        {
            skillEffect.Play();
        }
        // Ĳ�o���a�ʵe
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Skill1");
        }
        else if (animator != null)
        {
            animator.SetTrigger("Skill1");
        }
    }

    private void Update()
    {
        if (!hasAttacked)
        {
            Attack();
            hasAttacked = true;
        }
        Destroy(gameObject, 1f);
    }

    private void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius, targetLayer);
        foreach (Collider2D hit in hits)
        {
            CharactorBase target = hit.GetComponent<CharactorBase>();
            if (target != null)
            {
                target.TakeDamage(damage, transform);
                target.Defence -= defenseReduction;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
