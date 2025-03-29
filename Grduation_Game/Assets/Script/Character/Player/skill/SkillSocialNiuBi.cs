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

    // �����A���ɤl�S��
    public ParticleSystem socialEnergyEffect;

    // �R���S�ġ]�i��^
    public GameObject hitEffectPrefab;

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;

        // ���������a 10% ��q
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
                // ���_���H���a
                transform.position = origin.position;
            }

            // ���o�d�򤺩Ҧ��I��
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, effectRadius);

            foreach (Collider2D hit in hits)
            {
                // �������H�� CharactorBase
                CharactorBase enemy = hit.GetComponent<CharactorBase>();
                if (enemy == null) continue;

                // �p�G�O���a�ۤv�A�N���L
                if (enemy == origin.GetComponent<CharactorBase>())
                {
                    continue;
                }

                // �Y�B���Q�骬�A�h�����ˮ`
                if (enemy.SuperArmour) continue;

                // �p��ˮ`
                float newHealth = enemy.CurrentHealth - damagePerTick;
                if (newHealth > 0)
                {
                    enemy.CurrentHealth = newHealth;
                    enemy.OnTakeDamage?.Invoke(transform);

                    // �R���S��
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

        // �ĪG������P���ۤv
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }
}
