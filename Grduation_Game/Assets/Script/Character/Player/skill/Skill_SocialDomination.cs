using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Skill_SocialDomination : MonoBehaviour
{
    [Header("技能參數")]
    public float duration = 10f;       // 技能持续时间
    public float damagePerSecond = 10f; // 每秒伤害
    public float cooldown = 20f;       // 冷却时间
    private float lastUsedTime = -Mathf.Infinity;
    private bool isSkillActive = false;

    [Header("動畫設置")]
    public Animator skillAnimator;
    public string startAnimationName = "Player_Skill_社交牛逼症";

    [Header("特效引用")]
    public GameObject socialEnergyPrefab; // 预制体特效
    public Transform effectCenter;        // 特效中心点

    [Header("组件引用")]
    private CharactorBase character;
    private PlayerInput playerInput;
    private InputAction skillAction;

    private GameObject activeEffectInstance; // 当前激活的特效实例

    private void Awake()
    {
        character = GetComponent<CharactorBase>();
        if (character == null)
        {
            Debug.LogError("角色基础组件缺失！", this);
            enabled = false; // 禁用脚本防止后续错误
            return;
        }

        playerInput = new PlayerInput();
        //skillAction = playerInput.GamePlay.Skill2;
        skillAction.performed += _ => TriggerSkill();

        if (skillAnimator == null)
            skillAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isSkillActive && activeEffectInstance != null)
        {
            activeEffectInstance.transform.position = effectCenter.position;
        }
    }

    private void OnEnable() => playerInput.Enable();
    private void OnDisable()
    {
        playerInput.Disable();
        skillAction.performed -= _ => TriggerSkill();
    }

    public void TriggerSkill()
    {
        if (Time.time < lastUsedTime + cooldown) return;
        if (character == null) return;

        StartCoroutine(SkillSequence());
        lastUsedTime = Time.time;
    }

    private IEnumerator SkillSequence()
    {
        // 播放起始動畫
        if (skillAnimator != null)
        {
            skillAnimator.Play(startAnimationName);
            yield return new WaitForSeconds(GetAnimationLength(startAnimationName));
        }

        // 創建 Attack 物件並綁定到角色
        GameObject attackHolder = new GameObject("SkillAttack");
        attackHolder.transform.position = effectCenter.position;  // 設定位置為特效中心點
        attackHolder.transform.SetParent(transform);             // 設為角色的子物件

        Attack attack = attackHolder.AddComponent<Attack>();
        attack.Damage = character.CurrentHealth * 0.1f;

        // 扣除血量（此時 Attack 的 transform 已指向角色）
        character.TakeDamage(attack);

        // 啟動持續效果
        StartCoroutine(SocialEnergyRoutine());

        // 安全銷毀臨時物件（根據需求調整時間）
        Destroy(attackHolder, 0.1f);
    }

    private IEnumerator SocialEnergyRoutine()
    {
        isSkillActive = true;

        // 实例化特效
        if (socialEnergyPrefab != null)
        {
            Debug.Log($"特效生成位置：{effectCenter.position}");
            activeEffectInstance = Instantiate(socialEnergyPrefab, effectCenter.position, Quaternion.identity);
            var particleSystem = activeEffectInstance.GetComponent<ParticleSystem>();
            particleSystem.Play();
        }

        float timer = 0;
        while (timer < duration)
        {
            ApplyAreaDamage();
            timer += 1;
            yield return new WaitForSeconds(1);
        }

        // 结束特效
        if (activeEffectInstance != null)
        {
            var particleSystem = activeEffectInstance.GetComponent<ParticleSystem>();
            particleSystem.Stop();
            Destroy(activeEffectInstance, particleSystem.main.duration); // 等待粒子播放完毕再销毁
        }

        isSkillActive = false;
    }

    private void ApplyAreaDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            effectCenter.position,
            3f,
            LayerMask.GetMask("Enemy")
        );

        Debug.Log($"檢測到 {enemies.Length} 個敵人");

        foreach (Collider2D enemy in enemies)
        {
            // 創建有效的 Attack 物件並綁定到角色
            GameObject attackObj = new GameObject("SkillAttack");
            attackObj.transform.SetParent(transform);  // 設為角色的子物件
            Attack attack = attackObj.AddComponent<Attack>();
            attack.Damage = damagePerSecond;

            // 觸發傷害
            enemy.GetComponent<CharactorBase>()?.TakeDamage(attack);

            // 立即銷毀臨時物件
            Destroy(attackObj, 0.1f);
        }
    }

    private float GetAnimationLength(string animationName)
    {
        if (skillAnimator == null) return 0;

        foreach (AnimationClip clip in skillAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
                return clip.length;
        }
        return 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (effectCenter == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(effectCenter.position, 3f);
    }
}