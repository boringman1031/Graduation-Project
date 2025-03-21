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
    public GameObject skillEffectPrefab;
    public AudioClip skillSound;

    private PlayerInput playerInput;
    private InputAction skillAction;

    private void Awake()
    {
        // 綁定 Input System
        playerInput = new PlayerInput();
        skillAction = playerInput.GamePlay.Skill1;
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
        anim.SetTrigger("QianQianXiaoShou");

        // 生成特效（可搭配 Animation Event 更精準控制時機）
        Instantiate(skillEffectPrefab, attackPoint.position, attackPoint.rotation);

        // 播放音效
        AudioSource.PlayClipAtPoint(skillSound, transform.position);

        // 傷害判定（使用 OverlapCircle 或 Raycast）
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, LayerMask.GetMask("Enemy"));
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