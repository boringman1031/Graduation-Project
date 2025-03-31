using System.Collections;
using UnityEngine;

public class SkillLoveSong : MonoBehaviour, ISkillEffect
{
    [Header("技能參數")]
    public float speedBoost = 4f;       // 提升的速度數值
    public float boostDuration = 2f;    // 加速持續時間（秒）
    public float projectileSpeed = 25f; // 飛行道具速度
    public float damage = 50f;          // 傷害數值
    public float energyCost = 10f;      // 消耗能量

    [Header("音效特效設定")]
    public AudioClip spawnSound;        // 技能生成音效
    public AudioClip hitSound;          // 命中敵人音效
    public GameObject hitEffect;          // 命中敵人音效

    private Transform origin;

    // 這個方法會在玩家技能動畫事件中呼叫
    public void SetOrigin(Transform originTransform)
    {
        origin = originTransform;
        transform.position = origin.position;

        // 播放生成時音效
        if (spawnSound != null)
        {
            AudioSource.PlayClipAtPoint(spawnSound, transform.position);
        }

        // 根據玩家面向決定飛行方向
        float direction = -origin.localScale.x;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(projectileSpeed * direction, 0f);
        }

        // 呼叫玩家的 ApplySpeedBoost 方法，讓速度提升邏輯在玩家身上執行
        PlayerController player = origin.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ApplySpeedBoost(speedBoost, boostDuration);
        }

        // 3 秒後自動銷毀技能 prefab
        Destroy(gameObject, 1f);
    }
    public void SetPlayerAnimator(Animator animator)
    {
        // 如有需要可以同步動畫參數
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 當技能碰到敵人時，對敵人造成傷害並播放命中音效
        CharactorBase enemy = collision.GetComponent<CharactorBase>();
        if (collision.CompareTag("Player"))
            return;

        if (enemy != null)
        {
            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }

            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            enemy.TakeDamage(damage, transform);

            Destroy(gameObject);
        }
    }
}
