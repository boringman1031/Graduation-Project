using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class SkillLoveSong : MonoBehaviour, ISkillEffect
{
    [Header("技能參數")]
    public float speedBoost = 4f;       // 提升的速度數值
    public float boostDuration = 2f;    // 加速持續時間（秒）
    public float projectileSpeed = 25f; // 飛行道具速度
    public float damage = 50f;          // 傷害數值
    public float energyCost = 10f;      // 消耗能量

    [Header("音效特效設定")]
    public AudioDefination audioPlayer; // 音效播放器
    public AudioClip spawnSound;        // 技能生成音效
    public AudioClip hitSound;          // 命中敵人音效
    public GameObject hitEffect;          // 命中敵人音效

    private Transform origin;

    public CharacterEventSO powerChangeEvent;

    void costPower(CharactorBase _Charater) //扣除能量
    {
        _Charater.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(_Charater);
    }
    // 這個方法會在玩家技能動畫事件中呼叫
    public void SetOrigin(Transform originTransform)
    {
        origin = originTransform;
        transform.position = origin.position;

        // 取得玩家生命與能量管理組件 (例如 CharactorBase)
        CharactorBase character = origin.GetComponent<CharactorBase>();
        if (character == null)
        {
            Debug.LogWarning("未找到 CharactorBase 組件，無法施放技能");
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
        costPower(character); //扣除能量

        // 播放生成時音效
        if (audioPlayer != null && spawnSound != null)
        {
            audioPlayer.audioClip = spawnSound;
            audioPlayer.PlayAudioClip();
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
            if (audioPlayer != null && hitSound != null)
            {
                audioPlayer.audioClip = hitSound;
                audioPlayer.PlayAudioClip();
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
