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

    public CharacterEventSO powerChangeEvent;

    private bool isActivated = false; // 是否成功啟動技能

    void costPower(CharactorBase _Charater) //扣除能量
    {
        _Charater.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(_Charater);
    }

    // 實作 ISkillEffect 介面
    public void SetOrigin(Transform origin)
    {
        this.origin = origin;

        // 嘗試取得玩家的 CharactorBase
        CharactorBase character = origin.GetComponent<CharactorBase>();
        if (character == null)
        {
            Debug.LogWarning("施放者缺少 CharactorBase 元件，無法施放技能。");
            Destroy(gameObject);
            return;
        }

        // 檢查能量是否足夠
        if (character.CurrentPower < energyCost)
        {
            Debug.Log("能量不足，無法施放技能");
            Destroy(gameObject);
            return;
        }

        // 扣除能量
        costPower(character);
        isActivated = true;

        // 計算正確生成位置（依照玩家面向）
        int faceDir = origin.localScale.x >= 0 ? 1 : -1;
        transform.position = origin.position + new Vector3(spawnOffset.x * faceDir, spawnOffset.y, spawnOffset.z);

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
        // 觸發動畫
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
