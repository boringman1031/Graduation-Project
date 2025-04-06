using System.Collections;
using UnityEngine;

public class Skill_FinalDance : MonoBehaviour, ISkillEffect
{
    [Header("技能參數")]
    public float duration = 10f;            // 技能持續時間 10 秒
    public float damagePerSecond = 100f;      // 每秒傷害 100
    public float energyCost = 50f;            // 消耗能量 50
    public float auraRadius = 5f;             // 技能影響範圍

    [Header("音樂與特效設定")]
    public AudioClip backgroundMusic;         // 技能期間播放的背景音樂
    public GameObject finalDanceEffectPrefab;   // 持續特效預製物
    public Vector3 effectSpawnOffset = new Vector3(0, -1f, 0); // 特效生成位置偏移

    private Transform origin;                // 觸發技能的玩家 Transform
    private AudioSource musicSource;         // 用來播放背景音樂
    private GameObject effectInstance;       // 持續特效的實例

    public CharacterEventSO powerChangeEvent;

    void costPower(CharactorBase _Charater) //扣除能量
    {
        _Charater.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(_Charater);
    }
    public void SetPlayerAnimator(Animator animator)
    {
        // 可依需求實作
    }

    // 此方法由玩家技能動畫事件呼叫
    public void SetOrigin(Transform originTransform)
    {
        origin = originTransform;

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

        // 扣除能量
        costPower(character);
        //character.CurrentPower -= energyCost;

        // 生成持續特效，並設為玩家的子物件讓其跟隨玩家
        if (finalDanceEffectPrefab != null)
        {
            effectInstance = Instantiate(finalDanceEffectPrefab, origin.position + effectSpawnOffset, Quaternion.identity);
            effectInstance.transform.SetParent(origin);
        }

        // 播放背景音樂：在玩家上新增一個 AudioSource 來播放音樂，並設置為循環
        if (backgroundMusic != null)
        {
            musicSource = origin.gameObject.AddComponent<AudioSource>();
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }

        // 啟動持續傷害效果
        StartCoroutine(FinalDanceRoutine());
    }

    private IEnumerator FinalDanceRoutine()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            DealAuraDamage();
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }
        // 結束技能後停止背景音樂
        if (musicSource != null)
        {
            musicSource.Stop();
            Destroy(musicSource);
        }
        // 銷毀持續特效
        if (effectInstance != null)
        {
            Destroy(effectInstance);
        }
        Destroy(gameObject);
    }

    private void DealAuraDamage()
    {
        // 取得玩家周圍範圍內所有 Collider2D
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin.position, auraRadius);
        foreach (Collider2D hit in hits)
        {
            // 排除玩家本身，對具有 CharactorBase 組件的物件造成傷害
            CharactorBase enemy = hit.GetComponent<CharactorBase>();
            if (enemy != null && !enemy.CompareTag("Player"))
            {
                enemy.TakeDamage(damagePerSecond, transform);
            }
        }
    }

    // 在 Scene 編輯器中顯示技能影響範圍，方便調試
    private void OnDrawGizmosSelected()
    {
        if (origin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(origin.position, auraRadius);
        }
    }
}
