using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSocialNiuBi : MonoBehaviour, ISkillEffect
{
    private Transform origin;
    private Animator playerAnimator;

    public float effectDuration = 10f;
    public float tickInterval = 1f;
    public float damagePerTick = 10f;
    public float effectRadius = 5f;

    // 指派你的粒子特效
    public ParticleSystem socialEnergyEffect;

    // 命中特效（可選）
    public GameObject hitEffectPrefab;

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;

        // 先扣除玩家 10% 血量
        CharactorBase playerChar = origin.GetComponent<CharactorBase>();
        if (playerChar != null)
        {
            float healthToDeduct = playerChar.MaxHealth * 0.1f;
            playerChar.CurrentHealth -= healthToDeduct;
            if (playerChar.CurrentHealth < 0)
                playerChar.CurrentHealth = 0;

            playerChar.OnHealthChange?.Invoke(playerChar);
        }
    }

    public void SetPlayerAnimator(Animator animator)
    {
        playerAnimator = animator;
    }

    private void Start()
    {
        if (origin == null)
        {
            Debug.LogError("origin is not set in SkillSocialNiuBi.");
            return;
        }
        transform.position = origin.position;
        if (socialEnergyEffect != null)
        {
            socialEnergyEffect.Play();
        }
        StartCoroutine(EffectCoroutine());
    }

    private IEnumerator EffectCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < effectDuration)
        {
            if (origin != null)
            {
                // 不斷跟隨玩家
                transform.position = origin.position;
            }

            // 取得範圍內所有碰撞
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, effectRadius);

            foreach (Collider2D hit in hits)
            {
                // 先拿到對象的 CharactorBase
                CharactorBase enemy = hit.GetComponent<CharactorBase>();
                if (enemy == null) continue;

                // 如果是玩家自己，就跳過
                if (enemy == origin.GetComponent<CharactorBase>())
                {
                    continue;
                }

                // 若處於霸體狀態則不受傷害
                if (enemy.SuperArmour) continue;

                // 計算傷害
                float newHealth = enemy.CurrentHealth - damagePerTick;
                if (newHealth > 0)
                {
                    enemy.CurrentHealth = newHealth;
                    enemy.OnTakeDamage?.Invoke(transform);

                    // 命中特效
                    if (hitEffectPrefab != null)
                    {
                        Instantiate(hitEffectPrefab, enemy.transform.position, Quaternion.identity);
                    }
                }
                else
                {
                    enemy.CurrentHealth = 0;
                    enemy.OnDead?.Invoke();
                }
                enemy.OnHealthChange?.Invoke(enemy);
            }

            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }

        // 效果結束後銷毀自己
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }
}
