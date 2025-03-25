using UnityEngine;
using UnityEngine.InputSystem;

public class Skill_QianQianXiaoShou : MonoBehaviour
{
    [Header("技能參數")]
    public float damage = 30f;
    public float cooldown = 3f;
    public float attackRange = 2f;
    private float lastUsedTime;

    [Header("引用組件")]
    public Animator anim;
    public Transform attackPoint;
    public GameObject skillEffectPrefab; // 改為 Prefab 引用
    public AudioClip skillSound;
    private AudioSource audioSource;  // 新增音效播放組件

    private PlayerInput playerInput;
    private InputAction skillAction;

    private void Awake()
    {
        playerInput = new PlayerInput();
        //skillAction = playerInput.GamePlay.Skill1;
        anim = GetComponentInParent<Animator>();

        // 初始化音效組件
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void Update()
    {
        if (skillAction.triggered && Time.time > lastUsedTime + cooldown)
        {
            TriggerSkill();
            lastUsedTime = Time.time;
        }
    }

    void TriggerSkill()
    {
        // 觸發動畫
        anim.SetTrigger("Skill1");
    }
    // 在 Animator 的動畫事件中調用
    public void OnSkillEffectTrigger()
    {
        // 生成特效實例（不需要 Player 自身擁有 CFXR_Effect）
        if (skillEffectPrefab != null && attackPoint != null)
        {
            // 生成特效並設置位置
            GameObject effectInstance = Instantiate(
                skillEffectPrefab,
                attackPoint.position,
                attackPoint.rotation
            );
        }
        // 播放音效
        if (skillSound != null)
            audioSource.PlayOneShot(skillSound);

        // 傷害判定
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            LayerMask.GetMask("Enemy")
        );
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<CharactorBase>()?.TakeDamage(new Attack { Damage = damage });
        }
    }
    // 可視化攻擊範圍
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnEnable() => playerInput.Enable();
    private void OnDisable() => playerInput.Disable();
}