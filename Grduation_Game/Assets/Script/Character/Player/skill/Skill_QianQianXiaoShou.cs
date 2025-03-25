using UnityEngine;
using UnityEngine.InputSystem;

public class Skill_QianQianXiaoShou : MonoBehaviour
{
    [Header("�ޯ�Ѽ�")]
    public float damage = 30f;
    public float cooldown = 3f;
    public float attackRange = 2f;
    private float lastUsedTime;

    [Header("�ޥβե�")]
    public Animator anim;
    public Transform attackPoint;
    public GameObject skillEffectPrefab; // �אּ Prefab �ޥ�
    public AudioClip skillSound;
    private AudioSource audioSource;  // �s�W���ļ���ե�

    private PlayerInput playerInput;
    private InputAction skillAction;

    private void Awake()
    {
        playerInput = new PlayerInput();
        //skillAction = playerInput.GamePlay.Skill1;
        anim = GetComponentInParent<Animator>();

        // ��l�ƭ��Ĳե�
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
        // Ĳ�o�ʵe
        anim.SetTrigger("Skill1");
    }
    // �b Animator ���ʵe�ƥ󤤽ե�
    public void OnSkillEffectTrigger()
    {
        // �ͦ��S�Ĺ�ҡ]���ݭn Player �ۨ��֦� CFXR_Effect�^
        if (skillEffectPrefab != null && attackPoint != null)
        {
            // �ͦ��S�Ĩó]�m��m
            GameObject effectInstance = Instantiate(
                skillEffectPrefab,
                attackPoint.position,
                attackPoint.rotation
            );
        }
        // ���񭵮�
        if (skillSound != null)
            audioSource.PlayOneShot(skillSound);

        // �ˮ`�P�w
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
    // �i���Ƨ����d��
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnEnable() => playerInput.Enable();
    private void OnDisable() => playerInput.Disable();
}