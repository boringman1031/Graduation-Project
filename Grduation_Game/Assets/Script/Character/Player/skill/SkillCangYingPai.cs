using System.Collections;
using UnityEngine;

public class SkillCangYingPaiAnim : MonoBehaviour, ISkillEffect
{
    private Transform origin;
    private Animator anim;

    [Header("技能參數")]
    public float energyCost = 20f;
    public float damage = 50f;
    public float speedBuff = 10f;
    public float buffDuration = 3f;

    [Header("音效")]
    public AudioClip triggerSound;  // 觸發技能時播放
    public AudioClip impactSound;   // 拍擊地板時播放

    [Header("特效")]
    public GameObject impactEffectPrefab; // 拍擊地板時產生的特效

    // 當技能動畫結束時，透過 Animation Event 呼叫此函數
    public void OnAnimationFinish()
    {
        Destroy(gameObject);
    }

    // 當動畫播放到拍擊地板的關鍵幀時，藉由 Animation Event 呼叫此方法
    public void OnImpact()
    {
        // 播放撞擊音效
        if (impactSound != null)
        {
            AudioSource.PlayClipAtPoint(impactSound, transform.position);
        }
        // 生成撞擊特效
        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }
        // 可根據需要，若同時也要造成傷害，可以在此處呼叫 OnTriggerEnter2D 邏輯或其他方法
        Destroy(gameObject);
    }

    // ISkillEffect 實作：設定技能發出者
    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        // 扣除能量（假設玩家的能量管理在 CharactorBase 中）
        CharactorBase playerChar = origin.GetComponent<CharactorBase>();
        if (playerChar != null)
        {
            playerChar.CurrentPower -= energyCost;
            if (playerChar.CurrentPower < 0)
                playerChar.CurrentPower = 0;
            playerChar.OnHealthChange?.Invoke(playerChar);
        }
    }

    // ISkillEffect 實作：設定玩家動畫（如有需要）
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
        if (origin == null)
        {
            Debug.LogError("SkillCangYingPaiAnim: origin is not set!");
            Destroy(gameObject);
            return;
        }

        // 讓技能 prefab 成為玩家的子物件，這樣它會跟隨玩家移動
        transform.SetParent(origin);
        // 如果你的動畫是以本地座標播放，則把位置重置為零（或依需求調整）
        transform.localPosition = Vector3.zero;

        // 播放技能觸發時音效
        if (triggerSound != null)
        {
            AudioSource.PlayClipAtPoint(triggerSound, transform.position);
        }

        // 給玩家短暫增加速度
        PlayerController pc = origin.GetComponent<PlayerController>();
        if (pc != null)
        {
            float originalSpeed = pc.Speed;
            pc.Speed += speedBuff;
            StartCoroutine(RestoreSpeed(pc, originalSpeed, buffDuration));
        }
    }

    private IEnumerator RestoreSpeed(PlayerController pc, float originalSpeed, float delay)
    {
        yield return new WaitForSeconds(delay);
        pc.Speed = originalSpeed;
    }

    // 當技能物件碰撞到敵人時執行傷害判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 排除玩家（假設玩家的 Tag 為 "Player"）
        if (collision.CompareTag("Player"))
            return;

        CharactorBase target = collision.GetComponent<CharactorBase>();
        if (target != null)
        {
            // 扣除傷害邏輯
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
