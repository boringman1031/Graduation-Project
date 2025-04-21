using UnityEngine;

public class SkillCangYingPaiAnim : MonoBehaviour, ISkillEffect
{
    private Transform origin;
    private Animator anim;
    private CharactorBase character;
    private PlayerStats stats;
    private BuffManager buffManager;

    [Header("技能參數")]
    public float energyCost = 20f;
    public float baseDamage = 50f;
    public float speedBuff = 10f;
    public float buffDuration = 3f;

    [Header("音效與特效")]
    public AudioDefination audioPlayer;
    public AudioClip triggerSound;
    public AudioClip impactSound;
    public GameObject impactEffectPrefab;

    [Header("事件")]
    public CharacterEventSO powerChangeEvent;

    private bool isActivated = false;

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;

        character = origin.GetComponent<CharactorBase>();
        stats = origin.GetComponent<PlayerStats>();
        buffManager = origin.GetComponent<BuffManager>();

        if (character == null || stats == null || buffManager == null)
        {
            Debug.LogWarning("缺少必要元件，技能取消");
            Destroy(gameObject);
            return;
        }

        if (character.CurrentPower < energyCost)
        {
            Debug.Log("能量不足，無法施放技能！");
            Destroy(gameObject);
            return;
        }

        character.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(character);
        isActivated = true;
    }

    public void SetPlayerAnimator(Animator animator) { }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (!isActivated || origin == null)
        {
            Destroy(gameObject);
            return;
        }

        // 播放起始音效
        if (audioPlayer && triggerSound)
        {
            audioPlayer.audioClip = triggerSound;
            audioPlayer.PlayAudioClip();
        }

        // 設定位置與附加到玩家
        transform.SetParent(origin);
        transform.localPosition = Vector3.zero;

        // 使用 BuffManager 增加速度
        buffManager.ApplySpeedBuff(speedBuff, buffDuration);
    }

    public void OnImpact()
    {
        if (!isActivated) return;

        if (audioPlayer && impactSound)
        {
            audioPlayer.audioClip = impactSound;
            audioPlayer.PlayAudioClip();
        }

        if (impactEffectPrefab)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    public void OnAnimationFinish()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActivated || collision.CompareTag("Player")) return;

        CharactorBase target = collision.GetComponent<CharactorBase>();
        if (target != null)
        {
            float totalDamage = baseDamage + (stats?.attack ?? 0f);
            target.TakeDamage(totalDamage, transform);
        }
    }
}
