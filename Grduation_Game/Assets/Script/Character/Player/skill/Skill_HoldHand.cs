using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHoldHand : MonoBehaviour, ISkillEffect
{
    [Header("技能數值")]
    public float damage = 50f;
    public float defenseReduction = 10f;
    public float energyCost = 10f;
    public float attackRadius = 1f;

    [Header("效果設定")]
    public AudioClip skillSound;
    public ParticleSystem skillEffect;
    public Animator animator; // 預置物自身的 Animator，可選

    [Header("其他設定")]
    public LayerMask targetLayer;
    public Vector3 spawnOffset; // 例如 (1, 0, 0)

    [Header("動態參考")]
    public Transform origin; // 玩家或生成點參考
    public Animator playerAnimator; // 玩家角色的 Animator

    private bool hasAttacked = false;

    // 實作 ISkillEffect 介面
    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        // 根據玩家位置與朝向計算正確生成位置
        int faceDir = origin.localScale.x >= 0 ? 1 : -1;
        transform.position = origin.position + new Vector3(spawnOffset.x * faceDir, spawnOffset.y, spawnOffset.z);
    }

    public void SetPlayerAnimator(Animator animator)
    {
        playerAnimator = animator;
    }

    private void Start()
    {
        // 播放技能音效
        if (skillSound != null)
        {
            AudioSource.PlayClipAtPoint(skillSound, transform.position);
        }
        // 播放特效
        if (skillEffect != null)
        {
            skillEffect.Play();
        }
        // 觸發玩家動畫
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
