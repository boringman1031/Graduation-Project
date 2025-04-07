using System.Collections;
using UnityEngine;

public class SkillEmo : MonoBehaviour, ISkillEffect
{
    private Transform origin;

    [Header("技能參數")]
    public float duration = 10f;           // 技能持續時間
    public float spawnInterval = 0.5f;     // 負面效果生成間隔
    public float damagePerSecond = 10f;    // 負面效果傷害（每秒）
    public float healthDeductionPercent = 0.1f; // 扣除血量百分比（10%）

    [Header("生成位置參數")]
    public float spawnRangeX;         // 水平生成範圍（左右）
    public float spawnOffsetY;      // 垂直偏移（離玩家位置）

    [Header("負面能量效果預置物")]
    public GameObject negativeEffectPrefab; // 負面能量效果 prefab
    public AudioClip skillSound;

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        // 扣除玩家 10% 血量（以 MaxHealth 為基準）
        CharactorBase playerChar = origin.GetComponent<CharactorBase>();
        if (playerChar != null)
        {
            float deduction = playerChar.MaxHealth * healthDeductionPercent;
            playerChar.CurrentHealth -= deduction;
            if (playerChar.CurrentHealth < 0)
                playerChar.CurrentHealth = 0;
            playerChar.OnHealthChange?.Invoke(playerChar);
        }
    }

    public void SetPlayerAnimator(Animator animator)
    {
        // 不需使用
    }

    private void Start()
    {
        if (origin == null)
        {
            Debug.LogError("SkillEmo: origin is not set!");
            Destroy(gameObject);
            return;
        }
        // 將技能物件設為玩家的子物件，並歸零 localPosition
        transform.SetParent(origin);
        transform.localPosition = Vector3.zero;

        if (skillSound != null)
        {
            AudioSource.PlayClipAtPoint(skillSound, transform.position);
        }

        // 開始生成負面能量效果
        StartCoroutine(SpawnNegativeEffects());
        // 結束技能效果後銷毀自身
        StartCoroutine(EndSkillAfterDuration());
    }

    private IEnumerator SpawnNegativeEffects()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            SpawnNegativeEffect();
            yield return new WaitForSeconds(spawnInterval);
            elapsed += spawnInterval;
        }
    }

    private void SpawnNegativeEffect()
    {
        if (negativeEffectPrefab != null)
        {
            // 隨機產生左右偏移
            float offsetX = Random.Range(-spawnRangeX, spawnRangeX);
            Vector3 spawnPos = origin.position + new Vector3(offsetX, spawnOffsetY, 0);
            // 生成的效果直接設為玩家的子物件，這樣會隨玩家移動
            Instantiate(negativeEffectPrefab, spawnPos, Quaternion.identity);
        }
    }

    private IEnumerator EndSkillAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
