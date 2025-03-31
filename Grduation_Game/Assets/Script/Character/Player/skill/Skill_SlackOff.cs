using System.Collections;
using UnityEngine;

public class Skill_SlackOff : MonoBehaviour, ISkillEffect
{
    [Header("技能參數")]
    public float duration = 10f;         // 持續時間 10 秒
    public float damagePerSecond = 10f;    // 每秒造成 10 傷害
    public float energyCost = 20f;         // 消耗能量 20
    [Header("回血設定")]
    public float healPercentage = 0.1f;    // 回復 10% 血量

    [Header("範圍設定")]
    public float auraRadius = 5f;          // 技能效果範圍

    [Header("音效設定")]
    public AudioClip activationSound;      // 技能激活音效
    public AudioClip auraDamageSound;      // 每次傷害播放的音效 (可選)

    [Header("特效設定")]
    public GameObject auraEffectPrefab;    // 持續跟隨玩家的特效
    // 你可以定義左右兩側的偏移量，例如左側偏移 (-1,1,0)，右側偏移 (1,1,0)
    public Vector3 leftEffectOffset = new Vector3(-1f, 1f, 0);
    public Vector3 rightEffectOffset = new Vector3(1f, 1f, 0);

    private Transform origin;              // 觸發技能的玩家 Transform

    public void SetPlayerAnimator(Animator animator)
    {
        // 可依需求實作
    }

    // 此方法由玩家技能動畫事件呼叫
    public void SetOrigin(Transform originTransform)
    {
        origin = originTransform;

        // 播放激活音效
        if (activationSound != null)
        {
            AudioSource.PlayClipAtPoint(activationSound, origin.position);
        }

        // 取得玩家的生命與能量管理組件 (例如 CharactorBase)
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
        // 扣除能量
        character.CurrentPower -= energyCost;

        // 回復血量：回復 10% 的最大血量
        float healAmount = character.MaxHealth * healPercentage;
        character.AddHealth(healAmount);

        // 生成特效，並設為玩家的子物件，讓它一直跟隨玩家
        if (auraEffectPrefab != null)
        {
            // 生成左側特效，Z軸旋轉90度
            GameObject leftEffect = Instantiate(auraEffectPrefab, origin.position + leftEffectOffset, Quaternion.Euler(0, 0, 90));
            leftEffect.transform.SetParent(origin);

            // 生成右側特效，Z軸旋轉-90度
            GameObject rightEffect = Instantiate(auraEffectPrefab, origin.position + rightEffectOffset, Quaternion.Euler(0, 0, -90));
            rightEffect.transform.SetParent(origin);
        }

        // 開始技能持續效果，對周圍敵人造成傷害
        StartCoroutine(AuraEffectRoutine());
    }

    private IEnumerator AuraEffectRoutine()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            DealAuraDamage();
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }
        // 10 秒後銷毀技能物件
        Destroy(gameObject);
    }

    private void DealAuraDamage()
    {
        // 取得玩家周圍範圍內所有 Collider2D
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin.position, auraRadius);
        foreach (Collider2D hit in hits)
        {
            // 檢查對象是否為敵人 (假設敵人有 CharactorBase 且 tag 不是 "Player")
            CharactorBase enemy = hit.GetComponent<CharactorBase>();
            if (enemy != null && !enemy.CompareTag("Player"))
            {
                // 播放傷害音效 (可選)
                if (auraDamageSound != null)
                {
                    AudioSource.PlayClipAtPoint(auraDamageSound, hit.transform.position);
                }
                // 傳遞傷害數值
                enemy.TakeDamage(damagePerSecond, transform);
            }
        }
    }

    // 在 Scene 編輯器中顯示範圍 (方便調試)
    private void OnDrawGizmosSelected()
    {
        if (origin != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(origin.position, auraRadius);
        }
    }
}
