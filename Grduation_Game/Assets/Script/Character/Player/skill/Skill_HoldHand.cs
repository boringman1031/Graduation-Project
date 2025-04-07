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

    public CharacterEventSO powerChangeEvent;

    private bool isActivated = false; // �O�_���\�Ұʧޯ�

    void costPower(CharactorBase _Charater) //������q
    {
        _Charater.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(_Charater);
    }

    // ��@ ISkillEffect ����
    public void SetOrigin(Transform origin)
    {
        this.origin = origin;

        // ���ը��o���a�� CharactorBase
        CharactorBase character = origin.GetComponent<CharactorBase>();
        if (character == null)
        {
            Debug.LogWarning("�I��̯ʤ� CharactorBase ����A�L�k�I��ޯ�C");
            Destroy(gameObject);
            return;
        }

        // �ˬd��q�O�_����
        if (character.CurrentPower < energyCost)
        {
            Debug.Log("��q�����A�L�k�I��ޯ�");
            Destroy(gameObject);
            return;
        }

        // ������q
        costPower(character);
        isActivated = true;

        // �p�⥿�T�ͦ���m�]�̷Ӫ��a���V�^
        int faceDir = origin.localScale.x >= 0 ? 1 : -1;
        transform.position = origin.position + new Vector3(spawnOffset.x * faceDir, spawnOffset.y, spawnOffset.z);

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
        // Ĳ�o�ʵe
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Skill1");
        }
        else if (animator != null)
        {
            animator.SetTrigger("Skill1");
        }
    }

    public void SetPlayerAnimator(Animator animator)
    {
        playerAnimator = animator;
    }

    private void Update()
    {
        if (!isActivated) return;

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
