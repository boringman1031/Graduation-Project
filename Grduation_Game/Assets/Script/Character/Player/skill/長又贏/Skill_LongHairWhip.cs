using UnityEngine;

public class Skill_LongHairWhip : MonoBehaviour, ISkillEffect
{
    [Header("技能參數")]
    public float energyCost = 50f;
    public float damage = 100f;
    public float radius = 2f;
    public float buffDuration = 10f;
    public float speedBuff = 30f;
    public float healthBuff = 10f;

    [Header("特效")]
    public GameObject effectPrefab;
    public Vector3 effectOffset;
    public AudioClip whipSound;
    public AudioDefination audioPlayer;

    private Transform origin;

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        transform.position = origin.position;

        var character = origin.GetComponent<CharactorBase>();
        var stats = origin.GetComponent<PlayerStats>();
        var buffManager = origin.GetComponent<BuffManager>();

        if (character == null || stats == null || buffManager == null)
        {
            Debug.LogWarning("缺少必要元件");
            Destroy(gameObject);
            return;
        }

        if (character.CurrentPower < energyCost)
        {
            Debug.Log("能量不足，無法施放技能");
            Destroy(gameObject);
            return;
        }

        // 扣能量
        character.AddPower(-energyCost);

        // 播放特效
        if (effectPrefab != null)
        {
            GameObject fx = Instantiate(effectPrefab, origin.position + effectOffset, Quaternion.identity);
            fx.transform.SetParent(origin); // 綁定玩家可選
            Destroy(fx, 2f);
        }

        // 播音效
        if (audioPlayer && whipSound)
        {
            audioPlayer.audioClip = whipSound;
            audioPlayer.PlayAudioClip();
        }

        // AoE 傷害
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin.position, radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player")) continue;

            var enemy = hit.GetComponent<CharactorBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, transform);

                // 擊飛邏輯（可選）
                Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 knockback = (enemy.transform.position - origin.position).normalized * 5f;
                    rb.AddForce(knockback, ForceMode2D.Impulse);
                }
            }
        }

        // Buff：速度 + 血量
        buffManager.ApplySpeedBuff(speedBuff, buffDuration);
        buffManager.ApplyMaxHealthBuff(healthBuff, buffDuration);

        Destroy(gameObject); // 技能執行完畢自銷
    }

    public void SetPlayerAnimator(Animator animator) { }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (origin != null)
            Gizmos.DrawWireSphere(origin.position, radius);
    }
}
