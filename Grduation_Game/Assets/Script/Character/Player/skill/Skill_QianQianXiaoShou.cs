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
    public GameObject skillEffectPrefab;
    public AudioClip skillSound;

    private PlayerInput playerInput;
    private InputAction skillAction;

    private void Awake()
    {
        // �j�w Input System
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
        // Ĳ�o�ʵe
        anim.SetTrigger("QianQianXiaoShou");

        // �ͦ��S�ġ]�i�f�t Animation Event ���Ǳ���ɾ��^
        Instantiate(skillEffectPrefab, attackPoint.position, attackPoint.rotation);

        // ���񭵮�
        AudioSource.PlayClipAtPoint(skillSound, transform.position);

        // �ˮ`�P�w�]�ϥ� OverlapCircle �� Raycast�^
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, LayerMask.GetMask("Enemy"));
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