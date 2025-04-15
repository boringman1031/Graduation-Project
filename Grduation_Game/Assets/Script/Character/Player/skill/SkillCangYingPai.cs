using System.Collections;
using UnityEngine;

/// <summary>
/// 蒼蠅拍動畫技能：拍擊地板造成傷害，短暫提升移動速度，有能量消耗限制
/// </summary>
public class SkillCangYingPaiAnim : MonoBehaviour, ISkillEffect
{
    private Transform origin;          // 技能施放者
    private Animator anim;             // 技能物件上的 Animator

    [Header("技能參數")]
    public float energyCost = 20f;     // 施放技能消耗的能量
    public float damage = 50f;         // 技能造成的傷害
    public float speedBuff = 10f;      // 技能期間給玩家的速度加成
    public float buffDuration = 3f;    // 速度加成持續時間（秒）

    [Header("音效")]
    public AudioDefination audioPlayer; // 音效播放器
    public AudioClip triggerSound;     // 技能觸發音效
    public AudioClip impactSound;      // 拍擊地板時播放的音效

    [Header("特效")]
    public GameObject impactEffectPrefab; // 拍擊地板時產生的特效

    [Header("事件")]
    public CharacterEventSO powerChangeEvent; // 扣能量後更新 UI 用事件

    private bool isActivated = false;  // 用來判斷是否成功啟動技能（有扣到能量）

    /// <summary>
    /// 扣除玩家能量，並觸發 UI 更新事件
    /// </summary>
    void costPower(CharactorBase _Charater)
    {
        _Charater.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(_Charater);
    }

    /// <summary>
    /// 技能動畫播放完畢時由 Animation Event 呼叫，結束技能
    /// </summary>
    public void OnAnimationFinish()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 動畫事件：拍擊地板發生時播放音效與特效
    /// </summary>
    public void OnImpact()
    {
        if (!isActivated) return;

        // 播放地板撞擊音效
        if (audioPlayer != null && impactSound != null)
        {
            audioPlayer.audioClip = impactSound;
            audioPlayer.PlayAudioClip();
        }

        // 產生地板撞擊特效
        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }

        // 技能結束（如需延遲或其他收尾，也可改為延遲 Destroy）
        Destroy(gameObject);
    }

    /// <summary>
    /// 設定技能的施放來源（玩家），並檢查能量是否足夠
    /// </summary>
    public void SetOrigin(Transform origin)
    {
        this.origin = origin;

        CharactorBase playerChar = origin.GetComponent<CharactorBase>();
        if (playerChar == null)
        {
            Debug.LogWarning("CharactorBase 未找到，技能取消");
            Destroy(gameObject);
            return;
        }

        // 能量不足，技能取消
        if (playerChar.CurrentPower < energyCost)
        {
            Debug.Log("能量不足，無法施放技能！");
            Destroy(gameObject);
            return;
        }

        // 扣除能量與 UI 更新
        costPower(playerChar);
        isActivated = true;
    }

    /// <summary>
    /// 設定動畫控制器（目前無需實作，可留空）
    /// </summary>
    public void SetPlayerAnimator(Animator animator)
    {
        // 如有需要可以同步動畫參數
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        // 若未成功啟動技能（例如能量不足），中止技能
        if (!isActivated || origin == null)
        {
            Debug.LogWarning("技能未啟動或 origin 為 null，銷毀");
            Destroy(gameObject);
            return;
        }

        // 設為玩家子物件，跟隨位置
        transform.SetParent(origin);
        transform.localPosition = Vector3.zero;

        // 播放技能觸發音效
        if (audioPlayer != null && triggerSound != null)
        {
            audioPlayer.audioClip = triggerSound;
            audioPlayer.PlayAudioClip();
        }

        // 提升玩家速度，並啟動計時器恢復原速度
        PlayerController pc = origin.GetComponent<PlayerController>();
        if (pc != null)
        {
            float originalSpeed = pc.Speed;
            pc.Speed += speedBuff;
            StartCoroutine(RestoreSpeed(pc, originalSpeed, buffDuration));
        }
    }

    /// <summary>
    /// 在 buff 持續時間後恢復原速度
    /// </summary>
    private IEnumerator RestoreSpeed(PlayerController pc, float originalSpeed, float delay)
    {
        yield return new WaitForSeconds(delay);
        pc.Speed = originalSpeed;
    }

    /// <summary>
    /// 技能碰撞到敵人時造成傷害（忽略玩家）
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActivated) return;

        // 不處理玩家自己
        if (collision.CompareTag("Player"))
            return;

        CharactorBase target = collision.GetComponent<CharactorBase>();
        if (target != null)
        {
            float newHealth = target.CurrentHealth - damage;

            if (newHealth > 0)
            {
                target.CurrentHealth = newHealth;
                target.OnTakeDamage?.Invoke(transform);
            }
            else
            {
                target.CurrentHealth = 0;
                target.OnDead?.Invoke();
            }

            target.OnHealthChange?.Invoke(target);
        }
    }
}
